using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IDXWeb.Models
{
    public class BondSummary
    {
        public string BondType { get; set; }
        public string BondClassification { get; set; }
        public int IssuerNo { get; set; }
        public string Emission { get; set; }
        public string Serie { get; set; }
        public Nullable<decimal> OutstandingValueIDR { get; set; }
        public Nullable<decimal> OutstandingValueUSD { get; set; }
        public decimal TradingValueIDR { get; set; }
        public Nullable<decimal> TradingValueUSD { get; set; }
        public Nullable<decimal> TradingVolumeUSD { get; set; }
        public Nullable<decimal> TradingVolumeIDR { get; set; }
        public Nullable<decimal> TradingFreqIDR { get; set; }
        public Nullable<decimal> TradingFreqUSD { get; set; }
        public System.DateTime Date { get; set; }
        public string CorGovAbs { get; set; }
        public int PeriodeId { get; set; }
    }
}