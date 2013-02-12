namespace SiteMonitR.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class setupdatabase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MonitoredSites",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Url = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SiteStatusChecks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Status = c.Int(nullable: false),
                        TimeStamp = c.DateTime(nullable: false),
                        Site_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MonitoredSites", t => t.Site_Id)
                .Index(t => t.Site_Id);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.SiteStatusChecks", new[] { "Site_Id" });
            DropForeignKey("dbo.SiteStatusChecks", "Site_Id", "dbo.MonitoredSites");
            DropTable("dbo.SiteStatusChecks");
            DropTable("dbo.MonitoredSites");
        }
    }
}
