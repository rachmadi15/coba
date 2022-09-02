using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IDXWeb.Models
{
    public class ChartItem
    {
        public string Date { get; set; }
        public string XLabel { get; set; }
        public decimal Close { get; set; }
    }
}