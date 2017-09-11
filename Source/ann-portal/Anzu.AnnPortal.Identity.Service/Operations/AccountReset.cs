using Anzu.AnnPortal.Identity.Service.IdentityExtentions;
using Anzu.AnnPortal.Identity.Common.Model.Enum;
using Anzu.AnnPortal.Identity.Core;
using Microsoft.AspNet.Identity;
using System;
using System.Web;
using Anzu.AnnPortal.Identity.Data.Model;
using Anzu.AnnPortal.Data.Model.Core;
using Anzu.AnnPortal.Identity.Data.Model.Models;

namespace Anzu.AnnPortal.Identity.Service.Operations
{
    public class AccountReset
    {
        protected ApplicationUserManager UserManager { get; private set; }
        protected ApplicationRoleManager RoleManager { get; private set; }

        public AccountReset(ApplicationUserManager userManager, ApplicationRoleManager roleManager)
        {
            this.UserManager = userManager;
            this.RoleManager = roleManager;
        }


        /// <summary>
        /// Resets the user login.
        /// </summary>
        /// <param name="appUser">The application user.</param>
        /// <returns></returns>
        public IdentityResult ResetUserLogin(Data.Model.Models.ApplicationUser appUser)
        {
            ApplicationDbContext appContxt = new ApplicationDbContext();
            string tempPwd = System.Web.Security.Membership.GeneratePassword(9, 4);
            appUser.IsForceToChangePassword = true;
            appUser.PasswordHash = new PasswordHasher().HashPassword(tempPwd);

            IdentityResult updateUserResult = null;

            try
            {
                updateUserResult = this.UserManager.Update(appUser);

                if (updateUserResult.Succeeded)
                {
                    this.UserManager.ResetAccessFailedCount(appUser.Id);
                    UserManager.ResetAccessFailedCount(appUser.Id);

                    EmailService emailService = new EmailService();
                    // Data.Model.Models.ApplicationUser currentUser = this.UserManager.FindByName(HttpContext.Current.User.Identity.Name);
                    emailService.SendEmail(EmailMessageType.ResetUser, appUser, appUser, tempPwd);

                    appContxt.PreviousPasswords.Add(new PreviousPassword { UserId = appUser.Id, PasswordHash = new PasswordHasher().HashPassword(tempPwd), CreateDate = DateTime.UtcNow });
                    appContxt.SaveChanges();

                }
            }
            catch (Exception ex)
            {
            }
            return updateUserResult;
        }
    }
}