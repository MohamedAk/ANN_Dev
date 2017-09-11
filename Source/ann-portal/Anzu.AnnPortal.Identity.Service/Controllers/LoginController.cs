using Anzu.AnnPortal.Common.Log;
using Anzu.AnnPortal.Identity.Common.Model;
using Anzu.AnnPortal.Identity.Common.Model.DTO;
using Anzu.AnnPortal.Identity.Common.Model.Enum;
using Anzu.AnnPortal.Identity.Core;
using Anzu.AnnPortal.Identity.Data.Model;
using Anzu.AnnPortal.Identity.Data.Model.Models;
using Anzu.AnnPortal.Identity.Service.IdentityExtentions;
using Anzu.AnnPortal.Identity.Service.Operations;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace Anzu.AnnPortal.Identity.Service.Controllers
{
    public class LoginController : Controller
    {
        protected ApplicationUserManager UserManager { get; private set; }
        protected ApplicationRoleManager RoleManager { get; private set; }
        public const int PASSWORD_HISTORY_LIMIT = 10;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public LoginController()
            : this(new ApplicationUserManager(), new ApplicationRoleManager())
        {
        }

        public LoginController(ApplicationUserManager userManager, ApplicationRoleManager roleManager)
        {
            this.UserManager = userManager;
            this.RoleManager = roleManager;
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        // GET: Login
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {

            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            ApplicationDbContext appContxt = new ApplicationDbContext();

            try
            {
                if (ModelState.IsValid)
                {
                    var user = UserManager.FindByName(model.UserName);

                    if (user != null)
                    {
                        var accessLimit = WebConfigurationManager.AppSettings["accessFailedLimit"];
                        if (UserManager.GetAccessFailedCount(user.Id) >= Convert.ToInt32(accessLimit) - 1)
                        {
                            this.UserManager.AuditLoginUser(new LoginAudit
                            {
                                LoginAudiStatus = (int)Common.Model.Enum.LoginAuditStatus.AccountLockedOut,
                                LoginDateTime = DateTime.UtcNow,
                                LoginName = model.UserName
                            });
                            ViewBag.AttemptsExceeded = true;
                        }

                        // Proceed to login
                        user = await UserManager.FindAsync(model.UserName, model.Password);

                        if (user != null)
                        {
                            if (user.StatusId == (int)StatusType.Deactivate)
                            {
                                this.UserManager.AuditLoginUser(new LoginAudit
                                {
                                    LoginAudiStatus = (int)Common.Model.Enum.LoginAuditStatus.DeactivatedUser,
                                    LoginDateTime = DateTime.UtcNow,
                                    LoginName = model.UserName
                                });

                                // user is deactivated
                                ModelState.AddModelError("", "User has been deactivated.");
                                return View("Login", model);
                            }

                            // get user role
                            var tempRoleId = user.Roles.FirstOrDefault().RoleId;
                            var userRole = this.RoleManager.Roles.Single(k => k.Id == tempRoleId);
                            // var isAdmin = (userRole.Name == "Administrator" || userRole.Name == "Super Admin");
                            var isPracticeUser = (userRole.Name == "Practice User");
                            if (isPracticeUser && string.IsNullOrEmpty(user.PracticeName))
                            {
                                // user is not assigned to a practice
                                ModelState.AddModelError("", "User is not assigned to a practice");
                                return View("Login", model);
                            }

                            // reset fail count
                            UserManager.ResetAccessFailedCount(user.Id);
                            ViewBag.AttemptsExceeded = false;

                            // login success                                

                            this.UserManager.AuditLoginUser(new LoginAudit
                            {
                                LoginAudiStatus = (int)Common.Model.Enum.LoginAuditStatus.LoginSuccess,
                                LoginDateTime = DateTime.UtcNow,
                                LoginName = model.UserName
                            });

                            await SignInAsync(user, model.RememberMe);

                            if (user.IsForceToChangePassword)
                            {
                                return RedirectToAction("ChangePassword", new { isSecurityQuestionAnswered = user.IsSecurityQuestionAnswered, tempPassword = Identity.Core.Encryption.EncryptionManager.Encrypt(model.Password) });
                            }

                            else if (!string.IsNullOrEmpty(returnUrl))
                            {
                                return Redirect(returnUrl);
                            }
                            else
                            {
                                string redirectUrl = WebConfigurationManager.AppSettings["eEDApp"];
                                string navigateToPage = WebConfigurationManager.AppSettings["loginSuccuessURL"];
                                return Redirect(string.Format("{0}{1}", redirectUrl, navigateToPage));
                            }
                        }
                        else
                        {
                            this.UserManager.AuditLoginUser(new LoginAudit
                            {
                                LoginAudiStatus = (int)Common.Model.Enum.LoginAuditStatus.LoginFailed,
                                LoginDateTime = DateTime.UtcNow,
                                LoginName = model.UserName
                            });

                            ModelState.AddModelError("", "Invalid user ID or password.");
                            user = UserManager.FindByName(model.UserName);
                            if (user != null)
                            {
                                UserManager.AccessFailed(user.Id);
                            }
                        }
                    }
                    else
                    {

                        this.UserManager.AuditLoginUser(new LoginAudit
                        {
                            LoginAudiStatus = (int)Common.Model.Enum.LoginAuditStatus.InvalidUserName,
                            LoginDateTime = DateTime.UtcNow,
                            LoginName = model.UserName
                        });

                        ModelState.AddModelError("", "Invalid user ID or password.");
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error processing the request");
                logger.Log(LogLevel.Fatal, ex, "Login: " + ex.ToString());
            }

            // If we got this far, something failed, redisplay form
            return View("Login", model);
        }

        [NonAction]
        private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);


            var identity = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            var tempRoleId = user.Roles.FirstOrDefault().RoleId;
            var userRole = this.RoleManager.Roles.Single(k => k.Id == tempRoleId);

            identity.AddClaim(new Claim("UserId", user.Id));
            identity.AddClaim(new Claim("UserEmail", user.Email));
            identity.AddClaim(new Claim("UserName", user.UserName));
            identity.AddClaim(new Claim("FullName", string.Format("{0} {1}", user.FirstName, user.LastName))); // add user object to cookie            
            identity.AddClaim(new Claim("practiceId", user.PracticeId.ToString()));
            identity.AddClaim(new Claim("FirstName", user.FirstName));
            identity.AddClaim(new Claim("LastName", user.LastName));
            identity.AddClaim(new Claim("UserRole", userRole.Name.ToUpper()));
            identity.AddClaim(new Claim("emrId", string.Empty));

            var permissionCodes = new PermissionService().GetPermissionCodes(UserManager.GetRoles(user.Id).ToList());

            foreach (string code in permissionCodes)
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, code));
            }

            // adding permission roles
            var roles = UserManager.GetRoles(user.Id).ToList();
            foreach (var role in roles)
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, role));
            }

            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> FirstTimeLogin()
        {
            LoginViewModel model = new LoginViewModel();

            // get password from querystring
            var tempUserId = Request.QueryString["u"];
            if (tempUserId != null)
            {
                model.UserName = tempUserId;
            }

            // get password from querystring
            var tempPassword = Request.QueryString["p"];
            if (tempPassword != null)
            {
                model.Password = tempPassword;

                var user = await UserManager.FindAsync(model.UserName, Identity.Core.Encryption.EncryptionManager.Decrypt(tempPassword.ToString()));
                if (user != null)
                {
                    // Signout from current cookie if any
                    AuthenticationManager.SignOut();

                    await SignInAsync(user, model.RememberMe);
                    if (user.IsForceToChangePassword)
                    {
                        return RedirectToAction("ChangePassword", new { isSecurityQuestionAnswered = false, tempUserId = model.UserName, tempPassword = model.Password });
                    }
                }
            }

            return RedirectToAction("Login");
        }

        [HttpGet]
        [Authorize]
        public ActionResult ChangePassword(bool isSecurityQuestionAnswered, string tempUserId = "", string tempPassword = "")
        {
            UserManager = new ApplicationUserManager(true);

            ChangePasswordViewModel md = new ChangePasswordViewModel();
            ViewBag.Questions = new SelectList(this.UserManager.GetSecurityQuestions(), "Id", "Question");
            md.ShowOldPassword = false;
            md.ISSecurityQuestionAnswered = isSecurityQuestionAnswered;

            // get password from querystring
            if (!string.IsNullOrEmpty(tempUserId))
            {
                md.UserId = tempUserId;
            }

            // get password from querystring
            if (!string.IsNullOrEmpty(tempPassword))
            {
                md.OldPassword = Identity.Core.Encryption.EncryptionManager.Decrypt(tempPassword.ToString());
            }
            else
            {
                md.ShowOldPassword = true;
            }

            return View(md);
        }

        private bool IsPreviousPassword(string userId, string newPassword)
        {
            ApplicationDbContext appContxt = new ApplicationDbContext();

            ApplicationUser user = UserManager.Users.Where(k => k.Id == userId).First();
            //user.PreviousUserPasswords = 
            var prev = appContxt.PreviousPasswords.Where(p => p.UserId == userId).ToList();
            if (prev.Count != 0)
            {
                if (prev.OrderByDescending(x => x.CreateDate).
                Select(x => x.PasswordHash).Take(PASSWORD_HISTORY_LIMIT).Where(x => new PasswordHasher().VerifyHashedPassword(x, newPassword) != PasswordVerificationResult.Failed).
                Any())
                {
                    return true;
                }
            }
            return false;
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            ApplicationDbContext appContxt = new ApplicationDbContext();
            UserManager = new ApplicationUserManager(true);

            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.Questions = new SelectList(this.UserManager.GetSecurityQuestions(), "Id", "Question");
                    return View(model);
                }

                string userId = User.Identity.GetUserId();

                ApplicationUser userFound = await UserManager.FindAsync(User.Identity.GetUserName(), model.OldPassword);

                if (userFound != null)
                {
                    if (IsPreviousPassword(userId, model.NewPassword))
                    {
                        ModelState.AddModelError("", "The last 10 passwords cannot be reused");
                        ViewBag.Questions = new SelectList(this.UserManager.GetSecurityQuestions(), "Id", "Question");
                        return View(model);
                    }
                    var result = await UserManager.ChangePasswordAsync(userId, model.OldPassword, model.NewPassword);

                    if (result.Succeeded)
                    {
                        // force to change password remove
                        ApplicationUser appUser = await this.UserManager.FindByIdAsync(User.Identity.GetUserId());
                        appUser.IsForceToChangePassword = false;
                        this.UserManager.Update(appUser);
                        if (!model.ISSecurityQuestionAnswered)
                        {
                            this.UserManager.UpdateUserSecurityQuestion(userId, model.SecurityQuestionId, model.SecurityQuestionAnswer);
                            appUser.IsSecurityQuestionAnswered = true;
                            this.UserManager.Update(appUser);
                        }
                        appContxt.PreviousPasswords.Add(new PreviousPassword { UserId = userId, PasswordHash = new PasswordHasher().HashPassword(model.NewPassword), CreateDate = DateTime.UtcNow });
                        appContxt.SaveChanges();

                        var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                        if (user != null)
                        {
                            //var identity = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
                            //identity.AddClaim(new Claim(ClaimTypes.UserData, string.Format("{0} {1}", user.FirstName, user.LastName))); // add user object to cookie                        
                            //AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = false }, identity);

                            if (user.StatusId == (int)StatusType.Deactivate)
                            {
                                this.UserManager.AuditLoginUser(new LoginAudit
                                {
                                    LoginAudiStatus = (int)Common.Model.Enum.LoginAuditStatus.DeactivatedUser,
                                    LoginDateTime = DateTime.UtcNow,
                                    LoginName = user.UserName
                                });

                                // user is deactivated
                                ModelState.AddModelError("", "User has been deactivated.");
                                ViewBag.Questions = new SelectList(this.UserManager.GetSecurityQuestions(), "Id", "Question");
                                return View(model);
                            }

                            // get user role
                            var tempRoleId = user.Roles.FirstOrDefault().RoleId;
                            var userRole = this.RoleManager.Roles.Single(k => k.Id == tempRoleId);
                            // var isAdmin = (userRole.Name == "Administrator" || userRole.Name == "Super Admin");
                            var isPracticeUser = (userRole.Name == "Practice User");
                            if (isPracticeUser && string.IsNullOrEmpty(user.PracticeName))
                            {
                                // user is not assigned to a practice
                                ModelState.AddModelError("", "User is not assigned to a practice");
                                ViewBag.Questions = new SelectList(this.UserManager.GetSecurityQuestions(), "Id", "Question");
                                return View(model);
                            }


                            await this.SignInAsync(user, true);
                        }

                        using (HttpClient client = new HttpClient())
                        {
                            client.BaseAddress = new Uri(ConfigurationManager.AppSettings["Anzu.AnnPortal.Core.Service"]);
                            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                            HttpResponseMessage response = client.GetAsync("api/notification/UpdateEndDateForPasswordNotifications/" + user.UserName).Result;
                        }

                        bool hasDigitalSignatureCapability = User.IsInRole("DGT-SIG-CAP");

                        if (user.DigitalSignature == null && hasDigitalSignatureCapability)
                        {
                            return RedirectToAction("DigitalSignature");
                        }

                        string redirectUrl = WebConfigurationManager.AppSettings["eEDApp"];
                        string navigateToPage = WebConfigurationManager.AppSettings["loginSuccuessURL"];
                        return Redirect(string.Format("{0}{1}", redirectUrl, navigateToPage));
                    }

                    AddErrors(result);
                }
                else
                {
                    ModelState.AddModelError("OldPassword", "Incorrect Password");
                    ViewBag.Questions = new SelectList(this.UserManager.GetSecurityQuestions(), "Id", "Question");
                    return View(model);
                }
            }
            catch (Exception ex)
            {
            }
            ViewBag.Questions = new SelectList(this.UserManager.GetSecurityQuestions(), "Id", "Question");
            return View(model);
        }


        //
        // GET:ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            ViewBag.Questions = new SelectList(this.UserManager.GetSecurityQuestions(), "Id", "Question");
            ViewBag.Message = string.Empty;
            return View();
        }

        //
        // POST: ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(ForgotPasswordViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // TODO: Send email
                    ApplicationUser appUser = this.UserManager.FindByName(model.UserId);

                    if (appUser == null)
                    {
                        ModelState.AddModelError("", "Invalid user ID");
                    }
                    else if (UserManager.GetAccessFailedCount(appUser.Id) >= 5)
                    {
                        ModelState.AddModelError("", "Account is locked. Please contact system administrator to reset password");
                    }
                    else if (appUser.StatusId == (int)StatusType.Deactivate)
                    {
                        this.UserManager.AuditLoginUser(new LoginAudit
                        {
                            LoginAudiStatus = (int)Common.Model.Enum.LoginAuditStatus.DeactivatedUser,
                            LoginDateTime = DateTime.UtcNow,
                            LoginName = model.UserId
                        });

                        // Signout frm cookie
                        AuthenticationManager.SignOut();
                        // user is deactivated
                        ModelState.AddModelError("", "User has been deactivated.");
                    }
                    else
                    {
                        return RedirectToAction("ValidateSecurityQuestion", new { userId = appUser.UserName });
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error processing the request");
            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }


        /// <summary>
        /// Validates the security question.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult ValidateSecurityQuestion(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login");
            }
            ValidateSecurityQuestionViewModel vm = new ValidateSecurityQuestionViewModel();
            var securityQuestion = this.UserManager.GetUserSpecificSecurityQuestion(userId);
            if (securityQuestion == null)
            {
                return RedirectToAction("Login");
            }
            vm.SecurityQuestion = securityQuestion.Question.Question;
            vm.UserId = userId;

            ViewBag.ShowLink = false;
            ViewBag.AdminEmail = string.Empty;

            return View(vm);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ValidateSecurityQuestion(ValidateSecurityQuestionViewModel model)
        {
            var securityQuestion = this.UserManager.GetUserSpecificSecurityQuestion(model.UserId);
            ViewBag.ShowLink = false;
            if (ModelState.IsValid)
            {
                if (!string.Equals(securityQuestion.QuestionAnswer, model.Answer, StringComparison.OrdinalIgnoreCase))
                {
                    // get sysadmin email
                    UserService service = new UserService();
                    var adminUser = service.GetAdminUser();

                    ModelState.AddModelError("", "Incorrect answer to security question");
                    ViewBag.ShowLink = true;
                    ViewBag.AdminEmail = adminUser.Email;
                }
                else
                {
                    this.ResetUser(model.UserId);
                    return RedirectToAction("Success");
                }
            }

            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Success(string msg)
        {
            ViewBag.Message = msg;
            return View();
        }

        [NonAction]
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        [HttpGet]
        [Authorize]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Login");
        }

        [HttpGet]
        [Authorize]
        public ActionResult ChangePasswordCancel()
        {
            var user = UserManager.FindByName(User.Identity.Name);

            if (user.IsForceToChangePassword)
            {
                // if user is not changed password yet log out
                return RedirectToAction("LogOff");
            }
            else
            {
                string redirectUrl = WebConfigurationManager.AppSettings["eEDApp"];
                string navigateToPage = WebConfigurationManager.AppSettings["loginSuccuessURL"];
                return Redirect(string.Format("{0}{1}", redirectUrl, navigateToPage));
            }
        }

        [HttpGet]
        [Authorize]
        public ActionResult DigitalSignatureCancel()
        {
            var user = UserManager.FindByName(User.Identity.Name);
            var permissionCodes = new PermissionService().GetPermissionCodes(UserManager.GetRoles(user.Id).ToList());
            bool hasDigitalSignatureCapability = permissionCodes.Contains("DGT-SIG-CAP");
            if (user.DigitalSignature == null && hasDigitalSignatureCapability)
            {
                return RedirectToAction("LogOff");
            }
            else
            {
                string redirectUrl = WebConfigurationManager.AppSettings["eEDApp"];
                string navigateToPage = WebConfigurationManager.AppSettings["loginSuccuessURL"];
                return Redirect(string.Format("{0}{1}", redirectUrl, navigateToPage));
            }
        }

        private void ResetUser(string userId)
        {
            ApplicationUser appUser = this.UserManager.Users.Where<ApplicationUser>(a => a.UserName == userId).FirstOrDefault();
            AccountReset reset = new AccountReset(this.UserManager, this.RoleManager);
            var isSucceed = reset.ResetUserLogin(appUser);
        }

        [HttpGet]
        [Authorize]
        public ActionResult DigitalSignature(string isChange)
        {
            ViewBag.isChange = isChange;
            return View();
        }
    }
}