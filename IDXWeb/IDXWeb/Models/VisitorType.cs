using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core;
using Umbraco.Core.Models;

namespace IDXWeb.Models
{
    public class VisitorType : BaseContent
    {
        public VisitorType(IContent Content) : base(Content) { }
        public int Order => Content.GetValue<int>("orderNumber");
    }
}