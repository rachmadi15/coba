using System;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using Flurl;
using Flurl.Http;
using Newtonsoft.Json;
using Umbraco.Web.Mvc;
using IDXWeb.Models;
using Umbraco.Core.Logging;

namespace IDXWeb.Controllers
{
    public class HelperController : SurfaceController
    {
        public async Task<ActionResult> GetEmiten(string emitenType = "")
        {
            try {
                var link = ConfigurationManager.AppSettings["LINK_EMITEN_LIST"];
                var jsonResult = await link.SetQueryParam("emitenType", emitenType).GetJsonListAsync();
                var serializeObject = JsonConvert.SerializeObject(jsonResult);
                return Content(serializeObject, "application/json");
            } catch (Exception e) {
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e.Message, e);
                return null;
            }
        }

        public async Task<ActionResult> GetIndexChart(string indexCode = "COMPOSITE", string period = "1D")
        {
            try {
                var link = ConfigurationManager.AppSettings["LINK_INDEX_CHART"];
                var rawResultStream = await link.SetQueryParams(new {
                    indexCode,
                    period
                }).GetStreamAsync();

                return File(rawResultStream, "application/json");
            }
            catch (Exception e) {
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e.Message, e);
                return null;
            }
        }

        public async Task<ActionResult> GetIssuer(string id = "")
        {
            try {
                var link = ConfigurationManager.AppSettings["LINK_SECURITIES_BOND_ISSUER"];
                var jsonResult = await link.SetQueryParam("id", id).GetStringAsync();
                return Content(jsonResult, "application/json");
            }
            catch (Exception e) {
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e.Message, e);
                return null;
            }
        }

        public async Task<ActionResult> GetStockChart(string indexCode = "COMPOSITE", string period = "1D")
        {
            try {
                var link = ConfigurationManager.AppSettings["LINK_STOCK_CHART"];
                var rawResultStream = await link.SetQueryParams(new {
                    indexCode,
                    period
                }).GetStreamAsync();

                return File(rawResultStream, "application/json");
            } catch (Exception e) {
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e.Message, e);
                return null;
            }
        }

        public async Task<ActionResult> GetFuturesChart(string period = "1D") {
            try {
                var link = ConfigurationManager.AppSettings["LINK_MIDINDEXFUTURES_CHART"];
                var rawResultStream = await link.SetQueryParams(new { period }).GetStreamAsync();

                return File(rawResultStream, "application/json");
            }
            catch (Exception e) {
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e.Message, e);
                return null;
            }
        }

        public async Task<string> GetMarketTime(string culture = "id-id")
        {
            try {
                var link = ConfigurationManager.AppSettings["LINK_INDEX_MARKETTIME"];
                var jsonResult = await link.SetQueryParam("culture", culture).WithHeader("Content-Type", "application/json").GetStringAsync();

                return jsonResult;
            } catch (Exception e) {
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e.Message, e); 
                return null;
            }
        }

        public async Task<ActionResult> GetMaturityYear()
        {
            try {
                var link = await ConfigurationManager.AppSettings["LINK_SECURITIES_BOND_MATURITY_YEAR"].GetStringAsync();
                return Content(link, "application/json");
            } catch (Exception e) {
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e.Message, e);
                return null; 
            }
        }

        public async Task<ActionResult> GetSectors()
        {
            try {
                var link = await ConfigurationManager.AppSettings["LINK_SECURITIES_SECTOR_LIST"].GetStringAsync();
                return Content(link, "application/json");
            } catch (Exception e) {
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e.Message, e);
                return null;
            }
        }

        public async Task<ActionResult> GetBoards()
        {
            try {
                var link = await ConfigurationManager.AppSettings["LINK_SECURITIES_BOARD_LIST"].GetStringAsync();
                return Content(link, "application/json");
            }
            catch (Exception e) {
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e.Message, e);
                return null;
            }
        }

        public ActionResult GetSearchResult(string keyword = "", int pageNumber = 1, int pageSize = 10)
        {
            try {
                var subscriptionKey = ConfigurationManager.AppSettings["bingSubsKey"];
                var customConfigId = ConfigurationManager.AppSettings["bingCustConfigId"];
                var searchTerm = keyword;

                var offset = pageNumber * pageSize - pageSize;

                var url = ConfigurationManager.AppSettings["bingCustLink"] + "?" +
                          "q=" + searchTerm +
                          "&customconfig=" + customConfigId + "&count=" + pageSize + "&offset=" + offset;

                var client = new HttpClient();
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);
                var httpResponseMessage = client.GetAsync(url).Result;
                var responseContent = httpResponseMessage.Content.ReadAsStringAsync().Result;

                return Content(responseContent, "application/json");
            } catch (Exception e) {
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e.Message, e);
                return null;
            }
           
        }
    }

    public class BingCustomSearchResponse
    {
        public string _type { get; set; }
        public WebPages webPages { get; set; }
    }

    public class WebPages
    {
        public string webSearchUrl { get; set; }
        public int totalEstimatedMatches { get; set; }
        public WebPage[] value { get; set; }
    }

    public class WebPage
    {
        public string name { get; set; }
        public string url { get; set; }
        public string displayUrl { get; set; }
        public string snippet { get; set; }
        public DateTime dateLastCrawled { get; set; }
        public string cachedPageUrl { get; set; }
        public OpenGraphImage openGraphImage { get; set; }
    }

    public class OpenGraphImage
    {
        public string contentUrl { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }
}