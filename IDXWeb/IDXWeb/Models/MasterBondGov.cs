using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IDXWeb.Models
{
    public class MasterBondGov
    {
        public string SBNType { get; set; }
        public string SerieCode { get; set; }
        public string SerieName { get; set; }
        public System.DateTime IssuedDate { get; set; }
        public System.DateTime ListingDate { get; set; }
        public System.DateTime MatureDate { get; set; }
        public Nullable<System.DateTime> DelistingDate { get; set; }
    }
}