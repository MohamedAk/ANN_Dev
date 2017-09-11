namespace Anzu.AnnPortal.Data.EntityManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class R15 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Companies",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ProductTypes",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("annPortal.PracticeProcedures", "CompanyId", c => c.Long());
            AddColumn("annPortal.PracticeProcedures", "ProductTypeId", c => c.Long());
            AddColumn("annPortal.PracticeProcedures", "IsProductSale", c => c.Boolean(nullable: false));
            CreateIndex("annPortal.PracticeProcedures", "CompanyId");
            CreateIndex("annPortal.PracticeProcedures", "ProductTypeId");
            AddForeignKey("annPortal.PracticeProcedures", "CompanyId", "dbo.Companies", "Id");
            AddForeignKey("annPortal.PracticeProcedures", "ProductTypeId", "dbo.ProductTypes", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("annPortal.PracticeProcedures", "ProductTypeId", "dbo.ProductTypes");
            DropForeignKey("annPortal.PracticeProcedures", "CompanyId", "dbo.Companies");
            DropIndex("annPortal.PracticeProcedures", new[] { "ProductTypeId" });
            DropIndex("annPortal.PracticeProcedures", new[] { "CompanyId" });
            DropColumn("annPortal.PracticeProcedures", "IsProductSale");
            DropColumn("annPortal.PracticeProcedures", "ProductTypeId");
            DropColumn("annPortal.PracticeProcedures", "CompanyId");
            DropTable("dbo.ProductTypes");
            DropTable("dbo.Companies");
        }
    }
}
