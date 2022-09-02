using Flurl;
using Flurl.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Results;
using System.Web.Mvc;
using System.Web.UI;
using Umbraco.Core;
using Umbraco.Core.Events;
using Umbraco.Core.IO;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Umbraco.Web.UI.Pages;

namespace IDXWeb.Controllers.EventHandlers {
    public class UploadFile {

        public UploadFile(string _file, string _name) {
            file = _file;
            fileName = _name;
        }
        public string file { get; set; }
        public string fileName { get; set; }
    }
    public class SlbHandler : ApplicationEventHandler {
        private const string pageAlias = "slbPmePage";

        //Error Messages
        private string errorMessage = "Pesan error";
        private string missingFileMessage = "File {0} tidak dimasukkan";
        private string diffDateMessage = "File yang dimasukkan memiliki tanggal yang berbeda";
        private string otherMessage = "File yang diupload kurang dari 5 atau tidak menggunakan ekstensi .csv";

        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext) {
            //Listen for when content is being saved
            ContentService.Saving += ContentService_Saving;
        }

        private void ContentService_Saving(IContentService sender, SaveEventArgs<IContent> f) {
            var entity = f.SavedEntities.Where(e => e.ContentType.Alias.InvariantEquals(pageAlias)).FirstOrDefault();
            if (entity != null) {
                var files = entity
                .Properties.Where(e => e.PropertyType.PropertyEditorAlias == "Umbraco.UploadField");
                if (files.Where(e => e.Value != null
                     && !string.IsNullOrWhiteSpace(e.Value.ToString())
                     && !string.IsNullOrEmpty(e.Value.ToString())).Count() > 0) {
                    if (f.Messages.Count > 0) f.Messages.Dispose();
                    if (files.Where(e => e.Value != null
                     && !string.IsNullOrWhiteSpace(e.Value.ToString())
                     && !string.IsNullOrEmpty(e.Value.ToString())
                     && e.Value.ToString().Contains(".csv")).Count() == 5) {

                        if (files.Select(e => e.Value.ToString().Substring(e.Value.ToString().Length - 12, 8)).Distinct().Count() == 1) {

                            if (files.Where(e => e.Value.ToString().Contains("sblharian")).Count() == 1
                                && files.Where(e => e.Value.ToString().Contains("frqpinjamantertinggi")).Count() == 1
                                && files.Where(e => e.Value.ToString().Contains("sahamlendable")).Count() == 1
                                && files.Where(e => e.Value.ToString().Contains("npinjaman")).Count() == 1
                                && files.Where(e => e.Value.ToString().Contains("sahamteraktif")).Count() == 1) {
                                foreach (var file in files) {
                                    if (!string.IsNullOrWhiteSpace(file.Value?.ToString()) && !string.IsNullOrEmpty(file.Value?.ToString())) {
                                        var mediaPath = FileSystemProviderManager.Current.GetFileSystemProvider<MediaFileSystem>();
                                        var fullPath = mediaPath.GetFullPath(mediaPath.GetRelativePath(file.Value.ToString()));
                                        var fileBytes = System.IO.File.ReadAllBytes(fullPath);
                                        var result = SendFile(fileBytes, file.Value.ToString(), "LINK_OUT_SLB");
                                    }
                                }
                                //f.CancelOperation(new EventMessage("Pesan", "File berhasil diupload ke API", EventMessageType.Success));
                            }
                            else {
                                if (files.Where(e => e.Value.ToString().Contains("sblharian")).Count() == 0) {
                                    f.CancelOperation(new EventMessage(errorMessage, string.Format(missingFileMessage, "sblharian"), EventMessageType.Error));

                                }
                                else if (files.Where(e => e.Value.ToString().Contains("frqpinjamantertinggi")).Count() == 0) {
                                    f.CancelOperation(new EventMessage(errorMessage, string.Format(missingFileMessage, "frqpinjamantertinggi"), EventMessageType.Error));

                                }
                                else if (files.Where(e => e.Value.ToString().Contains("sahamlendable")).Count() == 0) {
                                    f.CancelOperation(new EventMessage(errorMessage, string.Format(missingFileMessage, "sahamlendable"), EventMessageType.Error));

                                }
                                else if (files.Where(e => e.Value.ToString().Contains("npinjaman")).Count() == 0) {
                                    f.CancelOperation(new EventMessage(errorMessage, string.Format(missingFileMessage, "npinjaman"), EventMessageType.Error));

                                }
                                else if (files.Where(e => e.Value.ToString().Contains("sahamteraktif")).Count() == 0) {
                                    f.CancelOperation(new EventMessage(errorMessage, string.Format(missingFileMessage, "sahamteraktif"), EventMessageType.Error));

                                }

                            }

                        }
                        else {
                            f.CancelOperation(new EventMessage(errorMessage, diffDateMessage, EventMessageType.Error));
                        }
                    }
                    else {

                        f.CancelOperation(new EventMessage(errorMessage, otherMessage, EventMessageType.Error));

                    }
                }
            }
            
        }

        private string SendFile(byte[] file, string path, string appPath) {
            System.Net.ServicePointManager.Expect100Continue = false;
            var link = ConfigurationManager.AppSettings[appPath];
            if (link != null) {
                var request = (HttpWebRequest)WebRequest.Create(link);
                request.ContentType = "application/json";
                request.Method = "POST";

                using (var writer = new StreamWriter(request.GetRequestStream())) {
                    var values = new UploadFile(Convert.ToBase64String(file), path.Split('/').Last());
                    writer.Write(JsonConvert.SerializeObject(values));
                }

                var response = (HttpWebResponse)request.GetResponse();
                using (var streamReader = new StreamReader(response.GetResponseStream())) {
                    var result = streamReader.ReadToEnd();
                }
                return "200";
            }
            else
                return "404";

        }

    }
}