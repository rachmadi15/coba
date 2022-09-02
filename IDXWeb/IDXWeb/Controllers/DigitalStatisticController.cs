using System.Configuration;
using System.Threading.Tasks;
using System.Web.Mvc;
using Flurl;
using Flurl.Http;
using MVCDatatableApp.Models;
using Newtonsoft.Json;
using Umbraco.Web.Mvc;
using IDXWeb.Models;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Text;
using System.Net.Http;
using System;
using Codaxy.WkHtmlToPdf;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace IDXWeb.Controllers
{
    public class DigitalStatiticController : SurfaceController
    {
        protected string BaseUrl = ConfigurationManager.AppSettings["DPS_BASE_URL"];
        public async Task<ActionResult> GetDailyIdxIndices()
        {
            var jsonResult = await (BaseUrl + ConfigurationManager.AppSettings["LINK_DPS_DAILY_IDX_INDICES"]).GetStringAsync();
            return Content(jsonResult, "application/json");
        }

        public async Task<ActionResult> GetJCISectoralMovement()
        {
            var jsonResult = await (BaseUrl + ConfigurationManager.AppSettings["LINK_DPS_JCI_SECTORAL_MOVEMENT"]).GetStringAsync();
            return Content(jsonResult, "application/json");
        }

        public async Task<ActionResult> GetTableDailyTradingInvestorForeign()
        {
            var jsonResult = await (BaseUrl + ConfigurationManager.AppSettings["LINK_DPS_TABLE_DAILY_TRADING_INVESTOR_FOREIGN"]).GetStringAsync();
            return Content(jsonResult, "application/json");
        }

        public async Task<ActionResult> GetTotalTradingInvestor()
        {
            var jsonResult = await (BaseUrl + ConfigurationManager.AppSettings["LINK_DPS_TOTAL_TRADING_INVESTOR"]).GetStringAsync();
            return Content(jsonResult, "application/json");
        }

        public async Task<ActionResult> GetDailyTradingValue()
        {
            var jsonResult = await (BaseUrl + ConfigurationManager.AppSettings["LINK_DPS_DAILY_TRADING_VALUE"]).GetStringAsync();
            return Content(jsonResult, "application/json");
        }

        public async Task<ActionResult> GetDailyTradingVolume()
        {
            var jsonResult = await (BaseUrl + ConfigurationManager.AppSettings["LINK_DPS_DAILY_TRADING_VOLUME"]).GetStringAsync();
            return Content(jsonResult, "application/json");
        }

        public async Task<ActionResult> GetDailyTradingFrequency()
        {
            var jsonResult = await (BaseUrl + ConfigurationManager.AppSettings["LINK_DPS_DAILY_TRADING_FREQUENCY"]).GetStringAsync();
            return Content(jsonResult, "application/json");
        }

        public async Task<ActionResult> GetTableDailyTradingInvestorDomestic()
        {
            var jsonResult = await (BaseUrl + ConfigurationManager.AppSettings["LINK_DPS_TABLE_DAILY_TRADING_INVESTOR_DOMESTIC"]).GetStringAsync();
            return Content(jsonResult, "application/json");
        }

        public async Task<ActionResult> GetDailyTradingMarketRegNonReg()
        {
            var jsonResult = await (BaseUrl + ConfigurationManager.AppSettings["LINK_DPS_DAILY_TRADING_MARKET_REGNONREG"]).GetStringAsync();
            return Content(jsonResult, "application/json");
        }

        public async Task<ActionResult> GetDailyTradingMarketNonReg()
        {
            var jsonResult = await (BaseUrl + ConfigurationManager.AppSettings["LINK_DPS_DAILY_TRADING_MARKET_NONREG"]).GetStringAsync();
            return Content(jsonResult, "application/json");
        }

        public async Task<ActionResult> GetApiData(string urlName, bool cumulative = false, string filteredType = "monthly", bool isPrint = false, string query = "")
        {
            var date = DateTime.Now.AddMonths(-1);
            var periodYear = date.Year;
            var periodMonth = 0;
            var periodQuarter = 0;

            DateTime? dateFiltered = date;
            DateTime? startDate = date;
            DateTime? endDate = date;

            //if (filteredType == "monthly") {
            //    periodQuarter = 0;
            //    periodMonth = date.Month;
            //} else if (filteredType == "quarterly" && date.Month <= 3) {
            //    periodYear -= 1;
            //    periodQuarter = 4;
            //} else if (filteredType == "quarterly") {
            //    periodMonth = 0;
            //    periodQuarter = (int)Math.Floor(((decimal)date.Month) / 3);
            //} else if (filteredType == "yearly") {
            //    periodQuarter = 4;
            //    periodMonth = 0;
            //}

            if (!string.IsNullOrWhiteSpace(query))
            {
                byte[] queryBase64 = Convert.FromBase64String(query);
                string queryJson = Encoding.UTF8.GetString(queryBase64);
                var queries = JObject.Parse(queryJson);
                var now = new DateTime();
                if (queries.Value<int>("year") > 0 && (queries.Value<int?>("month") > 0 || queries.Value<int?>("quarter") > 0)) {
                    periodYear = queries.Value<int>("year");
                    periodMonth = queries.Value<int?>("month") ?? periodMonth;
                    periodQuarter = queries.Value<int?>("quarter") ?? periodQuarter;
                }
                filteredType = queries.Value<string>("type");
                if (urlName == "LINK_FINANCIAL_BOND" && filteredType == "monthly")
                {
                    filteredType = "quarterly";
                }
            }
            if (isPrint)
            {
                if (!string.IsNullOrWhiteSpace(Request.Cookies["filteredYear"]?.Value))
                {
                    periodYear = Int32.Parse(Request.Cookies["filteredYear"].Value);
                }
                if (!string.IsNullOrWhiteSpace(Request.Cookies["filteredMonth"]?.Value))
                {
                    periodMonth = Int32.Parse(Request.Cookies["filteredMonth"].Value);
                }
                if (!string.IsNullOrWhiteSpace(Request.Cookies["filteredDate"]?.Value))
                {
                    dateFiltered = Convert.ToDateTime(Request.Cookies["filteredDate"].Value);
                }
                if (!string.IsNullOrWhiteSpace(Request.Cookies["startDate"]?.Value))
                {
                    startDate = Convert.ToDateTime(Request.Cookies["StartDate"].Value);
                    endDate = Convert.ToDateTime(Request.Cookies["endDate"].Value);
                }

                if (!string.IsNullOrWhiteSpace(Request.Cookies["filteredQuarter"]?.Value))
                {
                    periodQuarter = Int32.Parse(Request.Cookies["filteredQuarter"].Value);
                }
            }

            if (!urlName.Contains("LINK_DPS_")) {
                urlName = urlName.Replace("LINK_", "LINK_DPS_");
            }

            var url = (BaseUrl + ConfigurationManager.AppSettings[urlName])
                .SetQueryParams(new {
                    periodMonth,
                    periodQuarter,
                    periodYear,
                    dateFiltered,
                    startDate,
                    endDate,
                    cumulative
                });

            var jsonResult = await url.GetStringAsync();

            return Content(jsonResult, "application/json");
        }



        public async Task<ActionResult> GetApiDataSearch(string urlName, int periodYear = 0, int periodQuarter = 0, int periodMonth = 0, string periodType = "monthly", DateTime? dateFiltered = null, DateTime? startDate = null, DateTime? endDate = null, bool cumulative = false, int id = 0)
        {
            var karakter = urlName.Length;
            if (karakter != 0)
            {
                var a = urlName[karakter - 1];
                if (urlName[karakter - 1].Equals('?'))
                {
                    urlName = urlName.Remove(karakter - 1, 1);
                }
            }
            
            if (periodType == "monthly")
            {
                periodQuarter = 0;
            }
            if (periodType == "quarterly")
            {
                periodMonth = 0;
            }

            if (periodType == "yearly")
            {
                periodQuarter = 4;
                periodMonth = 0;
            }
            if (periodMonth == 0 && periodQuarter == 0 && periodYear == 0)
            {
                var dateNow = DateTime.Now;
                periodMonth = dateNow.Month;
                periodYear = dateNow.Year;
                if (periodType == "yearly")
                {
                    periodQuarter = 4;
                }
                if (periodType == "quarterly")
                {
                    periodQuarter = (int) Math.Floor(((decimal) dateNow.Month + 2) / 3);
                }

            }

            if (!urlName.Contains("LINK_DPS_"))
            {
                urlName = urlName.Replace("LINK_", "LINK_DPS_");
            }

            var url = (BaseUrl + ConfigurationManager.AppSettings[urlName])
                .SetQueryParams(new {
                    id,
                    periodMonth,
                    periodQuarter,
                    periodYear,
                    dateFiltered,
                    startDate,
                    endDate,
                    cumulative
                });

            var jsonResult = await url.GetStringAsync();

            //var jsonResult = await ConfigurationManager.AppSettings[urlName].GetStringAsync();
            return Content(jsonResult, "application/json");
        }

        public async Task<ActionResult> GetReportData(string type, string filecode, string filename, int periodQuarter = 0, string periodType = "monthly", int periodYear = 0, int periodMonth = 0, DateTime? dateFiltered = null, DateTime? startDate = null, DateTime? endDate = null, bool cumulative = false, int id = 0)
        {
            var tipe = "excel";
            if (type == "pdf")
            {
                tipe = "pdf";
            }
            if (type == "oldPdf")
            {
                tipe = "oldPdf";
            }
            if (filename == "bulk")
            {
                tipe = "bulk" + type.ToUpper();
            }

            var periodPrefix = "";
            if (periodType == "monthly")
            {
                periodPrefix = " - " + GetMonthName(periodMonth) + " " + periodYear;
                periodQuarter = 0;
            }
            if (periodType == "quarterly")
            {
                periodPrefix = " - Q" + periodQuarter + " " + periodYear;
                periodMonth = 0;
            }

            if (periodType == "yearly")
            {
                periodPrefix = " - " + periodYear;
                periodQuarter = 4;
                periodMonth = 0;
            }
            if (periodMonth == 0 && periodQuarter == 0 && periodYear == 0)
            {
                var dateNow = DateTime.Now.AddMonths(-1);
                periodMonth = dateNow.Month;
                periodYear = dateNow.Year;
                if (periodType == "yearly")
                {
                    periodQuarter = 4;
                }
                if (periodType == "quarterly")
                {
                    periodQuarter = (int) Math.Floor(((decimal) dateNow.Month + 2) / 3);
                }

            }

            var a = (BaseUrl + ConfigurationManager.AppSettings["LINK_DPS_REPORT"]) + tipe;

            var url = a.SetQueryParams(new
            {
                id,
                filecode,
                filename,
                dateFiltered,
                periodMonth,
                periodQuarter,
                periodYear,
                startDate,
                endDate,
                cumulative
            });

            var result = await url.GetAsync();
            var contentDisposition = result.Content.Headers.GetValues("Content-Disposition");
            var fullname = contentDisposition.FirstOrDefault().Split(new string[] { "; " }, StringSplitOptions.RemoveEmptyEntries).LastOrDefault();
            var extension = "." + fullname.Split('.').Last().Replace("\"","");
            var contentType = result.Content.Headers.GetValues("Content-Type");
            return File(await result.Content.ReadAsByteArrayAsync(), contentType.FirstOrDefault(), filename + periodPrefix + extension);


            // if (filename == "bulk")
            // {
            //     var tanggal = "";
            //
            //     if (periodMonth != 0)
            //     {
            //         var untilString = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(periodMonth) + " " + periodYear;
            //         var fromString = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(periodMonth) + " " + (periodYear - 1).ToString();
            //         tanggal = fromString + " - " + untilString;
            //     }
            //     else if (periodQuarter != 0)
            //     {
            //         tanggal = "Q" + periodQuarter;
            //     }
            //
            //     if (periodYear != 0)
            //     {
            //         tanggal += "_"+periodYear;
            //     }
            //
            //     if (startDate != null)
            //     {
            //         var untilString = endDate?.ToShortDateString();
            //         var fromString = startDate?.ToShortDateString();
            //         tanggal = fromString + " - " + untilString;
            //     }
            //
            //     filename = "Report " + "(" + tanggal + ")";
            //
            //     Dictionary<string, string> contentType = new Dictionary<string, string>();
            //     contentType["excel"] = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            //     contentType["pdf"] = "application/pdf";
            //
            //     Dictionary<string, string> extensions = new Dictionary<string, string>();
            //     extensions["excel"] = ".xlsx";
            //     extensions["pdf"] = ".pdf";
            //
            //     byte[] codesIDBase64 = Convert.FromBase64String(filecode);
            //     string codesIDJson = Encoding.UTF8.GetString(codesIDBase64);
            //     string[] codesIDs = JsonConvert.DeserializeObject<string[]>(codesIDJson);
            //
            //     HttpClient client = new HttpClient();
            //     client.DefaultRequestHeaders.Add("Referer", Request.UrlReferrer?.ToString());
            //     client.Timeout = TimeSpan.FromMinutes(300);
            //
            //     Dictionary<string, string> values = new Dictionary<string, string>();
            //     foreach (var id in codesIDs)
            //     {
            //         if (!string.IsNullOrWhiteSpace(Request.Cookies["reportFileCodes"+id]?.Value))
            //         {
            //             filecode = Request.Cookies["reportFileCodes"+id]?.Value;
            //             values["reportFileCodes" + id] = filecode;
            //         }
            //     }
            //
            //     var param = new FormUrlEncodedContent(values);
            //
            //     var response = await client.PostAsync(url, param);
            //
            //     byte[] responseString = await response.Content.ReadAsByteArrayAsync();
            //     
            //
            //     if (response.IsSuccessStatusCode)
            //     {
            //         HttpContent content = response.Content;
            //
            //         var contentStream = await content.ReadAsStreamAsync();
            //         return File(contentStream, contentType[type.ToLower()], filename+extensions[type.ToLower()]);
            //     }
            //     else
            //     {
            //         throw new FileNotFoundException();
            //     }
            // }
        }

        public async Task<JsonResult> GetApiStringData(string urlName)
        {
            var jsonResult = await ConfigurationManager.AppSettings[urlName].GetStringAsync();
            return Json(jsonResult, JsonRequestBehavior.AllowGet);
        }

        public List<BiggestMarketCapViewModel> GetBiggestMcap()
        {
            List<BiggestMarketCapViewModel> bmcap = new List<BiggestMarketCapViewModel>();
            
            using (var client = new HttpClient())
            {
                var url = (BaseUrl + ConfigurationManager.AppSettings["LINK_DPS_BIGGEST_MARKET_CAP"]);
                //HTTP GET
                var responseTask = client.GetAsync(url);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<List<BiggestMarketCapViewModel>>();
                    readTask.Wait();

                    bmcap = readTask.Result;

                }
            }

            return bmcap;
        }

        public async Task<ActionResult> GetAbsSearch(string yearMatured = "", string bondId = "")
        {
            var jsonResult = await (BaseUrl + ConfigurationManager.AppSettings["LINK_DPS_ABS_SEARCH"]).SetQueryParams(new
            {
                yearMatured,
                bondId,
                indexFrom = 0,
                pageSize = 100
            }).GetStringAsync();

            return Content(jsonResult, "application/json");
        }

        public async Task<ActionResult> GetBiggestMarketCap()
        {
            var jsonResult = await (BaseUrl + ConfigurationManager.AppSettings["LINK_DPS_BIGGEST_MARKET_CAP"]).GetStringAsync();
            return Content(jsonResult, "application/json");
        }

        public async Task<ActionResult> GetAbsSearchTable(DTParameters param, string yearMatured = "", string bondId = "")
        {
            var jsonResult = await (BaseUrl + ConfigurationManager.AppSettings["LINK_DPS_ABS_SEARCH"]).SetQueryParams(new
            {
                yearMatured,
                bondId,
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

        protected string GetMonthName(int month)
        {
            var temp = new DateTime(DateTime.Now.Year, month, 1);
            return temp.ToString("MMM");
        }
    }
}