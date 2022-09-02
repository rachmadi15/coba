using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IDXWeb.Entities.EtpSppa {
    public class SppaProfile {
        public int Id { get; set; }
        public int ContentId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Address { get;set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Website { get; set; }
        public string Email { get; set; }
        public string STDP { get; set; }
        public string User { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? CreatedDate { get; set; }

        public string ImageUrl { get; set; }
    }
}