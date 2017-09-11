namespace Anzu.AnnPortal.Data.EntityManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class R27 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "annPortal.PracticeActivationLog",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        EmrId = c.String(),
                        UpdatedDate = c.DateTime(nullable: false),
                        UserId = c.String(),
                        IsActivated = c.Boolean(nullable: false),
                        IsDataDeleted = c.Boolean(nullable: false),
                        RecordStatusId = c.Int(nullable: false),
                        CreatedBy = c.String(maxLength: 50),
                        CreatedDate = c.DateTimeOffset(precision: 7),
                        ModifiedBy = c.String(maxLength: 50),
                        ModifiedDate = c.DateTimeOffset(precision: 7),
                        TimeStamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("metadata.RecordStatuses", t => t.RecordStatusId)
                .Index(t => t.RecordStatusId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("annPortal.PracticeActivationLog", "RecordStatusId", "metadata.RecordStatuses");
            DropIndex("annPortal.PracticeActivationLog", new[] { "RecordStatusId" });
            DropTable("annPortal.PracticeActivationLog");
        }
    }
}
