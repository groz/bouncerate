using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LiveBounceChart.Web.Models
{
    public class BounceEntry
    {
        public TimeSpan BouncePeriod { get; set; }
        public DateTime UtcExitTime { get; set; }
    }
}