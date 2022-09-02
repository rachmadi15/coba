using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IDXWeb.Models
{
    public class BondAct
    {
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<int> IssuerCorp { get; set; }
        public Nullable<int> IssuesCorp { get; set; }
        public Nullable<int> SeriesCorp { get; set; }
        public Nullable<decimal> OutstandingCorpRp { get; set; }
        public Nullable<decimal> OutstandingCorpUSD { get; set; }
        public decimal TVolcorpRP { get; set; }
        public decimal TValcorpRP { get; set; }
        public decimal TFreqcorpRP { get; set; }
        public decimal TVolcorpUSD { get; set; }
        public decimal TValcorpUSD { get; set; }
        public decimal TFreqcorpUSD { get; set; }
        public Nullable<int> Tradingdayscorp { get; set; }
        public Nullable<int> IssuesGovRp { get; set; }
        public Nullable<int> IssuesGovUSD { get; set; }
        public Nullable<decimal> OutstandingGovRp { get; set; }
        public Nullable<decimal> OutstandingGovUSD { get; set; }
        public decimal TVolgovRP { get; set; }
        public decimal TValgovRP { get; set; }
        public decimal TFreqgovRP { get; set; }
        public decimal TVolgovUSD { get; set; }
        public decimal TValgovUSD { get; set; }
        public decimal TFreqgovUSD { get; set; }
        public Nullable<int> Tradingdaysgov { get; set; }
        public Nullable<int> Issuerabs { get; set; }
        public Nullable<int> Issuesabs { get; set; }
        public Nullable<int> Seriesabs { get; set; }
        public Nullable<decimal> OutstandingabsRp { get; set; }
        public Nullable<decimal> OutstandingabsUSD { get; set; }
        public decimal TVolabsRP { get; set; }
        public decimal TValabsRP { get; set; }
        public decimal TFreqabsRP { get; set; }
        public decimal TVolabsUSD { get; set; }
        public decimal TValabsUSD { get; set; }
        public decimal TFreqabsUSD { get; set; }
        public Nullable<int> Tradingdaysabs { get; set; }
        public int PeriodeId { get; set; }

    }
}