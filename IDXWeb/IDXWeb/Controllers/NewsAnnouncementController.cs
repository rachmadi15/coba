using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Flurl;
using Flurl.Http;
using IDXWeb.Models;
using Newtonsoft.Json;
using umbraco.NodeFactory;
using Umbraco.Core;
using Umbraco.Core.Logging;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Mvc;

namespace IDXWeb.Controllers
{
    public class NewsAnnouncementController : SurfaceController
    {

        #region Announcement
        public async Task<ActionResult> GetAllAnnouncement(string keywords = "", string dateFrom = "0", string dateTo = "0", int pageNumber = 0, int pageSize = 0, string language = "id-id") {
            try {
                var link = ConfigurationManager.AppSettings["LINK_ANNOUNCEMENT_ALL"];
                var url = link.SetQueryParams(new {
                    keywords,
                    dateFrom,
                    dateTo,
                    pageNumber,
                    pageSize,
                    language
                });
                var jsonResult = await url.GetStringAsync();
                var content = jsonResult.Replace(@"\\", "/").Replace(@"////IDXWEB-AD", "").Replace(@"//", "/");

                var contentToObject = JsonConvert.DeserializeObject<Response<AnnouncementItem>>(content);

                foreach (AnnouncementItem item in contentToObject.Items) {
                    item.PdfPath = item.PdfPath.Replace("\\\"", "\"");

                    var length = item.PdfPath.IndexOf('[');
                    var isArray = length >= 0;

                    if (isArray) {
                        item.Attachments = JsonConvert.DeserializeObject<List<AnnouncementAttachment>>(item.PdfPath);
                        item.Attachments = item.Attachments.OrderBy(x => x.IsAttachment).ToList();
                    }
                    else {
                        item.Attachments = new List<AnnouncementAttachment>();
                        var deserializeObject = JsonConvert.DeserializeObject<AnnouncementAttachment>(item.PdfPath);
                        item.Attachments.Add(deserializeObject);
                    }
                }

                return Content(JsonConvert.SerializeObject(contentToObject), "application/json");
            }
            catch (Exception e) {
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e.Message, e);
                return null;
            }
        }
        #endregion

        #region Suspension
        public async Task<ActionResult> GetSuspension(string keyword = "", string dateFrom = "", string dateTo = "", int indexFrom = 0, int pageSize = 0, string type = "", string language="id-id") {
            try {
                var linkCorpAction = await ConfigurationManager.AppSettings["LINK_NA_SUSPENSION"].SetQueryParams(new {
                    indexFrom,
                    pageSize,
                    keyword,
                    dateFrom,
                    dateTo,
                    type,
                    language
                }).GetStringAsync();
                return Content(linkCorpAction, "application/json");
            }
            catch (Exception e) {
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e.Message, e);
                return null;
            }
        }
        #endregion

        #region UMA
        public async Task<ActionResult> GetUma(string keyword = "", string dateFrom = "", string dateTo = "", int indexFrom = 0, int pageSize = 0) {
            try {
                var linkCorpAction = await ConfigurationManager.AppSettings["LINK_NA_UMA"].SetQueryParams(new {
                    indexFrom,
                    pageSize,
                    keyword,
                    dateFrom,
                    dateTo
                }).GetStringAsync();
                return Content(linkCorpAction, "application/json");
            }
            catch (Exception e) {
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e.Message, e);
                return null;
            }
        }
        #endregion

        #region Press Release
        public async Task<ActionResult> GetPressRelease(string keyword = "", string dateFrom = "", string dateTo = "", string locale = "", int pageNumber = 0, int pageSize = 0) {
            try {
                var linkResult = await ConfigurationManager.AppSettings["LINK_NA_PRESS_RELEASE_SEARCH"].SetQueryParams(new {
                    keyword,
                    dateFrom,
                    dateTo,
                    locale,
                    pageNumber,
                    pageSize
                }).GetStringAsync();
                return Content(linkResult, "application/json");
            }
            catch (Exception e) {
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e.Message, e);
                return null;
            }
        }
        public async Task<ActionResult> GetPressReleaseDetail(string id = "") {
            try {
                var linkResult = await (ConfigurationManager.AppSettings["LINK_NA_PRESS_RELEASE"] + "/" + id).GetStringAsync();
                return Content(linkResult, "application/json");
            }
            catch (Exception e) {
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e.Message, e);
                return null;
            }
        }
        #endregion

        #region ETD
        public async Task<ActionResult> GetEtdTd(string keyword = "", string dateFrom = "", string dateTo = "", string type = "", int indexFrom = 0, int pageSize = 0) {
            try {
                var linkResult = await ConfigurationManager.AppSettings["LINK_NA_ETDTD"].SetQueryParams(new {
                    keyword,
                    dateFrom,
                    dateTo,
                    type,
                    indexFrom,
                    pageSize
                }).GetStringAsync();
                return Content(linkResult, "application/json");
            }
            catch (Exception e) {
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e.Message, e);
                return null;
            }
        }
        #endregion

        #region News
        public async Task<ActionResult> GetNewsSearch(string keyword = "", string dateFrom = "", string dateTo = "", string locale = "", int pageNumber = 0, int pageSize = 0, string isHeadline = "") {
            try {
                var url = ConfigurationManager.AppSettings["LINK_ANNOUNCEMENT_NEWS_SEARCH"].SetQueryParams(new {
                    keyword,
                    dateFrom,
                    dateTo,
                    locale,
                    pageNumber,
                    pageSize,
                    isHeadline
                });
                var linkResult = await url.GetStringAsync();
                return Content(linkResult, "application/json");
            }
            catch (Exception e) {
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e.Message, e);
                return null;
            }
        }

        public async Task<ActionResult> GetNewsDetail(string id = "") {
            try {
                var linkResult = await (ConfigurationManager.AppSettings["LINK_ANNOUNCEMENT_NEWS"] + id).GetStringAsync();
                return Content(linkResult, "application/json");
            }
            catch (Exception e) {
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e.Message, e);
                return null;
            }
        }

        public async Task<ActionResult> GetNewsDetailWithLocale(string newsId = "", string locale = "") {
            try {
                var guidResult = Guid.NewGuid();
                var isNewsGuid = Guid.TryParse(newsId, out guidResult);

                if (isNewsGuid) {
                    var linkResult = await (ConfigurationManager.AppSettings["LINK_ANNOUNCEMENT_NEWS"]).SetQueryParams(new {
                        newsId,
                        locale
                    }).GetStringAsync();
                    return Content(linkResult, "application/json");
                }
                else {
                    var rs = ApplicationContext.Current.Services.RelationService;

                    var numberNewsId = 0;

                    var parseResult = int.TryParse(newsId, out numberNewsId);

                    if (parseResult) {
                        var relations = rs.GetByParentOrChildId(numberNewsId);
                        Node relatedNode = null;
                        foreach (Relation relation in relations) {
                            if (relation.RelationType.Alias == "relateDocumentOnCopy") {
                                relatedNode = relation.ParentId == numberNewsId ?
                                    new Node(relation.ChildId) :
                                    new Node(relation.ParentId);
                            }
                        }

                        if (relatedNode != null) {
                            var isLocaleCorrect = relatedNode.Url.Contains(locale);

                            if (isLocaleCorrect) {
                                numberNewsId = relatedNode.Id;
                            }

                            var content = Umbraco.Content(numberNewsId);

                            var newsItem = new NewsDetailItem();
                            newsItem.Title = content.newsTitle;
                            if (content.newContent != null) {
                                newsItem.Contents = ((HtmlString)content.newsContent).ToString();
                            }
                            var newsImage = content.newsImage;
                            newsItem.ImageUrl = newsImage.Url;
                            newsItem.PublishedDate = content.newsPublishDate;

                            return Content(JsonConvert.SerializeObject(newsItem), "application/json");
                        }
                    }
                }

                return null;
            }
            catch (Exception e) {
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e.Message, e);
                return null;
            }
        }

        public async Task<ActionResult> GetNewsRelated(string id = "", int limit = 4, int offset = 0, string locale = "") {
            try {
                var linkResult = await ConfigurationManager.AppSettings["LINK_ANNOUNCEMENT_NEWS_RELATED"].SetQueryParams(new {
                    id,
                    limit,
                    offset,
                    locale
                }).GetStringAsync();
                return Content(linkResult, "application/json");
            }
            catch (Exception e) {
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e.Message, e);
                return null;
            }
        }
        #endregion

        #region SPPA Announcement
        public async Task<ActionResult> GetSppaAnnouncement(string keyword = "", string dateFrom = "", string dateTo = "", string locale = "", int pageNumber = 0, int pageSize = 0) {
            try {
                var linkResult = await ConfigurationManager.AppSettings["LINK_SPPA_ANNOUNCEMENT"].SetQueryParams(new {
                    keyword,
                    dateFrom,
                    dateTo,
                    locale,
                    pageNumber,
                    pageSize
                }).GetStringAsync();
                return Content(linkResult, "application/json");
            }
            catch (Exception e) {
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e.Message, e);
                return null;
            }
        }
        public async Task<ActionResult> GetSppaAnnouncementDetail(string id = "", string locale = "") {
            try {
                var linkResult = await (ConfigurationManager.AppSettings["LINK_SPPA_ANNOUNCEMENT_DETAIL"] + "?id=" + id + "&locale=" + locale).GetStringAsync();
                return Content(linkResult, "application/json");
            }
            catch (Exception e) {
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e.Message, e);
                return null;
            }
        }
        #endregion

        #region SPPA Profile

        public async Task<ActionResult> GetSPPAProfile(string keyword = "", string dateFrom = "", string dateTo = "", string locale = "", int pageNumber = 0, int pageSize = 0) {
            try {
                var linkResult = await ConfigurationManager.AppSettings["LINK_SPPA_PROFILE"].SetQueryParams(new {
                    keyword,
                    dateFrom,
                    dateTo,
                    locale,
                    pageNumber,
                    pageSize
                }).GetStringAsync();
                return Content(linkResult, "application/json");
            }
            catch (Exception e) {
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e.Message, e);
                return null;
            }
        }
        public async Task<ActionResult> GetSPPAProfileDetail(string id = "") {
            try {
                var linkResult = await (ConfigurationManager.AppSettings["LINK_SPPA_PROFILE_DETAIL"] + "?id=" + id).GetStringAsync();
                return Content(linkResult, "application/json");
            }
            catch (Exception e) {
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e.Message, e);
                return null;
            }
        }

        #endregion
    }
    public class SppaProfileApi {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Website { get; set; }
        public string STDP { get; set; }
        public string User { get; set; }
        public string ImageUrl { get; set; }
    }

    public class SppaProfileApiReplyModel {
        public SppaProfileApiSearchModel SearchCriteria { get; set; }
        public List<SppaProfileApi> Results { get; set; }

        public void Dispose() => this.Dispose();
    }

    public class SppaProfileApiSearchModel {
        public long Id { get; set; }

        public string Keyword { get; set; }
    }



    public class NewsDetailItem
    {
        public int Id { get; set; }
        public string ItemId { get; set; }
        public DateTime PublishedDate { get; set; }
        public string ImageUrl { get; set; }
        public string Locale { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string PathBase { get; set; }
        public string PathFile { get; set; }
        public string Tags { get; set; }
        public bool IsHeadline { get; set; }
        public string Contents { get; set; }
    }
}