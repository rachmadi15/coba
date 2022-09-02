using System;
using System.Configuration;
using System.Dynamic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using Flurl;
using Flurl.Http;
using MVCDatatableApp.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Umbraco.Core.Logging;
using Umbraco.Web.Mvc;

namespace IDXWeb.Controllers
{
    public class BondSukukController : SurfaceController
    {
        #region BondIndex

        public async Task<ActionResult> GetCompositeBondIndex()
        {
            var link = ConfigurationManager.AppSettings["LINK_INDOBEX_COMPOSITE"];
            var jsonResult = await link.GetStringAsync();
            return Content(jsonResult, "application/json");
        }

        public async Task<JsonResult> GetGovernmentBondIndex()
        {
            var jsonResult = "";
            try {
                var link = ConfigurationManager.AppSettings["LINK_INDOBEX_GOVERNMENT"];
                jsonResult = await link.GetStringAsync();
            } catch (Exception e) {
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e.Message, e);
            }
            //var serializeObject = JsonConvert.SerializeObject(jsonResult);
            return Json(jsonResult, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetCorporateBondIndex()
        {
            var jsonResult = "";
            try {
                var link = ConfigurationManager.AppSettings["LINK_INDOBEX_CORPORATE"];
                jsonResult = await link.GetStringAsync();
            } catch (Exception e) {
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e.Message, e);
            }
            
            //var serializeObject = JsonConvert.SerializeObject(jsonResult);
            return Json(jsonResult, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region ETP

        #region Old ETP
        public async Task<JsonResult> GetETPTradingActivity()
        {
            var link = ConfigurationManager.AppSettings["LINK_ETP_TRADING_ACTIVITY"];
            var jsonResult = await link.GetJsonAsync();
            var serializeObject = JsonConvert.SerializeObject(jsonResult);
            return Json(serializeObject, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetETPMarketActivity()
        {
            var link = ConfigurationManager.AppSettings["LINK_ETP_MARKET_SUMMARY"];
            var jsonResult = await link.GetJsonAsync();
            var serializeObject = JsonConvert.SerializeObject(jsonResult);
            return Json(serializeObject, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetETPMarketActivityPrev()
        {
            var link = ConfigurationManager.AppSettings["LINK_ETP_MARKET_SUMMARY_PREV"];
            var jsonResult = await link.GetJsonAsync();
            var serializeObject = JsonConvert.SerializeObject(jsonResult);
            return Json(serializeObject, JsonRequestBehavior.AllowGet);
        }

        public async Task<string> GetETPDataTimeCurrent()
        {
            return await ConfigurationManager.AppSettings["LINK_ETP_DATATIME_CURRENT"].GetStringAsync();
        }

        public async Task<string> GetETPDataTimePrev()
        {
            return await ConfigurationManager.AppSettings["LINK_ETP_DATATIME_PREV"].GetStringAsync();
        }

        #endregion

        #region New ETP

        [HttpGet]
        public async Task<ActionResult> GetETPMarketSnapshot(string date = "") 
        {
            var link = ConfigurationManager.AppSettings["LINK_ETP_MARKET_SNAPSHOT"].SetQueryParams(new
            {
                date
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

        [HttpGet]
        public async Task<ActionResult> GetETPTradeActivity(string date = "")
        {
            var link = ConfigurationManager.AppSettings["LINK_ETP_TRADE_ACTIVITY"].SetQueryParams(new
            {
                date
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

        [HttpGet]
        public async Task<ActionResult> GetETPDailySummary(string date = "")
        {
            var link = ConfigurationManager.AppSettings["LINK_ETP_DAILY_SUMMARY"].SetQueryParams(new
            {
                date
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
        [HttpGet]
        public async Task<ActionResult> GetPdQuotation(string instrument = "")
        {
            var link = ConfigurationManager.AppSettings["LINK_PD_QUOTATION"].SetQueryParams(new
            {
                instrument
            });
            var jsonResult = await link.GetJsonAsync();

            var dataTable = new DTResult<dynamic>
            {
                data = jsonResult.Results
            };

            var serializeObject = JsonConvert.SerializeObject(dataTable);
            return Content(serializeObject, "application/json");
        }
        [HttpGet]
        public async Task<ActionResult> GetPdQuotationReference(string instrument = "") {
            var link = await ConfigurationManager.AppSettings["LINK_PD_QUOTATION_REFERENCE"].SetQueryParams(new {
                instrument
            }).GetStringAsync();
            return Content(link, "application/json");
        }

        public async Task<ActionResult> GetInstrumentList(string instrument = "") {
            var link = await ConfigurationManager.AppSettings["LINK_PD_QUOTATION_INSTRUMENT"].GetStringAsync();
            return Content(link, "application/json");
        }

        [HttpGet]
        public async Task<ActionResult> GetETPSecurityList(DTParameters param, string ticker = "")
        {
            var paramLength = param.Length > 0 ? param.Length : 10;
            var link = ConfigurationManager.AppSettings["LINK_ETP_SECURITY_LIST"].SetQueryParams(new
            {
                ticker,
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

        [HttpGet]
        public async Task<ActionResult> GetETPTickerList()
        {
            var jsonResult = await ConfigurationManager.AppSettings["LINK_ETP_TICKER_LIST"].GetStringAsync();
            var serializeObject = jsonResult.Replace(" ", string.Empty).Replace(System.Environment.NewLine, string.Empty);
            return Content(serializeObject, "application/json");
        }

        #endregion

        #endregion

        #region OTC

        public async Task<ActionResult> GetOTCLatestReporting(DTParameters param, bool isCorp, bool isGov)
        {
            dynamic jsonResult;
            if (isCorp)
                jsonResult = await ConfigurationManager.AppSettings["LINK_OTC_LATEST_REPORTING_CORP"].SetQueryParams(new
                {
                    pageNumber = (param.Start + param.Length) / param.Length,
                    pageSize = param.Length
                }).GetJsonAsync();
            else if (isGov)
                jsonResult = await ConfigurationManager.AppSettings["LINK_OTC_LATEST_REPORTING_GOV"].SetQueryParams(new
                {
                    pageNumber = (param.Start + param.Length) / param.Length,
                    pageSize = param.Length
                }).GetJsonAsync();
            else
                jsonResult = await ConfigurationManager.AppSettings["LINK_OTC_LATEST_REPORTING"].SetQueryParams(new
                {
                    pageNumber = (param.Start + param.Length) / param.Length,
                    pageSize = param.Length
                }).GetJsonAsync();

            var dataTable = new DTResult<dynamic>
            {
                data = jsonResult.Items,
                recordsTotal = (int)jsonResult.ItemCount,
                recordsFiltered = (int)jsonResult.ItemCount
            };

            var serializeObject = JsonConvert.SerializeObject(dataTable);
            return Content(serializeObject, "application/json");
        }

        public async Task<ActionResult> GetOTCLatestDataTime()
        {
            var jsonResult = await ConfigurationManager.AppSettings["LINK_OTC_LATEST_DATA_TIME"].GetStringAsync();
            return Content(jsonResult, "application/json");
        }

        public async Task<ActionResult> GetOTCLatestTrade(DTParameters param, bool isCorp, bool isGov)
        {
            dynamic jsonResult;
            if (isCorp)
                jsonResult = await ConfigurationManager.AppSettings["LINK_OTC_LATEST_TRADE_CORP"].SetQueryParams(new
                {
                    pageNumber = (param.Start + param.Length) / param.Length,
                    pageSize = param.Length
                }).GetJsonAsync();
            else if (isGov)
                jsonResult = await ConfigurationManager.AppSettings["LINK_OTC_LATEST_TRADE_GOV"].SetQueryParams(new
                {
                    pageNumber = (param.Start + param.Length) / param.Length,
                    pageSize = param.Length
                }).GetJsonAsync();
            else
                jsonResult = await ConfigurationManager.AppSettings["LINK_OTC_LATEST_TRADE"].SetQueryParams(new
                {
                    pageNumber = (param.Start + param.Length) / param.Length,
                    pageSize = param.Length
                }).GetJsonAsync();

            var records = (int)jsonResult.ItemCount;

            var dataTable = new DTResult<dynamic>
            {
                data = jsonResult.Items,
                recordsTotal = (records == 0 ? param.Length : records),
                recordsFiltered = (records == 0 ? param.Length : records),
            };

            var serializeObject = JsonConvert.SerializeObject(dataTable);
            return Content(serializeObject, "application/json");
        }

        public async Task<ActionResult> GetOTCSummaryReporting(DTParameters param, bool isCorp, bool isGov)
        {
            dynamic jsonResult;
            if (isCorp)
                jsonResult = await ConfigurationManager.AppSettings["LINK_OTC_SUMMARY_REPORTING_CORP"].SetQueryParams(new
                {
                    pageNumber = (param.Start + param.Length) / param.Length,
                    pageSize = param.Length
                }).GetJsonAsync();
            else if (isGov)
                jsonResult = await ConfigurationManager.AppSettings["LINK_OTC_SUMMARY_REPORTING_GOV"].SetQueryParams(new
                {
                    pageNumber = (param.Start + param.Length) / param.Length,
                    pageSize = param.Length
                }).GetJsonAsync();
            else
                jsonResult = await ConfigurationManager.AppSettings["LINK_OTC_SUMMARY_REPORTING"].SetQueryParams(new
                {
                    pageNumber = (param.Start + param.Length) / param.Length,
                    pageSize = param.Length
                }).GetJsonAsync();

            var dataTable = new DTResult<dynamic>
            {
                data = jsonResult.Items,
                recordsTotal = (int)jsonResult.ItemCount,
                recordsFiltered = (int)jsonResult.ItemCount
            };

            var serializeObject = JsonConvert.SerializeObject(dataTable);
            return Content(serializeObject, "application/json");
        }

        public async Task<ActionResult> GetOTCSummaryTrade(DTParameters param, bool isCorp, bool isGov)
        {
            dynamic jsonResult;
            if (isCorp)
                jsonResult = await ConfigurationManager.AppSettings["LINK_OTC_SUMMARY_TRADE_CORP"].SetQueryParams(new
                {
                    pageNumber = (param.Start + param.Length) / param.Length,
                    pageSize = param.Length
                }).GetJsonAsync();
            else if (isGov)
                jsonResult = await ConfigurationManager.AppSettings["LINK_OTC_SUMMARY_TRADE_GOV"].SetQueryParams(new
                {
                    pageNumber = (param.Start + param.Length) / param.Length,
                    pageSize = param.Length
                }).GetJsonAsync();
            else
                jsonResult = await ConfigurationManager.AppSettings["LINK_OTC_SUMMARY_TRADE"].SetQueryParams(new
                {
                    pageNumber = (param.Start + param.Length) / param.Length,
                    pageSize = param.Length
                }).GetJsonAsync();

            var dataTable = new DTResult<dynamic>
            {
                data = jsonResult.Items,
                recordsTotal = (int)jsonResult.ItemCount,
                recordsFiltered = (int)jsonResult.ItemCount
            };

            var serializeObject = JsonConvert.SerializeObject(dataTable);
            return Content(serializeObject, "application/json");
        }

        public async Task<ActionResult> GetOTCMostActiveBonds(DTParameters param, bool isCorp, bool isGov)
        {
            dynamic jsonResult;
            if (isCorp)
                jsonResult = await ConfigurationManager.AppSettings["LINK_OTC_MOST_ACTIVE_BONDS_CORP"].SetQueryParams(new
                {
                    pageNumber = (param.Start + param.Length) / param.Length,
                    pageSize = param.Length
                }).GetJsonAsync();
            else
                jsonResult = await ConfigurationManager.AppSettings["LINK_OTC_MOST_ACTIVE_BONDS_GOV"].SetQueryParams(new
                {
                    pageNumber = (param.Start + param.Length) / param.Length,
                    pageSize = param.Length
                }).GetJsonAsync();

            var dataTable = new DTResult<dynamic>
            {
                data = jsonResult.Items,
                recordsTotal = (int)jsonResult.ItemCount,
                recordsFiltered = (int)jsonResult.ItemCount
            };

            var serializeObject = JsonConvert.SerializeObject(dataTable);
            return Content(serializeObject, "application/json");
        }

        public async Task<ActionResult> GetOTCMostActiveParticipantsCorp(DTParameters param, bool isBank, bool isSecurities, bool isCustodian)
        {
            dynamic jsonResult;
            if (isBank)
                jsonResult = await ConfigurationManager.AppSettings["LINK_OTC_MOST_ACTIVE_PARTICIPANTS_CORP_BANK"].SetQueryParams(new
                {
                    pageNumber = (param.Start + param.Length) / param.Length,
                    pageSize = param.Length
                }).GetJsonAsync();
            else if (isSecurities)
                jsonResult = await ConfigurationManager.AppSettings["LINK_OTC_MOST_ACTIVE_PARTICIPANTS_CORP_SECURITIES"].SetQueryParams(new
                {
                    pageNumber = (param.Start + param.Length) / param.Length,
                    pageSize = param.Length
                }).GetJsonAsync();
            else if (isCustodian)
                jsonResult = await ConfigurationManager.AppSettings["LINK_OTC_MOST_ACTIVE_PARTICIPANTS_CORP_CUSTODIAN"].SetQueryParams(new
                {
                    pageNumber = (param.Start + param.Length) / param.Length,
                    pageSize = param.Length
                }).GetJsonAsync();
            else
                jsonResult = await ConfigurationManager.AppSettings["LINK_OTC_MOST_ACTIVE_PARTICIPANTS_CORP"].SetQueryParams(new
                {
                    pageNumber = (param.Start + param.Length) / param.Length,
                    pageSize = param.Length
                }).GetJsonAsync();

            var dataTable = new DTResult<dynamic>
            {
                data = jsonResult.Items,
                recordsTotal = (int)jsonResult.ItemCount,
                recordsFiltered = (int)jsonResult.ItemCount
            };

            var serializeObject = JsonConvert.SerializeObject(dataTable);
            return Content(serializeObject, "application/json");
        }

        public async Task<ActionResult> GetOTCMostActiveParticipantsGov(DTParameters param, bool isBank, bool isSecurities, bool isCustodian)
        {
            dynamic jsonResult;
            if (isBank)
                jsonResult = await ConfigurationManager.AppSettings["LINK_OTC_MOST_ACTIVE_PARTICIPANTS_GOV_BANK"].SetQueryParams(new
                {
                    pageNumber = (param.Start + param.Length) / param.Length,
                    pageSize = param.Length
                }).GetJsonAsync();
            else if (isSecurities)
                jsonResult = await ConfigurationManager.AppSettings["LINK_OTC_MOST_ACTIVE_PARTICIPANTS_GOV_SECURITIES"].SetQueryParams(new
                {
                    pageNumber = (param.Start + param.Length) / param.Length,
                    pageSize = param.Length
                }).GetJsonAsync();
            else if (isCustodian)
                jsonResult = await ConfigurationManager.AppSettings["LINK_OTC_MOST_ACTIVE_PARTICIPANTS_GOV_CUSTODIAN"].SetQueryParams(new
                {
                    pageNumber = (param.Start + param.Length) / param.Length,
                    pageSize = param.Length
                }).GetJsonAsync();
            else
                jsonResult = await ConfigurationManager.AppSettings["LINK_OTC_MOST_ACTIVE_PARTICIPANTS_GOV"].SetQueryParams(new
                {
                    pageNumber = (param.Start + param.Length) / param.Length,
                    pageSize = param.Length
                }).GetJsonAsync();

            var dataTable = new DTResult<dynamic>
            {
                data = jsonResult.Items,
                recordsTotal = (int)jsonResult.ItemCount,
                recordsFiltered = (int)jsonResult.ItemCount
            };

            var serializeObject = JsonConvert.SerializeObject(dataTable);
            return Content(serializeObject, "application/json");
        }

        public async Task<ActionResult> GetOTCCancelled(DTParameters param)
        {
            var jsonResult = await ConfigurationManager.AppSettings["LINK_OTC_CANCELLED"].SetQueryParams(new
            {
                pageNumber = (param.Start + param.Length) / param.Length,
                pageSize = param.Length
            }).GetJsonAsync();

            var dataTable = new DTResult<dynamic>
            {
                data = jsonResult.Items,
                recordsTotal = (int)jsonResult.ItemCount,
                recordsFiltered = (int)jsonResult.ItemCount
            };

            var serializeObject = JsonConvert.SerializeObject(dataTable);
            return Content(serializeObject, "application/json");
        }

        public async Task<ActionResult> GetCancelled()
        {
            try
            {
                var linkCorpAction = await ConfigurationManager.AppSettings["LINK_CANCELLED"].GetJsonListAsync();
                var serializeObject = JsonConvert.SerializeObject(linkCorpAction);
                return Json(serializeObject, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e.Message, e);
                return null;
            }
        }

        public async Task<JsonResult> GetSummaryStock()
        {
            try
            {
                var linkCorpAction = await ConfigurationManager.AppSettings["LINK_SUMMARY_STOCK"].GetJsonListAsync();
                var serializeObject = JsonConvert.SerializeObject(linkCorpAction);
                return Json(serializeObject, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e.Message, e);
                return null;
            }
        }

        #endregion

        #region BondSearch

        public async Task<ActionResult> GetBondCorpSearch(DTParameters param, string yearMatured = "", string issuerCode = "", string instrumentId = "", string bondId = "", string bondClass = "")
        {
            var jsonResult = await ConfigurationManager.AppSettings["LINK_SECURITIES_BOND_CORP_SEARCH"]
                .SetQueryParams(new
                {
                    yearMatured,
                    issuerCode,
                    instrumentId,
                    bondId,
                    bondClass,
                    pageNumber = (param.Start + param.Length) / param.Length,
                    pageSize = param.Length
                }).GetJsonAsync();

            var dataTable = new DTResult<dynamic>
            {
                data = jsonResult.Items,
                recordsTotal = (int)jsonResult.ItemCount,
                recordsFiltered = (int)jsonResult.ItemCount
            };

            var serializeObject = JsonConvert.SerializeObject(dataTable);
            return Content(serializeObject, "application/json");
        }

        public async Task<ActionResult> GetBondGovSearch(DTParameters param, string yearMatured = "", string issuerCode = "", string bondId = "", string bondCategory = "", string langId = "")
        {
            var jsonResult = await ConfigurationManager.AppSettings["LINK_SECURITIES_BOND_GOV_SEARCH"]
                .SetQueryParams(new
                {
                    yearMatured,
                    issuerCode,
                    bondId,
                    bondCategory,
                    langId,
                    pageNumber = (param.Start + param.Length) / param.Length,
                    pageSize = param.Length
                }).GetJsonAsync();

            var dataTable = new DTResult<dynamic>
            {
                data = jsonResult.Items,
                recordsTotal = (int)jsonResult.ItemCount,
                recordsFiltered = (int)jsonResult.ItemCount
            };

            var serializeObject = JsonConvert.SerializeObject(dataTable);
            return Content(serializeObject, "application/json");
        }

        public async Task<ActionResult> GetGovIssuer(string instrumentType = "", int pageNumber = 1, int pageSize = 25)
        {
            var link = ConfigurationManager.AppSettings["LINK_SECURITIES_BOND_GOV_ISSUER"];
            var jsonResult = await link.SetQueryParams(new
            {
                instrumentType,
                pageSize,
                pageNumber
            }).GetStringAsync();
            return Content(jsonResult, "application/json");
        }

        public async Task<ActionResult> GetGovBondCategory(string langId = "")
        {
            var link = ConfigurationManager.AppSettings["LINK_SECURITIES_BOND_GOV_CATEGORY"] + "/" + langId;
            var jsonResult = await link.GetStringAsync();
            return Content(jsonResult, "application/json");
        }

        #endregion

    }
}