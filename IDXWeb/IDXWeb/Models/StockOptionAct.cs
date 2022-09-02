using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IDXWeb.Models
{
    public class StockOptionAct
    {
        public System.DateTime Date { get; set; }
        public Nullable<int> Days { get; set; }
        public Nullable<decimal> CocContract { get; set; }
        public Nullable<decimal> CocValue { get; set; }
        public Nullable<decimal> CocFreq { get; set; }
        public Nullable<decimal> PocContract { get; set; }
        public Nullable<decimal> PocValue { get; set; }
        public Nullable<decimal> PocFreq { get; set; }
        public Nullable<decimal> TotVolume { get; set; }
        public Nullable<decimal> TotValue { get; set; }
        public Nullable<decimal> TotFreq { get; set; }
        public int ID { get; set; }
        public int PeriodeId { get; set; }
        public Nullable<decimal> AvDailyTradVol { get; set; }
        public Nullable<decimal> AvDailyTradVal { get; set; }
        public Nullable<decimal> AvDailyTradFreq { get; set; }
    
    }
}