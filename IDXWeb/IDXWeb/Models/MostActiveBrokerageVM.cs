using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IDXWeb.Models
{
    public class MostActiveBrokerageVM
    {
        public string BrokerageName { get; set; }
        public decimal RegulerVol { get; set; }
        public decimal RegulerVal { get; set; }
        public decimal RegulerFreq { get; set; }
        public decimal NonRegulerVol { get; set; }
        public decimal NonRegulerFreq { get; set; }
        public decimal NonRegulerVal { get; set; }
        public decimal TotalFreq { get; set; }
        public decimal TotalVal { get; set; }
        public decimal TotalVol{ get; set; }
    }
}