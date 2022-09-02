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
    public class ExchangeMemberController : SurfaceController
    {
        public async Task<ActionResult> GetBroker(DTParameters param)
        {
            try {
                var paramLength = param.Length > 0 ? param.Length : 10;
                var result = await ConfigurationManager.AppSettings["LINK_EMP_BROKER"].SetQueryParams(new {
                    pageNumber = (param.Start + param.Length) / param.Length,
                    pageSize = paramLength
                }).GetJsonAsync();

                var dataTable = new DTResult<dynamic> {
                    data = result.Items,
                    recordsTotal = (int)result.ItemCount,
                    recordsFiltered = (int)result.ItemCount
                };

                var serializeObject = JsonConvert.SerializeObject(dataTable);
                return Content(serializeObject, "application/json");
            } catch (Exception e) {
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

        public async Task<ActionResult> GetBrokerSearch(DTParameters param, string option = "", string name = "", string license = "")
        {
            try {
                var paramLength = param.Length > 0 ? param.Length : 10;
                var result = await ConfigurationManager.AppSettings["LINK_EMP_BROKER_SEARCH"].SetQueryParams(new {
                    pageNumber = (param.Start + param.Length) / param.Length,
                    pageSize = paramLength,
                    option,
                    name,
                    license
                }).GetJsonAsync();

                var dataTable = new DTResult<dynamic> {
                    data = result.Items,
                    recordsTotal = (int)result.ItemCount,
                    recordsFiltered = (int)result.ItemCount
                };

                var serializeObject = JsonConvert.SerializeObject(dataTable);
                return Content(serializeObject, "application/json");
            } catch (Exception e) {
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

        public async Task<ActionResult> GetBrokerFilters(DTParameters param, string firstname = "", string city = "")
        {
            try {
                var paramLength = param.Length > 0 ? param.Length : 10;
                var result = await ConfigurationManager.AppSettings["LINK_EMP_BROKER_FILTER"].SetQueryParams(new {
                    pageNumber = (param.Start + param.Length) / param.Length,
                    pageSize = paramLength,
                    firstname,
                    city
                }).GetJsonAsync();

                var dataTable = new DTResult<dynamic> {
                    data = result.Items,
                    recordsTotal = (int)result.ItemCount,
                    recordsFiltered = (int)result.ItemCount
                };

                var serializeObject = JsonConvert.SerializeObject(dataTable);
                return Content(serializeObject, "application/json");
            } catch (Exception e) {
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

        public async Task<ActionResult> GetBrokerDetail(string code)
        {
            try {
                var result = await (ConfigurationManager.AppSettings["LINK_EMP_BROKER"] + "/" + code).GetStringAsync();
                return Content(result, "application/json");
            } catch (Exception e) {
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e.Message, e);
                return Content("");
            }
        }

        public async Task<ActionResult> GetMkbdSummary(string code)
        {
            try {
                var result = await (ConfigurationManager.AppSettings["LINK_EMP_BROKER"] + "/" + code + "/mkbdsummary").GetStringAsync();
                return Content(result, "application/json");
            } catch (Exception e) {
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e.Message, e);
                return Content("");
            }
        }

        public async Task<ActionResult> GetTransactionSummary(string code)
        {
            try {
                var result = await (ConfigurationManager.AppSettings["LINK_EMP_BROKER"] + "/" + code + "/transactionsummary").GetStringAsync();
                return Content(result, "application/json");
            } catch (Exception e) {
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e.Message, e);
                return Content("");
            }
        }

        public async Task<ActionResult> GetFinancialReport(string code, string period, string year)
        {
            try {
                var result = await (ConfigurationManager.AppSettings["LINK_EMP_BROKER"] + "/" + code + "/financialstatement").SetQueryParams(new {
                    code,
                    period,
                    year
                }).GetStringAsync();

                result = result.Replace(@"\\\\172.18.50.7\\www\\Portals\\0", "").Replace(@"\\\\172.17.50.10", "");
                return Content(result, "application/json");
            } catch (Exception e) {
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e.Message, e);
                return Content("");
            }
        }

        public async Task<ActionResult> GetBranch(DTParameters param, string code)
        {
            try {
                var paramLength = param.Length > 0 ? param.Length : 10;
                var result = await (ConfigurationManager.AppSettings["LINK_EMP_BROKER"] + "/" + code + "/branch").SetQueryParams(new {
                    pageNumber = (param.Start + param.Length) / param.Length,
                    pageSize = paramLength
                }).GetJsonAsync();

                var dataTable = new DTResult<dynamic> {
                    data = result.Items,
                    recordsTotal = (int)result.ItemCount,
                    recordsFiltered = (int)result.ItemCount
                };

                var serializeObject = JsonConvert.SerializeObject(dataTable);
                return Content(serializeObject, "application/json");
            } catch (Exception e) {
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

        public async Task<ActionResult> GetParticipant(DTParameters param)
        {
            try {
                var paramLength = param.Length > 0 ? param.Length : 10;
                var result = await ConfigurationManager.AppSettings["LINK_EMP_PARTICIPANT"].SetQueryParams(new {
                    pageNumber = (param.Start + param.Length) / param.Length,
                    pageSize = paramLength
                }).GetJsonAsync();

                var dataTable = new DTResult<dynamic> {
                    data = result.Items,
                    recordsTotal = (int)result.ItemCount,
                    recordsFiltered = (int)result.ItemCount
                };

                var serializeObject = JsonConvert.SerializeObject(dataTable);
                return Content(serializeObject, "application/json");
            } catch (Exception e) {
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

        public async Task<ActionResult> GetParticipantCodeList(string keyword = "", int start = 1, int pageSize = 10)
        {
            try {
                var result = await ConfigurationManager.AppSettings["LINK_EMP_PARTICIPANT_SEARCH"].SetQueryParams(new {
                    pageNumber = start,
                    pageSize,
                    name = keyword,
                    licenseType = "",
                    firstname = ""
                }).GetJsonAsync();

                var dataTable = new DTResult<dynamic> {
                    data = result.Items,
                    recordsTotal = (int)result.ItemCount,
                    recordsFiltered = (int)result.ItemCount
                };

                var serializeObject = JsonConvert.SerializeObject(dataTable);
                return Content(serializeObject, "application/json");
            } catch (Exception e) {
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

        public async Task<ActionResult> GetParticipantSearch(DTParameters param, string licenseType = "", string name = "", string firstName = "")
        {
            try {
                var paramLength = param.Length > 0 ? param.Length : 10;
                var result = await ConfigurationManager.AppSettings["LINK_EMP_PARTICIPANT_SEARCH"].SetQueryParams(new {
                    pageNumber = (param.Start + param.Length) / param.Length,
                    pageSize = paramLength,
                    licenseType,
                    name,
                    firstName
                }).GetJsonAsync();

                var dataTable = new DTResult<dynamic> {
                    data = result.Items,
                    recordsTotal = (int)result.ItemCount,
                    recordsFiltered = (int)result.ItemCount
                };

                var serializeObject = JsonConvert.SerializeObject(dataTable);
                return Content(serializeObject, "application/json");
            } catch (Exception e) {
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

        public async Task<ActionResult> GetParticipantDetail(string code)
        {
            try {
                var result = await (ConfigurationManager.AppSettings["LINK_EMP_PARTICIPANT"] + "/" + code).GetStringAsync();
                return Content(result, "application/json");
            } catch (Exception e) {
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e.Message, e);
                return Content("");
            }
        }

        public async Task<ActionResult> GetPrimaryDealer(DTParameters param)
        {
            try {
                var paramLength = param.Length > 0 ? param.Length : 10;
                var result = await ConfigurationManager.AppSettings["LINK_EMP_PRIMARY_DEALER"].SetQueryParams(new {
                    pageNumber = (param.Start + param.Length) / param.Length,
                    pageSize = paramLength
                }).GetJsonAsync();

                var dataTable = new DTResult<dynamic> {
                    data = result.Items,
                    recordsTotal = (int)result.ItemCount,
                    recordsFiltered = (int)result.ItemCount
                };

                var serializeObject = JsonConvert.SerializeObject(dataTable);
                return Content(serializeObject, "application/json");
            } catch (Exception e) {
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

        public async Task<ActionResult> GetPrimaryDealerSearch(DTParameters param, string licenseType = "", string name = "", string firstName = "")
        {
            try {
                var paramLength = param.Length > 0 ? param.Length : 10;
                var result = await ConfigurationManager.AppSettings["LINK_EMP_PRIMARY_DEALER_SEARCH"].SetQueryParams(new {
                    pageNumber = (param.Start + param.Length) / param.Length,
                    pageSize = paramLength,
                    licenseType,
                    name,
                    firstName
                }).GetJsonAsync();

                var dataTable = new DTResult<dynamic> {
                    data = result.Items,
                    recordsTotal = (int)result.ItemCount,
                    recordsFiltered = (int)result.ItemCount
                };

                var serializeObject = JsonConvert.SerializeObject(dataTable);
                return Content(serializeObject, "application/json");
            } catch (Exception e) {
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

        public async Task<ActionResult> GetPrimaryDealerDetail(string code)
        {
            try {
                var result = await (ConfigurationManager.AppSettings["LINK_EMP_PRIMARY_DEALER"] + "/" + code).GetStringAsync();
                return Content(result, "application/json");
            } catch (Exception e) {
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e.Message, e);
                return Content("{}");
            }
        }

        public async Task<ActionResult> GetBrokerCodeList(int start= 1, int pageSize = 10, string keyword = "")
        {
            try {
                var url = ConfigurationManager.AppSettings["LINK_EMP_BROKER_SEARCH"].SetQueryParams(new {
                    pageNumber = start,
                    pageSize,
                    option = 0,
                    name = keyword,
                    license = ""
                });
                var result = await url.GetJsonAsync();

                var dataTable = new DTResult<dynamic> {
                    data = result.Items,
                    recordsTotal = (int)result.ItemCount,
                    recordsFiltered = (int)result.ItemCount
                };

                var serializeObject = JsonConvert.SerializeObject(dataTable);
                return Content(serializeObject, "application/json");
            } catch (Exception e) {
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