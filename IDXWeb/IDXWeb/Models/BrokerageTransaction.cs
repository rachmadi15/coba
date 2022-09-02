using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IDXWeb.Models
{
    public class BrokerageTransaction
    {
        public System.DateTime Date { get; set; }
        public string BrokerageCode { get; set; }
        public string Board { get; set; }
        public string InstrId { get; set; }
        public Nullable<decimal> Volume { get; set; }
        public Nullable<decimal> Value { get; set; }
        public Nullable<decimal> Freq { get; set; }
    }
}