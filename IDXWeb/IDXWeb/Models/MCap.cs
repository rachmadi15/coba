using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IDXWeb.Models
{
    public class MCap
    {
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<int> IssuerID { get; set; }
        public string IssuerCode { get; set; }
        public string SubSectorCode { get; set; }
        public Nullable<int> IndexSectoralID { get; set; }
        public Nullable<decimal> Mc { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<decimal> ListedShares { get; set; }
        public int PeriodeId { get; set; }
    }
}