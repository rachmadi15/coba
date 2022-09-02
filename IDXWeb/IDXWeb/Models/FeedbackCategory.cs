using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core.Models;

namespace IDXWeb.Models
{
    public class FeedbackCategory : BaseContent
    {
        public FeedbackCategory(IContent Content) : base(Content) { }
        public int Order => Content.GetValue<int>("orderNumber");
    }
}