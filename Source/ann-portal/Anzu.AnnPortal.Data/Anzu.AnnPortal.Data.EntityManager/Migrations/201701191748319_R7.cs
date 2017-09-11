namespace Anzu.AnnPortal.Data.EntityManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class R7 : DbMigration
    {
        public override void Up()
        {
            AddColumn("annPortal.Practices", "AddressLine1", c => c.String());
            AddColumn("annPortal.Practices", "AddressLine2", c => c.String());
            DropColumn("annPortal.Practices", "Address1");
            DropColumn("annPortal.Practices", "Address2");
        }
        
        public override void Down()
        {
            AddColumn("annPortal.Practices", "Address2", c => c.String());
            AddColumn("annPortal.Practices", "Address1", c => c.String());
            DropColumn("annPortal.Practices", "AddressLine2");
            DropColumn("annPortal.Practices", "AddressLine1");
        }
    }
}
