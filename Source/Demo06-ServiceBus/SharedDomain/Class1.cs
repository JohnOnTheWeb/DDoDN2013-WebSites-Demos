using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedDomain
{
    public class SitePingResult
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string Status { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
