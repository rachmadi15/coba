using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IDXWeb.Models
{
    public class MasterBrokerage
    {
        public string BrokerageCode { get; set; }
        public string BrokerageName { get; set; }
        public string Status { get; set; }
        public Nullable<System.DateTime> DeactivatedDate { get; set; }
        public string Board { get; set; }
    }
}