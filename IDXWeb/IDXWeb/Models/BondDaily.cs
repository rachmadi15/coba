using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IDXWeb.Models
{
    public class BondDaily
    {
        public System.DateTime Date { get; set; }
        public Nullable<decimal> TotalVolCorpIDR { get; set; }
        public Nullable<decimal> TotalValCorpIDR { get; set; }
        public Nullable<decimal> TotalFreqCorpIDR { get; set; }
        public Nullable<decimal> TotalVolGovIDR { get; set; }
        public Nullable<decimal> TotalValGovIDR { get; set; }
        public Nullable<decimal> TotalFreqGovIDR { get; set; }
        public Nullable<decimal> TotalVolABSIDR { get; set; }
        public Nullable<decimal> TotalValABSIDR { get; set; }
        public Nullable<decimal> TotalFreqABSIDR { get; set; }
        public Nullable<decimal> TotalVolumeIDR { get; set; }
        public Nullable<decimal> TotalValueIDR { get; set; }
        public Nullable<decimal> TotalFreqIDR { get; set; }
        public Nullable<decimal> TotalVolCorpUSD { get; set; }
        public Nullable<decimal> TotalValCorpUSD { get; set; }
        public Nullable<decimal> TotalFreqCorpUSD { get; set; }
        public Nullable<decimal> TotalVolGovUSD { get; set; }
        public Nullable<decimal> TotalValGovUSD { get; set; }
        public Nullable<decimal> TotalFreqGovUSD { get; set; }
        public Nullable<decimal> TotalVolumeUSD { get; set; }
        public Nullable<decimal> TotalValueUSD { get; set; }
        public Nullable<decimal> TotalFreqUSD { get; set; }
        public Nullable<int> PeriodeID { get; set; }
        public int PeriodeId { get; set; }

    }
}