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
    public class NewsHandler : ApplicationEventHandler
    {
        private const string NewsUploadAlias = "newsDetail";

        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            ContentService.Published += PublishNews;
            ContentService.Trashed += TrashNews;
        }

        private void TrashNews(IContentService sender, MoveEventArgs<IContent> e)
        {
            foreach (var node in e.MoveInfoCollection)
            {
                if (node.Entity.ContentType.Alias == NewsUploadAlias)
                {
                    using (var uow = new UnitOfWork())
                    {
                        uow.NewsRepository.RemoveByUmbracoId(node.Entity.Id.ToString());
                        uow.Commit();
                    }
                }
            }
        }

        private void PublishNews(IPublishingStrategy sender, PublishEventArgs<IContent> e)
        {
            foreach (var node in e.PublishedEntities)
            {
                if (node.ContentType.Alias == NewsUploadAlias)
                {
                    var rootNode = new Node(int.Parse(node.Path.Split(',')[1]));
                    var newsModel = new News();
                    newsModel.ItemID = node.Id.ToString();
                    newsModel.Judul = node.GetValue("newsTitle").ToString();
                    newsModel.Isi = node.GetValue("newsContent").ToString();
                    newsModel.Ringkasan = node.GetValue("newsSummary").ToString();
                    newsModel.Locale = rootNode.Name;
                    var umbracoHelper = new UmbracoHelper(UmbracoContext.Current);
                    var nodeTypedContent = umbracoHelper.TypedContent(node.Id);
                    var imageObect = nodeTypedContent.GetPropertyValue<IPublishedContent>("newsImage");
                    if (imageObect != null)
                    {
                        newsModel.ImageURL = imageObect.Url;
                    }
                    var publishedDate = node.GetValue("newsPublishDate").ToString();

                    if (string.IsNullOrEmpty(publishedDate))
                    {
                        newsModel.TanggalPublish = DateTime.Now;
                    }
                    else
                    {
                        newsModel.TanggalPublish =  DateTime.Parse(publishedDate);
                    }

                    newsModel.IsHeadLine = (int)node.GetValue("newsHeadline") == 1;
                    newsModel.Tags = node.GetValue("newsTag").ToString().Replace("[", string.Empty).Replace("]", string.Empty);

                    using (var uow = new UnitOfWork())
                    {
                        uow.NewsRepository.Save(newsModel);
                        uow.Commit();
                    }
                }
            }
        }
    }
}