using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IDXWeb.Models
{
    public class StockPriceViewModel
    {
        public string SectorName { get; set; }
        public string SubSectorName { get; set; }
        public string Code { get; set; }
        public string StockName { get; set; }
        public bool Sharia { get; set; }
        public int Board { get; set; }
        public decimal? Prev { get; set; }
        public decimal? High { get; set; }
        public decimal? Low { get; set; }
        public decimal? Close { get; set; }
        public decimal? IndividualIndex { get; set; }
    }
}