using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IDXWeb.Models
{
    public class MasterBondABS
    {
        public string ABSCode { get; set; }
        public string ABSName { get; set; }
        public string Type { get; set; }
        public string InvestmentManager { get; set; }
        public string Custodian { get; set; }
        public System.DateTime ListingDate { get; set; }
        public System.DateTime MatureDate { get; set; }
        public Nullable<System.DateTime> DelistedDate { get; set; }
        public Nullable<decimal> IssuedValueIDR { get; set; }
        public Nullable<decimal> IssuedValueUSD { get; set; }
    }
}