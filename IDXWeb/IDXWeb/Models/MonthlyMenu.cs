using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IDXWeb.Models
{
    public class MonthlyMenu
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<MenuLinks> Links { get; set; }
    }
}