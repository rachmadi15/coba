using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IDXWeb.Models
{
    public class BiggestMarketCapViewModel
    {
        public int ID { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public string IssuerName { get; set; }
        public Nullable<int> IndexSectoralID { get; set; }
        public Nullable<decimal> Mc { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<decimal> ListedShares { get; set; }
        public int PeriodeId { get; set; }
    }
}