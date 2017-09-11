namespace Anzu.AnnPortal.Identity.Data.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class R5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "DocumentContent", c => c.Binary());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "DocumentContent");
        }
    }
}
