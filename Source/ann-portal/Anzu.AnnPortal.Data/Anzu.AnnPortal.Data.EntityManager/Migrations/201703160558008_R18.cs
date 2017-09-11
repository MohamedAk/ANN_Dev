namespace Anzu.AnnPortal.Data.EntityManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class R18 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "annPortal.TotalProcedures",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false, storeType: "date"),
                        Value = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Date, unique: true);
            
            CreateTable(
                "annPortal.TotalRevenues",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false, storeType: "date"),
                        Value = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Date, unique: true);
            
        }
        
        public override void Down()
        {
            DropIndex("annPortal.TotalRevenues", new[] { "Date" });
            DropIndex("annPortal.TotalProcedures", new[] { "Date" });
            DropTable("annPortal.TotalRevenues");
            DropTable("annPortal.TotalProcedures");
        }
    }
}
