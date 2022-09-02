using System;
using System.Collections.Generic;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace IDXWeb.Models
{
    public class BaseContent
    {
        //protected UmbracoHelper helper;
        protected IContent Content;
        protected IContentService service;

        public int Id => Content.Id;
        public string Name => Content.Name;
        public string Icon => Content.ContentType.Icon;
        public string UpdateDateStr => Content.UpdateDate.ToString();
        public Guid Key => Content.Key;
        public GuidUdi Udi { get { return Content.GetUdi(); } }
        public string Uid => System.Text.RegularExpressions.Regex.Replace(Key.ToString(), "-", "");

        public BaseContent(IContent Content)
        {
            //helper = new UmbracoHelper(UmbracoContext.Current);
            service = ApplicationContext.Current.Services.ContentService;
            this.Content = Content;
        }
    }
}