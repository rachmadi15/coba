using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IDXWeb.Models
{
    public class Index
    {
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<int> MasterIndexId { get; set; }
        public string IndexName { get; set; }
        public Nullable<decimal> High { get; set; }
        public Nullable<decimal> Low { get; set; }
        public Nullable<decimal> Open { get; set; }
        public Nullable<decimal> Close { get; set; }
        public Nullable<decimal> Value { get; set; }
        public Nullable<decimal> Freq { get; set; }
        public Nullable<decimal> Volume { get; set; }
        public int PeriodeId { get; set; }
        public Nullable<decimal> Prev { get; set; }
        public Nullable<decimal> MCap { get; set; }
        public Nullable<decimal> ListedShares { get; set; }
    }
}