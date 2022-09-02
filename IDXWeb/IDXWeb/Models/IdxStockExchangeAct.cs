using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IDXWeb.Models
{
    public class IdxStockExchangeAct
    {
        public string Year_Month { get; set; }
        public decimal USDRate { get; set; }
        public decimal TotTradVol { get; set; }
        public decimal TotTradVal { get; set; }
        public decimal TotTradFreq { get; set; }
        public decimal AvDailyTrading { get; set; }
        public decimal AvDailyTradFreq { get; set; }
        public int Days { get; set; }
        public decimal Jci { get; set; }
        public decimal MCap { get; set; }
        public int ListedComp { get; set; }
        public decimal ListedShares { get; set; }
        public int ID { get; set; }
        public int PeriodeId { get; set; }
    }
}