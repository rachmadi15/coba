using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Newtonsoft.Json;
using Umbraco.Core.Logging;
using Umbraco.Web.Mvc;

namespace IDXWeb.Controllers
{
    public class StatisticController : SurfaceController
    {
        public ActionResult GetStatistic(int nodeId, string type, string year)
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
                            if (string.Compare(type, child.GetPropertyValue("statisticType"), StringComparison.InvariantCultureIgnoreCase) == 0 &&
                                string.Compare(year, child.GetPropertyValue("statisticYear"), StringComparison.InvariantCultureIgnoreCase) == 0)
                            {
                                list.Add(new
                                {
                                    id = child.SortOrder,
                                    year = child.GetPropertyValue("statisticYear"),
                                    type = child.GetPropertyValue("statisticType"),
                                    number = child.GetPropertyValue("statisticNumber"),
                                    description = child.GetPropertyValue("statisticDescription"),
                                    file = child.GetPropertyValue("statisticFile").Url,
                                });
                            }
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