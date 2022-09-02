using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;

namespace IDXWeb.Models
{
    public class PDF_CMS
    {
        public long id { get; set; }
        
        public string PDFFilename { get; set; }
        
        public string FullSavePath { get; set; }
        
        [StringLength(50)]
        public string JMSXGroupId { get; set; }

        [StringLength(50)]
        public string CorrelationID { get; set; }

        public bool IsAttachment { get; set; }

        public string OriginalFilename { get; set; }

        public int CMSPosition { get; set; }

        public bool IsHidden { get; set; }
    }
}