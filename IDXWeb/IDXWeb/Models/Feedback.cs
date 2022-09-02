using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace IDXWeb.Models
{
    public class Feedback : BasePublishedContent
    {
        public Feedback() { }
        public Feedback(IContent content) : base(content) { }

        public new string Name => Content.Name;
        public string Description => Content.GetValue<string>("description");
        public string OtherCategory => Content.GetValue<string>("otherCategory");
        public int Rating => Content.GetValue<int>("rating");

        public string name => Name;
        public int id => Id;
        public string icon => Icon;
        public Guid key => Content.Key;

        public DateTime CreateDate => Content.CreateDate;
        public DateTime UpdateDate => Content.UpdateDate;
        public string CreateDateStr => CreateDate.ToString("dd/MM/yyyy");
        public string UpdateDateStr => UpdateDate.ToString("dd/MM/yyyy");

        public string Category
        {
            get
            {
                string result = null;
                var udiStr = Content.GetValue<string>("category");
                if (udiStr != null && udiStr != "")
                {
                    var udis = udiStr.Split(',');
                    for (int i = 0; i < udis.Length; i++)
                    {
                        var guid = udis[i].Replace("umb://document/", "");
                        var category = service.GetById(new Guid(guid));
                        if (category == null)
                        {
                            continue;
                        }

                        result = (result == "" || result == null) ? category.Name : result + ", " + category.Name;
                    }
                }

                if (OtherCategory != null && OtherCategory != "")
                {
                    result = (result == "" || result == null) ? OtherCategory : result + ", " + OtherCategory;
                }

                return result;
            }
        }

        public string VisitorType
        {
            get
            {
                var udi = Content.GetValue<string>("visitorType");
                if (udi == null || udi == "")
                {
                    return null;
                }

                var guid = udi.Replace("umb://document/", "");
                var result = service.GetById(new Guid(guid));
                if (result == null)
                {
                    return null;
                }

                return result.Name;
            }
        }
    }
}