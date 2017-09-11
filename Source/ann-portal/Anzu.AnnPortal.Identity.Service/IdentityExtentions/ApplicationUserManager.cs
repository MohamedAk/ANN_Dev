using Anzu.AnnPortal.Identity.Data.Model;
using Anzu.AnnPortal.Identity.Data.Model.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Anzu.AnnPortal.Identity.Service.IdentityExtentions
{
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationUserManager"/> class.
        /// </summary>
        public ApplicationUserManager()
            : base(new UserStore<ApplicationUser>(new ApplicationDbContext()))
        {
            PasswordValidator = new CustomizePasswordValidation();

            // Enable Lock outs
            this.UserLockoutEnabledByDefault = true;
            this.MaxFailedAccessAttemptsBeforeLockout = 6;

            // if you want to lock out indefinitely 200 years should be enough
            this.DefaultAccountLockoutTimeSpan = TimeSpan.FromDays(365 * 200);
        }

        public ApplicationUserManager(bool isChangePW)
            : base(new UserStore<ApplicationUser>(new ApplicationDbContext()))
        {
            PasswordValidator = new CustomizePasswordValidation(isChangePW);

            // Enable Lock outs
            this.UserLockoutEnabledByDefault = true;
            this.MaxFailedAccessAttemptsBeforeLockout = 6;

            // if you want to lock out indefinitely 200 years should be enough
            this.DefaultAccountLockoutTimeSpan = TimeSpan.FromDays(365 * 200);
        }

        /// <summary>
        /// Gets the admin user.
        /// </summary>
        /// <returns></returns>
        public List<ApplicationUser> GetAdminUsers()
        {
            ApplicationDbContext appDb = new ApplicationDbContext();

            var users = appDb.Roles.Where(r => r.Name.Equals("Admin", StringComparison.InvariantCultureIgnoreCase)).SelectMany(r => r.Users);

            List<ApplicationUser> appUsers = new List<ApplicationUser>();

            foreach (IdentityUserRole userRole in users)
            {
                appUsers.Add(this.FindById(userRole.UserId));
            }

            return appUsers;
        }

        /// <summary>
        /// Audits the login user.
        /// </summary>
        /// <param name="loginAudit">The login audit.</param>
        public void AuditLoginUser(LoginAudit loginAudit)
        {
            ApplicationDbContext appDb = new ApplicationDbContext();

            appDb.Set<LoginAudit>().Add(loginAudit);
            appDb.SaveChanges();
        }

        /// <summary>
        /// Gets the user designation.
        /// </summary>
        /// <param name="designationId">The designation identifier.</param>
        /// <returns></returns>
        public Designation GetUserDesignation(long designationId)
        {
            ApplicationDbContext appDb = new ApplicationDbContext();

            var deisgnation = appDb.Designation.Where(r => r.Id == designationId).SingleOrDefault();

            return deisgnation;
        }

        /// <summary>
        /// Gets the security questions.
        /// </summary>
        /// <returns></returns>
        public List<SecurityQuestion> GetSecurityQuestions()
        {
            ApplicationDbContext appDb = new ApplicationDbContext();

            return appDb.SecurityQuestion.ToList();
        }

        /// <summary>
        /// Updates the user security question.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="questionId">The question identifier.</param>
        /// <param name="answer">The answer.</param>
        /// <returns></returns>
        public bool UpdateUserSecurityQuestion(string userId, int questionId, string answer)
        {
            ApplicationDbContext appDb = new ApplicationDbContext();

            ApplicationUserSecurityQuestion securityQuestion = new ApplicationUserSecurityQuestion();
            securityQuestion.UserId = userId;
            securityQuestion.QuestionAnswer = answer;
            securityQuestion.QuestionId = questionId;

            appDb.UserSecurityQuestion.RemoveRange(appDb.UserSecurityQuestion.Where(q => q.UserId == userId));
            appDb.UserSecurityQuestion.Add(securityQuestion);

            int saveResult = appDb.SaveChanges();
            if (saveResult == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the user specific security question.
        /// </summary>
        /// <param name="userName">The user identifier.</param>
        /// <returns></returns>
        public ApplicationUserSecurityQuestion GetUserSpecificSecurityQuestion(string userName)
        {
            ApplicationDbContext appDb = new ApplicationDbContext();
            var selectedUser = appDb.Users.Where(usr => usr.UserName == userName).SingleOrDefault();

            return appDb.UserSecurityQuestion.Include("Question").Where(que => que.UserId == selectedUser.Id).FirstOrDefault();
        }

    }
}