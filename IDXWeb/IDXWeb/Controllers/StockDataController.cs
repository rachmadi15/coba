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
    public class StockDataController : SurfaceController
    {
        public async Task<ActionResult> GetConstituent()
        {
            try {
            var link = ConfigurationManager.AppSettings["LINK_INDEX_SUMMARY_CONSTITUENT"];
            var jsonResult = await link.GetStringAsync();
            return Content(jsonResult, "application/json"); }
            catch (Exception e) {
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e.Message, e);
                return null;
            }
        }

        //public async Task<ActionResult> GetSectoral()
        //{
        //    try { 
        //    var link = ConfigurationManager.AppSettings["LINK_INDEX_SUMMARY_SECTORAL"];
        //    var jsonResult = await link.GetStringAsync();
        //    return Content(jsonResult, "application/json");}
        //    catch (Exception e) {
        //        LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e.Message, e);
        //        return null;
        //    }
        //}

        public async Task<ActionResult> GetSecuritiesStock(DTParameters param, string code = "", string sector = "", string board = "")
        {
            try { 
            var paramLength = param.Length > 0 ? param.Length : 10;
            var result = await ConfigurationManager.AppSettings["LINK_SECURITIES_STOCK_SEARCH"].SetQueryParams(new
            {
                pageNumber = (param.Start + param.Length) / param.Length,
                pageSize = paramLength,
                code,
                sector,
                board
            }).GetJsonAsync();

            var dataTable = new DTResult<dynamic>
            {
                data = result.Items,
                recordsTotal = (int)result.ItemCount,
                recordsFiltered = (int)result.ItemCount
            };

            var serializeObject = JsonConvert.SerializeObject(dataTable);
            return Content(serializeObject, "application/json");}
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

        public async Task<ActionResult> GetMarginStock(string year)
        {
            try {
            var link = ConfigurationManager.AppSettings["LINK_INDEX_MARGIN_STOCK"];
            var jsonResult = await link.SetQueryParam("year", year).GetStringAsync();
            return Content(jsonResult, "application/json"); }
            catch (Exception e) {
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e.Message, e);
                return null;
            }
        }

        public async Task<ActionResult> GetPreOpeningStock(string year)
        {
            try { 
            var link = ConfigurationManager.AppSettings["LINK_INDEX_PREOPENING_STOCK"];
            var jsonResult = await link.SetQueryParam("year", year).GetStringAsync();
            return Content(jsonResult, "application/json");}
            catch (Exception e) {
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e.Message, e);
                return null;
            }
        }

        public async Task<ActionResult> GetIndexConstituent(DTParameters param, string code = "", string year = "")
        {
            try {
            var paramLength = param.Length > 0 ? param.Length : 10;
            var link = ConfigurationManager.AppSettings["LINK_INDEX_CONSTITUENT"];
            var jsonResult = await link.SetQueryParams(new
            {
                code,
                year,
                indexFrom = (param.Start + param.Length) / param.Length,
                pageSize = paramLength,
            }).GetJsonAsync();

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

        public async Task<ActionResult> GetIndexGroup(int group = 2)
        {
            try {
            var link = ConfigurationManager.AppSettings["LINK_INDEX_GROUP"] + "/" + group;
            var jsonResult = await link.GetStringAsync();
            return Content(jsonResult, "application/json"); }
            catch (Exception e) {
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e.Message, e);
                return null;
            }
        }

        public ActionResult GetIndexGroupPrevalues()
        {
            try {
            var datatype = Umbraco.DataTypeService.GetDataTypeDefinitionByName("Stock Index Uploader Item - Stock Index Type - Dropdown");
            var preValues = Umbraco.DataTypeService.GetPreValuesByDataTypeId(datatype.Id);

            var items = new List<object>();

            foreach (var item in preValues)
            {
                items.Add(new
                {
                    IndexCode = item
                });
            }

            return Content(JsonConvert.SerializeObject(items), "application/json"); }
            catch (Exception e) {
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e.Message, e);
                return null;
            }
        }

        #region Index IC
        public async Task<ActionResult> GetIndexIC() {
            try {
                var link = ConfigurationManager.AppSettings["LINK_INDEX_SUMMARY_IC"];
                var jsonResult = await link.SetQueryParams(new {
                    indexGroup = 3
                }).GetJsonAsync();

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
                return null;
            }
        }
        #endregion
    }
}