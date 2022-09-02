using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Newtonsoft.Json;
using Umbraco.Core.Logging;
using Umbraco.Web.Mvc;

namespace IDXWeb.Controllers
{
    public class MarginStockController : SurfaceController
    {
        public ActionResult GetMarginStock(int nodeId, string year = "")
        {
            try { 
            var data = Umbraco.Content(nodeId).Children.Where("Visible");
            List<object> list = new List<object>();

            if (data != null)
            {
                foreach (var child in data)
                {
                    if (string.IsNullOrEmpty(year) || string.Compare(year, child.GetPropertyValue("fileYear"), StringComparison.InvariantCultureIgnoreCase) == 0)
                    {
                            try
                            {
                                list.Add(new
                                {
                                    id = child.SortOrder,
                                    year = child.GetPropertyValue("fileYear"),
                                    description = child.GetPropertyValue("fileDescription"),
                                    size = child.GetPropertyValue("fileSize"),
                                    file = child.GetPropertyValue("fileAttachment").Url,
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
            }

            var serializeObject = JsonConvert.SerializeObject(list);
            return Content(serializeObject, "application/json");}
            catch (Exception e) {
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e.Message, e);
                return null;
            }
        }
    }
}