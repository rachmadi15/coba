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
    public class StatisticalReportController : SurfaceController
    {
        public async Task<string> GetStatisticDaily()
        {
            try {
            var link = ConfigurationManager.AppSettings["LINK_STATISTIC_DAILY"];
            var jsonResult = await link.GetStringAsync();
            return jsonResult; }
            catch (Exception e) {
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e.Message, e);
                return null;
            }
        }

        public async Task<JsonResult> GetStatisticPeriodically(string year, string period)
        {
            try { 
            var link = ConfigurationManager.AppSettings["LINK_STATISTIC"];
            var jsonResult = await link.SetQueryParams(new {
                year,
                period
            }).GetJsonAsync();
            var serializeObject = JsonConvert.SerializeObject(jsonResult);
            return Json(serializeObject, JsonRequestBehavior.AllowGet);}
            catch (Exception e) {
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e.Message, e);
                return null;
            }
        }

        public async Task<ActionResult> GetPerformanceSummary(DTParameters param, string code = "*")
        {
            try {
            var link = ConfigurationManager.AppSettings["LINK_PERFORMANCE_SUMMARY"];
            var url = link.SetQueryParams(new
            {
                code,
                pageNumber = (param.Start + param.Length)/param.Length,
                pageSize = param.Length
            });
            var jsonResult = await url.GetJsonAsync();

            var dataTable = new DTResult<dynamic>
            {
                data = jsonResult.Items,
                recordsTotal = (int)jsonResult.ItemCount,
                recordsFiltered = (int)jsonResult.ItemCount
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

        public async Task<JsonResult> GetLQ45()
        {
            try { 
            var link = ConfigurationManager.AppSettings["LINK_LQ45"];
            var jsonResult = await link.SetQueryParam("year", "").GetJsonAsync();
            var serializeObject = JsonConvert.SerializeObject(jsonResult);
            return Json(serializeObject, JsonRequestBehavior.AllowGet);}
            catch (Exception e) {
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e.Message, e);
                return null;
            }
        }

        public async Task<ActionResult> GetFactBook(int pageNumber = 1, int pageSize = 3)
        {
            try {
            var link = ConfigurationManager.AppSettings["LINK_FACT_BOOK"].SetQueryParams(new
            {
                pageNumber,
                pageSize
            });
            var jsonResult = await link.GetStringAsync();
            return Content(jsonResult, "application/json"); }
            catch (Exception e) {
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e.Message, e);
                return null;
            }
        }

        public async Task<ActionResult> GetBondBook(int pageNumber = 1, int pageSize = 3)
        {
            try { 
            var link = ConfigurationManager.AppSettings["LINK_BOND_BOOK"].SetQueryParams(new
            {
                pageNumber,
                pageSize
            });
            var jsonResult = await link.GetStringAsync();
            return Content(jsonResult, "application/json");}
            catch (Exception e) {
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e.Message, e);
                return null;
            }
        }
    }
}