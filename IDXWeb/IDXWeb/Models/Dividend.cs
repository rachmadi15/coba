using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IDXWeb.Models
{
    public class Dividend
    {
        public Nullable<System.DateTime> CumDate { get; set; }
        public Nullable<System.DateTime> ExDate { get; set; }
        public System.DateTime RecordingDate { get; set; }
        public Nullable<System.DateTime> PaymentDate { get; set; }
        public string Note1 { get; set; }
        public Nullable<decimal> CashDividendIDR { get; set; }
        public Nullable<decimal> CashDividendUSD { get; set; }
        public Nullable<int> StockDividend { get; set; }
        public int IssuerID { get; set; }
        public int PeriodeId { get; set; }
        public string IssuerCode { get; set; }
    }
}