using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web;

namespace IDXWeb.Support
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class ValidateReCaptcha : FilterAttribute, IAuthorizationFilter
    {
        protected UmbracoHelper helper;
        public ValidateReCaptcha()
        {
            helper = new Umbraco.Web.UmbracoHelper(Umbraco.Web.UmbracoContext.Current);
        }

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            var request = filterContext.RequestContext.HttpContext.Request;
            if (request.Form["captcha"] == null)
            {
                throw new UnauthorizedAccessException();
            }

            string secret = ConfigurationManager.AppSettings["gRecaptchaSecret"];

            HttpClient client = new HttpClient();
            var rootNodes = helper.TypedContentAtRoot();
            var goPublicRoot = rootNodes.FirstOrDefault(o => o.DocumentTypeAlias == "islamic");
            if (goPublicRoot != null && goPublicRoot.GetPropertyValue<string>("proxyServer") != null && goPublicRoot.GetPropertyValue<string>("proxyServer") != "")
            {
                var server = goPublicRoot.GetPropertyValue<string>("proxyServer");
                var port = goPublicRoot.GetPropertyValue<string>("proxyPort") ?? "";
                var httpClientHandler = new HttpClientHandler
                {
                    Proxy = new WebProxy(server + ":" + port, false),
                    UseProxy = true
                };

                client = new HttpClient(httpClientHandler);
            }
            
            
            var response = client.GetAsync("https://www.google.com/recaptcha/api/siteverify?secret=" + secret + "&response=" + request.Form["captcha"]).Result;

            if (!response.IsSuccessStatusCode || response.Content == null)
            {
                throw new UnauthorizedAccessException();
            }

            string responseString = response.Content.ReadAsStringAsync().Result;
            var json = JObject.Parse(responseString);

            // If success: false
            if (!json.Value<bool>("success"))
            {
                filterContext.Result = new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
        }
    }
}