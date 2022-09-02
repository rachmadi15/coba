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
    public class ListingActivityController : SurfaceController
    {
        public async Task<ActionResult> GetIpoRelisting(string emitenType = "", string year = "", string status = "", int indexFrom = 0, int pageSize = 0, string firstChar = "")
        {
            try { 
            var link = ConfigurationManager.AppSettings["LINK_LISTING_ACTIVITY_IPO_RELISTING"];
            var jsonResult = await link.SetQueryParams(new
            {
                indexFrom,
                pageSize,
                emitenType,
                year,
                status,
                firstChar
            }).GetStringAsync();

            return Content(jsonResult, "application/json");}
            catch (Exception e) {
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e.Message, e);
                return null;
            }
        }

        public async Task<ActionResult> GetDelisting(string emitenType, string year, string status, int indexFrom = 0, int pageSize = 0, string firstChar = "")
        {
            try {
            var link = ConfigurationManager.AppSettings["LINK_LISTING_ACTIVITY_DELISTING"];
            var jsonResult = await link.SetQueryParams(new
            {
                indexFrom,
                pageSize,
                emitenType,
                year,
                status,
                firstChar
            }).GetStringAsync();
            return Content(jsonResult, "application/json"); }
            catch (Exception e) {
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e.Message, e);
                return null;
            }
        }

        public async Task<ActionResult> GetIssuedHistory(DTParameters param, string kodeEmiten = "", string caType = "", string dateFrom = "", string dateTo = "", string language = "id-id")
        {
            try {
            var link = ConfigurationManager.AppSettings["LINK_LISTING_ACTIVITY_ISSUED_HISTORY"];
            var paramLength = param.Length > 0 ? param.Length : 10;
            var url = link.SetQueryParams(new
            {
                indexFrom = (param.Start + param.Length) / param.Length,
                pageSize = paramLength,
                kodeEmiten,
                caType,
                dateFrom,
                dateTo,
                language
            });
            var jsonResult = await url.GetJsonAsync();

            var dataTable = new DTResult<dynamic>
            {
                data = jsonResult.Replies,
                recordsTotal = (int)jsonResult.ResultCount,
                recordsFiltered = (int)jsonResult.ResultCount
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
    }
}