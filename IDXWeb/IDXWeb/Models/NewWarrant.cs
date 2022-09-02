using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IDXWeb.Models
{
    public class NewWarrant
    {
        public int IssuerID { get; set; }
        public Nullable<int> PeriodeId { get; set; }

        public string IssuerCode { get; set; }
        public string WarrantCode { get; set; }
        public decimal WarrantIssued { get; set; }
        public string Ratio { get; set; }
        public Nullable<decimal> ExPrice { get; set; }
        public Nullable<System.DateTime> ListingDate { get; set; }
        public Nullable<System.DateTime> StartDateTradingWarrant { get; set; }
        public Nullable<System.DateTime> EndDateTradingWarrant { get; set; }
        public Nullable<System.DateTime> StartDateExWarrant { get; set; }
        public Nullable<System.DateTime> EndDateExWarrant { get; set; }
    }
}