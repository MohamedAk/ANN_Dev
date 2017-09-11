namespace Anzu.AnnPortal.Data.EntityManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class R8 : DbMigration
    {
        public override void Up()
        {
            AddColumn("annPortal.Users", "RRUserId", c => c.String());
            AddColumn("annPortal.Users", "Email", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("annPortal.Users", "Email");
            DropColumn("annPortal.Users", "RRUserId");
        }
    }
}
