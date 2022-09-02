using System;

namespace IDXWeb.Entities
{
    public class PressRelease
    {
        public int ID { get; set; }
        public string ItemID { get; set; }
        public string ImageURL { get; set; }
        public DateTime? TanggalPublish { get; set; }
        public string Locale { get; set; }
        public string Judul { get; set; }
        public string Ringkasan { get; set; }
        public string Isi { get; set; }
        public string Folder { get; set; }
        public string File { get; set; }
        public string Tags { get; set; }
        public bool IsHeadLine { get; set; }
    }
}