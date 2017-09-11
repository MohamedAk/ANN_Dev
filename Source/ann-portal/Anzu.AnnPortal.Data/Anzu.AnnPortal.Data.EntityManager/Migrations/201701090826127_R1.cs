namespace Anzu.AnnPortal.Data.EntityManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class R1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "annPortal.BrestImplants",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        RecordStatusId = c.Int(nullable: false),
                        CreatedBy = c.String(maxLength: 50),
                        CreatedDate = c.DateTimeOffset(precision: 7),
                        ModifiedBy = c.String(maxLength: 50),
                        ModifiedDate = c.DateTimeOffset(precision: 7),
                        TimeStamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("metadata.RecordStatuses", t => t.RecordStatusId)
                .Index(t => t.RecordStatusId);
            
            CreateTable(
                "metadata.RecordStatuses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 64),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "annPortal.Dashboards",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 64),
                        RecordStatusId = c.Int(nullable: false),
                        CreatedBy = c.String(maxLength: 50),
                        CreatedDate = c.DateTimeOffset(precision: 7),
                        ModifiedBy = c.String(maxLength: 50),
                        ModifiedDate = c.DateTimeOffset(precision: 7),
                        TimeStamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("metadata.RecordStatuses", t => t.RecordStatusId)
                .Index(t => t.RecordStatusId);
            
            CreateTable(
                "annPortal.PracticeBrestImplants",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        PracticeId = c.Long(),
                        BIId = c.Long(),
                        FromDate = c.DateTime(nullable: false),
                        ToDate = c.DateTime(nullable: false),
                        RecordStatusId = c.Int(nullable: false),
                        CreatedBy = c.String(maxLength: 50),
                        CreatedDate = c.DateTimeOffset(precision: 7),
                        ModifiedBy = c.String(maxLength: 50),
                        ModifiedDate = c.DateTimeOffset(precision: 7),
                        TimeStamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("annPortal.BrestImplants", t => t.BIId)
                .ForeignKey("annPortal.Practices", t => t.PracticeId)
                .ForeignKey("metadata.RecordStatuses", t => t.RecordStatusId)
                .Index(t => t.PracticeId)
                .Index(t => t.BIId)
                .Index(t => t.RecordStatusId);
            
            CreateTable(
                "annPortal.Practices",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        EmrId = c.String(),
                        Name = c.String(nullable: false),
                        RegionId = c.Int(),
                        ZipCodeId = c.Int(),
                        City = c.String(),
                        StateId = c.Int(),
                        IsActive = c.Boolean(nullable: false),
                        ContactNumber = c.String(),
                        ContactPerson = c.String(),
                        RecordStatusId = c.Int(nullable: false),
                        CreatedBy = c.String(maxLength: 50),
                        CreatedDate = c.DateTimeOffset(precision: 7),
                        ModifiedBy = c.String(maxLength: 50),
                        ModifiedDate = c.DateTimeOffset(precision: 7),
                        TimeStamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("metadata.RecordStatuses", t => t.RecordStatusId)
                .ForeignKey("metadata.States", t => t.StateId)
                .ForeignKey("metadata.ZipCodes", t => t.ZipCodeId)
                .Index(t => t.ZipCodeId)
                .Index(t => t.StateId)
                .Index(t => t.RecordStatusId);
            
            CreateTable(
                "metadata.States",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false),
                        Name = c.String(nullable: false),
                        RegionId = c.Long(),
                        RecordStatusId = c.Int(nullable: false),
                        CreatedBy = c.String(maxLength: 50),
                        CreatedDate = c.DateTimeOffset(precision: 7),
                        ModifiedBy = c.String(maxLength: 50),
                        ModifiedDate = c.DateTimeOffset(precision: 7),
                        TimeStamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("metadata.RecordStatuses", t => t.RecordStatusId)
                .ForeignKey("metadata.Regions", t => t.RegionId)
                .Index(t => t.RegionId)
                .Index(t => t.RecordStatusId);
            
            CreateTable(
                "metadata.Regions",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 64),
                        RecordStatusId = c.Int(nullable: false),
                        CreatedBy = c.String(maxLength: 50),
                        CreatedDate = c.DateTimeOffset(precision: 7),
                        ModifiedBy = c.String(maxLength: 50),
                        ModifiedDate = c.DateTimeOffset(precision: 7),
                        TimeStamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("metadata.RecordStatuses", t => t.RecordStatusId)
                .Index(t => t.RecordStatusId);
            
            CreateTable(
                "metadata.ZipCodes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false),
                        RecordStatusId = c.Int(nullable: false),
                        CreatedBy = c.String(maxLength: 50),
                        CreatedDate = c.DateTimeOffset(precision: 7),
                        ModifiedBy = c.String(maxLength: 50),
                        ModifiedDate = c.DateTimeOffset(precision: 7),
                        TimeStamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("metadata.RecordStatuses", t => t.RecordStatusId)
                .Index(t => t.RecordStatusId);
            
            CreateTable(
                "annPortal.PracticeProcedures",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        PracticeId = c.Long(),
                        EmrProcedure = c.String(),
                        ProcedureId = c.Long(),
                        RecordStatusId = c.Int(nullable: false),
                        CreatedBy = c.String(maxLength: 50),
                        CreatedDate = c.DateTimeOffset(precision: 7),
                        ModifiedBy = c.String(maxLength: 50),
                        ModifiedDate = c.DateTimeOffset(precision: 7),
                        TimeStamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("annPortal.Practices", t => t.PracticeId)
                .ForeignKey("annPortal.Procedures", t => t.ProcedureId)
                .ForeignKey("metadata.RecordStatuses", t => t.RecordStatusId)
                .Index(t => t.PracticeId)
                .Index(t => t.ProcedureId)
                .Index(t => t.RecordStatusId);
            
            CreateTable(
                "annPortal.Procedures",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 64),
                        IsUseProduct = c.Boolean(nullable: false),
                        ProcedureLevelOne = c.Long(),
                        ProcedureLevelTwo = c.Long(),
                        ProcedureLevelThree = c.Long(),
                        ProcedureLevelFour = c.Long(),
                        ProcedureLevelFive = c.Long(),
                        ProcedureLevelSix = c.Long(),
                        ProcedureLevelSeven = c.Long(),
                        PracticeProductFlag = c.Boolean(nullable: false),
                        RecordStatusId = c.Int(nullable: false),
                        CreatedBy = c.String(maxLength: 50),
                        CreatedDate = c.DateTimeOffset(precision: 7),
                        ModifiedBy = c.String(maxLength: 50),
                        ModifiedDate = c.DateTimeOffset(precision: 7),
                        TimeStamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("annPortal.ProcedureLevels", t => t.ProcedureLevelOne)
                .ForeignKey("annPortal.ProcedureLevels", t => t.ProcedureLevelTwo)
                .ForeignKey("annPortal.ProcedureLevels", t => t.ProcedureLevelThree)
                .ForeignKey("annPortal.ProcedureLevels", t => t.ProcedureLevelFour)
                .ForeignKey("annPortal.ProcedureLevels", t => t.ProcedureLevelFive)
                .ForeignKey("annPortal.ProcedureLevels", t => t.ProcedureLevelSix)
                .ForeignKey("annPortal.ProcedureLevels", t => t.ProcedureLevelSeven)
                .ForeignKey("metadata.RecordStatuses", t => t.RecordStatusId)
                .Index(t => t.ProcedureLevelOne)
                .Index(t => t.ProcedureLevelTwo)
                .Index(t => t.ProcedureLevelThree)
                .Index(t => t.ProcedureLevelFour)
                .Index(t => t.ProcedureLevelFive)
                .Index(t => t.ProcedureLevelSix)
                .Index(t => t.ProcedureLevelSeven)
                .Index(t => t.RecordStatusId);
            
            CreateTable(
                "annPortal.ProcedureLevels",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        LevelNumber = c.Long(nullable: false),
                        Name = c.String(nullable: false),
                        RecordStatusId = c.Int(nullable: false),
                        CreatedBy = c.String(maxLength: 50),
                        CreatedDate = c.DateTimeOffset(precision: 7),
                        ModifiedBy = c.String(maxLength: 50),
                        ModifiedDate = c.DateTimeOffset(precision: 7),
                        TimeStamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("metadata.RecordStatuses", t => t.RecordStatusId)
                .Index(t => t.RecordStatusId);
            
            CreateTable(
                "annPortal.RelatedProcedures",
                c => new
                    {
                        RelatedProcedureId = c.Long(nullable: false),
                        ProdecureId = c.Long(nullable: false),
                        RecordStatusId = c.Int(nullable: false),
                        CreatedBy = c.String(maxLength: 50),
                        CreatedDate = c.DateTimeOffset(precision: 7),
                        ModifiedBy = c.String(maxLength: 50),
                        ModifiedDate = c.DateTimeOffset(precision: 7),
                        TimeStamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => new { t.RelatedProcedureId, t.ProdecureId })
                .ForeignKey("annPortal.Procedures", t => t.RelatedProcedureId)
                .ForeignKey("metadata.RecordStatuses", t => t.RecordStatusId)
                .Index(t => t.RelatedProcedureId)
                .Index(t => t.RecordStatusId);
            
            CreateTable(
                "annPortal.RoleDashboards",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        RoleId = c.Long(),
                        DahsboardId = c.Long(),
                        RecordStatusId = c.Int(nullable: false),
                        CreatedBy = c.String(maxLength: 50),
                        CreatedDate = c.DateTimeOffset(precision: 7),
                        ModifiedBy = c.String(maxLength: 50),
                        ModifiedDate = c.DateTimeOffset(precision: 7),
                        TimeStamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("annPortal.Dashboards", t => t.DahsboardId)
                .ForeignKey("metadata.RecordStatuses", t => t.RecordStatusId)
                .ForeignKey("annPortal.Roles", t => t.RoleId)
                .Index(t => t.RoleId)
                .Index(t => t.DahsboardId)
                .Index(t => t.RecordStatusId);
            
            CreateTable(
                "annPortal.Roles",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 64),
                        RecordStatusId = c.Int(nullable: false),
                        CreatedBy = c.String(maxLength: 50),
                        CreatedDate = c.DateTimeOffset(precision: 7),
                        ModifiedBy = c.String(maxLength: 50),
                        ModifiedDate = c.DateTimeOffset(precision: 7),
                        TimeStamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("metadata.RecordStatuses", t => t.RecordStatusId)
                .Index(t => t.RecordStatusId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                        Description = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        RRUserId = c.String(),
                        PracticeId = c.Long(),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("annPortal.Practices", t => t.PracticeId)
                .Index(t => t.PracticeId)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "PracticeId", "annPortal.Practices");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("annPortal.RoleDashboards", "RoleId", "annPortal.Roles");
            DropForeignKey("annPortal.Roles", "RecordStatusId", "metadata.RecordStatuses");
            DropForeignKey("annPortal.RoleDashboards", "RecordStatusId", "metadata.RecordStatuses");
            DropForeignKey("annPortal.RoleDashboards", "DahsboardId", "annPortal.Dashboards");
            DropForeignKey("annPortal.RelatedProcedures", "RecordStatusId", "metadata.RecordStatuses");
            DropForeignKey("annPortal.RelatedProcedures", "RelatedProcedureId", "annPortal.Procedures");
            DropForeignKey("annPortal.PracticeProcedures", "RecordStatusId", "metadata.RecordStatuses");
            DropForeignKey("annPortal.PracticeProcedures", "ProcedureId", "annPortal.Procedures");
            DropForeignKey("annPortal.Procedures", "RecordStatusId", "metadata.RecordStatuses");
            DropForeignKey("annPortal.Procedures", "ProcedureLevelSeven", "annPortal.ProcedureLevels");
            DropForeignKey("annPortal.Procedures", "ProcedureLevelSix", "annPortal.ProcedureLevels");
            DropForeignKey("annPortal.Procedures", "ProcedureLevelFive", "annPortal.ProcedureLevels");
            DropForeignKey("annPortal.Procedures", "ProcedureLevelFour", "annPortal.ProcedureLevels");
            DropForeignKey("annPortal.Procedures", "ProcedureLevelThree", "annPortal.ProcedureLevels");
            DropForeignKey("annPortal.Procedures", "ProcedureLevelTwo", "annPortal.ProcedureLevels");
            DropForeignKey("annPortal.Procedures", "ProcedureLevelOne", "annPortal.ProcedureLevels");
            DropForeignKey("annPortal.ProcedureLevels", "RecordStatusId", "metadata.RecordStatuses");
            DropForeignKey("annPortal.PracticeProcedures", "PracticeId", "annPortal.Practices");
            DropForeignKey("annPortal.PracticeBrestImplants", "RecordStatusId", "metadata.RecordStatuses");
            DropForeignKey("annPortal.PracticeBrestImplants", "PracticeId", "annPortal.Practices");
            DropForeignKey("annPortal.Practices", "ZipCodeId", "metadata.ZipCodes");
            DropForeignKey("metadata.ZipCodes", "RecordStatusId", "metadata.RecordStatuses");
            DropForeignKey("annPortal.Practices", "StateId", "metadata.States");
            DropForeignKey("metadata.States", "RegionId", "metadata.Regions");
            DropForeignKey("metadata.Regions", "RecordStatusId", "metadata.RecordStatuses");
            DropForeignKey("metadata.States", "RecordStatusId", "metadata.RecordStatuses");
            DropForeignKey("annPortal.Practices", "RecordStatusId", "metadata.RecordStatuses");
            DropForeignKey("annPortal.PracticeBrestImplants", "BIId", "annPortal.BrestImplants");
            DropForeignKey("annPortal.Dashboards", "RecordStatusId", "metadata.RecordStatuses");
            DropForeignKey("annPortal.BrestImplants", "RecordStatusId", "metadata.RecordStatuses");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUsers", new[] { "PracticeId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("annPortal.Roles", new[] { "RecordStatusId" });
            DropIndex("annPortal.RoleDashboards", new[] { "RecordStatusId" });
            DropIndex("annPortal.RoleDashboards", new[] { "DahsboardId" });
            DropIndex("annPortal.RoleDashboards", new[] { "RoleId" });
            DropIndex("annPortal.RelatedProcedures", new[] { "RecordStatusId" });
            DropIndex("annPortal.RelatedProcedures", new[] { "RelatedProcedureId" });
            DropIndex("annPortal.ProcedureLevels", new[] { "RecordStatusId" });
            DropIndex("annPortal.Procedures", new[] { "RecordStatusId" });
            DropIndex("annPortal.Procedures", new[] { "ProcedureLevelSeven" });
            DropIndex("annPortal.Procedures", new[] { "ProcedureLevelSix" });
            DropIndex("annPortal.Procedures", new[] { "ProcedureLevelFive" });
            DropIndex("annPortal.Procedures", new[] { "ProcedureLevelFour" });
            DropIndex("annPortal.Procedures", new[] { "ProcedureLevelThree" });
            DropIndex("annPortal.Procedures", new[] { "ProcedureLevelTwo" });
            DropIndex("annPortal.Procedures", new[] { "ProcedureLevelOne" });
            DropIndex("annPortal.PracticeProcedures", new[] { "RecordStatusId" });
            DropIndex("annPortal.PracticeProcedures", new[] { "ProcedureId" });
            DropIndex("annPortal.PracticeProcedures", new[] { "PracticeId" });
            DropIndex("metadata.ZipCodes", new[] { "RecordStatusId" });
            DropIndex("metadata.Regions", new[] { "RecordStatusId" });
            DropIndex("metadata.States", new[] { "RecordStatusId" });
            DropIndex("metadata.States", new[] { "RegionId" });
            DropIndex("annPortal.Practices", new[] { "RecordStatusId" });
            DropIndex("annPortal.Practices", new[] { "StateId" });
            DropIndex("annPortal.Practices", new[] { "ZipCodeId" });
            DropIndex("annPortal.PracticeBrestImplants", new[] { "RecordStatusId" });
            DropIndex("annPortal.PracticeBrestImplants", new[] { "BIId" });
            DropIndex("annPortal.PracticeBrestImplants", new[] { "PracticeId" });
            DropIndex("annPortal.Dashboards", new[] { "RecordStatusId" });
            DropIndex("annPortal.BrestImplants", new[] { "RecordStatusId" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("annPortal.Roles");
            DropTable("annPortal.RoleDashboards");
            DropTable("annPortal.RelatedProcedures");
            DropTable("annPortal.ProcedureLevels");
            DropTable("annPortal.Procedures");
            DropTable("annPortal.PracticeProcedures");
            DropTable("metadata.ZipCodes");
            DropTable("metadata.Regions");
            DropTable("metadata.States");
            DropTable("annPortal.Practices");
            DropTable("annPortal.PracticeBrestImplants");
            DropTable("annPortal.Dashboards");
            DropTable("metadata.RecordStatuses");
            DropTable("annPortal.BrestImplants");
        }
    }
}
