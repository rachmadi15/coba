using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IDXWeb.Models
{
    public class MostActiveStockBundle
    {
        public List<MostActiveStockViewModel> Data { get; set; }
        public decimal? TotalValue { get; set; }
        public decimal? TotalVolume { get; set; }
        public decimal? TotalFreq { get; set; }
        public decimal? TotalValuePercentage { get; set; }
        public decimal? TotalVolumePercentage { get; set; }
        public decimal? TotalFreqPercentage { get; set; }
        public decimal? TotalValueTop { get; set; }
        public decimal? TotalVolumeTop { get; set; }
        public decimal? TotalFreqTop { get; set; }
    }
}