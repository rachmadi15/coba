using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
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
    public class ListedCompanyController : SurfaceController
    {
        public async Task<ActionResult> GetFinancialReport(int indexFrom = 0, int pageSize = 0,
            string dateFrom = "", string dateTo = "",
            string reportType = "", string kodeEmiten = "", string year = "", string periode = "", string language = "id-id")
        {
            try
            {
                var link = ConfigurationManager.AppSettings["LINK_FINANCIAL_REPORT"];
                var jsonResult = await link.SetQueryParams(new
                {
                    indexFrom,
                    pageSize,
                    dateFrom,
                    dateTo,
                    reportType,
                    kodeEmiten,
                    year,
                    periode,
                    language
                }).GetStringAsync();

                return Content(jsonResult, "application/json");
            }
            catch (Exception e)
            {
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e.Message, e);
                return null;
            }
        }

        public async Task<ActionResult> GetCompanyProfiles(DTParameters param, string kodeEmiten = "", string emitenType = "", string language = "id-id")
        {
            try
            {
                var link = ConfigurationManager.AppSettings["LINK_LISTED_COMPANY_PROFILE"].SetQueryParams(new
                {
                    indexFrom = (param.Start + param.Length) / param.Length,
                    pageSize = param.Length,
                    kodeEmiten,
                    emitenType,
                    language
                });
                var jsonResult = await link.GetJsonAsync();

                var dataTable = new DTResult<dynamic>
                {
                    data = jsonResult.Profiles,
                    recordsTotal = (int)jsonResult.ResultCount,
                    recordsFiltered = (int)jsonResult.ResultCount
                };

                var serializeObject = JsonConvert.SerializeObject(dataTable);
                return Content(serializeObject, "application/json");
            }
            catch (Exception e)
            {
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e.Message, e);
                var dataTable = new DTResult<dynamic>
                {
                    data = new List<dynamic>(),
                    recordsTotal = (int)0,
                    recordsFiltered = (int)0
                };
                var serializeObject = JsonConvert.SerializeObject(dataTable);
                return Content(serializeObject, "application/json");
            }
        }

        public async Task<ActionResult> GetCompanyProfilesDetail(string kodeEmiten = "", string emitenType = "", string language = "id-id")
        {
            try
            {
                var link = ConfigurationManager.AppSettings["LINK_LISTED_COMPANY_PROFILE"];
                var jsonResult = await link.SetQueryParams(new
                {
                    indexFrom = 1,
                    pageSize = 1,
                    kodeEmiten,
                    emitenType,
                    language
                }).GetJsonAsync();

                var serializeObject = JsonConvert.SerializeObject(jsonResult);
                return Content(serializeObject, "application/json");
            }
            catch (Exception e)
            {
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e.Message, e);
                return null;
            }
        }

        public async Task<ActionResult> GetProfileAnnouncement(string dateFrom = "", string dateTo = "", string keyword = "", string kodeEmiten = "", string emitenType = "", int pageSize = 0, int indexFrom = 0, string language = "id-id")
        {
            try
            {
                var link = ConfigurationManager.AppSettings["LINK_ANNOUNCEMENT_PROFILE"];
                var jsonResult = await link.SetQueryParams(new
                {
                    indexFrom,
                    pageSize,
                    kodeEmiten,
                    emitenType,
                    dateFrom,
                    dateTo,
                    query = keyword,
                    language
                }).GetJsonAsync();

                var serializeObject = JsonConvert.SerializeObject(jsonResult);
                return Content(serializeObject, "application/json");
            }
            catch (Exception e)
            {
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e.Message, e);
                return null;
            }
        }

        public async Task<ActionResult> GetCompanyProfilesFinance(string kodeEmiten = "", string language = "id-id")
        {
            try
            {
                var link = ConfigurationManager.AppSettings["LINK_LISTED_COMPANY_FINANCE"];
                var jsonResult = await link.SetQueryParams(new
                {
                    kodeEmiten,
                    language
                }).GetJsonAsync();

                var serializeObject = JsonConvert.SerializeObject(jsonResult);
                return Content(serializeObject, "application/json");
            }
            catch (Exception e)
            {
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e.Message, e);
                return null;
            }
        }

        public async Task<ActionResult> GetTradingInfoSS(DTParameters param, string code = "", string language = "id-id")
        {
            try
            {
                var link = ConfigurationManager.AppSettings["LINK_LISTED_COMPANY_TRADING_INFO_SS"];
                var jsonResult = await link.SetQueryParams(new
                {
                    code,
                    pageSize = param.Length,
                    language
                }).GetJsonAsync();

                var serializeObject = JsonConvert.SerializeObject(jsonResult);
                return Content(serializeObject, "application/json");
            }
            catch (Exception e)
            {
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e.Message, e);
                return null;
            }
        }

        public async Task<ActionResult> GetTradingInfoDaily(string code = "", string language = "id-id")
        {
            try
            {
                var link = ConfigurationManager.AppSettings["LINK_LISTED_COMPANY_TRADING_INFO_DAILY"];
                var jsonResult = await link.SetQueryParams(new
                {
                    code,
                    language
                }).GetJsonAsync();

                var serializeObject = JsonConvert.SerializeObject(jsonResult);
                return Content(serializeObject, "application/json");
            }
            catch (Exception e)
            {
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e.Message, e);
                return null;
            }
        }

        public async Task<ActionResult> GetAnnouncement(int indexFrom = 0, int pageSize = 0, string keyword = "", string emitenType = "", string dateFrom = "", string dateTo = "", string kodeEmiten = "", string language = "id-id")
        {
            try
            {
                var jsonResult = await ConfigurationManager.AppSettings["LINK_LISTED_COMPANY_ANNOUNCEMENT"]
                    .SetQueryParams(new
                    {
                        indexFrom,
                        pageSize,
                        query = keyword,
                        emitenType,
                        dateFrom,
                        dateTo,
                        kodeEmiten,
                        language
                    })
                    .GetStringAsync();
                return Content(jsonResult, "application/json");
            }
            catch (Exception e)
            {
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e.Message, e);
                return null;
            }
        }

        public ActionResult GetProspectusItem(DTParameters param, int nodeId, string year = "")
        {
            try
            {
                var data = Umbraco.Content(nodeId).Children.Where("Visible");
                List<ProspectusItem> list = new List<ProspectusItem>();

                if (data != null)
                {
                    foreach (var child in data)
                    {
                        try
                        {
                            list.Add(new ProspectusItem
                            {
                                Description = child.GetPropertyValue("prospectusDescription"),
                                ListingDate = child.GetPropertyValue("prospectusListingDate"),
                                Prospectus = child.GetPropertyValue("prospectusAttachment").Url,
                            });
                        }
                        catch (Exception ex)
                        {
                            LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, $@"Error in retrieving data
xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
id: {child.Id}
Name: {child.Name}
Message: {ex.Message}
xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
", ex);
                        }
                    }
                }

                var filteredList = list;

                if (year != string.Empty)
                {
                    filteredList = list.Where(x => x.ListingDate.Year == int.Parse(year)).ToList();
                }

                var pagedList = filteredList.Skip(param.Start).Take(param.Length).ToList();

                var dataTable = new DTResult<ProspectusItem>
                {
                    data = pagedList,
                    recordsTotal = list.Count,
                    recordsFiltered = filteredList.Count
                };

                var serializeObject = JsonConvert.SerializeObject(dataTable);
                return Content(serializeObject, "application/json");
            }
            catch (Exception e)
            {
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e.Message, e);
                var dataTable = new DTResult<dynamic>
                {
                    data = new List<dynamic>(),
                    recordsTotal = (int)0,
                    recordsFiltered = (int)0
                };
                var serializeObject = JsonConvert.SerializeObject(dataTable);
                return Content(serializeObject, "application/json");
            }
        }

        public async Task<ActionResult> GetSpecialNotation(DTParameters param, string language = "id-id")
        {
            try
            {
                var paramLength = param.Length > 0 ? param.Length : 10;
                var link = ConfigurationManager.AppSettings["LINK_LISTED_COMPANY_SPECIAL_NOTATION"].SetQueryParams(new
                {
                    indexFrom = (param.Start + paramLength) / paramLength,
                    pageSize = param.Length,
                    language = language
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
            catch (Exception e)
            {
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e.Message, e);
                var dataTable = new DTResult<dynamic>
                {
                    data = new List<dynamic>(),
                    recordsTotal = (int)0,
                    recordsFiltered = (int)0
                };
                var serializeObject = JsonConvert.SerializeObject(dataTable);
                return Content(serializeObject, "application/json");
            }
        }

        public async Task<ActionResult> GetSpecialNotationCms(DTParameters param, string language = "id-id")
        {
            try
            {
                var paramLength = param.Length > 0 ? param.Length : 10;
                var link = (ConfigurationManager.AppSettings["LINK_LISTED_COMPANY_SPECIAL_NOTATION"] + "/Cms").SetQueryParams(new
                {
                    indexFrom = (param.Start + paramLength) / paramLength,
                    pageSize = param.Length,
                    language = language
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
            catch (Exception e)
            {
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e.Message, e);
                var dataTable = new DTResult<dynamic>
                {
                    data = new List<dynamic>(),
                    recordsTotal = (int)0,
                    recordsFiltered = (int)0
                };
                var serializeObject = JsonConvert.SerializeObject(dataTable);
                return Content(serializeObject, "application/json");
            }
        }

        public async Task<ActionResult> GetNotationLatestDate(DTParameters param)
        {
            try
            {
                var paramLength = param.Length > 0 ? param.Length : 10;

                var link = ConfigurationManager.AppSettings["LINK_LISTED_COMPANY_SPECIAL_NOTATION_DATE"];
                var jsonResult = await link.GetJsonAsync();

                var serializeObject = JsonConvert.SerializeObject(jsonResult);
                return Content(serializeObject, "application/json");
            }
            catch (Exception e)
            {
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e.Message, e);
                return null;
            }
        }

        public async Task<ActionResult> GetPerubahanNamaHistory(string kodeEmiten, string Year)
        {
            try
            {
                var link = ConfigurationManager.AppSettings["Link_ListedCompany_NameChanges_Dwh"].SetQueryParams(new
                {
                    KodeEmiten = kodeEmiten,
                    Year
                });
                var jsonResult = await link.GetJsonAsync();

                var serializeObject = JsonConvert.SerializeObject(jsonResult);
                return Content(serializeObject, "application/json");
            }
            catch (Exception e)
            {
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e.Message, e);
                return null;
            }
        }

        public async Task<ActionResult> GetCoreBusinessHistory(string kodeEmiten, string Year)
        {
            try
            {
                var link = ConfigurationManager.AppSettings["Link_ListedCompany_CoreBusiness_Dwh"].SetQueryParams(new
                {
                    KodeEmiten = kodeEmiten,
                    Year
                });
                var jsonResult = await link.GetJsonAsync();

                var serializeObject = JsonConvert.SerializeObject(jsonResult);
                return Content(serializeObject, "application/json");
            }
            catch (Exception e)
            {
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e.Message, e);
                return null;
            }
        }

        public async Task<ActionResult> GetDividenHistory(DTParameters param)
        {
            try
            {
                var paramLength = param.Length > 0 ? param.Length : 10;

                var link = ConfigurationManager.AppSettings["Link_ListedCompany_Dividen_Dwh"];
                var jsonResult = await link.GetJsonAsync();

                var serializeObject = JsonConvert.SerializeObject(jsonResult);
                return Content(serializeObject, "application/json");
            }
            catch (Exception e)
            {
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e.Message, e);
                return null;
            }
        }
        public async Task<ActionResult> GetPemegangSahamHistory(string kodeEmiten, string Year)
        {
            try
            {
                var link = ConfigurationManager.AppSettings["Link_ListedCompany_StockHolder_Dwh"].SetQueryParams(new
                {
                    KodeEmiten = kodeEmiten,
                    Year
                }); ;
                var jsonResult = await link.GetJsonAsync();

                var serializeObject = JsonConvert.SerializeObject(jsonResult);
                return Content(serializeObject, "application/json");
            }
            catch (Exception e)
            {
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e.Message, e);
                return null;
            }
        }

        public async Task<ActionResult> GetPemegangSahamPengendaliHistory(string kodeEmiten, string Year)
        {
            try
            {
                var link = ConfigurationManager.AppSettings["Link_ListedCompany_StockHolderPengendali_Dwh"].SetQueryParams(new
                {
                    KodeEmiten = kodeEmiten,
                    Year
                }); ;
                var jsonResult = await link.GetJsonAsync();

                var serializeObject = JsonConvert.SerializeObject(jsonResult);
                return Content(serializeObject, "application/json");
            }
            catch (Exception e)
            {
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e.Message, e);
                return null;
            }
        }

        public async Task<ActionResult> GetDividenSaham(string kodeEmiten, string Year)
        {
            try
            {
                var link = ConfigurationManager.AppSettings["Link_ListedCompany_DividenSaham_Dwh"].SetQueryParams(new
                {
                    KodeEmiten = kodeEmiten,
                    Year
                }); ;
                var jsonResult = await link.GetJsonAsync();

                var serializeObject = JsonConvert.SerializeObject(jsonResult);
                return Content(serializeObject, "application/json");
            }
            catch (Exception e)
            {
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e.Message, e);
                return null;
            }
        }
        public async Task<ActionResult> GetDividenTunai(string kodeEmiten, string Year)
        {
            try
            {
                var link = ConfigurationManager.AppSettings["Link_ListedCompany_DividenTunai_Dwh"].SetQueryParams(new
                {
                    KodeEmiten = kodeEmiten,
                    Year
                }); ;
                var jsonResult = await link.GetJsonAsync();

                var serializeObject = JsonConvert.SerializeObject(jsonResult);
                return Content(serializeObject, "application/json");
            }
            catch (Exception e)
            {
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e.Message, e);
                return null;
            }
        }

        public async Task<ActionResult> GetESGInformation(string kodeEmiten = "", int year = 0)
        {
            try
            {
                var link = ConfigurationManager.AppSettings["LINK_ESG_INFORMATION"].SetQueryParams(new
                {
                    kodeEmiten,
                    year
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

            catch (Exception e)
            {
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e.Message, e);
                var dataTable = new DTResult<dynamic>
                {
                    data = new List<dynamic>(),
                    recordsTotal = (int)0,
                    recordsFiltered = (int)0
                };
                var serializeObject = JsonConvert.SerializeObject(dataTable);
                return Content(serializeObject, "application/json");
            }
        }
    }

    public class ProspectusItem
    {
        public string Description { get; set; }
        public DateTime ListingDate { get; set; }
        public string Prospectus { get; set; }
    }
}