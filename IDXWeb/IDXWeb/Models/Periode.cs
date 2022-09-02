using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IDXWeb.Models
{
    public class Periode
    {
        public int PeriodMonth { get; set; }
        public int PeriodYear { get; set; }
        public DateTime UploadedDate { get; set; }
        public DateTime EditedDate { get; set; }

    }
}