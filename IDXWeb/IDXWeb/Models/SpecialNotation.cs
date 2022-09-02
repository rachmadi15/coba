using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IDXWeb.Models
{
    public class SpecialNotation
    {
        public string EmitenCode { get; set; }
        public DateTime Date { get; set; }
        public string Notation { get; set; }
        public string Remarks { get; set; }
        public string Description { get; set; }
        public string Language { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}