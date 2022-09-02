using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IDXWeb.Models
{
    public class TableTradingViewModel
    {
        public string Name { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }
        public decimal Volume { get; set; }
        public decimal Value { get; set; }
        public decimal Freq { get; set; }
        public decimal Day { get; set; }
    }
}