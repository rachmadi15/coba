using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCDatatableApp.Models;
using Newtonsoft.Json;
using umbraco.MacroEngines;
using Umbraco.Core.Logging;
using Umbraco.Web.Mvc;

namespace IDXWeb.Controllers
{
    public class PerformanceSummaryController : SurfaceController
    {
        public ActionResult GetPerformanceSummary(DTParameters param, int nodeId, string code = "")
        {
            try {
            var data = Umbraco.Content(nodeId).Children.Where("Visible");
            List<object> list = new List<object>();

            if (data != null)
            {
                var folderId = Umbraco.Content(nodeId).GetPropertyValue("attachmentFolder");
                var folder = new DynamicMedia(folderId.Id);
                var childrenItems = folder.Children.Items.OrderBy(x => x.Name).ToList();

                foreach (var child in data)
                {
                        try
                        {
                            if (string.IsNullOrEmpty(code) || string.Compare(code, child.GetPropertyValue("companyCode"), StringComparison.InvariantCultureIgnoreCase) == 0)
                            {
                                var fileUrl = childrenItems.FirstOrDefault(x => x.Name.Trim().ToLower() == (child.GetPropertyValue("companyCode") + ".pdf").Trim().ToLower())?.Url;
                                list.Add(new
                                {
                                    id = child.SortOrder,
                                    code = child.GetPropertyValue("companyCode"),
                                    name = child.GetPropertyValue("companyName"),
                                    file = fileUrl,
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

            var pagedList = list.Skip(param.Start).Take(param.Length).ToList();

            var dataTable = new DTResult<dynamic>
            {
                data = pagedList,
                recordsTotal = list.Count,
                recordsFiltered = list.Count
            };

            var serializeObject = JsonConvert.SerializeObject(dataTable);
            return Content(serializeObject, "application/json"); }
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