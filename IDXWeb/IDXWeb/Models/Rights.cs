using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IDXWeb.Models
{
    public class Rights
    {
        public string IssuerCode { get; set; }
        public string Ratio { get; set; }
        public Nullable<System.DateTime> Listeddate { get; set; }
        public Nullable<System.DateTime> ExDate { get; set; }
        public Nullable<System.DateTime> RecordingDate { get; set; }
        public Nullable<decimal> AllotmentShares { get; set; }
        public Nullable<decimal> SharesIssued { get; set; }
        public Nullable<decimal> ExPrice { get; set; }
        public Nullable<decimal> FundRaised { get; set; }
        public string RightCertificateTradingPeriod { get; set; }
        public Nullable<System.DateTime> CumDate { get; set; }
        public Nullable<System.DateTime> Allotmentdate { get; set; }
        public Nullable<System.DateTime> Refunddate { get; set; }
    }
}