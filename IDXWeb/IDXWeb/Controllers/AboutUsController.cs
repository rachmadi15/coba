using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Newtonsoft.Json;
using Umbraco.Core.Logging;
using Umbraco.Web.Mvc;

namespace IDXWeb.Controllers
{
    public class AboutUsController : SurfaceController
    {
        public ActionResult GetAnnualReportItem(int nodeId)
        {
            try {
                var data = Umbraco.Content(nodeId).Children.Where("Visible");
                List<object> list = new List<object>();

                if (data != null) {
                    foreach (var child in data) {
                        try {
                            list.Add(new
                            {
                                title = child.GetPropertyValue("documentTitle"),
                                attachmentSize = child.GetPropertyValue("documentSize"),
                                attachment = child.GetPropertyValue("documentAttachment").Url,
                                thumbnail = child.GetPropertyValue("documentThumbnail").Url,
                            });
                        }
                        catch (Exception ex)
                        {
                            LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, $@"Error in retrieving data
xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
id: {child.Id}
Name: {child.Name}
Message: {ex.Message}
xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
", ex);
                        }
                    }
                }

                var serializeObject = JsonConvert.SerializeObject(list);
                return Content(serializeObject, "application/json");
            } catch (Exception e) {
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e.Message, e);
                return Content("{}");
            }
            
        }
    }
}