using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MVCDatatableApp.Models;
using Newtonsoft.Json;
using Umbraco.Core.Logging;
using Umbraco.Web.Mvc;

namespace IDXWeb.Controllers
{
    public class StockIndexController : SurfaceController
    {
        public ActionResult GetStockIndex(DTParameters param, int nodeId, string year, string code)
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
                            if (string.Compare(code, child.GetPropertyValue("stockIndexType"), StringComparison.InvariantCultureIgnoreCase) == 0 &&
                                string.Compare(year, child.GetPropertyValue("stockIndexYear"), StringComparison.InvariantCultureIgnoreCase) == 0)
                            {
                                list.Add(new
                                {
                                    id = child.SortOrder,
                                    year = child.GetPropertyValue("stockIndexYear"),
                                    type = child.GetPropertyValue("stockIndexType"),
                                    description = child.GetPropertyValue("stockIndexDescription"),
                                    size = child.GetPropertyValue("stockIndexSize"),
                                    file = child.GetPropertyValue("stockIndexAttachment").Url,
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

            var recordsTotal = list.Count;

            var paramLength = param.Length > 0 ? param.Length : 10;
            list = list.Skip(param.Start).Take(paramLength).ToList();

            var dataTable = new DTResult<dynamic>
            {
                data = list,
                recordsTotal = recordsTotal,
                recordsFiltered = recordsTotal
            };

            var serializeObject = JsonConvert.SerializeObject(dataTable);
            return Content(serializeObject, "application/json");}
            catch (Exception e) {
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e.Message, e);
                var dataTable = new DTResult<dynamic> {
                    data = new List<dynamic>(),
                    recordsTotal = (int)0,
                    recordsFiltered = (int)0
                };
                var serializeObject = JsonConvert.SerializeObject(dataTable);
                return Content(serializeObject, "application/json");
            }
        }
    }
}