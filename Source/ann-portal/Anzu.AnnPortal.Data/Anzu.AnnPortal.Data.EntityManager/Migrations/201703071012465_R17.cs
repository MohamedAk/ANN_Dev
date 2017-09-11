namespace Anzu.AnnPortal.Data.EntityManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class R17 : DbMigration
    {
        public override void Up()
        {
       
            AddColumn("dbo.Companies", "RecordStatusId", c => c.Int(nullable: false));
            AddColumn("dbo.Companies", "CreatedBy", c => c.String(maxLength: 50));
            AddColumn("dbo.Companies", "CreatedDate", c => c.DateTimeOffset(precision: 7));
            AddColumn("dbo.Companies", "ModifiedBy", c => c.String(maxLength: 50));
            AddColumn("dbo.Companies", "ModifiedDate", c => c.DateTimeOffset(precision: 7));
            AddColumn("dbo.Companies", "TimeStamp", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AddColumn("dbo.ProductTypes", "RecordStatusId", c => c.Int(nullable: false));
            AddColumn("dbo.ProductTypes", "CreatedBy", c => c.String(maxLength: 50));
            AddColumn("dbo.ProductTypes", "CreatedDate", c => c.DateTimeOffset(precision: 7));
            AddColumn("dbo.ProductTypes", "ModifiedBy", c => c.String(maxLength: 50));
            AddColumn("dbo.ProductTypes", "ModifiedDate", c => c.DateTimeOffset(precision: 7));
            AddColumn("dbo.ProductTypes", "TimeStamp", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            Sql("UPDATE dbo.Companies SET RecordStatusId = 1 ");
            Sql("UPDATE dbo.ProductTypes SET RecordStatusId = 1 ");
            CreateIndex("dbo.Companies", "RecordStatusId");
            CreateIndex("dbo.ProductTypes", "RecordStatusId");
            AddForeignKey("dbo.Companies", "RecordStatusId", "metadata.RecordStatuses", "Id");
            AddForeignKey("dbo.ProductTypes", "RecordStatusId", "metadata.RecordStatuses", "Id");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProductTypes", "RecordStatusId", "metadata.RecordStatuses");
            DropForeignKey("dbo.Companies", "RecordStatusId", "metadata.RecordStatuses");
            DropIndex("dbo.ProductTypes", new[] { "RecordStatusId" });
            DropIndex("dbo.Companies", new[] { "RecordStatusId" });
            DropColumn("dbo.ProductTypes", "TimeStamp");
            DropColumn("dbo.ProductTypes", "ModifiedDate");
            DropColumn("dbo.ProductTypes", "ModifiedBy");
            DropColumn("dbo.ProductTypes", "CreatedDate");
            DropColumn("dbo.ProductTypes", "CreatedBy");
            DropColumn("dbo.ProductTypes", "RecordStatusId");
            DropColumn("dbo.Companies", "TimeStamp");
            DropColumn("dbo.Companies", "ModifiedDate");
            DropColumn("dbo.Companies", "ModifiedBy");
            DropColumn("dbo.Companies", "CreatedDate");
            DropColumn("dbo.Companies", "CreatedBy");
            DropColumn("dbo.Companies", "RecordStatusId");
        }
    }
}
