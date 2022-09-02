using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IDXWeb.Models
{
    public class CompositeStockPriceModel
    {
        public string title { get; set; }
        public string subtitle { get; set; }
        public List<CompositeStockPriceSeries> series { get; set; }
    }
}