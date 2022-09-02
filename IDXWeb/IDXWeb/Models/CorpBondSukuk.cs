using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IDXWeb.Models
{
    public class CorpBondSukuk
    {
        public Nullable<int> MasterBondCorpID { get; set; }
        public Nullable<System.DateTime> ListingDate { get; set; }
        public Nullable<System.DateTime> MatureDate { get; set; }
        public Nullable<decimal> IssuedValue { get; set; }
        public Nullable<decimal> IssuedValueUSD { get; set; }
        public Nullable<decimal> OutstandingValue { get; set; }
        public Nullable<decimal> OutstandingValueUSD { get; set; }
        public Nullable<decimal> TradingVol { get; set; }
        public Nullable<decimal> TradingVolUsd { get; set; }
        public Nullable<decimal> TradingFreq { get; set; }
        public Nullable<decimal> TradingFreqUSD { get; set; }
        public Nullable<decimal> TradingValue { get; set; }
        public Nullable<decimal> TradingValueUSD { get; set; }
        public string Coupon { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public int PeriodeId { get; set; }
        public string Rating { get; set; }
        public string SerieCode { get; set; }
        public string Tenure { get; set; }
        public decimal Interest { get; set; }
    }
}