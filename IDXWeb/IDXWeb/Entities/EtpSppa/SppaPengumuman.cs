using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IDXWeb.Entities.EtpSppa {
    public class SppaAnnouncement {
        public int Id { get; set; }
        public int ContentId { get; set; }
        public string ImageURL { get; set; }
        public DateTime? TanggalPublish { get; set; }
        public string Locale { get; set; }
        public string Judul { get; set; }
        public string Ringkasan { get; set; }
        public string Isi { get; set; }
        public int ContentPairId { get; set; }
        
        public DateTime UpdatedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}