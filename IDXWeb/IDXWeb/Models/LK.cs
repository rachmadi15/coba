using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IDXWeb.Models
{
    public class LK
    {
        public System.DateTime PeriodDate { get; set; }
        public System.DateTime Date { get; set; }
        public string IssuerCode { get; set; }
        public Nullable<int> FiscalYear { get; set; }
        public string FiscalYearEnd { get; set; }
        public string Audit { get; set; }
        public string Opini { get; set; }
        public Nullable<decimal> TAssets { get; set; }
        public Nullable<decimal> TLiabilities { get; set; }
        public Nullable<decimal> TEquity { get; set; }
        public Nullable<decimal> EBT { get; set; }
        public Nullable<decimal> Profit { get; set; }
        public Nullable<decimal> ProfitEntity { get; set; }
        public Nullable<decimal> Sales { get; set; }
        public Nullable<decimal> EPS { get; set; }
        public Nullable<decimal> BV { get; set; }
        public Nullable<decimal> PER { get; set; }
        public Nullable<decimal> PBV { get; set; }
        public Nullable<decimal> DER { get; set; }
        public Nullable<decimal> ROA { get; set; }
        public Nullable<decimal> ROE { get; set; }
        public Nullable<decimal> NPM { get; set; }
        public int PeriodeId { get; set; }
    }
}