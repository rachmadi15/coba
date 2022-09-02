using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IDXWeb.Models
{
    public class TradingIndexSummary
    {
        public int ID { get; set; }
        public string IndexName { get; set; }
        public int IndexID { get; set; }
        public System.DateTime Date { get; set; }
        public decimal Prev { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }
        public decimal Change { get; set; }
        public decimal Stock { get; set; }
        public decimal Volume { get; set; }
        public decimal Value { get; set; }
        public decimal Freq { get; set; }
        public decimal MCap { get; set; }
        public decimal PER { get; set; }
        public decimal PBV { get; set; }
        public decimal Shares { get; set; }
        public int PeriodeId { get; set; }

    }
}