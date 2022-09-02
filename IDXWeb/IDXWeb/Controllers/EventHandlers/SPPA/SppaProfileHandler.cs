using IDXWeb.Controllers.Backoffice;
using IDXWeb.Entities.EtpSppa;
using IDXWeb.Repositories;
using System;
using umbraco.NodeFactory;
using Umbraco.Core;
using Umbraco.Core.Events;
using Umbraco.Core.Models;
using Umbraco.Core.Publishing;
using Umbraco.Core.Services;

namespace IDXWeb.Controllers.EventHandlers {

    public class SppaProfileHandler : ApplicationEventHandler {
        private const string pageAlias = "profileSPPA";

        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext) {
            ContentService.Published += PublishPressRelease;
            ContentService.Trashed += TrashPressRelease;
        }

        private void TrashPressRelease(IContentService sender, MoveEventArgs<IContent> e) {
            foreach (var node in e.MoveInfoCollection) {
                if (node.Entity.ContentType.Alias == pageAlias) {
                    using (var uow = new UnitOfWork()) {
                        uow.SppaProfileRepository.RemoveByUmbracoId(node.Entity.Id);
                        uow.Commit();
                    }
                }
            }
        }

        private void PublishPressRelease(IPublishingStrategy sender, PublishEventArgs<IContent> e) {
            foreach (var node in e.PublishedEntities) {
                if (node.ContentType.Alias == pageAlias) {
                    var rootNode = new Node(int.Parse(node.Path.Split(',')[1]));
                    var sppaProfile = new SppaProfile();
                    sppaProfile.ContentId = node.Id;
                    var img = node.GetValue("sppaProfileImage");
                    if (img != null) {
                        var ct = new ToolsController();
                        Udi udi;
                        string imgUrl = null;
                        if (Udi.TryParse(img.ToString(), out udi)) {
                             imgUrl = ct.GetImageUrlFromUid(udi);
                        }
                        sppaProfile.ImageUrl = imgUrl;
                    }
                    sppaProfile.Code = node.GetValue("sppaProfileCode")?.ToString();
                    sppaProfile.Name = node.GetValue("sppaProfileName")?.ToString();
                    sppaProfile.Address = node.GetValue("sppaProfileAddress")?.ToString();
                    sppaProfile.City = node.GetValue("sppaProfileCity")?.ToString();
                    sppaProfile.PostalCode = node.GetValue("sppaProfilePostalCode")?.ToString();
                    sppaProfile.Phone = node.GetValue("sppaProfilePhone")?.ToString();
                    sppaProfile.Fax = node.GetValue("sppaProfileFax")?.ToString();
                    sppaProfile.Website = node.GetValue("sppaProfileWebsite")?.ToString();
                    sppaProfile.Email = node.GetValue("sppaProfileEmail")?.ToString();
                    sppaProfile.STDP = node.GetValue("sppaProfileSTDP")?.ToString();
                    sppaProfile.User = node.GetValue("sppaProfileUser")?.ToString();
                    sppaProfile.UpdatedDate = DateTime.Now;
                    sppaProfile.CreatedDate = DateTime.Now;

                    using (var uow = new UnitOfWork()) {
                        uow.SppaProfileRepository.Save(sppaProfile);
                        uow.Commit();
                    }
                }
            }
        }
    }
}