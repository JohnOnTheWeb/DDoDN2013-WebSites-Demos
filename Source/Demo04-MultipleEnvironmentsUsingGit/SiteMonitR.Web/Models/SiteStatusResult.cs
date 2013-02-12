using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiteMonitR.Web.Models
{
    public class SiteStatusResult
    {
        public int SiteId { get; set; }
        public string Url { get; set; }
        public string Status { get; set; }
    }
}