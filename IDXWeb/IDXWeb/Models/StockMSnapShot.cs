using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IDXWeb.Models
{
    public class StockMSnapshot
    {
        public System.DateTime Date { get; set; }
        public string Currency { get; set; }
        public Nullable<decimal> TradingVol { get; set; }
        public Nullable<decimal> TradingValue { get; set; }
        public Nullable<decimal> TradingFreq { get; set; }
        public Nullable<decimal> AVDTRVOL { get; set; }
        public Nullable<decimal> AVDTRVAL { get; set; }
        public decimal AVDTRFREQ { get; set; }
        public Nullable<int> TradingDays { get; set; }
        public Nullable<decimal> JCI { get; set; }
        public Nullable<decimal> MCap { get; set; }
        public Nullable<decimal> ListedCompanies { get; set; }
        public Nullable<decimal> ListedShares { get; set; }
        public Nullable<decimal> AdditionalShares { get; set; }
        public int PeriodeId { get; set; }
    }
}