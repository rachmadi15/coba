using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IDXWeb.Models
{
    public class GovSBNBond
    {
        public int MasterBondGovID { get; set; }
        public string Coupon { get; set; }
        public string SerieCode { get; set; }
        public Nullable<int> PeriodeId { get; set; }
        public int Tenor { get; set; }
        public string TenorUnit { get; set; }
        public string Currency { get; set; }
        public Nullable<decimal> IDRValue { get; set; }
        public Nullable<decimal> USDValue { get; set; }
        public Nullable<decimal> TradingVolIDR { get; set; }
        public Nullable<decimal> TradingVolUSD { get; set; }
        public Nullable<decimal> TradingFreqIDR { get; set; }
        public Nullable<decimal> TradingFreqUSD { get; set; }
        public Nullable<decimal> TradingValIDR { get; set; }
        public Nullable<decimal> TradingValUSD { get; set; }
        public Nullable<decimal> Interest { get; set; }
        public System.DateTime Date { get; set; }
    }
}