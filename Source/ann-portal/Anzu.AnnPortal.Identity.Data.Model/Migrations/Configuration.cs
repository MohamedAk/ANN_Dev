namespace Anzu.AnnPortal.Identity.Data.Model.Migrations
{
    using Anzu.AnnPortal.Identity.Common.Model.Enum;
    using Anzu.AnnPortal.Identity.Data.Model.Models;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Anzu.AnnPortal.Identity.Data.Model.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(Anzu.AnnPortal.Identity.Data.Model.ApplicationDbContext context)
        {

            //context.Module.AddOrUpdate(new Module { Id = 1, Name = "Sites" });
            //context.Module.AddOrUpdate(new Module { Id = 2, Name = "Encounter Documentation" });
            //context.Module.AddOrUpdate(new Module { Id = 3, Name = "Contacts" });
            //context.Module.AddOrUpdate(new Module { Id = 4, Name = "PI Projects" });
            //context.Module.AddOrUpdate(new Module { Id = 5, Name = "Reports" });
            //context.Module.AddOrUpdate(new Module { Id = 6, Name = "Fax" });

            context.Status.AddOrUpdate(new Status { Id = (int)StatusType.Activate, Description = "Active" });
            context.Status.AddOrUpdate(new Status { Id = (int)StatusType.Deactivate, Description = "Deactivated" });

            //context.Permissions.AddOrUpdate(new Permission { Id = 1, ModuleId = 1, Code = "VW-ST-LST", Name = "View Site List", StatusId = (int)StatusType.Activate });
            //context.Permissions.AddOrUpdate(new Permission { Id = 2, ModuleId = 1, Code = "VW-ST-BK", Name = "View Site Book", StatusId = (int)StatusType.Activate });
            //context.Permissions.AddOrUpdate(new Permission { Id = 3, ModuleId = 1, Code = "CRT-ST", Name = "Create Sites", StatusId = (int)StatusType.Activate });
            //context.Permissions.AddOrUpdate(new Permission { Id = 4, ModuleId = 1, Code = "EDT-BSC-ST-INFO", Name = "Edit Basic Site Information", StatusId = (int)StatusType.Activate });
            //context.Permissions.AddOrUpdate(new Permission { Id = 5, ModuleId = 1, Code = "DACT-ST", Name = "Deactivate Sites", StatusId = (int)StatusType.Activate });
            //context.Permissions.AddOrUpdate(new Permission { Id = 6, ModuleId = 1, Code = "DLT-ST", Name = "Delete Sites", StatusId = (int)StatusType.Activate });
            //context.Permissions.AddOrUpdate(new Permission { Id = 7, ModuleId = 1, Code = "MNG-ST-CNT", Name = "Manage Site Content", StatusId = (int)StatusType.Activate });
            //context.Permissions.AddOrUpdate(new Permission { Id = 8, ModuleId = 1, Code = "MNG-PRC", Name = "Manage Protocols", StatusId = (int)StatusType.Activate });

            //context.Permissions.AddOrUpdate(new Permission { Id = 9, ModuleId = 2, Code = "VW-ENC-DOC", Name = "View Encounter Documents", StatusId = (int)StatusType.Activate });
            //context.Permissions.AddOrUpdate(new Permission { Id = 10, ModuleId = 2, Code = "CRT-ENC", Name = "Create Encounters", StatusId = (int)StatusType.Activate });
            //context.Permissions.AddOrUpdate(new Permission { Id = 11, ModuleId = 2, Code = "EDT-ENC", Name = "Edit Encounters", StatusId = (int)StatusType.Activate });
            //context.Permissions.AddOrUpdate(new Permission { Id = 12, ModuleId = 2, Code = "DLT-INC-ENC", Name = "Delete Incomplete Encounters", StatusId = (int)StatusType.Activate });
            //context.Permissions.AddOrUpdate(new Permission { Id = 13, ModuleId = 2, Code = "DGT-SIG-CAP", Name = "Electronic Signature Capability", StatusId = (int)StatusType.Activate });


            //context.Permissions.AddOrUpdate(new Permission { Id = 14, ModuleId = 3, Code = "VW-CNTC-LST", Name = "View Contact List", StatusId = (int)StatusType.Activate });
            //context.Permissions.AddOrUpdate(new Permission { Id = 15, ModuleId = 3, Code = "CRT-CNTC", Name = "Create Contacts", StatusId = (int)StatusType.Activate });
            //context.Permissions.AddOrUpdate(new Permission { Id = 16, ModuleId = 3, Code = "EDT-CNTC", Name = "Edit Contacts", StatusId = (int)StatusType.Activate });
            //context.Permissions.AddOrUpdate(new Permission { Id = 17, ModuleId = 3, Code = "DLT-CNTC", Name = "Delete Contacts", StatusId = (int)StatusType.Activate });

            //context.Permissions.AddOrUpdate(new Permission { Id = 18, ModuleId = 4, Code = "VW-PI-LST", Name = "View PI Project list", StatusId = (int)StatusType.Activate });
            //context.Permissions.AddOrUpdate(new Permission { Id = 19, ModuleId = 4, Code = "VW-PI-PRJ", Name = "View PI Projects", StatusId = (int)StatusType.Activate });
            //context.Permissions.AddOrUpdate(new Permission { Id = 20, ModuleId = 4, Code = "CHRT-ABS-VAL", Name = "Chart Abstraction and Validation", StatusId = (int)StatusType.Activate });
            //context.Permissions.AddOrUpdate(new Permission { Id = 21, ModuleId = 4, Code = "CLI-VAL", Name = "Clinical Validation", StatusId = (int)StatusType.Activate });


            //context.Permissions.AddOrUpdate(new Permission { Id = 22, ModuleId = 6, Code = "CRT-FAX", Name = "Create New Fax", StatusId = (int)StatusType.Activate });
            //context.Permissions.AddOrUpdate(new Permission { Id = 23, ModuleId = 6, Code = "VW-FAX", Name = "View/Download Faxes", StatusId = (int)StatusType.Activate });
            //context.Permissions.AddOrUpdate(new Permission { Id = 24, ModuleId = 6, Code = "ATCH-DOC", Name = "Attach to Documents", StatusId = (int)StatusType.Activate });
            //context.Permissions.AddOrUpdate(new Permission { Id = 25, ModuleId = 6, Code = "ATCH-ST", Name = "Attach to Sites", StatusId = (int)StatusType.Activate });


            context.Organizations.AddOrUpdate(new Organization { Id = 1, Name = "General", StatusId = (int)StatusType.Activate });

            context.Designation.AddOrUpdate(new Designation { Id = 1, Name = "ADMINISTRATOR", StatusId = (int)StatusType.Activate });
            context.Designation.AddOrUpdate(new Designation { Id = 2, Name = "SUPER_ADMIN", StatusId = (int)StatusType.Activate });
            context.Designation.AddOrUpdate(new Designation { Id = 3, Name = "ASAP_USER", StatusId = (int)StatusType.Activate });
            context.Designation.AddOrUpdate(new Designation { Id = 4, Name = "PRACTICE_USER", StatusId = (int)StatusType.Activate });

            context.SecurityQuestion.AddOrUpdate(new SecurityQuestion { Id = 1, Question = "Which phone number do you remember most from your childhood?" });
            context.SecurityQuestion.AddOrUpdate(new SecurityQuestion { Id = 2, Question = "In what city were you born?" });
            context.SecurityQuestion.AddOrUpdate(new SecurityQuestion { Id = 3, Question = "What is the name of your first school?" });
            context.SecurityQuestion.AddOrUpdate(new SecurityQuestion { Id = 4, Question = "What is your mother's maiden name?" });
            context.SecurityQuestion.AddOrUpdate(new SecurityQuestion { Id = 5, Question = "What street did you grow up on?" });
            context.SecurityQuestion.AddOrUpdate(new SecurityQuestion { Id = 6, Question = "What was the make of your first car?" });
            context.SecurityQuestion.AddOrUpdate(new SecurityQuestion { Id = 7, Question = "What is your father's middle name?" });
            context.SecurityQuestion.AddOrUpdate(new SecurityQuestion { Id = 8, Question = "What is the name of your first grade teacher?" });

            context.SaveChanges();

            #region Application User Add

            ApplicationUser appUser = new ApplicationUser();
            appUser.Email = "bi3dev_notification@brandixithree.com";
            appUser.EmailConfirmed = true;
            appUser.FirstName = "System";
            appUser.LastName = "Administrator";
            appUser.OrganizationId = 1;
            appUser.DesignationId = 2;
            appUser.UserDesignation = "System Administrator";
            appUser.IsForceToChangePassword = false;
            appUser.PasswordHash = "AAzwVYFhXiZQiq2ym7mAsT7jHbrfLoNyT1Qdg7t2uavN1AXE/4Rlp+f25xnH8fPF9A==";
            appUser.UserName = "SysAdmin";
            appUser.StatusId = (int)StatusType.Activate;
            appUser.CreatedBy = "Shifka";
            appUser.CreatedDateTime = DateTime.UtcNow;
            appUser.Id = "068a6aa6-090a-4c3f-944d-4ff81605e3e8";
            appUser.SecurityStamp = "07b0b964-d725-4254-b56e-d908c4dab608";

            context.Users.AddOrUpdate(appUser);
            context.SaveChanges();

            ApplicationRole role = new ApplicationRole();
            role.Id = "924eb461-d630-4d20-a6bf-bfe6a1ba5080";
            role.Name = "Administrator";
            role.StatusId = (int)StatusType.Activate;
            role.CreatedBy = "SysAdmin";
            role.CreatedDateTime = DateTime.UtcNow;
            context.Roles.AddOrUpdate(role);

            ApplicationRole superAdminRole = new ApplicationRole();
            superAdminRole.Id = "118c3eae-13b5-4f2e-9d47-2432118e522a";
            superAdminRole.Name = "Super Admin";
            superAdminRole.StatusId = (int)StatusType.Deactivate;
            superAdminRole.CreatedBy = "SysAdmin";
            superAdminRole.CreatedDateTime = DateTime.UtcNow;
            context.Roles.AddOrUpdate(superAdminRole);

            context.Roles.AddOrUpdate(new ApplicationRole { Id = "846a0718-2314-4add-844c-c9864e272a54", Name = "ASAPs User", StatusId = (int)StatusType.Activate, CreatedBy = "SysAdmin", CreatedDateTime = DateTime.UtcNow });
            context.Roles.AddOrUpdate(new ApplicationRole { Id = "60f463da-0e90-4092-bb04-9d01ec763643", Name = "Practice User", StatusId = (int)StatusType.Activate, CreatedBy = "SysAdmin", CreatedDateTime = DateTime.UtcNow });

            context.SaveChanges();

            // context.UserRoles.AddOrUpdate(new UserRole { RoleId = role.Id, UserId = appUser.Id });
            context.UserRoles.AddOrUpdate(new UserRole { RoleId = superAdminRole.Id, UserId = appUser.Id });

            // context.Set<IdentityUserRole>().AddOrUpdate(new IdentityUserRole { RoleId = role.Id, UserId = appUser.Id });
            context.Set<IdentityUserRole>().AddOrUpdate(new IdentityUserRole { RoleId = superAdminRole.Id, UserId = appUser.Id });
            context.SaveChanges();

            #endregion
        }
    }
}
