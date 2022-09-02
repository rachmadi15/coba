using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Umbraco.Web;
using Umbraco.Web.Mvc;
using IDXWeb.Support;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;
using SystemTask = System.Threading.Tasks.Task;
using System.Net;
using IDXWeb.Models;
using IDXWeb.Repositories;

namespace IDXWeb.Controllers
{
    public class FeedbackController : SurfaceController
    {
        UmbracoHelper helper;

        public FeedbackController()
        {
            helper = new UmbracoHelper(UmbracoContext.Current);
        }

        private IPublishedContent getBaseNode(string locale = "id-id")
        {
            var root = helper.TypedContentAtRoot().FirstOrDefault(o => o.Name == locale);
            if (root == null) return root;

            var node = root.Children.FirstOrDefault(o => o.Name == "User Feedback");
            if (node == null) return root;

            return node;
        }

        public JsonResult GetFeedbackCategories(string locale = "id-id")
        {
            var results = new List<IPublishedContent>();
            var baseNode = getBaseNode(locale);
            if (baseNode != null)
            {
                var list = baseNode.Children.FirstOrDefault(o => o.DocumentTypeAlias == "feedbackCategories");
                if (list != null)
                {
                    results = list.Children.Where(o => o.DocumentTypeAlias == "feedbackCategory").OrderBy(o => o.GetPropertyValue("orderNumber")).ToList();
                }
            }

            return Json(results, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetVisitorTypes(string locale = "id-id")
        {
            var results = new List<IPublishedContent>();
            var baseNode = getBaseNode(locale);
            if (baseNode != null)
            {
                var list = baseNode.Children.FirstOrDefault(o => o.DocumentTypeAlias == "visitorTypes");
                if (list != null)
                {
                    results = list.Children.Where(o => o.DocumentTypeAlias == "visitorType").OrderBy(o => o.GetPropertyValue("orderNumber")).ToList();
                }
            }

            return Json(results, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateReCaptcha]
        public HttpResponseMessage Submit(HttpRequestMessage request, int visitorTypeId, int rating, string description, int[] categoryIds, int baseNodeId = 0, string otherCategory = null)
        {
            if (baseNodeId <= 0)
            {
                throw new EntryPointNotFoundException("Internal Server Error");
            }

            var baseNode = Umbraco.TypedContent(baseNodeId);
            if (baseNode == null)
            {
                throw new EntryPointNotFoundException("Internal Server Error");
            }

            var listNode = baseNode.Children().FirstOrDefault(o => o.DocumentTypeAlias == "feedbackList");
            if (listNode == null)
            {
                throw new EntryPointNotFoundException("Internal Server Error");
            }

            var categories = new List<string>();
            if (categoryIds != null)
            {
                for (int i = 0; i < categoryIds.Length; i++)
                {
                    var category = Umbraco.TypedContent(categoryIds[i]);
                    if (category != null)
                    {
                        categories.Add(Udi.Create(Constants.UdiEntityType.Document, category.GetKey()).ToString());
                    }
                }
            }

            var visitorType = Umbraco.TypedContent(visitorTypeId);
            var visitorTypeUdi = Udi.Create(Constants.UdiEntityType.Document, visitorType.GetKey()).ToString();
            if (visitorTypeUdi == null)
            {
                throw new InvalidOperationException("Internal Server Error");
            }

            if (rating <= 0)
            {
                return request.CreateResponse(HttpStatusCode.OK);
            }

            IContentService service = Services.ContentService;
            var content = service.CreateContent("", listNode.Id, "feedback");
            content.Name = "user_" + DateTime.Now.ToString("yyyyMMddHHmmss");
            content.SetValue("visitorType", visitorTypeUdi);
            content.SetValue("rating", rating);
            content.SetValue("category", String.Join(",", categories));
            content.SetValue("description", description);
            content.SetValue("otherCategory", otherCategory);

            service.Save(content);

            return request.CreateResponse(System.Net.HttpStatusCode.OK);
        }

        [HttpGet]
        public string FormAccess()
        {
            var formAccess = new CustomAntiForgeryRequest(Response);
            formAccess.SetCookie();
            return formAccess.GetToken();
        }
    }
}