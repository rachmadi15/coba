using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core;

namespace IDXWeb.Controllers.Backoffice {
    public class ToolsController : BaseApiController{
        public int GetContentIdFromUid(Udi uid) {
            var guid = uid as GuidUdi;
            var dat = Services.ContentService.GetById(guid.Guid);
            return dat.Id;
        }

        public string GetImageUrlFromUid(Udi uid) {
            var guid = uid as GuidUdi;
            var dat = Services.MediaService.GetById(guid.Guid);
            var urlDat = dat.Properties.FirstOrDefault(e => e.Alias == "umbracoFile").Value.ToString();
            if (urlDat.Contains('{') && urlDat.Contains('}')){
                var json = JsonConvert.DeserializeObject<imgProp>(urlDat);
                urlDat = json.src;
            }
            return urlDat;
        }
    }

    public class imgProp {
        public string src { get; set; }
    }
}
