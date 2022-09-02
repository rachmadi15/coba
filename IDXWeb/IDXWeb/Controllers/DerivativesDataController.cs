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
    public class DerivativesDataController : SurfaceController
    {
        public async Task<JsonResult> GetFutureToday()
        {
            try {
                var jsonResult = await ConfigurationManager.AppSettings["LINK_MIDINDEXFUTURES_FUTURE_TODAY"].GetJsonListAsync();
                var serializeObject = JsonConvert.SerializeObject(jsonResult);
                return Json(serializeObject, JsonRequestBehavior.AllowGet);
            } catch (Exception e) {
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e.Message, e);
                return Json("");
            }
        }

        public async Task<JsonResult> GetMarketSummary()
        {
            try {
                var jsonResult = await ConfigurationManager.AppSettings["LINK_MIDINDEXFUTURES_MARKET_SUMMARY"].GetJsonListAsync();
                var serializeObject = JsonConvert.SerializeObject(jsonResult);
                return Json(serializeObject, JsonRequestBehavior.AllowGet);
            } catch (Exception e) {
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e.Message, e);
                return Json("");
            }
        }

        public async Task<JsonResult> GetMarketSummaryPrevious()
        {
            try {
                var jsonResult = await ConfigurationManager.AppSettings["LINK_MIDINDEXFUTURES_MARKET_SUMMARY_PREV"].GetJsonListAsync();
                var serializeObject = JsonConvert.SerializeObject(jsonResult);
                return Json(serializeObject, JsonRequestBehavior.AllowGet);
            } catch (Exception e) {
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e.Message, e);
                return Json("");
            }
        }

        public async Task<JsonResult> GetMostActiveBroker()
        {
            try {
                var jsonResult = await ConfigurationManager.AppSettings["LINK_MIDINDEXFUTURES_MOST_ACTIVE_BROKER"].GetJsonListAsync();
                var serializeObject = JsonConvert.SerializeObject(jsonResult);
                return Json(serializeObject, JsonRequestBehavior.AllowGet);
            } catch (Exception e) {
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e.Message, e);
                return Json("");
            }
        }

        public async Task<JsonResult> GetMostActiveContract()
        {
            try {
                var jsonResult = await ConfigurationManager.AppSettings["LINK_MIDINDEXFUTURES_MOST_ACTIVE_CONTRACT"].GetJsonListAsync();
                var serializeObject = JsonConvert.SerializeObject(jsonResult);
                return Json(serializeObject, JsonRequestBehavior.AllowGet);
            } catch (Exception e) {
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e.Message, e);
                return Json("");
            }
        }
        
        public async Task<ActionResult> GetMarketHistory(DTParameters param, string date = "") {
            try {
                var paramLength = param.Length > 0 ? param.Length : 10;
                var link = ConfigurationManager.AppSettings["LINK_MIDINDEXFUTURES_MARKET_HISTORY"].SetQueryParams(new {
                    date,
                    indexFrom = (param.Start + paramLength) / paramLength,
                    pageSize = param.Length
                });
                var jsonResult = await link.GetJsonAsync();

                var dataTable = new DTResult<dynamic> {
                    data = jsonResult.Results,
                    recordsTotal = (int)jsonResult.ResultCount,
                    recordsFiltered = (int)jsonResult.ResultCount
                };

                var serializeObject = JsonConvert.SerializeObject(dataTable);
                return Content(serializeObject, "application/json");
            } catch (Exception e) {
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

        public async Task<ActionResult> GetContractCodeList() {
            try {
                var jsonResult = await ConfigurationManager.AppSettings["LINK_MIDINDEXFUTURES_CONTRACTCODE"].GetStringAsync();
                var serializeObject = jsonResult.Replace(" ", string.Empty).Replace(System.Environment.NewLine, string.Empty);
                return Content(serializeObject, "application/json");
            } catch (Exception e) {
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e.Message, e);
                return Content("");
            }
        }

        public async Task<ActionResult> GetFuturesChart(string period, string contractCode) {
            try {
                var link = ConfigurationManager.AppSettings["LINK_MIDINDEXFUTURES_CHART"];
                var rawResultStream = await link.SetQueryParams(new {
                    period,
                    contractCode
                }).GetStreamAsync();

                return File(rawResultStream, "application/json");
            } catch (Exception e) {
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e.Message, e);
                return Content("");
            }
        }
    }
}