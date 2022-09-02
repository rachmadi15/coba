using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Mvc;
using Umbraco.Web;

namespace IDXWeb.Controllers.DPS
{
    public class DPSMenuController : Umbraco.Web.Mvc.SurfaceController
    {

        [System.Web.Http.HttpGet]
        public JsonResult Monthly(string lang = "id-id")
        {
            var menuJsonFile = System.Web.Hosting.HostingEnvironment.MapPath("~/data/MonthlyMenu.json");
            var menuWhitelist = System.Web.Hosting.HostingEnvironment.MapPath("~/data/MonthlyMenuWhitelist.json");

            return Json(GetMenu(menuJsonFile, menuWhitelist, lang), JsonRequestBehavior.AllowGet);
        }

        [System.Web.Http.HttpGet]
        public JsonResult Quarterly(string lang = "id-id")
        {
            var menuJsonFile = System.Web.Hosting.HostingEnvironment.MapPath("~/data/QuarterlyMenu.json");
            var menuWhitelist = System.Web.Hosting.HostingEnvironment.MapPath("~/data/QuarterlyMenuWhitelist.json");

            return Json(GetMenu(menuJsonFile, menuWhitelist, lang), JsonRequestBehavior.AllowGet);
        }

        private IEnumerable<object> GetMenu(string menuJsonFile, string menuWhitelist, string lang)
        {
            var results = new List<object>();
            var root = Umbraco.TypedContentAtRoot().FirstOrDefault(x => x.Name == lang);
            if (root == null) return results;
            var digitalStatistic = root.Descendant("dpsHome");
            if (digitalStatistic == null) {
                digitalStatistic = root.Descendant("statisticDigital");
            }
            if (digitalStatistic == null) return results;
            var items = digitalStatistic.Children;

            StreamReader r = new StreamReader(menuJsonFile);
            var menuJsonStr = r.ReadToEnd();
            r = new StreamReader(menuWhitelist);
            var whitelistStr = r.ReadToEnd();
            r.Close();
            var mMenu = JArray.Parse(menuJsonStr);
            var mWhitelist = JArray.Parse(whitelistStr);
            var list = mMenu.ToList<object>();
            var num = 1;
            foreach (var level0 in mMenu)
            {
                bool whitelisted = mWhitelist.FirstOrDefault(x => x.Value<string>() == level0.Value<string>("id")) != null;
                var whitelistChildren = false;
                if (mWhitelist.Count() > 0 && whitelisted) whitelistChildren = true;
                var children = new List<object>();
                var childrenJson = level0.Value<IEnumerable<JToken>>("links");
                char c = 'A';
                foreach (var level1 in childrenJson)
                {
                    var url = level1.Value<string>("url");
                    var existing = items.FirstOrDefault(x => x.UrlName == url);
                    if (existing == null) continue;
                    whitelisted = mWhitelist.FirstOrDefault(x => x.ToString() == level1.Value<string>("url")) != null;
                    if (mWhitelist.Count() > 0 && !whitelisted && !whitelistChildren) continue;
                    var childName = level1.Value<string>("name");
                    children.Add(new {
                        url,
                        name = c + "." + childName.Split('.')[1]
                    });
                    c++;
                }

                if (children.Count() <= 0) continue;

                var parentName = level0.Value<string>("name");
                results.Add(new {
                    id = level0.Value<string>("id"),
                    name = num + "." + parentName.Split('.')[1],
                    links = children
                });
                num++;
            }

            return results;
        }
    }
}