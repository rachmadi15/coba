using System;
using System.Configuration;
using System.IO;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Security;
using Umbraco;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Security;
using Umbraco.Core.Services;
using Umbraco.Web;

namespace IDXWeb.Controllers
{
    public class MediaController : Controller
    {
        public ActionResult Index(string id, string file)
        {
            string mediaPath = "/media/" + id + "/" + file;

            var isPublic = ConfigurationManager.AppSettings["MEDIA_PUBLIC"] ?? "false";

            if (isPublic == "false")
            {
                var ticket = new HttpContextWrapper(System.Web.HttpContext.Current).GetUmbracoAuthTicket();
                if (ticket == null)
                {
                    throw new FileNotFoundException();
                }
            }
            
            try
            {
                FileStream fileStream = new FileStream(Server.MapPath(mediaPath), FileMode.Open);

                return new FileStreamResult(fileStream, MimeMapping.GetMimeMapping(file));
            }
            catch
            {
                throw new FileNotFoundException();
            }
            
        }
    }
}