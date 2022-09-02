using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IDXWeb.Models
{
    public class StockSplit
    {
       public int IssuerID { get; set; }
        public string Ratio { get; set; }
        public string IssuerCode { get; set; }
        public Nullable<decimal> NominalValue { get; set; }
        public Nullable<decimal> NominalValueNew { get; set; }
        public Nullable<decimal> ListedShares { get; set; }
        public Nullable<decimal> ListedSharesNew { get; set; }
        public Nullable<decimal> AdditionalListedShares { get; set; }
        public System.DateTime ListingDate { get; set; }
        public Nullable<System.DateTime> AnnualDate { get; set; }
        public int PeriodeId { get; set; }
    }
}