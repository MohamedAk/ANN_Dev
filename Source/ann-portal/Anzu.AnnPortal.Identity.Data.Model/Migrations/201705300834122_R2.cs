namespace Anzu.AnnPortal.Identity.Data.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class R2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Modules", "PermissionCategoryId", "dbo.PermissionCategories");
            DropIndex("dbo.Modules", new[] { "PermissionCategoryId" });
            DropTable("dbo.PermissionCategories");
            DropTable("dbo.PreviousHubs");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.PreviousHubs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        HubId = c.Int(nullable: false),
                        UserId = c.String(),
                        CreatedDate = c.DateTime(),
                        CreatedBy = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PermissionCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.Modules", "PermissionCategoryId");
            AddForeignKey("dbo.Modules", "PermissionCategoryId", "dbo.PermissionCategories", "Id");
        }
    }
}
