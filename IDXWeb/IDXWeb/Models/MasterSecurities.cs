using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IDXWeb.Models
{
    public class MasterSecurities
    {
        public int IndexSectoralID { get; set; }
        public Nullable<int> IssuerId { get; set; }
        public string IssuerCode { get; set; }
        public string SubSectorCode { get; set; }
        public string SecclassID { get; set; }
        public string SecuritiesCode { get; set; }
        public string SecuritiesName { get; set; }
        public string InstrId { get; set; }
        public string ISIN { get; set; }
    }
}