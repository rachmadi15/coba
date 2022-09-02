using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IDXWeb.Models
{
    public class MasterBondCorp
    {
        public string IssuerCode { get; set; }
        public int IndexSectoralId { get; set; }
        public string SubSectorCode { get; set; }
        public string EmissionCode { get; set; }
        public string EmissionName { get; set; }
        public string SerieCode { get; set; }
        public string SerieName { get; set; }
        public string Trustee { get; set; }
        public string SecuritiesType { get; set; }
        public string OfferType { get; set; }
        public Nullable<System.DateTime> IssuedDate { get; set; }
        public Nullable<System.DateTime> ListingDate { get; set; }
        public Nullable<System.DateTime> MatureDate { get; set; }
        public Nullable<System.DateTime> DelistingDate { get; set; }
    }
}