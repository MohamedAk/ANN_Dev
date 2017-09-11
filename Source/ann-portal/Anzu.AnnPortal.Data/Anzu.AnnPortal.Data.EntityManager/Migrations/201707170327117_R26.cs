namespace Anzu.AnnPortal.Data.EntityManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class R26 : DbMigration
    {
        public override void Up()
        {
            AddColumn("annPortal.JobQueuePractice", "EmrId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("annPortal.JobQueuePractice", "EmrId");
        }
    }
}
