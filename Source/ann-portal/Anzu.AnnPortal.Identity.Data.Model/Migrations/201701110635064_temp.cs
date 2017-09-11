namespace Anzu.AnnPortal.Identity.Data.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class temp : DbMigration
    {
        public override void Up()
        {
            // AddColumn("dbo.AspNetUsers", "PIN", c => c.Int());
        }
        
        public override void Down()
        {
            // DropColumn("dbo.AspNetUsers", "PIN");
        }
    }
}
