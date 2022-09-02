using System;
using IDXWeb.Entities;
using IDXWeb.Repositories;
using umbraco.NodeFactory;
using Umbraco.Core;
using Umbraco.Core.Events;
using Umbraco.Core.Models;
using Umbraco.Core.Publishing;
using Umbraco.Core.Services;
using Umbraco.Web;

namespace IDXWeb.Controllers.EventHandlers
{
    public class PressReleaseHandler : ApplicationEventHandler
    {
        private const string PressReleaseUploadAlias = "pressReleaseDetail";

        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            ContentService.Published += PublishPressRelease;
            ContentService.Trashed += TrashPressRelease;
        }

        private void TrashPressRelease(IContentService sender, MoveEventArgs<IContent> e)
        {
            foreach (var node in e.MoveInfoCollection)
            {
                if (node.Entity.ContentType.Alias == PressReleaseUploadAlias)
                {
                    using (var uow = new UnitOfWork())
                    {
                        uow.PressReleaseRepository.RemoveByUmbracoId(node.Entity.Id.ToString());
                        uow.Commit();
                    }
                }
            }
        }

        private void PublishPressRelease(IPublishingStrategy sender, PublishEventArgs<IContent> e)
        {
            foreach (var node in e.PublishedEntities)
            {
                if (node.ContentType.Alias == PressReleaseUploadAlias)
                {
                    var rootNode = new Node(int.Parse(node.Path.Split(',')[1]));
                    var pressRelease = new PressRelease();
                    pressRelease.ItemID = node.Id.ToString();
                    pressRelease.Judul = node.GetValue("pressReleaseTitle").ToString();
                    pressRelease.Isi = node.GetValue("pressReleaseContent").ToString();
                    pressRelease.Ringkasan = node.GetValue("pressReleaseSummary").ToString();
                    pressRelease.Locale = rootNode.Name;
                    var umbracoHelper = new UmbracoHelper(UmbracoContext.Current);
                    var nodeTypedContent = umbracoHelper.TypedContent(node.Id);
                    var imageObect = nodeTypedContent.GetPropertyValue<IPublishedContent>("pressReleaseImage");
                    if (imageObect != null)
                    {
                        pressRelease.ImageURL = imageObect.Url;
                    }

                    var publishedDate = node.GetValue("pressReleasePublishedDate").ToString();

                    if (string.IsNullOrEmpty(publishedDate))
                    {
                        pressRelease.TanggalPublish = DateTime.Now;
                    }
                    else
                    {
                        pressRelease.TanggalPublish = DateTime.Parse(publishedDate);
                    }

                    using (var uow = new UnitOfWork())
                    {
                        uow.PressReleaseRepository.Save(pressRelease);
                        uow.Commit();
                    }
                }
            }
        }
    }
}