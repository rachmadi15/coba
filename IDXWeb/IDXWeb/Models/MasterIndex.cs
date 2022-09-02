using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IDXWeb.Models
{
    public class MasterIndex
    {
        public string IndexName { get; set; }
        public string IndexCode { get; set; }
        public string Color { get; set; }
        public int PlacingOrder { get; set; }
        public DateTime? AppliedAt { get; set; }
        public DateTime? ExpiredAt { get; set; }
        public Nullable<bool> IsSector { get; set; }

    }
}