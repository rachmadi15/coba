using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Newtonsoft.Json;
using Umbraco.Core.Logging;
using Umbraco.Web.Mvc;

namespace IDXWeb.Controllers
{
    public class RegulationController : SurfaceController
    {
        public ActionResult GetRegulationItem(int nodeId)
        {
            try {
            var data = Umbraco.Content(nodeId).Children.Where("Visible");
            List<object> list = new List<object>();

            if (data != null)
            {
                foreach (var child in data)
                {
                        try
                        {
                            list.Add(new
                            {
                                sortOrder = child.SortOrder,
                                numberOfDecree = child.GetPropertyValue("numberOf"),
                                description = child.GetPropertyValue("description"),
                                attachmentSize = child.GetPropertyValue("attachmentSize"),
                                attachment = child.GetPropertyValue("attachment").Url,
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
            return Content(serializeObject, "application/json"); }
            catch (Exception e) {
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e.Message, e);
                return null;
            }
        }
    }
}