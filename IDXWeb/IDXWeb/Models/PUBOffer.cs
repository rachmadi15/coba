using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IDXWeb.Models
{
    public class PUBOffer
    {
        public Nullable<int> IssuerID { get; set; }
        public string IssuerCode { get; set; }
        public Nullable<decimal> SharesOffer { get; set; }
        public Nullable<decimal> TotalListedShares { get; set; }
        public Nullable<decimal> PercentageOfSharesOffer { get; set; }
        public Nullable<decimal> IPOPrice { get; set; }
        public Nullable<decimal> FundRaised { get; set; }
        public Nullable<System.DateTime> ListingDate { get; set; }
        public string BusinessScope { get; set; }
        public string IsSharia { get; set; }
        public string Board { get; set; }
        public Nullable<decimal> ListingMCap { get; set; }
        public Nullable<decimal> MarketTot { get; set; }
        public Nullable<decimal> Nominal { get; set; }
        public Nullable<decimal> StockSplit { get; set; }
        public Nullable<decimal> NominalNow { get; set; }
        public Nullable<System.DateTime> SplitDate { get; set; }
        public string Notes { get; set; }
        public Nullable<System.DateTime> ClosingDate { get; set; }
        public Nullable<System.DateTime> ClosingAnnualDate { get; set; }
        public Nullable<System.DateTime> ClosingAnnual_ { get; set; }
        public string LeadUnderwriter { get; set; }
        public Nullable<System.DateTime> DateEstablish { get; set; }
        public string LKIPO { get; set; }
        public string FoldUpCapital { get; set; }
        public int PeriodeId { get; set; }
    }
}