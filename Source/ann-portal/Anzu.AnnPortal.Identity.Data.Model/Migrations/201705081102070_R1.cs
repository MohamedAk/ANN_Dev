namespace Anzu.AnnPortal.Identity.Data.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class R1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Designations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        StatusId = c.Int(),
                        DisplayName = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Status", t => t.StatusId)
                .Index(t => t.StatusId);
            
            CreateTable(
                "dbo.Status",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.LoginAudits",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        LoginName = c.String(),
                        LoginAudiStatus = c.Int(nullable: false),
                        LoginDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.LoginAuditStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Modules",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PermissionCategoryId = c.Int(),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PermissionCategories", t => t.PermissionCategoryId)
                .Index(t => t.PermissionCategoryId);
            
            CreateTable(
                "dbo.PermissionCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Permissions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ModuleId = c.Int(nullable: false),
                        Code = c.String(nullable: false),
                        Name = c.String(nullable: false),
                        StatusId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Modules", t => t.ModuleId)
                .ForeignKey("dbo.Status", t => t.StatusId)
                .Index(t => t.ModuleId)
                .Index(t => t.StatusId);
            
            CreateTable(
                "dbo.Organizations",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Name = c.String(nullable: false),
                        StatusId = c.Int(nullable: false),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Status", t => t.StatusId)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .Index(t => t.StatusId)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.PreviousHubs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        HubId = c.Int(nullable: false),
                        UserId = c.String(),
                        CreatedDate = c.DateTime(),
                        CreatedBy = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PreviousPasswords",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PasswordHash = c.String(),
                        CreateDate = c.DateTimeOffset(nullable: false, precision: 7),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
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
                        StatusId = c.Int(),
                        FirstName = c.String(maxLength: 20),
                        LastName = c.String(maxLength: 20),
                        OrganizationId = c.Int(),
                        IsForceToChangePassword = c.Boolean(),
                        DesignationId = c.Int(),
                        UserDesignation = c.String(maxLength: 30),
                        CreatedBy = c.String(),
                        CreatedDateTime = c.DateTime(),
                        DigitalSignature = c.Binary(),
                        ModifiedBy = c.String(),
                        ModifiedDateTime = c.DateTime(),
                        IsSecurityQuestionAnswered = c.Boolean(),
                        PIN = c.Int(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Designations", t => t.DesignationId)
                .ForeignKey("dbo.Organizations", t => t.OrganizationId)
                .ForeignKey("dbo.Status", t => t.StatusId)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex")
                .Index(t => t.StatusId)
                .Index(t => t.OrganizationId)
                .Index(t => t.DesignationId);
            
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
                "dbo.RolePermissions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PermissionId = c.Int(nullable: false),
                        ApplicationRoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetRoles", t => t.ApplicationRoleId)
                .ForeignKey("dbo.Permissions", t => t.PermissionId)
                .Index(t => t.PermissionId)
                .Index(t => t.ApplicationRoleId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                        StatusId = c.Int(),
                        CreatedBy = c.String(),
                        CreatedDateTime = c.DateTime(),
                        ModifiedBy = c.String(),
                        ModifiedDateTime = c.DateTime(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Status", t => t.StatusId)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex")
                .Index(t => t.StatusId);
            
            CreateTable(
                "dbo.SecurityQuestions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Question = c.String(maxLength: 250),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserOrganizations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OrganizationId = c.Int(nullable: false),
                        ApplicationUserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId)
                .ForeignKey("dbo.Organizations", t => t.OrganizationId)
                .Index(t => t.OrganizationId)
                .Index(t => t.ApplicationUserId);
            
            CreateTable(
                "dbo.UserRoles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        RoleId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.ApplicationUserSecurityQuestions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        QuestionId = c.Int(nullable: false),
                        QuestionAnswer = c.String(maxLength: 40),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SecurityQuestions", t => t.QuestionId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.QuestionId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ApplicationUserSecurityQuestions", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ApplicationUserSecurityQuestions", "QuestionId", "dbo.SecurityQuestions");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.UserOrganizations", "OrganizationId", "dbo.Organizations");
            DropForeignKey("dbo.UserOrganizations", "ApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.RolePermissions", "PermissionId", "dbo.Permissions");
            DropForeignKey("dbo.AspNetRoles", "StatusId", "dbo.Status");
            DropForeignKey("dbo.RolePermissions", "ApplicationRoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.AspNetUsers", "StatusId", "dbo.Status");
            DropForeignKey("dbo.Organizations", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.PreviousPasswords", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "OrganizationId", "dbo.Organizations");
            DropForeignKey("dbo.AspNetUsers", "DesignationId", "dbo.Designations");
            DropForeignKey("dbo.Organizations", "StatusId", "dbo.Status");
            DropForeignKey("dbo.Permissions", "StatusId", "dbo.Status");
            DropForeignKey("dbo.Permissions", "ModuleId", "dbo.Modules");
            DropForeignKey("dbo.Modules", "PermissionCategoryId", "dbo.PermissionCategories");
            DropForeignKey("dbo.Designations", "StatusId", "dbo.Status");
            DropIndex("dbo.ApplicationUserSecurityQuestions", new[] { "QuestionId" });
            DropIndex("dbo.ApplicationUserSecurityQuestions", new[] { "UserId" });
            DropIndex("dbo.UserRoles", new[] { "RoleId" });
            DropIndex("dbo.UserRoles", new[] { "UserId" });
            DropIndex("dbo.UserOrganizations", new[] { "ApplicationUserId" });
            DropIndex("dbo.UserOrganizations", new[] { "OrganizationId" });
            DropIndex("dbo.AspNetRoles", new[] { "StatusId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.RolePermissions", new[] { "ApplicationRoleId" });
            DropIndex("dbo.RolePermissions", new[] { "PermissionId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", new[] { "DesignationId" });
            DropIndex("dbo.AspNetUsers", new[] { "OrganizationId" });
            DropIndex("dbo.AspNetUsers", new[] { "StatusId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.PreviousPasswords", new[] { "UserId" });
            DropIndex("dbo.Organizations", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Organizations", new[] { "StatusId" });
            DropIndex("dbo.Permissions", new[] { "StatusId" });
            DropIndex("dbo.Permissions", new[] { "ModuleId" });
            DropIndex("dbo.Modules", new[] { "PermissionCategoryId" });
            DropIndex("dbo.Designations", new[] { "StatusId" });
            DropTable("dbo.ApplicationUserSecurityQuestions");
            DropTable("dbo.UserRoles");
            DropTable("dbo.UserOrganizations");
            DropTable("dbo.SecurityQuestions");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.RolePermissions");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.PreviousPasswords");
            DropTable("dbo.PreviousHubs");
            DropTable("dbo.Organizations");
            DropTable("dbo.Permissions");
            DropTable("dbo.PermissionCategories");
            DropTable("dbo.Modules");
            DropTable("dbo.LoginAuditStatus");
            DropTable("dbo.LoginAudits");
            DropTable("dbo.Status");
            DropTable("dbo.Designations");
        }
    }
}
