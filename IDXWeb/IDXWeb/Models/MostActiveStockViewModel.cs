using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IDXWeb.Models
{
    public class MostActiveStockViewModel
    {
        public int ID { get; set; }
        public string StockName { get; set; }
        public decimal? Volume { get; set; }
        public decimal? Value { get; set; }
        public decimal? Freq { get; set; }
        public decimal? VolumePercent { get; set; }
        public decimal? ValuePercent { get; set; }
        public decimal? FreqPercent { get; set; }
        public int TradingDays { get; set; }
    }
}