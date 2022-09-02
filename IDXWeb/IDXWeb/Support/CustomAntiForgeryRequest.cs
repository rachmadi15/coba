using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace IDXWeb.Support
{
    public class CustomAntiForgeryRequest
    {
        private HttpResponseBase response;
        private string token;

        public CustomAntiForgeryRequest(HttpResponseBase Response)
        {
            this.response = Response;
        }

        public void SetCookie()
        {
            Guid guid = Guid.NewGuid();
            token = Convert.ToBase64String(guid.ToByteArray());
            token = token.Replace("=", "").Replace("+", "").Replace("/", "");

            HttpCookie cookie = new HttpCookie("__RequestVerificationToken", token);
            cookie.Expires = DateTime.Now.AddHours(5);
            response.Cookies.Add(cookie);
        }

        public string GetToken()
        {
            var salt = ConfigurationManager.AppSettings["TokenSalt"];
            var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(salt));
            var bytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(token));
            var hashed = BitConverter.ToString(bytes).Replace("-", "").ToLower();
            return hashed;
        }
    }
}