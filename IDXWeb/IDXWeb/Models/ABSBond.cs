using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IDXWeb.Models
{
    public class ABSBond
    {
        public int MasterABSID { get; set; }
        public string SerieCode { get; set; }
        public Nullable<decimal> IssuedValueIDR { get; set; }
        public Nullable<decimal> IssuedValueUSD { get; set; }
        public Nullable<decimal> OutstandingValueIDR { get; set; }
        public Nullable<decimal> OutstandingValueUSD { get; set; }
        public string Rating { get; set; }
        public string interest { get; set; }
        public int Tenor { get; set; }
        public string TenorUnit { get; set; }
        public DateTime Date { get; set; }
        public Nullable<decimal> TradingValueIDR { get; set; }
        public Nullable<decimal> TradingValueUSD { get; set; }
        public Nullable<decimal> TradingVolumeUSD { get; set; }
        public Nullable<decimal> TradingVolumeIDR { get; set; }
        public Nullable<decimal> TradingFreqIDR { get; set; }
        public Nullable<decimal> TradingFreqUSD { get; set; }
    }
}