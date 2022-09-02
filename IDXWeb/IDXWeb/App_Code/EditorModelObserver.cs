using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core;
using Umbraco.Web.Editors;
using Umbraco.Web.Models.ContentEditing;

namespace IDXWeb.App_Code
{
    public class EditorModelObserver : ApplicationEventHandler
    {
        private string[] disablePreview = {
            "feedback",
            "feedbackList",
            "feedbackCategory",
            "feedbackCategories",
            "visitorType",
            "visitorTypes"
        };

        private string[] disableCreate = {
            "feedback",
            "feedbackList"
        };

        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            EditorModelEventManager.SendingContentModel += this.SendingContentModel;
            //base.ApplicationStarted(umbracoApplication, applicationContext);
        }

        private void SendingContentModel(System.Web.Http.Filters.HttpActionExecutedContext sender, EditorModelEventArgs<ContentItemDisplay> e)
        {
            var contentItemDisplay = e.Model;
            var context = e.UmbracoContext;
            
            DisablePreview(contentItemDisplay);

            if (contentItemDisplay.ContentTypeAlias == "feedbackList")
            {
                FeedbackStatsTab(contentItemDisplay);
            }
            // this.DisableCreate(contentItemDisplay);
        }

        private void DisablePreview(ContentItemDisplay contentItemDisplay)
        {
            if (Array.IndexOf(this.disablePreview, contentItemDisplay.ContentTypeAlias) > -1)
            {
                contentItemDisplay.AllowPreview = false;
                contentItemDisplay.Urls = new string[0];
            }
        }

        private void FeedbackStatsTab(ContentItemDisplay contentItemDisplay)
        {
            var tab = contentItemDisplay.Tabs.FirstOrDefault(o => o.Alias == "Stats");
            if (tab != null)
            {
                tab.Properties = tab.Properties.Select(o => {
                    o.HideLabel = true;
                    return o;
                });
            };
        }

        //private void DisableCreate(ContentItemDisplay contentItemDisplay)
        //{
        //    if (Array.IndexOf(this.disableCreate, contentItemDisplay.ContentTypeAlias) > -1)
        //    {
        //        contentItemDisplay.AllowedActions = contentItemDisplay.AllowedActions.Where(o => o == "C");
        //    }
        //}
    }
}