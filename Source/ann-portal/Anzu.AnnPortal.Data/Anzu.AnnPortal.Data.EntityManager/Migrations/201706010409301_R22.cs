namespace Anzu.AnnPortal.Data.EntityManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class R22 : DbMigration
    {
        public override void Up()
        {
            AddColumn("annPortal.PracticeEditInformation", "IsBreastImplantUpdated", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("annPortal.PracticeEditInformation", "IsBreastImplantUpdated");
        }
    }
}
