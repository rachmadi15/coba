using System;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.Mvc;
using Flurl;
using Flurl.Http;
using Newtonsoft.Json;
using Umbraco.Core.Logging;
using Umbraco.Web.Mvc;

namespace IDXWeb.Controllers {
    public class HomeController : SurfaceController {
        public async Task<JsonResult> GetCorporateAction(int resultCount = 0) {
            try {
                var linkCorpAction = await ConfigurationManager.AppSettings["LINK_CORPACTION"].SetQueryParam("resultcount", resultCount).GetJsonListAsync();
                var serializeObject = JsonConvert.SerializeObject(linkCorpAction);
                return Json(serializeObject, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e) {
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e.Message, e);
                return null;
            }
        }

        public async Task<JsonResult> GetRelistingData(int resultCount = 7) {
            try {
                var linkRelisting = await ConfigurationManager.AppSettings["LINK_RELISTING"].SetQueryParam("resultcount", resultCount).GetJsonAsync();
                var serializeObject = JsonConvert.SerializeObject(linkRelisting);
                return Json(serializeObject, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e) {
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e.Message, e);
                return null;
            }
        }

        public async Task<JsonResult> GetUmaData(int resultCount = 7) {
            try {
                var linkUma = await ConfigurationManager.AppSettings["LINK_UMA"].SetQueryParam("resultcount", resultCount).GetJsonAsync();
                var serializeObject = JsonConvert.SerializeObject(linkUma);
                return Json(serializeObject, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e) {
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e.Message, e);
                return null;
            }
        }

        public async Task<JsonResult> GetSuspendData(int resultCount = 7, string language = "id-id") {
            try {
                var linkSuspend = await ConfigurationManager.AppSettings["LINK_SUSPEND"].SetQueryParam("resultcount", resultCount).SetQueryParam("language", language).GetJsonAsync();
                var serializeObject = JsonConvert.SerializeObject(linkSuspend);
                return Json(serializeObject, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e) {
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e.Message, e);
                return null;
            }
        }

        public async Task<JsonResult> GetTopGainer(int resultCount = 7) {
            try {
                var linkTopGainer = await ConfigurationManager.AppSettings["LINK_STOCK_TOPGAINER"]
                    .SetQueryParam("resultcount", resultCount).GetJsonListAsync();
                var serializeObject = JsonConvert.SerializeObject(linkTopGainer);
                return Json(serializeObject, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e) {
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e.Message, e);
                return null;
            }
        }

        public async Task<JsonResult> GetTopFrequent(int resultCount = 7) {
            try {
                var jsonResult = await ConfigurationManager.AppSettings["LINK_STOCK_TOPFREQUENT"]
                    .SetQueryParams(new {
                        resultcount = resultCount
                    }).GetJsonListAsync();
                var serializeObject = JsonConvert.SerializeObject(jsonResult);
                return Json(serializeObject, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e) {
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e.Message, e);
                return null;
            }
        }

        public async Task<JsonResult> GetTopValue(int resultCount = 7) {
            try {
                var jsonResult = await ConfigurationManager.AppSettings["LINK_STOCK_TOPVALUE"]
                    .SetQueryParams(new {
                        resultcount = resultCount
                    }).GetJsonListAsync();
                var serializeObject = JsonConvert.SerializeObject(jsonResult);
                return Json(serializeObject, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e) {
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e.Message, e);
                return null;
            }
        }

        public async Task<JsonResult> GetTopVolume(int resultCount = 7) {
            try {
                var jsonResult = await ConfigurationManager.AppSettings["LINK_STOCK_TOPVOLUME"]
                    .SetQueryParams(new {
                        resultcount = resultCount
                    }).GetJsonListAsync();
                var serializeObject = JsonConvert.SerializeObject(jsonResult);
                return Json(serializeObject, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e) {
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e.Message, e);
                return null;
            }
        }

        public async Task<JsonResult> GetTradeSummary() {
            try {
                var jsonResult = await ConfigurationManager.AppSettings["LINK_TRADE_SUMMARY"].GetJsonListAsync();
                var serializeObject = JsonConvert.SerializeObject(jsonResult);
                return Json(serializeObject, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e) {
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e.Message, e);
                return null;
            }
        }

        public async Task<JsonResult> GetBondSummary() {
            try {
                var jsonResult = await ConfigurationManager.AppSettings["LINK_BOND_SUMMARY"].GetJsonListAsync();
                var serializeObject = JsonConvert.SerializeObject(jsonResult);
                return Json(serializeObject, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e) {
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e.Message, e);
                return null;
            }
        }

        public async Task<ActionResult> GetDerivativeSummary() {
            try {
                var jsonResult = await ConfigurationManager.AppSettings["LINK_DERIVATIVE_SUMMARY"].GetStringAsync();
                return Content(jsonResult, "application/json");
            }
            catch (Exception e) {
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e.Message, e);
                return null;
            }
        }

        public async Task<JsonResult> GetIndexList(string cultureInfo = "id-id") {
            try {
                var jsonResult = await ConfigurationManager.AppSettings["LINK_INDEX_LIST"]
                    .SetQueryParams(new {
                        indexGroup = 0,
                        cultureInfo
                    }).GetJsonListAsync();
                var serializeObject = JsonConvert.SerializeObject(jsonResult);
                return Json(serializeObject, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e) {
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e.Message, e);
                return null;
            }
        }

        public async Task<ActionResult> GetCalendar(string range = "m", string date = "", string language = "id-id") {
            try {
                if (string.IsNullOrEmpty(date) || string.IsNullOrWhiteSpace(date))
                    date = DateTime.Today.ToString("yyyyMMdd");
                var jsonResult = await ConfigurationManager.AppSettings["LINK_CALENDAR"]
                    .SetQueryParams(new {
                        range,
                        date,
                        language
                    }).GetStringAsync();
                jsonResult = jsonResult.Replace("KodeEmiten", "title").Replace("Lokasi", "location").Replace("Deskripsi", "description").Replace("Tanggal", "start");
                return Content(jsonResult, "application/json");
            }
            catch (Exception e) {
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e.Message, e);
                return null;
            }
        }

        public async Task<ActionResult> GetStockInfo(string code = "") {
            try {
                var jsonResult = await ConfigurationManager.AppSettings["LINK_STOCK_INFO"]
                    .SetQueryParams(new {
                        code
                    }).GetJsonAsync();
                var serializeObject = JsonConvert.SerializeObject(jsonResult);
                return Content(serializeObject, "application/json");
            }
            catch (Exception e) {
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e.Message, e);
                return null;
            }
        }

        public string SendSentryErrorCapture() {
            int i2 = 0;
            int i = 10 / i2;
            return "";
        }
    }
}