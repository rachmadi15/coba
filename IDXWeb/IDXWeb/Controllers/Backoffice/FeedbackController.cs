using DocumentFormat.OpenXml.Spreadsheet;
using IDXWeb.Models;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Umbraco.Web;
using Umbraco.Web.WebApi;
using System.Web.Script.Serialization;
using umbraco.MacroEngines;
using UmbracoExamine.DataServices;
using SystemTask = System.Threading.Tasks.Task;
using Umbraco.Web.Models;
using IDXWeb.Repositories;

namespace IDXWeb.Controllers.Backoffice
{
    // [RoutePrefix("{controller}/{action}")]
    public class FeedbackController : BaseApiController
    {
        [HttpGet]
        public IEnumerable<Feedback> GetAll(string start = null, string end = null)
        {
            IEnumerable<IContent> results = new List<IContent>();
            var localized = GetAllWithLocales(start, end);
            foreach (var feedbacks in localized)
            {
                results = results.Concat(feedbacks.Value);
            }
            return results.Select(o => new Feedback(o));
        }

        [HttpGet]
        [System.Web.Http.AcceptVerbs("GET")]
        public IEnumerable<VisitorType> GetVisitorTypes(int currentNodeId = 0)
        {
            var results = new List<IContent>();

            if (currentNodeId > 0)
            {
                var node = contentService.GetById(currentNodeId);
                if (node != null)
                {
                    var parent = node.Parent();
                    if (parent != null && parent.Name == "User Feedback")
                    {
                        var visitorTypesNode = contentService.GetChildren(parent.Id).FirstOrDefault(o => o.ContentType.Alias == "visitorTypes");
                        if (visitorTypesNode != null)
                        {
                            results = contentService.GetChildren(visitorTypesNode.Id).Where(o => o.ContentType.Alias == "visitorType").ToList<IContent>();
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < rootContent.Count(); i++)
                {
                    var userFeedbackNode = rootContent.ElementAt(i).Children().FirstOrDefault(o => o.Name == "User Feedback");
                    if (userFeedbackNode != null)
                    {
                        var visitorTypesNode = userFeedbackNode.Children().FirstOrDefault(o => o.ContentType.Alias == "visitorTypes");
                        if (visitorTypesNode != null)
                        {
                            results = results.Concat(contentService.GetChildren(visitorTypesNode.Id).Where(o => o.ContentType.Alias == "visitorType")).ToList<IContent>();
                        }
                    }
                }
            }

            return results.Select(o => new VisitorType(o));
        }

        [HttpGet]
        [System.Web.Http.AcceptVerbs("GET")]
        public IEnumerable<FeedbackCategory> GetCategories(int currentNodeId = 0)
        {
            var results = new List<IContent>();

            if (currentNodeId > 0)
            {
                var node = contentService.GetById(currentNodeId);
                if (node != null)
                {
                    var parent = node.Parent();
                    if (parent != null && parent.Name == "User Feedback")
                    {
                        var visitorTypesNode = contentService.GetChildren(parent.Id).FirstOrDefault(o => o.ContentType.Alias == "feedbackCategories");
                        if (visitorTypesNode != null)
                        {
                            results = contentService.GetChildren(visitorTypesNode.Id).Where(o => o.ContentType.Alias == "feedbackCategory").ToList<IContent>();
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < rootContent.Count(); i++)
                {
                    var userFeedbackNode = rootContent.ElementAt(i).Children().FirstOrDefault(o => o.Name == "User Feedback");
                    if (userFeedbackNode != null)
                    {
                        var categoriesNode = userFeedbackNode.Children().FirstOrDefault(o => o.ContentType.Alias == "feedbackCategories");
                        if (categoriesNode != null)
                        {
                            results = results.Concat(contentService.GetChildren(categoriesNode.Id).Where(o => o.ContentType.Alias == "feedbackCategory")).ToList<IContent>();
                        }
                    }
                }
            }

            return results.Select(o => new FeedbackCategory(o));
        }

        [HttpGet]
        [System.Web.Http.AcceptVerbs("GET")]
        public object GetStats(int nid, string start, string end, bool merge_vt = false)
        {
            //var node = contentService.GetById(nid);
            //if (node == null)
            //{
            //    return new { OverallRating = 0, List = new List<object>() };
            //}

            //var home = node.Ancestors().FirstOrDefault(o => o.ContentType.Alias == "home");

            //var items = GetAllWithLocale(home.Name, start, end);
            //IEnumerable<Feedback> parsed = items.Select(o => new Feedback(o));

            //if (merge_vt)
            //{
            //    var grouped = parsed.GroupBy(o => o.CreateDateStr);
            //    return from item in grouped
            //           select new
            //           {
            //               Date = item.First().CreateDateStr,
            //               Count = item.Count()
            //           };
            //}
            //double sum = parsed.Sum(o => o.Rating);
            //double overallRating = sum / (double)(parsed.Count());
            //if (double.IsNaN(overallRating))
            //{
            //    overallRating = 0;
            //}

            //var groupByVisitorType = parsed.GroupBy(o => o.VisitorType);
            //var groupByDate = from item in groupByVisitorType select item.GroupBy(o => o.CreateDateStr);
            //var list = from visitorTypeGroup in groupByDate
            //           select new
            //           {
            //               visitorTypeGroup.First().First().VisitorType,
            //               Items = from dateGroup in visitorTypeGroup
            //                       select new
            //                       {
            //                           Date = dateGroup.First().CreateDateStr,
            //                           Count = dateGroup.Count()
            //                       }
            //           };

            object list;
            double overallRating = 0;
            var roots = Umbraco.TypedContentAtRoot().Where(o => o.DocumentTypeAlias == "home");

            using (var uow = new UnitOfWork("umbracoDbDSN"))
            {
                if (merge_vt)
                {
                    return uow.FeedbackRepository.GetTodayFeedbackList(Services.ContentService, roots, nid, start, end);
                }
                list = uow.FeedbackRepository.GetTimelineList(Services.ContentService, roots, nid, start, end);
                overallRating = uow.FeedbackRepository.GetOverallRating(nid, start, end);
            }
            var results = new { OverallRating = overallRating, List = list };
            return results;
        }

        [HttpGet]
        [System.Web.Http.AcceptVerbs("GET")]
        public HttpResponseMessage Export(string start = null, string end = null)
        {
            var roots = Umbraco.TypedContentAtRoot().Where(o => o.DocumentTypeAlias == "home");
            var allList = new Dictionary<string, IEnumerable<Entities.Feedback>>();
            using (var uow = new UnitOfWork("umbracoDbDSN"))
            {
                foreach (var root in roots)
                {
                    var parent = root.Descendant("feedbackList");
                    if (parent == null) continue;
                    allList.Add(root.Name, uow.FeedbackRepository.Get(Services.ContentService, roots, parent.Id, start, end));
                }
            }

            ExcelPackage ep = new ExcelPackage();

            foreach (var list in allList)
            {
                ExcelWorksheet sheet = ep.Workbook.Worksheets.Add(list.Key);

                sheet.Cells["A1"].Value = "Name";
                sheet.Cells["B1"].Value = "Visitor Type";
                sheet.Cells["C1"].Value = "Rating";
                sheet.Cells["D1"].Value = "Category";
                sheet.Cells["E1"].Value = "Other Category";
                sheet.Cells["F1"].Value = "Description";
                sheet.Cells["G1"].Value = "Submited at";
                sheet.Cells["H1"].Value = "Submited at (formattable)";

                int rowNum = 2;
                foreach (var feedback in list.Value)
                {
                    sheet.Cells[string.Format("A{0}", rowNum)].Value = feedback.Name;
                    sheet.Cells[string.Format("B{0}", rowNum)].Value = feedback.VisitorTypeName;
                    sheet.Cells[string.Format("C{0}", rowNum)].Value = feedback.Rating;
                    sheet.Cells[string.Format("D{0}", rowNum)].Value = feedback.CategoryNamesStr;
                    sheet.Cells[string.Format("E{0}", rowNum)].Value = feedback.OtherCategory;
                    sheet.Cells[string.Format("F{0}", rowNum)].Value = feedback.Description;
                    sheet.Cells[string.Format("G{0}", rowNum)].Value = feedback.CreateDate.ToString("dd/MM/yyyy HH:mm");
                    sheet.SetValue(string.Format("H{0}", rowNum), feedback.CreateDate.ToOADate());
                    rowNum++;
                }
            }

            var path = server.MapPath("~/App_Data/TEMP/ExportFeedbacks");
            var tempDirectory = new DirectoryInfo(path);

            // Create directory if it doesn't exists
            if (!tempDirectory.Exists)
            {
                tempDirectory = Directory.CreateDirectory(path);
                tempDirectory.Create();
            }

            // Clear previous files
            foreach (FileInfo oldFile in tempDirectory.GetFiles())
            {
                oldFile.Delete();
            }

            string filename = "feedbacks_";
            if (start == null && end == null) {
                filename += "all";
            } else {
                if (start != null) {
                    var startDt = DateTime.ParseExact(start, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    filename += startDt.ToString("yyyyMMdd");
                } else {
                    filename += "first";
                }

                filename += "-";
                if (end != null) {
                    var endDt = DateTime.ParseExact(end, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    filename += endDt.ToString("yyyyMMdd");
                } else {
                    filename += "last";
                }
            }
            filename += ".xls";
            var filepath = path + "/" + filename;
            var file = new FileInfo(filepath);
            ep.SaveAs(file);

            // Transmit
            var response = HttpContext.Current.Response;
            response.ClearContent();
            response.Clear();
            response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
            response.ContentType = "application/x-zip-compressed";
            response.TransmitFile(filepath);
            response.Flush();
            response.End();

            return Request.CreateResponse(HttpStatusCode.OK);
        }

            [HttpGet]
        [System.Web.Http.AcceptVerbs("GET")]
        public HttpResponseMessage ExportBak(string start = null, string end = null)
        {
            var localized = GetAllWithLocales(start, end);
            ExcelPackage ep = new ExcelPackage();

            foreach (var feedbacks in localized)
            {
                ExcelWorksheet sheet = ep.Workbook.Worksheets.Add(feedbacks.Key);
                sheet.Cells["A1"].Value = "Name";
                sheet.Cells["B1"].Value = "Visitor Type";
                sheet.Cells["C1"].Value = "Category";
                sheet.Cells["D1"].Value = "Rating";
                sheet.Cells["E1"].Value = "Description";
                sheet.Cells["F1"].Value = "Submited at";
                sheet.Cells["G1"].Value = "Submited at (formattable)";

                var stylesheet = sheet.Workbook.Styles.NumberFormats;

                var items = feedbacks.Value.Select(o => new Feedback(o));
                int rowNum = 2;
                foreach (var feedback in items)
                {
                    sheet.Cells[string.Format("A{0}", rowNum)].Value = feedback.Name;
                    sheet.Cells[string.Format("B{0}", rowNum)].Value = feedback.VisitorType;
                    sheet.Cells[string.Format("C{0}", rowNum)].Value = feedback.Category;
                    sheet.Cells[string.Format("D{0}", rowNum)].Value = feedback.Rating;
                    sheet.Cells[string.Format("E{0}", rowNum)].Value = feedback.Description;
                    sheet.Cells[string.Format("F{0}", rowNum)].Value = feedback.CreateDate.ToString("dd/MM/yyyy HH:mm");
                    sheet.SetValue(string.Format("G{0}", rowNum), feedback.CreateDate.ToOADate());

                    rowNum++;
                }
            }

            var path = server.MapPath("~/App_Data/TEMP/ExportFeedbacks");
            var tempDirectory = new DirectoryInfo(path);

            // Create directory if it doesn't exists
            if (!tempDirectory.Exists)
            {
                tempDirectory = Directory.CreateDirectory(path);
                tempDirectory.Create();
            }

            // Clear previous files
            foreach (FileInfo oldFile in tempDirectory.GetFiles())
            {
                oldFile.Delete();
            }

            var filepath = path + "/feedbacks_" + DateTime.Now.ToString("yyyyMMddHHmmsss") + ".xls";
            var file = new FileInfo(filepath);
            ep.SaveAs(file);

            // Transmit
            var response = HttpContext.Current.Response;
            response.ClearContent();
            response.Clear();
            response.AppendHeader("Content-Disposition", "attachment; filename=feedbacks_" + DateTime.Now.ToString("yyyyMMdd_HHmmsss") + ".xls");
            response.ContentType = "application/x-zip-compressed";
            response.TransmitFile(filepath);
            response.Flush();
            response.End();

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        private Dictionary<string, IEnumerable<IContent>> GetAllWithLocales(string start = null, string end = null, string locale = null)
        {
            var results = new Dictionary<string, IEnumerable<IContent>>();
            foreach (var home in rootContent)
            {
                if (!string.IsNullOrEmpty(locale) && home.Name != locale)
                {
                    continue;
                }

                var homeContent = contentService.GetById(home.Id);
                var feedbacks = homeContent.Descendants().Where(o => o.ContentType.Alias == "feedback");

                if (start != null)
                {
                    var startDatetime = DateTime.ParseExact(start, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    feedbacks = feedbacks.Where(o => o.CreateDate > startDatetime);
                }

                if (end != null)
                {
                    var endDatetime = DateTime.ParseExact(end + " 23:59", "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                    feedbacks = feedbacks.Where(o => o.CreateDate < endDatetime);
                }

                results[home.Name] = feedbacks;
            }

            return results;
        }

        private IEnumerable<IContent> GetAllWithLocale(string locale, string start = null, string end = null)
        {
            var results = GetAllWithLocales(start, end, locale);
            return results[locale];
        }

        private IEnumerable<IContent> GetAllContent(string locale = null)
        {
            var roots = contentService.GetRootContent().Where(o => o.ContentType.Alias == "home");

            var en = roots.FirstOrDefault(o => o.Name == "en-us");
            var enFeedbackList = en.Descendants().FirstOrDefault(o => o.ContentType.Alias == "feedbackList");
            var enFeedbacks = enFeedbackList.Descendants().Where(o => o.ContentType.Alias == "feedback");

            var id = roots.FirstOrDefault(o => o.Name == "id-id");
            var idFeedbackList = id.Descendants().FirstOrDefault(o => o.ContentType.Alias == "feedbackList");
            var idFeedbacks = idFeedbackList.Descendants().Where(o => o.ContentType.Alias == "feedback");

            if (locale == "en")
            {
                return enFeedbacks;
            }
            else if (locale == "id")
            {
                return idFeedbacks;
            }

            return enFeedbacks.Concat(idFeedbacks);
        }

        private string GetStatusMessage(IEnumerable<IContent> feedbacks)
        {
            int total = feedbacks.Count();
            int published = feedbacks.Where(o => o.Status == ContentStatus.Published).Count();
            int unpublished = feedbacks.Where(o => o.Status == ContentStatus.Unpublished).Count();
            return string.Format("Total Feedbacks: {0}, Published: {1}, Unpublished: {2}.", total, published, unpublished);
        }

        [HttpGet]
        [System.Web.Http.AcceptVerbs("GET")]
        public HttpResponseMessage Status()
        {
            var feedbacks = GetAllContent();
            return Request.CreateResponse<string>(HttpStatusCode.OK, GetStatusMessage(feedbacks));
        }
    }
}