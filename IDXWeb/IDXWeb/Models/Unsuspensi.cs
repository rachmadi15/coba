using System;

namespace IDXWeb.Models
{
    public class Unsuspensi
    {
        public int Id { get; set; }
        public string ItemID { get; set; }
        public string Judul { get; set; }
        public string Pengumuman { get; set; }
        public DateTime Date { get; set; }
        public string info_type { get; set; }
        public string data_download { get; set; }
        public string langID { get; set; }
        public bool IsHidden { get; set; }
        public bool IsAttachmentHidden { get; set; }
    }
}