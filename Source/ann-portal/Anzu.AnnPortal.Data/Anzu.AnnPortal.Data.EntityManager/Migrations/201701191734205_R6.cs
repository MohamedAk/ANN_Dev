namespace Anzu.AnnPortal.Data.EntityManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class R6 : DbMigration
    {
        public override void Up()
        {
            AddColumn("annPortal.Practices", "Address1", c => c.String());
            AddColumn("annPortal.Practices", "Address2", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("annPortal.Practices", "Address2");
            DropColumn("annPortal.Practices", "Address1");
        }
    }
}
