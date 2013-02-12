namespace SiteMonitR.Web.Migrations
{
    using SiteMonitR.Web.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<SiteMonitR.Web.Models.SiteMonitRContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(SiteMonitR.Web.Models.SiteMonitRContext context)
        {
            context.MonitoredSites.AddOrUpdate(x => x.Url,
                new MonitoredSite { Url = "http://bradygaster.com" },
                new MonitoredSite { Url = "http://localhost:1234" }
                );
        }
    }
}
