namespace Anzu.AnnPortal.Data.EntityManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class R2 : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("annPortal.RelatedProcedures");
            AddColumn("annPortal.RelatedProcedures", "ProcedureId", c => c.Long(nullable: false));
            AddPrimaryKey("annPortal.RelatedProcedures", new[] { "RelatedProcedureId", "ProcedureId" });
            DropColumn("annPortal.RelatedProcedures", "ProdecureId");
        }

        public override void Down()
        {
            AddColumn("annPortal.RelatedProcedures", "ProdecureId", c => c.Long(nullable: false));
            DropPrimaryKey("annPortal.RelatedProcedures");
            DropColumn("annPortal.RelatedProcedures", "ProcedureId");
            AddPrimaryKey("annPortal.RelatedProcedures", new[] { "RelatedProcedureId", "ProdecureId" });
        }
    }
}
