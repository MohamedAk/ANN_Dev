namespace Anzu.AnnPortal.Data.EntityManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class R16 : DbMigration
    {
        public override void Up()
        {
            AddColumn("annPortal.Users", "DocumentContent", c => c.Binary());
        }
        
        public override void Down()
        {
            DropColumn("annPortal.Users", "DocumentContent");
        }
    }
}
