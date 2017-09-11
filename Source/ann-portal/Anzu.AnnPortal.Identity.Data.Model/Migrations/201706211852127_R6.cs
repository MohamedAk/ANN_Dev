namespace Anzu.AnnPortal.Identity.Data.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class R6 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "PracticeId", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "PracticeId");
        }
    }
}
