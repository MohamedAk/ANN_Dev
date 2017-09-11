namespace Anzu.AnnPortal.Data.EntityManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class R24 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("annPortal.Users", "UserName", c => c.String(nullable: false, maxLength: 250));
        }
        
        public override void Down()
        {
            AlterColumn("annPortal.Users", "UserName", c => c.String(nullable: false, maxLength: 64));
        }
    }
}
