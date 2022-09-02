using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IDXWeb.Models
{
    public class HistoryIndexSectoral
    {
        public System.DateTime Date { get; set; }
        public string IssuerCode { get; set; }
        public int SubSectorCode { get; set; }
        public int PeriodeId { get; set; }

    }
}