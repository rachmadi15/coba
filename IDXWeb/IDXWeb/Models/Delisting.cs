using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IDXWeb.Models
{
    public class Delisting
    {
        public Nullable<int> IssuerID { get; set; }
        public string IssuerCode { get; set; }
        public Nullable<System.DateTime> DelistingDate { get; set; }
        public string BoardDelist { get; set; }
        public string IsSharia { get; set; }
        public string SubSectorCode { get; set; }
        public Nullable<decimal> IndexIndividual { get; set; }
        public Nullable<decimal> PriceDelist { get; set; }
        public Nullable<System.DateTime> LastTradedDate { get; set; }
        public Nullable<decimal> NumOfSharesDelist { get; set; }
        public Nullable<decimal> MCap { get; set; }
        public Nullable<System.DateTime> DateEstablished { get; set; }
        public Nullable<System.DateTime> ListingDate { get; set; }
        public Nullable<decimal> IPOPrice { get; set; }
        public Nullable<System.DateTime> AnnouncementDate { get; set; }
        public Nullable<decimal> NonRegPrice { get; set; }
        public string DelistReason { get; set; }
        public string ForcedDelist { get; set; }
        public string VoluntaryDelist { get; set; }
        public int PeriodeId { get; set; }

    }
}