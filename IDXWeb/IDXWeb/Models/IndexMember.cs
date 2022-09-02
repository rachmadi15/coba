using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IDXWeb.Models
{
    public class IndexMember
    {
        public int IssuerID { get; set; }
        public string IndexCode { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public string SecuritiesCode { get; set; }
        public int PeriodeId { get; set; }

    }
}