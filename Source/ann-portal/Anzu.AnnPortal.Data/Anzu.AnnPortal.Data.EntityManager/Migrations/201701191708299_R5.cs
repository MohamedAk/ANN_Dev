namespace Anzu.AnnPortal.Data.EntityManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class R5 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("annPortal.Users", "PracticeId", "annPortal.Practices");
            DropForeignKey("annPortal.PracticeBrestImplants", "BIId", "annPortal.BrestImplants");
            DropForeignKey("annPortal.PracticeBrestImplants", "PracticeId", "annPortal.Practices");
            DropIndex("annPortal.Users", new[] { "PracticeId" });
            DropIndex("annPortal.PracticeBrestImplants", new[] { "PracticeId" });
            DropIndex("annPortal.PracticeBrestImplants", new[] { "BIId" });
        }
        
        public override void Down()
        {
            CreateIndex("annPortal.PracticeBrestImplants", "BIId");
            CreateIndex("annPortal.PracticeBrestImplants", "PracticeId");
            CreateIndex("annPortal.Users", "PracticeId");
            AddForeignKey("annPortal.PracticeBrestImplants", "PracticeId", "annPortal.Practices", "Id");
            AddForeignKey("annPortal.PracticeBrestImplants", "BIId", "annPortal.BrestImplants", "Id");
            AddForeignKey("annPortal.Users", "PracticeId", "annPortal.Practices", "Id");
        }
    }
}
