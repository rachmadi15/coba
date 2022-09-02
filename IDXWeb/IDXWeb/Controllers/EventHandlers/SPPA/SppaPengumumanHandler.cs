using IDXWeb.Controllers.Backoffice;
using IDXWeb.Entities.EtpSppa;
using IDXWeb.Repositories;
using System;
using System.Linq;
using umbraco.NodeFactory;
using Umbraco.Core;
using Umbraco.Core.Events;
using Umbraco.Core.Models;
using Umbraco.Core.Publishing;
using Umbraco.Core.Services;
using Umbraco.Web;

namespace IDXWeb.Controllers.EventHandlers.SPPA {
    public class SppaPengumumanHandler : ApplicationEventHandler {
        private const string pageAlias = "pengumumanSPPA";
        private ApplicationContext _appCtx;
        private UmbracoApplicationBase _umbraco;

        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext) {
            ContentService.Published += Publish;
            ContentService.Trashed += Trash;
            _appCtx = applicationContext;
            _umbraco = umbracoApplication;
        }

        private void Trash(IContentService sender, MoveEventArgs<IContent> e) {
            foreach (var node in e.MoveInfoCollection) {
                if (node.Entity.ContentType.Alias == pageAlias) {
                    using (var uow = new UnitOfWork()) {
                        uow.SppaPengumumanRepository.RemoveByUmbracoId(node.Entity.Id);
                        uow.Commit();
                    }
                }
            }
        }

        private void Publish(IPublishingStrategy sender, PublishEventArgs<IContent> e) {
            foreach (var node in e.PublishedEntities) {
                if (node.ContentType.Alias == pageAlias) {
                    var rootNode = new Node(int.Parse(node.Path.Split(',')[1]));
                    var sppaAnnouncement = new SppaAnnouncement();
                    sppaAnnouncement.ContentId = node.Id;
                    sppaAnnouncement.Judul = node.GetValue("sppaAnnouncementTitle").ToString();
                    sppaAnnouncement.Ringkasan = node.GetValue("sppaAnnouncementSummary").ToString();
                    sppaAnnouncement.TanggalPublish = (DateTime)node.GetValue("sppaAnnouncementPublishDate");
                    var pairId = node.GetValue("sppaAnnouncementContentPair");
                    if (pairId != null) {
                        try {

                            ToolsController ct = new ToolsController();
                            Udi parseUdi;
                            if (Udi.TryParse(pairId.ToString(), out parseUdi)) {
                                var dat = ct.GetContentIdFromUid(parseUdi);
                                sppaAnnouncement.ContentPairId = dat;
                            }
                        }
                        catch (Exception ex) { }
                    }
                    var localeKey = node.GetValue("sppaAnnouncementLocale")?.ToString();
                    if (!string.IsNullOrEmpty(localeKey)) {
                        try {
                            IDataTypeService dtSvc = ApplicationContext.Current.Services.DataTypeService;
                            var def = dtSvc.GetAllDataTypeDefinitions().First(x => "Pengumuman SPPA - Locale - Dropdown".InvariantEquals(x.Name)).Id;
                            var col = dtSvc.GetPreValuesCollectionByDataTypeId(def);
                            PreValue preValue;
                            if (col.IsDictionaryBased) {
                                preValue = col.PreValuesAsDictionary.FirstOrDefault(e1 => e1.Value.Id.ToString() == localeKey).Value;
                            }
                            else {
                                preValue = col.PreValuesAsArray.First(e1 => e1.Id.ToString() == localeKey);
                            }

                            sppaAnnouncement.Locale = preValue.Value;
                        }
                        catch (Exception ex) { }
                    }
                    sppaAnnouncement.Isi = node.GetValue("sppaAnnouncementContent").ToString();
                    sppaAnnouncement.UpdatedDate = DateTime.Now;
                    sppaAnnouncement.CreatedDate = DateTime.Now;

                    using (var uow = new UnitOfWork()) {
                        uow.SppaPengumumanRepository.Save(sppaAnnouncement);
                        uow.Commit();
                    }
                }
            }
        }
    }
}