using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IDXWeb.Models
{
    public class Listing
    {
        public Nullable<int> IssuerID { get; set; }
        public string IssuerCode { get; set; }
        public Nullable<decimal> NumOfShares { get; set; }
        public string Type { get; set; }
        public Nullable<System.DateTime> ListingDate { get; set; }
        public int PeriodeId { get; set; }
    }
}