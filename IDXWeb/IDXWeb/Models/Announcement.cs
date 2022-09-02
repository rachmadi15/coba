using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IDXWeb.Models
{
    public class Announcement
    {
        public int Id { get; set; }
        public string ItemID { get; set; }

        public string NoPengumuman { get; set; }
        public DateTime DATE { get; set; }
        public string TITLE { get; set; }
        public string JenisPengumuman { get; set; }
        public string CODE { get; set; }

        public string FormID { get; set; }
        public string JMSXGroupId { get; set; }
        public string JENIS { get; set; }
        public string PDF_PATH { get; set; }
        public string PerihalPengumuman { get; set; }
        public string Lang { get; set; }

        public string KodeDivisi { get; set; }
        public string Divisi { get; set; }
        public string JenisEmiten { get; set; }

        public bool IsHidden { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}