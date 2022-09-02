using Flurl;
using Flurl.Http;
using MVCDatatableApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;
using Umbraco.Core.Logging;
using Umbraco.Web.Mvc;

namespace IDXWeb.Controllers {
    public class EDDController : SurfaceController {
        public static string _etfMarketLink = ConfigurationManager.AppSettings["LINK_EDD_ETF_MARKET"];
        public static string _direMarketLink = ConfigurationManager.AppSettings["LINK_EDD_DIRE_MARKET"];
        public static string _dinfraMarketLink = ConfigurationManager.AppSettings["LINK_EDD_DINFRA_MARKET"];

        public async Task<ActionResult> GetEtfMarket(DTParameters param, List<Dictionary<string, string>> columns, IEnumerable<Dictionary<string, string>> order, string year = "", string search = "") {
            var filters = new Dictionary<string, string>();
            if (year != "") {
                filters["year"] = year;
            }
            param.Search = new DTSearch();
            param.Search.Value = search;
            return await GetMarketData(param, columns, order, _etfMarketLink, filters);
        }

        public async Task<ActionResult> GetDireMarket(DTParameters param, List<Dictionary<string, string>> columns, IEnumerable<Dictionary<string, string>> order, string search = "") {
            var filters = new Dictionary<string, string>();
            param.Search = new DTSearch();
            param.Search.Value = search;
            return await GetMarketData(param, columns, order, _direMarketLink, filters);
        }

        public async Task<ActionResult> GetDinfraMarket(DTParameters param, List<Dictionary<string, string>> columns, IEnumerable<Dictionary<string, string>> order, string search = "") {
            var filters = new Dictionary<string, string>();
            param.Search = new DTSearch();
            param.Search.Value = search;
            return await GetMarketData(param, columns, order, _dinfraMarketLink, filters);
        }

        protected async Task<ActionResult> GetMarketData(DTParameters param, List<Dictionary<string, string>> columns, IEnumerable<Dictionary<string, string>> order, string actionUrl, Dictionary<string, string> filters) {
            dynamic jsonResult;
            try {
                string orderColumn = "TanggalPencatatan";
                if (order != null && order.First() != null && order.First()["column"] != null && order.First()["column"] != "") {
                    orderColumn = columns[int.Parse(order.First()["column"])]["data"];
                }

                var queryParamPairs = new Dictionary<string, string>()
                {
                    { "orderColumn", orderColumn },
                    { "orderDirection", order != null ? order.First()["dir"] : null },
                    { "pageNumber", ((param.Start + param.Length) / param.Length).ToString() },
                    { "pageSize", param.Length.ToString() },
                    { "search", param.Search != null ? param.Search.Value : null }
                };

                // Apply filters
                foreach (var keyValuePair in filters) {
                    queryParamPairs.Add(keyValuePair.Key, keyValuePair.Value);
                }

                Url url = actionUrl.SetQueryParams(queryParamPairs);
                jsonResult = await url.GetJsonAsync();

                var records = (int)jsonResult.ResultCount;

                var dataTable = new DTResult<dynamic> {
                    data = jsonResult.Results,
                    recordsTotal = (records == 0 ? param.Length : records),
                    recordsFiltered = (records == 0 ? param.Length : records),
                };

                var serializeObject = JsonConvert.SerializeObject(dataTable);
                return Content(serializeObject, "application/json");
            }
            catch (Exception e) {
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e.Message, e);

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
