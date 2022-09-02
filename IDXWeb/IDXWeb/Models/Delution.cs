using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IDXWeb.Models
{
    public class Delution
    {
        public Nullable<int> IssuerID { get; set; }
        public string IssuerCode { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public decimal Delutions { get; set; }
        public int PeriodeId { get; set; }

    }
}