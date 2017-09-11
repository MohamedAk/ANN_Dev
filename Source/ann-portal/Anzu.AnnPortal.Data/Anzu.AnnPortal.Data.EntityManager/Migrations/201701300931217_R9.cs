namespace Anzu.AnnPortal.Data.EntityManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class R9 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("annPortal.Users", "FirstName", c => c.String(maxLength: 64));
        }
        
        public override void Down()
        {
            AlterColumn("annPortal.Users", "FirstName", c => c.String(nullable: false, maxLength: 64));
        }
    }
}
