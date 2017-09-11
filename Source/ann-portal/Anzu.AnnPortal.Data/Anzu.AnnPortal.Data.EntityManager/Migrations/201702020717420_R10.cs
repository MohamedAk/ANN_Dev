namespace Anzu.AnnPortal.Data.EntityManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class R10 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "annPortal.UserRoles",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 64),
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
            
            AddColumn("annPortal.Users", "UserRoleId", c => c.Long());
            CreateIndex("annPortal.Users", "UserRoleId");
            AddForeignKey("annPortal.Users", "UserRoleId", "annPortal.UserRoles", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("annPortal.Users", "UserRoleId", "annPortal.UserRoles");
            DropForeignKey("annPortal.UserRoles", "RecordStatusId", "metadata.RecordStatuses");
            DropIndex("annPortal.UserRoles", new[] { "RecordStatusId" });
            DropIndex("annPortal.Users", new[] { "UserRoleId" });
            DropColumn("annPortal.Users", "UserRoleId");
            DropTable("annPortal.UserRoles");
        }
    }
}
