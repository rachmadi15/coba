using System;
using System.Collections.Generic;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace IDXWeb.Models
{
    public class BasePublishedContent
    {
        //protected UmbracoHelper helper;
        protected IContent Content;
        protected IContentService service;

        public int Id => Content.Id;
        public string Name => Content.Name;
        public string Icon => Content.ContentType.Icon;
        public Guid Key => Content.Key;
        public string Uid => System.Text.RegularExpressions.Regex.Replace(Key.ToString(), "-", "");

        public BasePublishedContent() { }
        public BasePublishedContent(IContent publishedContent)
        {
            //helper = new UmbracoHelper(UmbracoContext.Current);
            service = ApplicationContext.Current.Services.ContentService;
            Content = publishedContent;
        }
    }
}