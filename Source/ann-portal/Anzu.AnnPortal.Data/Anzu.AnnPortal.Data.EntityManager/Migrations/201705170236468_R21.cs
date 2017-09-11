namespace Anzu.AnnPortal.Data.EntityManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class R21 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "annPortal.PracticeEditInformation",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        PracticeId = c.Long(nullable: false),
                        EmrId = c.String(),
                        IsEMRMappingUpdated = c.Boolean(),
                        EMRMappingUpdatedTime = c.DateTimeOffset(nullable: false, precision: 7),
                        IsCubeUpdated = c.Boolean(),
                        CubeUpdatedTime = c.DateTimeOffset(precision: 7),
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
            DropForeignKey("annPortal.PracticeEditInformation", "RecordStatusId", "metadata.RecordStatuses");
            DropIndex("annPortal.PracticeEditInformation", new[] { "RecordStatusId" });
            DropTable("annPortal.PracticeEditInformation");
        }
    }
}
