using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IDXWeb.Models
{
    public class MasterIssuer
    {
        public string IssuerID { get; set; }
        public string IssuerCompanyCode { get; set; }
        public string IssuerCode { get; set; }
        public string IssuerName { get; set; }
        public int IndexSectoralID { get; set; }
        public string SubSectorCode { get; set; }
        public System.DateTime ListingDate { get; set; }
        public Nullable<System.DateTime> DelistingDate { get; set; }
        public Nullable<System.DateTime> RelistingDate { get; set; }
        public bool Sharia { get; set; }
        public string Status { get; set; }
        public string TipeEfek { get; set; }
    }
}