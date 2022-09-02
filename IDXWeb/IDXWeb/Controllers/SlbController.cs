using Flurl;
using Flurl.Http;
using MVCDatatableApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web.Mvc;

namespace IDXWeb.Controllers
{

    public class SlbController : SurfaceController
    {
        // GET: Slb
        public async Task<ActionResult> TopLenderFreq() {
            var link = ConfigurationManager.AppSettings["LINK_SLB_TOP_FREQ"];
            var jsonResult = await link.GetJsonAsync();

            var dataTable = new DTResult<dynamic> {
                data = jsonResult.Results,
                recordsTotal = (int)jsonResult.ResultCount,
                recordsFiltered = (int)jsonResult.ResultCount
            };

            var serializeObject = JsonConvert.SerializeObject(dataTable);
            return Content(serializeObject, "application/json");
        }

        public async Task<ActionResult> TopLenderVal() {
            var link = ConfigurationManager.AppSettings["LINK_SLB_TOP_VALUE"];
            var jsonResult = await link.GetJsonAsync();

            var dataTable = new DTResult<dynamic> {
                data = jsonResult.Results,
                recordsTotal = (int)jsonResult.ResultCount,
                recordsFiltered = (int)jsonResult.ResultCount
            };

            var serializeObject = JsonConvert.SerializeObject(dataTable);
            return Content(serializeObject, "application/json");
        }
        public async Task<ActionResult> TopActiveStock() {
            var link = ConfigurationManager.AppSettings["LINK_SLB_TOP_ACTIVE"];
            var jsonResult = await link.GetJsonAsync();

            var dataTable = new DTResult<dynamic> {
                data = jsonResult.Results,
                recordsTotal = (int)jsonResult.ResultCount,
                recordsFiltered = (int)jsonResult.ResultCount
            };

            var serializeObject = JsonConvert.SerializeObject(dataTable);
            return Content(serializeObject, "application/json");
        }

        public async Task<ActionResult> LendableStock(DTParameters param) {
            var paramLength = param.Length > 0 ? param.Length : 10;
            var link = ConfigurationManager.AppSettings["LINK_SLB_LENDABLE"].SetQueryParams(new {
                indexFrom = (param.Start + paramLength) / paramLength,
                pageSize = param.Length
            });
            var jsonResult = await link.GetJsonAsync();

            var dataTable = new DTResult<dynamic> {
                data = jsonResult.Results,
                recordsTotal = (int)jsonResult.ResultCount,
                recordsFiltered = (int)jsonResult.ResultCount
            };

            var serializeObject = JsonConvert.SerializeObject(dataTable);
            return Content(serializeObject, "application/json");
        }


        public async Task<ActionResult> Daily(DTParameters param) {
            var paramLength = param.Length > 0 ? param.Length : 10;
            var link = ConfigurationManager.AppSettings["LINK_SLB_DAILY"].SetQueryParams(new {
                indexFrom = (param.Start + paramLength) / paramLength,
                pageSize = param.Length
            });
            var jsonResult = await link.GetJsonAsync();

            var dataTable = new DTResult<dynamic> {
                data = jsonResult.Results,
                recordsTotal = (int)jsonResult.ResultCount,
                recordsFiltered = (int)jsonResult.ResultCount
            };

            var serializeObject = JsonConvert.SerializeObject(dataTable);
            return Content(serializeObject, "application/json");
        }

        public async Task<ActionResult> Files(DTParameters param) {
            var paramLength = param.Length > 0 ? param.Length : 10;
            var link = ConfigurationManager.AppSettings["LINK_SLB_FILES"].SetQueryParams(new {
                indexFrom = (param.Start + paramLength) / paramLength,
                pageSize = param.Length
            });
            var jsonResult = await link.GetStringAsync();

            return Content(jsonResult, "application/json");
        }

        public async Task<ActionResult> GetLatestTime() {
            var link = ConfigurationManager.AppSettings["LINK_SLB_LATESTTIME"];
            var jsonResult = await link.GetStringAsync();

            return Content(jsonResult, "application/json");
        }
    }
}