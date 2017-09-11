namespace Anzu.AnnPortal.Data.EntityManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class R3 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "annPortal.Users",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        FirstName = c.String(nullable: false, maxLength: 64),
                        LastName = c.String(),
                        UserName = c.String(nullable: false, maxLength: 64),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PracticeId = c.Long(),
                        RecordStatusId = c.Int(nullable: false),
                        CreatedBy = c.String(maxLength: 50),
                        CreatedDate = c.DateTimeOffset(precision: 7),
                        ModifiedBy = c.String(maxLength: 50),
                        ModifiedDate = c.DateTimeOffset(precision: 7),
                        TimeStamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("annPortal.Practices", t => t.PracticeId)
                .ForeignKey("metadata.RecordStatuses", t => t.RecordStatusId)
                .Index(t => t.PracticeId)
                .Index(t => t.RecordStatusId);
            
            AddColumn("dbo.AspNetUsers", "FirstName", c => c.String(nullable: false, maxLength: 64));
            AddColumn("dbo.AspNetUsers", "LastName", c => c.String());
        }
        
        public override void Down()
        {
            DropForeignKey("annPortal.Users", "RecordStatusId", "metadata.RecordStatuses");
            DropForeignKey("annPortal.Users", "PracticeId", "annPortal.Practices");
            DropIndex("annPortal.Users", new[] { "RecordStatusId" });
            DropIndex("annPortal.Users", new[] { "PracticeId" });
            DropColumn("dbo.AspNetUsers", "LastName");
            DropColumn("dbo.AspNetUsers", "FirstName");
            DropTable("annPortal.Users");
        }
    }
}
