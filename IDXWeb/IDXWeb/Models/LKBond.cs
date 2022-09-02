using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IDXWeb.Models
{
    public class LKBond
    {
        public System.DateTime Date { get; set; }
        public string IssuerCode { get; set; }
        public Nullable<int> FiscalYear { get; set; }
        public string FiscalYearEnd { get; set; }
        public string Audit { get; set; }
        public string Opini { get; set; }
        public decimal TAssets { get; set; }
        public decimal TLiabilities { get; set; }
        public decimal TEquity { get; set; }
        public decimal Sales { get; set; }
        public decimal EBT { get; set; }
        public decimal Profit { get; set; }
        public decimal ProfitEntity { get; set; }
        public Nullable<int> PeriodeId { get; set; }
    }
}