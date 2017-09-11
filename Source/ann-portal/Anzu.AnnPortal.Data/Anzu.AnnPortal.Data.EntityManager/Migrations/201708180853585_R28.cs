namespace Anzu.AnnPortal.Data.EntityManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class R28 : DbMigration
    {
        public override void Up()
        {
            AddColumn("annPortal.PracticeActivationLog", "IsRefreshLater", c => c.Boolean(nullable: false));
            AddColumn("annPortal.PracticeActivationLog", "IsCubeRefreshed", c => c.Boolean(nullable: true));
        }
        
        public override void Down()
        {
            DropColumn("annPortal.PracticeActivationLog", "IsCubeRefreshed");
            DropColumn("annPortal.PracticeActivationLog", "IsRefreshLater");
        }
    }
}
