namespace Anzu.AnnPortal.Identity.Data.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class R4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "PracticeName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "PracticeName");
        }
    }
}
