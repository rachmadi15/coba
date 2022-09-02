using Flurl;
using Flurl.Http;
using MVCDatatableApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Umbraco.Core.Logging;
using Umbraco.Web.Mvc;

namespace IDXWeb.Controllers
{
    public class StructuredWarrantController : SurfaceController
    {
        public async Task<ActionResult> GetIssuerList(DTParameters param, string language) {
            try { 

            var paramLength = param.Length > 0 ? param.Length : 10;
            var link = ConfigurationManager.AppSettings["LINK_SW_ISSUER"].SetQueryParams(new
            {
                indexFrom = (param.Start + paramLength) / paramLength,
                pageSize = param.Length,
                language
            });
            var jsonResult = await link.GetJsonAsync();

            var dataTable = new DTResult<dynamic>
            {
                data = jsonResult.Results,
                recordsTotal = (int)jsonResult.ResultCount,
                recordsFiltered = (int)jsonResult.ResultCount
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

        public async Task<ActionResult> GetInformationList(DTParameters param, string language = null, string swtype = null, string underlying = null, string issuer = null)
        {
            try { 
            var paramLength = param.Length > 0 ? param.Length : 10;
            var link = ConfigurationManager.AppSettings["LINK_SW_INFORMATION"].SetQueryParams(new
            {
                indexFrom = (param.Start + paramLength) / paramLength,
                pageSize = param.Length,
                language,
                swtype,
                underlying,
                issuer
            });
            var jsonResult = await link.GetJsonAsync();

            var dataTable = new DTResult<dynamic>
            {
                data = jsonResult.Results,
                recordsTotal = (int)jsonResult.ResultCount,
                recordsFiltered = (int)jsonResult.ResultCount
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

        public async Task<ActionResult> GetTradingList(DTParameters param, 
            string language = null, 
            string datefrom = null, 
            string dateto = null, 
            string sw = null,
            string issuer = null,
            string underlying = null)
        {
            try { 
            var paramLength = param.Length > 0 ? param.Length : 10;
            var link = ConfigurationManager.AppSettings["LINK_SW_TRADING"].SetQueryParams(new
            {
                indexFrom = (param.Start + paramLength) / paramLength,
                pageSize = param.Length,
                language,
                datefrom,
                dateto,
                sw,
                issuer,
                underlying
            });
            var jsonResult = await link.GetJsonAsync();

            var dataTable = new DTResult<dynamic>
            {
                data = jsonResult.Results,
                recordsTotal = (int)jsonResult.ResultCount,
                recordsFiltered = (int)jsonResult.ResultCount
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

        public async Task<ActionResult> GetIssuerDDL()
        {
            try { 
            var jsonResult = await ConfigurationManager.AppSettings["LINK_SW_DDL_ISSUER"].GetStringAsync();
            var serializeObject = jsonResult.Replace(System.Environment.NewLine, string.Empty);
            return Content(serializeObject, "application/json");}
            catch (Exception e) {
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e.Message, e);
                return null;
            }
        }
        public async Task<ActionResult> GetKodeSw()
        {
            try { 
            var jsonResult = await ConfigurationManager.AppSettings["LINK_SW_CODE"].GetStringAsync();
            var serializeObject = jsonResult.Replace(System.Environment.NewLine, string.Empty);
            return Content(serializeObject, "application/json");}
            catch (Exception e) {
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e.Message, e);
                return null;
            }
        }
        public async Task<ActionResult> GetJenisSw()
        {
            try { 
            var jsonResult = await ConfigurationManager.AppSettings["LINK_SW_TYPE"].GetStringAsync();
            var serializeObject = jsonResult.Replace(System.Environment.NewLine, string.Empty);
            return Content(serializeObject, "application/json");}
            catch (Exception e) {
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e.Message, e);
                return null;
            }
        }

        public async Task<ActionResult> GetGraph(DTParameters param,
            string datefrom = null,
            string dateto = null,
            string issuer = null,
            string underlying = null)
        {
            try {
            var paramLength = param.Length > 0 ? param.Length : 10;
            var link = ConfigurationManager.AppSettings["LINK_SW_GRAPH"].SetQueryParams(new
            {
                datefrom,
                dateto,
                issuer,
                underlying
            });
            var stream = await link.GetStreamAsync();

            return File(stream, "application/json"); }
            catch (Exception e) {
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e.Message, e);
                return null;
            }
        }
    }
}