namespace Anzu.AnnPortal.Data.EntityManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class R25 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "annPortal.JobQueue",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserId = c.String(),
                        StartTime = c.DateTimeOffset(nullable: false, precision: 7),
                        EndTime = c.DateTimeOffset(precision: 7),
                        JobMode = c.Int(nullable: false),
                        JobStatusId = c.Int(nullable: false),
                        RecordStatusId = c.Int(nullable: false),
                        CreatedBy = c.String(maxLength: 50),
                        CreatedDate = c.DateTimeOffset(precision: 7),
                        ModifiedBy = c.String(maxLength: 50),
                        ModifiedDate = c.DateTimeOffset(precision: 7),
                        TimeStamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("annPortal.JobStatus", t => t.JobStatusId)
                .ForeignKey("metadata.RecordStatuses", t => t.RecordStatusId)
                .Index(t => t.JobStatusId)
                .Index(t => t.RecordStatusId);
            
            CreateTable(
                "annPortal.JobStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "annPortal.JobQueuePractice",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        JobQueueId = c.Long(nullable: false),
                        PracticeId = c.Long(nullable: false),
                        RecordStatusId = c.Int(nullable: false),
                        CreatedBy = c.String(maxLength: 50),
                        CreatedDate = c.DateTimeOffset(precision: 7),
                        ModifiedBy = c.String(maxLength: 50),
                        ModifiedDate = c.DateTimeOffset(precision: 7),
                        TimeStamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("annPortal.JobQueue", t => t.JobQueueId)
                .ForeignKey("annPortal.Practices", t => t.PracticeId)
                .ForeignKey("metadata.RecordStatuses", t => t.RecordStatusId)
                .Index(t => t.JobQueueId)
                .Index(t => t.PracticeId)
                .Index(t => t.RecordStatusId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("annPortal.JobQueuePractice", "RecordStatusId", "metadata.RecordStatuses");
            DropForeignKey("annPortal.JobQueuePractice", "PracticeId", "annPortal.Practices");
            DropForeignKey("annPortal.JobQueuePractice", "JobQueueId", "annPortal.JobQueue");
            DropForeignKey("annPortal.JobQueue", "RecordStatusId", "metadata.RecordStatuses");
            DropForeignKey("annPortal.JobQueue", "JobStatusId", "annPortal.JobStatus");
            DropIndex("annPortal.JobQueuePractice", new[] { "RecordStatusId" });
            DropIndex("annPortal.JobQueuePractice", new[] { "PracticeId" });
            DropIndex("annPortal.JobQueuePractice", new[] { "JobQueueId" });
            DropIndex("annPortal.JobQueue", new[] { "RecordStatusId" });
            DropIndex("annPortal.JobQueue", new[] { "JobStatusId" });
            DropTable("annPortal.JobQueuePractice");
            DropTable("annPortal.JobStatus");
            DropTable("annPortal.JobQueue");
        }
    }
}
