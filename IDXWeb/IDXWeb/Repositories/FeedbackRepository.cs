using Dapper;
using DocumentFormat.OpenXml.Office2013.PowerPoint;
using IDXWeb.Entities;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using Umbraco.Core.Media;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Umbraco.Web;

namespace IDXWeb.Repositories
{
    public class FeedbackRepository : RepositoryBase, IFeedbackRepository
    {
		protected IContentService ContentService;
        public FeedbackRepository(IDbTransaction transaction) : base(transaction)
        {}

        public IEnumerable<Feedback> Get(IContentService ContentService, IEnumerable<IPublishedContent> roots, int parentId, string start = "", string end = "")
        {
			this.ContentService = ContentService;

			var feedbackInitQuery = GetFeedbackInitQuery(parentId, start, end);
			var visitorTypesInitQuery = GetVisitorTypesInitQuery(roots);
			var categoriesInitQuery = GetCategoriesInitQuery(roots);
			var propertyTypesInitQuery = GetPropertyTypesInitQuery();

			string query = visitorTypesInitQuery + "\n\n" +
				categoriesInitQuery + "\n\n" +
				propertyTypesInitQuery + "\n\n" +
				feedbackInitQuery + "\n\n" +
				@"
				SELECT
					f.ContentId,
					f.Name,
					vt.Id AS VisitorTypeId,
					vt.Name AS VisitorTypeName,
					rating.dataDecimal AS Rating,
					otherCategories.dataNvarchar AS OtherCategory,
					descriptions.dataNtext AS Description,
					SUBSTRING(
					(
						SELECT ',' + Name FROM (
							SELECT CAST(fc.Name AS NVARCHAR(100)) AS Name FROM (
								SELECT splitdata FROM dbo.splitstring(category.dataNtext, ',')
							) AS catStr
							JOIN @FeedbackCategories AS fc ON CAST(fc.Udi AS NVARCHAR(MAX)) = catStr.splitdata
						) AS temp FOR XML PATH('')
					), 2, 9999
					) AS CategoryNamesStr,
					f.CreateDate,
					f.UpdateDate
				FROM @feedbacks AS f
				LEFT JOIN dbo.cmsPropertyData AS visitorTypes 
					ON visitorTypes.contentNodeId = f.ContentId 
					AND visitorTypes.versionId = f.VersionId 
					AND visitorTypes.propertytypeid = (SELECT TOP(1) pt.Id FROM @PropertyTypes AS pt WHERE pt.Alias = 'visitorType')
				LEFT JOIN @VisitorTypes AS vt ON vt.Udi = visitorTypes.DataNvarchar
				LEFT JOIN dbo.cmsPropertyData AS rating 
					ON rating.contentNodeId = f.ContentId 
					AND rating.versionId = f.VersionId
					AND rating.propertytypeid = (SELECT TOP(1) pt.Id FROM @PropertyTypes AS pt WHERE pt.Alias = 'rating')
				LEFT JOIN dbo.cmsPropertyData AS category 
					ON category.contentNodeId = f.ContentId 
					AND category.versionId = f.VersionId
					AND category.propertytypeid = (SELECT TOP(1) pt.Id FROM @PropertyTypes AS pt WHERE pt.Alias = 'category')
				LEFT JOIN dbo.cmsPropertyData AS otherCategories 
					ON otherCategories.contentNodeId = f.ContentId 
					AND otherCategories.versionId = f.VersionId
					AND otherCategories.propertytypeid = (SELECT TOP(1) pt.Id FROM @PropertyTypes AS pt WHERE pt.Alias = 'otherCategory')
				LEFT JOIN dbo.cmsPropertyData AS descriptions 
					ON descriptions.contentNodeId = f.ContentId 
					AND descriptions.versionId = f.VersionId
					AND descriptions.propertytypeid = (SELECT TOP(1) pt.Id FROM @PropertyTypes AS pt WHERE pt.Alias = 'description')
				ORDER BY CreateDate DESC";
            return Connection.Query<Feedback>(query, null, Transaction);
        }

		protected IEnumerable<StatsQueryResult> GetTimelineData(IEnumerable<IPublishedContent> roots, int parentId, string start = "", string end = "")
        {
			string feedbackInitQuery = GetFeedbackInitQuery(parentId, start, end);
			string visitorTypesInitQuery = GetVisitorTypesInitQuery(roots);
			string propertyTypesInitQuery = GetPropertyTypesInitQuery();

			var query = visitorTypesInitQuery + "\n\n" +
				propertyTypesInitQuery + "\n\n" +
				feedbackInitQuery + "\n\n" +
				@"
				SELECT 
					VisitorTypeId,
					CONVERT(VARCHAR(10), feedbacks.CreateDate, 103) AS Date,
					COUNT(ContentId) AS Count
				FROM (
					SELECT
						f.ContentId,
						vt.Id AS VisitorTypeId,
						vt.Name AS VisitorTypeName,
						f.CreateDate
					FROM @feedbacks AS f
					LEFT JOIN dbo.cmsPropertyData AS visitorTypes 
						ON visitorTypes.contentNodeId = f.ContentId 
						AND visitorTypes.versionId = f.VersionId 
						AND visitorTypes.propertytypeid = (SELECT TOP(1) pt.Id FROM @PropertyTypes AS pt WHERE pt.Alias = 'visitorType')
					LEFT JOIN @VisitorTypes AS vt ON vt.Udi = visitorTypes.DataNvarchar
				) AS feedbacks
				WHERE feedbacks.VisitorTypeId IS NOT NULL
				GROUP BY feedbacks.VisitorTypeId,
				CONVERT(VARCHAR(10), feedbacks.CreateDate, 103)
				ORDER BY Date
			";
			return Connection.Query<StatsQueryResult>(query, null, Transaction);
		}

		public object GetTimelineList(IContentService ContentService, IEnumerable<IPublishedContent> roots, int parentId, string start = "", string end = "")
        {
			this.ContentService = ContentService;

			var items = GetTimelineData(roots, parentId, start, end);
			var groups = items.GroupBy(x => x.VisitorTypeId);

			var visitorTypes = GetVisitorTypes(roots);
			var results = from visitorTypeGroup in groups
						  select new
						  {
							  VisitorType = visitorTypes.First(o => o.Id == visitorTypeGroup.First().VisitorTypeId).Name,
							  Items = visitorTypeGroup
						  };
			return results;
		}

		public object GetTodayFeedbackList(IContentService ContentService, IEnumerable<IPublishedContent> roots, int parentId, string start = "", string end = "")
        {
			this.ContentService = ContentService;

			var items = GetTimelineData(roots, parentId, start, end);
			var groups = items.GroupBy(x => x.Date);
			return from dateGroup in groups
				   select new
				   {
					   Date = dateGroup.First().Date,
					   Count = dateGroup.Sum(x => x.Count)
				   };
		}

		public double GetOverallRating(int parentId, string start = "", string end = "")
        {
			string feedbackInitQuery = GetFeedbackInitQuery(parentId, start, end);
			string propertyTypesInitQuery = GetPropertyTypesInitQuery();

			string query = propertyTypesInitQuery + "\n\n" +
				feedbackInitQuery + "\n\n" +
				@"
				SELECT ISNULL(AVG(dataDecimal),0) AS average FROM @feedbacks AS f
				LEFT JOIN dbo.cmsPropertyData AS visitorTypes 
					ON visitorTypes.contentNodeId = f.ContentId 
					AND visitorTypes.versionId = f.VersionId 
					AND visitorTypes.propertytypeid = (SELECT TOP(1) pt.Id FROM @PropertyTypes AS pt WHERE pt.Alias = 'rating')
				WHERE dataDecimal > 0
			";

			return Connection.Query<double>(query, null, Transaction).First();
		}

		protected IEnumerable<Models.VisitorType> GetVisitorTypes(IEnumerable<IPublishedContent> roots)
        {
			var visitorTypeDocs = new List<IPublishedContent>();
			foreach (var root in roots)
			{
				visitorTypeDocs = visitorTypeDocs.Concat(root.Descendants("visitorType")).ToList();
			}
			return visitorTypeDocs.Select(o => new Models.VisitorType(ContentService.GetById(o.Id))).ToList();
		}

		protected string GetFeedbackInitQuery(int parentId, string start, string end)
        {
			string dateFilter = "";
			if (!string.IsNullOrEmpty(start))
			{
				var startDatetime = DateTime.ParseExact(start, "dd/MM/yyyy", CultureInfo.InvariantCulture);
				dateFilter += "AND CreateDate >= '" + startDatetime.ToString("yyyy-MM-dd") + " 00:00:00'\n";
			}

			if (!string.IsNullOrEmpty(end))
			{
				var endDatetime = DateTime.ParseExact(end, "dd/MM/yyyy", CultureInfo.InvariantCulture);
				dateFilter += "AND CreateDate <= '" + endDatetime.ToString("yyyy-MM-dd") + " 23:59:59'\n";
			}

			return @"
			/** Get all feedback nodeId and latest versionId **/
				DECLARE @feedbacks TABLE (ContentId INT, VersionId UNIQUEIDENTIFIER, Name NVARCHAR(255), CreateDate DATETIME, UpdateDate DATETIME)
				INSERT INTO @Feedbacks (ContentId, VersionId, Name, CreateDate, UpdateDate)
				SELECT 
					nodeId,
					cmsDocument.versionId, 
					cmsDocument.text,
					VersionDate AS createDate,
					updateDate 
				FROM dbo.cmsDocument
				LEFT JOIN (
					SELECT ContentId, MIN(VersionDate) AS VersionDate FROM dbo.cmsContentVersion GROUP BY ContentId
				) AS versions ON versions.ContentId = nodeId
				LEFT JOIN umbracoNode AS un ON un.id = cmsDocument.nodeId
				WHERE parentID = " + parentId + @"
				AND nodeId IN (
					SELECT cmsContent.nodeId FROM cmsContent
					JOIN dbo.cmsContentType ON cmsContentType.nodeId = cmsContent.contentType
					WHERE cmsContentType.alias = 'feedback'
					AND newest = 1
				)
				" + dateFilter + @"
				ORDER BY updateDate DESC
			";
        }

		protected string GetVisitorTypesInitQuery(IEnumerable<IPublishedContent> roots)
        {
			var visitorTypes = GetVisitorTypes(roots);
			string visitorTypesInitQuery = "";
			foreach (var visitorType in visitorTypes)
			{
				if (visitorTypesInitQuery != "")
				{
					visitorTypesInitQuery += ",";
				}
				visitorTypesInitQuery += "(" + visitorType.Id + ", '" + visitorType.Name + "', '" + visitorType.Udi + "')";
			}
			return @"
				DECLARE @VisitorTypes TABLE (Id INT, Name NVARCHAR(255), Udi NVARCHAR(255))
				INSERT INTO @VisitorTypes VALUES
			" + visitorTypesInitQuery;
		}

		protected IEnumerable<Models.FeedbackCategory> GetFeedbackCategories(IEnumerable<IPublishedContent> roots)
		{
			var categoryDocs = new List<IPublishedContent>();
			foreach (var root in roots)
			{
				categoryDocs = categoryDocs.Concat(root.Descendants("feedbackCategory")).ToList();
			}
			return categoryDocs.Select(o => new Models.FeedbackCategory(ContentService.GetById(o.Id))).ToList();
		}

		protected string GetCategoriesInitQuery(IEnumerable<IPublishedContent> roots)
        {
			var categories = GetFeedbackCategories(roots);
			string categoriesInitQuery = "";
			foreach (var category in categories)
			{
				if (categoriesInitQuery != "")
				{
					categoriesInitQuery += ",";
				}
				categoriesInitQuery += "(" + category.Id + ", '" + category.Name + "', '" + category.Udi + "')";
			}
			return @"
				DECLARE @FeedbackCategories TABLE (Id INT, Name NVARCHAR(255), Udi NTEXT)
				INSERT INTO @FeedbackCategories VALUES
			" + categoriesInitQuery;
		}

		protected string GetPropertyTypesInitQuery()
        {
			return @"
				DECLARE @PropertyTypes TABLE (Id INT, Alias NVARCHAR(255), Name NVARCHAR(255))
				INSERT INTO @PropertyTypes (Id, Alias, Name)
				SELECT cmsPropertyType.id, cmsPropertyType.Alias, cmsPropertyType.Name FROM dbo.cmsPropertyType
				JOIN dbo.cmsContentType ON cmsContentType.nodeId = cmsPropertyType.contentTypeId
				WHERE cmsContentType.Alias = 'feedback'
			";
		}

		public void Add(Feedback entity)
        {
            throw new NotImplementedException();
        }

        public Feedback GetByUmbracoId(string itemId)
        {
            throw new NotImplementedException();
        }

        public void Remove(int id)
        {
            throw new NotImplementedException();
        }

        public void Remove(Feedback entity)
        {
            throw new NotImplementedException();
        }

        public void RemoveByUmbracoId(string itemId)
        {
            throw new NotImplementedException();
        }

        public void Save(Feedback entity)
        {
            throw new NotImplementedException();
        }

        public void Update(Feedback entity)
        {
            throw new NotImplementedException();
        }
    }

	public class StatsQueryResult
    {
		public int VisitorTypeId { get; set; }
		public string Date { get; set; }
		public int Count { get; set; }

    }
}