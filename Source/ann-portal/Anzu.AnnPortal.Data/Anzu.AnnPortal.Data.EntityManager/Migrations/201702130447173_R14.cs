namespace Anzu.AnnPortal.Data.EntityManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class R14 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("annPortal.ProcedureMappings", "RecordStatusId", "metadata.RecordStatuses");
            DropIndex("annPortal.ProcedureMappings", new[] { "RecordStatusId" });
            DropTable("annPortal.ProcedureMappings");
        }
        
        public override void Down()
        {
            CreateTable(
                "annPortal.ProcedureMappings",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        LevelOneId = c.Long(nullable: false),
                        LevelOneName = c.String(),
                        LevelTwoId = c.Long(nullable: false),
                        LevelTwoName = c.String(),
                        LevelThreeId = c.Long(nullable: false),
                        LevelThreeName = c.String(),
                        LevelFourId = c.Long(nullable: false),
                        LevelFourName = c.String(),
                        RecordStatusId = c.Int(nullable: false),
                        CreatedBy = c.String(maxLength: 50),
                        CreatedDate = c.DateTimeOffset(precision: 7),
                        ModifiedBy = c.String(maxLength: 50),
                        ModifiedDate = c.DateTimeOffset(precision: 7),
                        TimeStamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("annPortal.ProcedureMappings", "RecordStatusId");
            AddForeignKey("annPortal.ProcedureMappings", "RecordStatusId", "metadata.RecordStatuses", "Id");
        }
    }
}
