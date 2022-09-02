using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Umbraco.Web.WebApi;
using Umbraco.Web;

namespace IDXWeb.Controllers.Backoffice
{
    public class BaseApiController : UmbracoAuthorizedApiController
    {
        protected HttpServerUtility server;
        protected IContentService contentService;
        protected IEnumerable<IPublishedContent> rootContent;
        protected UmbracoHelper helper;
        protected UmbracoContext ct;

        public BaseApiController()
        {
            server = HttpContext.Current.Server;
            contentService = Services.ContentService;
            rootContent = Umbraco.TypedContentAtRoot().Where(o => o.DocumentTypeAlias == "home");
            helper = new UmbracoHelper(UmbracoContext);
            ct = UmbracoContext;
        }
    }
}