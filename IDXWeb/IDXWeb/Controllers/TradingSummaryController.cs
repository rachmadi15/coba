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

namespace IDXWeb.Controllers {
    public class TradingSummaryController : SurfaceController {

        public async Task<ActionResult> GetIndexSummary(DTParameters param, string date = "") {
            try {
                //return await GetSummary(param, date, "LINK_INDEX_SUMMARY");
                if (string.IsNullOrEmpty(date) || string.IsNullOrWhiteSpace(date) || date.Length < 8)
                    date = DateTime.Today.ToString("yyyyMMdd");

                var paramLength = param.Length > 0 ? param.Length : 10;
                var link = ConfigurationManager.AppSettings["LINK_INDEX_SUMMARY"].SetQueryParams(new {
                    date,
                    indexFrom = (param.Start + paramLength) / paramLength,
                    pageSize = paramLength
                });
                dynamic jsonResult = null;
                try {
                    jsonResult = await link.GetJsonAsync();
                }
                catch (Exception e) {
                }

                var dataTable = new DTResult<dynamic> {
                    data = jsonResult?.Results ?? "",
                    recordsTotal = (int)jsonResult?.ResultCount,
                    recordsFiltered = (int)jsonResult?.ResultCount
                };

                var serializeObject = JsonConvert.SerializeObject(dataTable);
                return Content(serializeObject, "application/json");
            }
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

        public async Task<ActionResult> GetRecapSummary(DTParameters param, string date = "") {
            try {
                //return await GetSummary(param, date, "LINK_RECAP_SUMMARY");
                if (string.IsNullOrEmpty(date) || string.IsNullOrWhiteSpace(date) || date.Length < 8)
                    date = DateTime.Today.ToString("yyyyMMdd");

                var paramLength = param.Length > 0 ? param.Length : 10;
                var link = ConfigurationManager.AppSettings["LINK_RECAP_SUMMARY"].SetQueryParams(new {
                    date,
                    indexFrom = (param.Start + paramLength) / paramLength,
                    pageSize = paramLength
                });

                dynamic jsonResult = null;
                try {
                    jsonResult = await link.GetJsonAsync();
                }
                catch (Exception e) {
                }
                var dataTable = new DTResult<dynamic> {
                    data = jsonResult.Results,
                    recordsTotal = (int)jsonResult.ResultCount,
                    recordsFiltered = (int)jsonResult.ResultCount
                };

                var serializeObject = JsonConvert.SerializeObject(dataTable);
                return Content(serializeObject, "application/json");
            }
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

        public async Task<ActionResult> GetBrokerSummary(DTParameters param, string date = "") {
            try {
                //return await GetSummary(param, date, "LINK_BROKER_SUMMARY");
                if (string.IsNullOrEmpty(date) || string.IsNullOrWhiteSpace(date) || date.Length < 8)
                    date = DateTime.Today.ToString("yyyyMMdd");

                var paramLength = param.Length > 0 ? param.Length : 10;
                var link = ConfigurationManager.AppSettings["LINK_BROKER_SUMMARY"].SetQueryParams(new {
                    date,
                    indexFrom = (param.Start + paramLength) / paramLength,
                    pageSize = param.Length
                });

                dynamic jsonResult = null;
                try {
                    jsonResult = await link.GetJsonAsync();
                }
                catch (Exception e) {
                }
                var dataTable = new DTResult<dynamic> {
                    data = jsonResult.Results,
                    recordsTotal = (int)jsonResult.ResultCount,
                    recordsFiltered = (int)jsonResult.ResultCount
                };

                var serializeObject = JsonConvert.SerializeObject(dataTable);
                return Content(serializeObject, "application/json");
            }
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

        public async Task<ActionResult> GetStockSummary(DTParameters param, string date = "") {
            try {
                //return await GetSummary(param, date, "LINK_STOCK_SUMMARY");
                if (string.IsNullOrEmpty(date) || string.IsNullOrWhiteSpace(date) || date.Length < 8)
                    date = DateTime.Today.ToString("yyyyMMdd");

                var paramLength = param.Length > 0 ? param.Length : 10;
                var link = ConfigurationManager.AppSettings["LINK_STOCK_SUMMARY"].SetQueryParams(new {
                    date,
                    indexFrom = (param.Start + paramLength) / paramLength,
                    pageSize = param.Length
                });

                dynamic jsonResult = null;
                jsonResult = await link.GetJsonAsync();
                var dataTable = new DTResult<dynamic> {
                    data = jsonResult.Results,
                    recordsTotal = (int)jsonResult.ResultCount,
                    recordsFiltered = (int)jsonResult.ResultCount
                };

                var serializeObject = JsonConvert.SerializeObject(dataTable);
                return Content(serializeObject, "application/json");
            }
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


        public async Task<ActionResult> DownloadRecapSummary(string date = "", int pageSize = 1000) {
            try {
                if (string.IsNullOrEmpty(date) || string.IsNullOrWhiteSpace(date) || date.Length < 8)
                    date = DateTime.Today.ToString("yyyyMMdd");

                var link = ConfigurationManager.AppSettings["LINK_RECAP_SUMMARY"].SetQueryParams(new {
                    date,
                    indexFrom = 1,
                    pageSize
                });
                var jsonResult = await link.GetStringAsync();
                return Content(jsonResult, "application/json");
            }
            catch (Exception e) {
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e.Message, e);
                return null;
            }
        }

        public async Task<ActionResult> DownloadStockSummary(string date = "", int pageSize = 1000) {
            try {
                //return await DownloadSummary(date, pageSize, "LINK_STOCK_SUMMARY");
                if (string.IsNullOrEmpty(date) || string.IsNullOrWhiteSpace(date) || date.Length < 8)
                    date = DateTime.Today.ToString("yyyyMMdd");

                var link = ConfigurationManager.AppSettings["LINK_STOCK_SUMMARY"].SetQueryParams(new {
                    date,
                    indexFrom = 1,
                    pageSize
                });
                var jsonResult = await link.GetStringAsync();
                return Content(jsonResult, "application/json");
            }
            catch (Exception e) {
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e.Message, e);
                return null;
            }
        }

        public async Task<ActionResult> DownloadBrokerSummary(string date = "", int pageSize = 1000) {
            try {
                if (string.IsNullOrEmpty(date) || string.IsNullOrWhiteSpace(date) || date.Length < 8)
                    date = DateTime.Today.ToString("yyyyMMdd");

                var link = ConfigurationManager.AppSettings["LINK_BROKER_SUMMARY"].SetQueryParams(new {
                    date,
                    indexFrom = 1,
                    pageSize
                });
                var jsonResult = await link.GetStringAsync();
                return Content(jsonResult, "application/json");
            }
            catch (Exception e) {
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e.Message, e);
                return null;
            }
        }

        public async Task<ActionResult> DownloadIndexSummary(string date = "", int pageSize = 1000) {
            try {
                if (string.IsNullOrEmpty(date) || string.IsNullOrWhiteSpace(date) || date.Length < 8)
                    date = DateTime.Today.ToString("yyyyMMdd");

                var link = ConfigurationManager.AppSettings["LINK_INDEX_SUMMARY"].SetQueryParams(new {
                    date,
                    indexFrom = 1,
                    pageSize
                });
                var jsonResult = await link.GetStringAsync();
                return Content(jsonResult, "application/json");
            }
            catch (Exception e) {
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e.Message, e);
                return null;
            }
        }

        public async Task<ActionResult> GetPedSummary(DTParameters param, string date = "") {
            try {
                if (string.IsNullOrEmpty(date) || string.IsNullOrWhiteSpace(date) || date.Length < 8)
                    date = DateTime.Today.ToString("yyyyMMdd");

                var paramLength = param.Length > 0 ? param.Length : 10;
                var link = ConfigurationManager.AppSettings["LINK_PED_SUMMARY"].SetQueryParams(new {
                    date,
                    indexFrom = (param.Start + paramLength) / paramLength,
                    pageSize = param.Length
                });

                dynamic jsonResult = null;
                try {
                    jsonResult = await link.GetJsonAsync();
                }
                catch (Exception e) {
                }
                var dataTable = new DTResult<dynamic> {
                    data = jsonResult.Results,
                    recordsTotal = (int)jsonResult.ResultCount,
                    recordsFiltered = (int)jsonResult.ResultCount
                };

                var serializeObject = JsonConvert.SerializeObject(dataTable);
                return Content(serializeObject, "application/json");
            }
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

        public async Task<ActionResult> DownloadPedSummary(string date = "", int pageSize = 1000) {
            try {
                if (string.IsNullOrEmpty(date) || string.IsNullOrWhiteSpace(date) || date.Length < 8)
                    date = DateTime.Today.ToString("yyyyMMdd");

                var link = ConfigurationManager.AppSettings["LINK_PED_SUMMARY"].SetQueryParams(new {
                    date,
                    indexFrom = 1,
                    pageSize
                });
                var jsonResult = await link.GetStringAsync();
                return Content(jsonResult, "application/json");
            }
            catch (Exception e) {
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e.Message, e);
                return null;
            }
        }
    }
}