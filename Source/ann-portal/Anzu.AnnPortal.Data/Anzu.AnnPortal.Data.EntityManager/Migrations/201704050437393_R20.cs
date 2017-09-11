namespace Anzu.AnnPortal.Data.EntityManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class R20 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("annPortal.PracticeBrestImplants", "ToDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("annPortal.PracticeBrestImplants", "ToDate", c => c.DateTime(nullable: false));
        }
    }
}
