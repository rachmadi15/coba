using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IDXWeb.Models
{
    public class LKSummary
    {
        public string SubSectorCode { get; set; }
        public string SectorCode { get; set; }
        public System.DateTime Date { get; set; }
        public int IndexSectoralId { get; set; }
        public Nullable<decimal> AnnualEPS { get; set; }
        public Nullable<decimal> BV { get; set; }
        public Nullable<decimal> PER { get; set; }
        public Nullable<decimal> PBV { get; set; }
        public Nullable<decimal> DER { get; set; }
        public Nullable<decimal> ROA { get; set; }
        public Nullable<decimal> ROE { get; set; }
        public Nullable<decimal> NPM { get; set; }
        public Nullable<int> PeriodeId { get; set; }

    }
}