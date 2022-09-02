using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IDXWeb.Models
{
    public class IndexPerPbv
    {
         public int MasterIndexID { get; set; }
        public Nullable<decimal> PER { get; set; }
        public Nullable<decimal> PBV { get; set; }
        public Nullable<int> FsYear { get; set; }
        public Nullable<int> FsMonth { get; set; }
        public Nullable<int> FsPeriod { get; set; }
        public System.DateTime Date { get; set; }
        public int PeriodeId { get; set; }
    }
}