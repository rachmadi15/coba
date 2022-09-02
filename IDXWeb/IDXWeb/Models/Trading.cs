using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IDXWeb.Models
{
    public class Trading
    {
        public Nullable<System.DateTime> Date { get; set; }
        public int MasterSecuritiesID { get; set; }
        public int MasterInstrumentID { get; set; }
        public Nullable<decimal> HighValue { get; set; }
        public Nullable<decimal> LowValue { get; set; }
        public Nullable<decimal> CloseValue { get; set; }
        public Nullable<decimal> Freq { get; set; }
        public Nullable<decimal> Value { get; set; }
        public Nullable<decimal> Volume { get; set; }
        public string Board { get; set; }
        public decimal? IndividualIndex { get; set; }
        public Nullable<System.DateTime> LastDateTraded { get; set; }
        public int PeriodeId { get; set; }
        public Nullable<decimal> OpenValue { get; set; }
        public string SecuritiesCode { get; set; }
        public string InstrumentCode { get; set; }
    }
}