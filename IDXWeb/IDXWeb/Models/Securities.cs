using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IDXWeb.Models
{
    public class Securities
    {
         public int MasterSecurityID { get; set; }
        public int MasterInstrumentID { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public int MasterIndexID { get; set; }
        public string SecuritiesCode { get; set; }
        public string IndexName { get; set; }
        public string InstrumentCode { get; set; }
        public string Remarks1 { get; set; }
        public string Remarks2 { get; set; }
        public Nullable<int> ListedShares { get; set; }
        public Nullable<int> AdditionalShares { get; set; }
        public string Status { get; set; }
    }
}