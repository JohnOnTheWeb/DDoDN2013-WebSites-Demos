using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiteMonitR.Web.Models
{
    public class MonitoredSite
    {
        public MonitoredSite()
        {
            this.StatusHistory = new List<SiteStatusCheck>();
        }

        public int Id { get; set; }
        public string Url { get; set; }
        public List<SiteStatusCheck> StatusHistory { get; set; }
    }
}