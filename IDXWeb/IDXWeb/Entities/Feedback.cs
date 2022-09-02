using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IDXWeb.Entities
{
    public class Feedback
    {
        public int ContentId { get; set; }
        public string Name { get; set; }
        public int VisitorTypeId { get; set; }
        public string VisitorTypeName { get; set; }
        public double Rating { get; set; }
        public string CategoryIdsStr { get; set; }
        public IEnumerable<int> CategoryIds { get { return CategoryIdsStr == null ? null : CategoryIdsStr.Split(',').Select(o => int.Parse(o)); } }
        public string CategoryNamesStr { get; set; }
        public string[] CategoryNames { get { return CategoryNamesStr == null ? null : CategoryNamesStr.Split(','); } }
        public string OtherCategory { get; set; }
        public string Description { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}