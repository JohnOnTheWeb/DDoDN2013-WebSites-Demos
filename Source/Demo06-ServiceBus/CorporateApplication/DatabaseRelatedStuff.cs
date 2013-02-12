using SharedDomain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorporateApplication
{
    public class SitePingDataContext : DbContext
    {
        public SitePingDataContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<SitePingResult> SitePingResults { get; set; }
    }
}
