using System.Data.Entity;

namespace SiteMonitR.Web.Models
{
    public class SiteMonitRContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, add the following
        // code to the Application_Start method in your Global.asax file.
        // Note: this will destroy and re-create your database with every model change.
        // 
        // System.Data.Entity.Database.SetInitializer(new System.Data.Entity.DropCreateDatabaseIfModelChanges<SiteMonitR.Web.Models.SiteMonitRContext>());

        public SiteMonitRContext()
            : base("name=DefaultConnection")
        {
        }

        public DbSet<MonitoredSite> MonitoredSites { get; set; }
        public DbSet<SiteStatusCheck> SiteStatusChecks { get; set; }
    }
}
