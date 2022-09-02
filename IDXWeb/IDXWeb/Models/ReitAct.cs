using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IDXWeb.Models
{
    public class ReitAct
    {
        public System.DateTime Date { get; set; }
        public Nullable<int> Days { get; set; }
        public Nullable<decimal> TotTradVol { get; set; }
        public Nullable<decimal> TotTradVal { get; set; }
        public Nullable<decimal> TotTradFreq { get; set; }
        public int ID { get; set; }
        public Nullable<decimal> AvDailyTradVol { get; set; }
        public Nullable<decimal> AvDailyTradVal { get; set; }
        public Nullable<decimal> AvDailyTradFreq { get; set; }
        public Nullable<int> NumberREIT { get; set; }
        public int PeriodeId { get; set; }

    }
}