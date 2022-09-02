using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IDXWeb.Models
{
    public class CABond
    {
        public string SerieCode { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public string TypeOfCA { get; set; }
        public Nullable<decimal> ValueCA { get; set; }
        public Nullable<decimal> OutstandingValue { get; set; }
        public Nullable<int> MasterBondCorpId { get; set; }
        public string isCorGov { get; set; }
        public int PeriodeId { get; set; }
    }
}