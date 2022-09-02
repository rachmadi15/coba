using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Examine;
using Examine.LuceneEngine;
using Examine.LuceneEngine.SearchCriteria;
using Examine.SearchCriteria;
using Flurl;
using Flurl.Http;
using Newtonsoft.Json;
using Umbraco.Core.Logging;
using Umbraco.Web.Mvc;

namespace IDXWeb.Controllers
{
    public class SearchController : SurfaceController
    {
        public async Task<ActionResult> GetSearch(string keyword, int indexFrom, int pageSize = 10)
        {
            try {
            var url = ConfigurationManager.AppSettings["googleSearchLink"].SetQueryParams(new
            {
                cx = ConfigurationManager.AppSettings["googleCxKey"],
                key = ConfigurationManager.AppSettings["googleApiKey"],
                q = keyword,
                start = indexFrom,
                num = pageSize
            });

            var result = await url.GetStringAsync();
            return Content(result, "application/json"); }
            catch (Exception e) {
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e.Message, e);
                return null;
            }
        }


        public List<SearchResult> SearchResults
        {
            get
            {
                try {
                if (!string.IsNullOrEmpty(Request.QueryString["keyword"]))
                {
                    var searcher = ExamineManager.Instance.SearchProviderCollection["ExternalSearcher"];
                    // Search criteria.
                    var searchCriteria = searcher.CreateSearchCriteria(BooleanOperation.Or);
                    var q = Request.QueryString["keyword"].ToLower().Trim().Split(' ');
                    q = q.Where(i => i.Length > 3).ToArray();
                    var query = searchCriteria
                        .GroupedOr(new[] { "nodeName" }, q.Select(x => x.Boost(150)).ToArray())
                        .Or()
                        .GroupedOr(new[] { "grid" }, q.Select(x => x.Boost(80)).ToArray())
                        .Or()
                        .GroupedOr(new[] { "tags", "themes", "institutions" }, q.Select(x => x.Boost(110)).ToArray());
                    // Search results
                    var searchResults = searcher.Search(query.Compile()).OrderByDescending(x => x.Score);
                    return searchResults.ToList();
                }
                return new List<SearchResult>(); }
                catch (Exception e) {
                    if (e.Message.Contains("429")) {
                        LogHelper.Info(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Google Search: (code:429) daily query limit exceeds. Quota will be reset in 15:00 WIB");
                    }
                    else if (e.Message.Contains("400")) {
                        LogHelper.Info(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Google Search: (code:400) bad request");
                        LogHelper.Info(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Google Search Keyword: " + Request.QueryString["keyword"]);
                    }
                    else {
                        LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e.Message, e);
                    }
                    return null;
                }
            }
        }
    }
}