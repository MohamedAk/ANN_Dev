namespace Anzu.AnnPortal.Data.EntityManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class R19 : DbMigration
    {
        public override void Up()
        {
            AddColumn("annPortal.PracticeProcedures", "IsDiscarded", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("annPortal.PracticeProcedures", "IsDiscarded");
        }
    }
}
