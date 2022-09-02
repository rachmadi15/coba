using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IDXWeb.Models
{
    public class IndexChart
    {
        public List<ChartItem> ChartData { get; set; }
        public string IndexCode { get; set; }
        public string Period { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public decimal OpenPrice { get; set; }
        public decimal MaxPrice { get; set; }
        public decimal MinPrice { get; set; }
        public string Step { get; set; }
    }
}