using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IDXWeb.Models
{
    public class AveragePERPBV
    {
        public System.DateTime Date { get; set; }
        public Nullable<decimal> AVGPER { get; set; }
        public Nullable<decimal> WAVGPER { get; set; }
        public Nullable<decimal> AVGPBV { get; set; }
        public Nullable<decimal> WAVGPBV { get; set; }

    }
}