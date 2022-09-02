using System;
using System.Collections.Generic;

namespace IDXWeb.Models
{
    public class AnnouncementItem
    {
        public string Id { get; set; }
        public string ItemID { get; set; }

        public string AnnouncementNo { get; set; }
        public DateTime PublishDate { get; set; }
        public string Title { get; set; }
        public string AnnouncementType { get; set; }
        public string Code { get; set; }

        public string FormID { get; set; }
        public string JmsxGroupId { get; set; }
        public string Jenis { get; set; }
        public List<AnnouncementAttachment> Attachments { get; set; }
        public string PdfPath { get; set; }
        public string Perihal { get; set; }
        public string Lang { get; set; }

        public string KodeDivisi { get; set; }
        public string Divisi { get; set; }
        public string JenisEmiten { get; set; }

        public bool IsHidden { get; set; }
    }

    public class AnnouncementAttachment
    {
        public string PDFFilename { get; set; }
        public string FullSavePath { get; set; }
        public int IsAttachment { get; set; }
        public string OriginalFilename { get; set; }
    }
}