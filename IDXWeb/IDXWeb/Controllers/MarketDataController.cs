using System;
using System.Collections.Generic;
using System.Configuration;
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
    public class MarketDataController : SurfaceController
    {
        public async Task<ActionResult> GetAbs()
        {
            try {
                var jsonResult = await ConfigurationManager.AppSettings["LINK_ABS"].GetStringAsync();
                return Content(jsonResult, "application/json");
            }
            catch (Exception e) {
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e.Message, e);
                return null;
            }
        }

        public async Task<ActionResult> GetAbsSearch(string yearMatured = "", string bondId = "")
        {
            try {
                var jsonResult = await ConfigurationManager.AppSettings["LINK_ABS_SEARCH"].SetQueryParams(new {
                    yearMatured,
                    bondId,
                    indexFrom = 0,
                    pageSize = 100
                }).GetStringAsync();

                return Content(jsonResult, "application/json");
            }
            catch (Exception e) {
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e.Message, e);
                return null;
            }
        }

        public async Task<ActionResult> GetAbsSearchTable(DTParameters param, string yearMatured = "", string bondId = "")
        {
            try {
                var jsonResult = await ConfigurationManager.AppSettings["LINK_ABS_SEARCH"].SetQueryParams(new {
                    yearMatured,
                    bondId,
                    pageNumber = (param.Start + param.Length) / param.Length,
                    pageSize = param.Length
                }).GetJsonAsync();

                var dataTable = new DTResult<dynamic> {
                    data = jsonResult.Items,
                    recordsTotal = (int)jsonResult.ItemCount,
                    recordsFiltered = (int)jsonResult.ItemCount
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

        public async Task<ActionResult> GetEtf(DTParameters param, string year = "")
        {
            try {
                var url = year == ""
                    ? ConfigurationManager.AppSettings["LINK_ETF"]
                    : ConfigurationManager.AppSettings["LINK_ETF"] + "/" + year;
                var jsonResult = await url.SetQueryParams(new {
                    pageNumber = (param.Start + param.Length) / param.Length,
                    pageSize = param.Length
                }).GetJsonAsync();

                var dataTable = new DTResult<dynamic> {
                    data = jsonResult.Items,
                    recordsTotal = (int)jsonResult.ItemCount,
                    recordsFiltered = (int)jsonResult.ItemCount
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

        public async Task<ActionResult> GetSector()
        {
            try { 
            var jsonResult = await ConfigurationManager.AppSettings["LINK_SECURITIES_SECTOR"].GetStringAsync();
            return Content(jsonResult, "application/json");} catch (Exception e) {
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e.Message, e);
                return null;
            }
        }

        public async Task<ActionResult> GetSubSector()
        {
            try { 
            var jsonResult = await ConfigurationManager.AppSettings["LINK_SECURITIES_SUBSECTOR"].GetStringAsync();
            return Content(jsonResult, "application/json");}
            catch (Exception e) {
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e.Message, e);
                return null;
            }
        }
    }
}