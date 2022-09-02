using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IDXWeb.Models
{
    public class DinfraAct
    {
         public System.DateTime Date { get; set; }
        public Nullable<int> Days { get; set; }
        public Nullable<decimal> TotTradFreq { get; set; }
        public Nullable<decimal> TotTradVol { get; set; }
        public Nullable<decimal> AvDailyTradVol { get; set; }
        public Nullable<decimal> AvDailyTradFreq { get; set; }
        public Nullable<decimal> NumberDINFRA { get; set; }
        public int ID { get; set; }
        public int PeriodeId { get; set; }
        public Nullable<decimal> TotTradVal { get; set; }
        public Nullable<decimal> AvDailyTradVal { get; set; }
    }
}