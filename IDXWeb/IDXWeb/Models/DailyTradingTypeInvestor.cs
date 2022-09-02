using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IDXWeb.Models
{
    public class DailyTradingTypeInvestor
    {
      public System.DateTime Date { get; set; }
      public Nullable<decimal> FFTV { get; set; }
      public Nullable<decimal> FFTA { get; set; }
      public Nullable<decimal> FFTF { get; set; }
      public Nullable<decimal> FLTV { get; set; }
      public Nullable<decimal> FLTA { get; set; }
      public Nullable<decimal> FLTF { get; set; }
      public Nullable<decimal> LFTV { get; set; }
      public Nullable<decimal> LFTA { get; set; }
      public Nullable<decimal> LFTF { get; set; }
      public Nullable<decimal> LLTV { get; set; }
      public Nullable<decimal> LLTA { get; set; }
      public Nullable<decimal> LLTF { get; set; }
      public Nullable<decimal> VolumeMT { get; set; }
      public Nullable<decimal> FreqMT { get; set; }
      public Nullable<decimal> ValueMT { get; set; }
        public int PeriodeId { get; set; }
    }
}