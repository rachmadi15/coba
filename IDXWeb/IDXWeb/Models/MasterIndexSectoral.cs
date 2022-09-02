using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IDXWeb.Models
{
    public class MasterIndexSectoral
    {
        public int ID { get; set; }
        public string SectorCode { get; set; }
        public string SubSectorCode { get; set; }
        public string IndustryCode { get; set; }
        public string SubIndustryCode { get; set; }
        public string SectorName { get; set; }
        public string SubSectorName { get; set; }
        public string IndustryName { get; set; }
        public string SubIndustryName { get; set; }
        public DateTime? AppliedAt { get; set; }
        public DateTime? ExpiredAt { get; set; }
    }
}