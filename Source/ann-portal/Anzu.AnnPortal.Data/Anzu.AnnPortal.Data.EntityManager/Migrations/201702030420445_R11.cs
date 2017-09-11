namespace Anzu.AnnPortal.Data.EntityManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class R11 : DbMigration
    {
        public override void Up()
        {
            AddColumn("annPortal.ProcedureLevels", "IsProduct", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("annPortal.ProcedureLevels", "IsProduct");
        }
    }
}
