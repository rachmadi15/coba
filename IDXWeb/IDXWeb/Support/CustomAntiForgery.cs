using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace IDXWeb.Support
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class CustomAntiForgery : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            var request = filterContext.RequestContext.HttpContext.Request;
            if (request.HttpMethod != "POST")
                return;

            var hashed = request.Form["__RequestVerificationToken"];
            if (hashed == null)
                throw new HttpAntiForgeryException("No __RequestVerificationToken provided.");

            var token = request.Cookies.Get("__RequestVerificationToken").Value;
            if (token == null)
                throw new HttpAntiForgeryException("The anti-forgery cookie \"__RequestVerificationToken\" is not provided.");

            var salt = ConfigurationManager.AppSettings["TokenSalt"];
            var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(salt));
            var bytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(token));
            var hash = BitConverter.ToString(bytes).Replace("-", "").ToLower();
            if (hash != hashed)
                throw new HttpAntiForgeryException("Anti-forgery token doesn't match.");
        }
    }
}