using System;
using System.Collections.Generic;
using System.Configuration;
using System.Dynamic;
using System.Threading.Tasks;
using System.Web.Mvc;
using Flurl;
using Flurl.Http;
using MVCDatatableApp.Models;
using Newtonsoft.Json;
using Umbraco.Core.Logging;
using Umbraco.Web.Mvc;

namespace IDXWeb.Controllers
{
    public class SpecialMonitoringEffectsController : SurfaceController
    {
        public async Task<JsonResult> GetMonitoringEffectsToday()
        {
            //return Json(null, JsonRequestBehavior.AllowGet);
            try
            {
                var jsonResult = await "http://localhost:53343/api/get/SpecialMonitoringEffects/MonitoringEffectsToday".GetJsonListAsync();
                var serializeObject = JsonConvert.SerializeObject(jsonResult);
                return Json(serializeObject, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                string value = ConfigurationManager.AppSettings["LINK_SPECIAL_MONITORING_EFFECTS_TODAY"];
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, value, e);
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e.Message, e);
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<ActionResult> GetMonitoringEffectsHistory(DTParameters param, string startDate = null, string endDate = null, string name=null)
        {
            try
            {
                var paramLength = param.Length > 0 ? param.Length : 10;
                var link = ConfigurationManager.AppSettings["LINK_SPECIAL_MONITORING_EFFECTS_HISTORY"].SetQueryParams(new
                {
                    startDate,
                    endDate,
                    name,
                    indexFrom = (param.Start + paramLength) / paramLength,
                    pageSize = param.Length
                });
                var jsonResult = await link.GetJsonAsync();

                var dataTable = new DTResult<dynamic>
                {
                    data = jsonResult.Results,
                    recordsTotal = (int)jsonResult.ResultCount,
                    recordsFiltered = (int)jsonResult.ResultCount
                };

                var serializeObject = JsonConvert.SerializeObject(dataTable);
                return Content(serializeObject, "application/json");
            }
            catch (Exception e)
            {
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e.Message, e);
                var dataTable = new DTResult<dynamic>
                {
                    data = new List<dynamic>(),
                    recordsTotal = (int)0,
                    recordsFiltered = (int)0
                };
                var serializeObject = JsonConvert.SerializeObject(dataTable);
                return Content(serializeObject, "application/json");
            }
        }

        public async Task<ActionResult> DownloadMonitoringEffectsHistory(DTParameters param, string startDate = null)
        {
            try
            {
                var paramLength = param.Length > 0 ? param.Length : 10;
                var link = ConfigurationManager.AppSettings["LINK_SPECIAL_MONITORING_EFFECTS_HISTORY"].SetQueryParams(new
                {
                    startDate,
                    indexFrom = 1,
                    pageSize = 99999,
                    view="Export"
                });
                var jsonResult = await link.GetStringAsync();
                return Content(jsonResult, "application/json");
            }
            catch (Exception e)
            {
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e.Message, e);
                return null;
            }
        }
    }
}