using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiteMonitR.Web.Models
{
    public class SiteStatusCheck
    {
        public int Id { get; set; }
        public MonitoredSite Site { get; set; }
        public SiteStatus Status { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}