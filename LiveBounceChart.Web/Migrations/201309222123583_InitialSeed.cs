namespace LiveBounceChart.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialSeed : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BounceEntry",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BouncePeriod = c.Time(nullable: false),
                        UtcExitTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.BounceEntry");
        }
    }
}
