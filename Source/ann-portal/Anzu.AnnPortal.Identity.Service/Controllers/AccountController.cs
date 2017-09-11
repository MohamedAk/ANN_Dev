using Anzu.AnnPortal.Identity.Common.Model;
using Anzu.AnnPortal.Identity.Common.Model.DTO;
using Anzu.AnnPortal.Identity.Common.Model.Enum;
using Anzu.AnnPortal.Identity.Core;
using Anzu.AnnPortal.Identity.Data.Model;
using Anzu.AnnPortal.Identity.Data.Model.Models;
using Anzu.AnnPortal.Identity.Service.Filters;
using Anzu.AnnPortal.Identity.Service.IdentityExtentions;
using Anzu.AnnPortal.Identity.Service.Operations;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;

namespace Anzu.AnnPortal.Identity.Service.Controllers
{
    /// <summary>
    /// AccountController Class
    /// </summary>
    /// <seealso cref="Anzu.AnnPortal.Identity.Service.Controllers.BaseApiController" />
    public class AccountController : BaseApiController
    {
        /// <summary>
        /// Gets the user manager.
        /// </summary>
        /// <value>
        /// The user manager.
        /// </value>
        protected ApplicationUserManager UserManager { get; private set; }
        /// <summary>
        /// Gets the role manager.
        /// </summary>
        /// <value>
        /// The role manager.
        /// </value>
        protected ApplicationRoleManager RoleManager { get; private set; }

        // messages
        /// <summary>
        /// The duplicat e_ use r_ identifier
        /// </summary>
        private const string DUPLICATE_USER_ID = "Entered user id exists within the system";
        /// <summary>
        /// The duplicat e_ emai l_ identifier
        /// </summary>
        private const string DUPLICATE_EMAIL_ID = "Entered email exists within the system";
        /// <summary>
        /// The n o_ role s_ create
        /// </summary>
        private const string NO_ROLES_CREATE = "Please select at least one role to create the user";
        /// <summary>
        /// The n o_ role s_ update
        /// </summary>
        private const string NO_ROLES_UPDATE = "Please select at least one role to update the user";
        /// <summary>
        /// The imag e_ siz e_ validation
        /// </summary>
        private const string IMAGE_SIZE_VALIDATION = "Image size is not compatible. Please upload 200px x 60px size image.";
        /// <summary>
        /// The role Administrator name
        /// </summary>
        private const string roleAdminName = "Administrator";

        CommonHelper ch;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountController"/> class.
        /// </summary>
        public AccountController()
            : this(new ApplicationUserManager(), new ApplicationRoleManager())
        {
            this.ch = new CommonHelper();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountController"/> class.
        /// </summary>
        /// <param name="userManager">The user manager.</param>
        /// <param name="roleManager">The role manager.</param>
        public AccountController(ApplicationUserManager userManager, ApplicationRoleManager roleManager)
        {
            this.UserManager = userManager;
            this.RoleManager = roleManager;
            this.ch = new CommonHelper();
        }

        /// <summary>
        /// Creates the user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        [Route("api/Account/create")]
        [HttpPost]
        //[Authorize]
        public async Task<IHttpActionResult> CreateUser(CreateUserViewModel user)
        {
            //user.OrganizationId = 1;// remove orgnization fix
            ch.LogMethodStartAPI(System.Reflection.MethodBase.GetCurrentMethod().Name, DateTime.Now);

            if (!ModelState.IsValid)
            {
                ch.LogMethodEndAPI(System.Reflection.MethodBase.GetCurrentMethod().Name, DateTime.Now);
                return BadRequest(ModelState);
            }

            //if (this.UserManager.FindByName(user.UserId) != null)
            //{
            //    ch.LogMethodEndAPI(System.Reflection.MethodBase.GetCurrentMethod().Name, DateTime.Now);
            //    return BadRequest(DUPLICATE_USER_ID);
            //}

            if (this.UserManager.FindByEmail(user.Email) != null)
            {
                ch.LogMethodEndAPI(System.Reflection.MethodBase.GetCurrentMethod().Name, DateTime.Now);
                return BadRequest(DUPLICATE_EMAIL_ID);
            }

            // validate for single role
            if (user.RoleId == null)
            {
                ch.LogMethodEndAPI(System.Reflection.MethodBase.GetCurrentMethod().Name, DateTime.Now);
                return BadRequest(NO_ROLES_CREATE);
            }

            // multiple roles assigned
            //if (user.Roles != null ? user.Roles.Count == 0 : false)
            //{
            //    ch.LogMethodEndAPI(System.Reflection.MethodBase.GetCurrentMethod().Name, DateTime.Now);
            //    return BadRequest(NO_ROLES_CREATE);
            //}

            ApplicationUser appUser = new ApplicationUser();
            appUser.Email = user.Email;
            appUser.EmailConfirmed = true;
            appUser.FirstName = user.FirstName;
            appUser.LastName = user.LastName;
            appUser.OrganizationId = 1; // user.OrganizationId;
            appUser.IsForceToChangePassword = true;
            appUser.IsSecurityQuestionAnswered = false;

            string tempPassword = System.Web.Security.Membership.GeneratePassword(9, 4);

            appUser.PasswordHash = new PasswordHasher().HashPassword(tempPassword);
            appUser.UserName = user.Email;
            // Create user deactive - admin will activate
            appUser.StatusId = (int)StatusType.Deactivate;
            appUser.CreatedBy = "SysAdmin";
            appUser.CreatedDateTime = DateTime.UtcNow;

            // set designation
            var role = this.RoleManager.Roles.Single(k => k.Id == user.RoleId);

            switch (role.Name)
            {
                case "Administrator":
                    appUser.DesignationId = 1;
                    appUser.UserDesignation = "ADMINISTRATOR";
                    break;
                case "Super Admin":
                    appUser.DesignationId = 2;
                    appUser.UserDesignation = "SUPER_ADMIN";
                    break;
                case "ASAPs User":
                    appUser.DesignationId = 3;
                    appUser.PracticeId = 1;
                    appUser.PracticeName = "";
                    appUser.UserDesignation = "ASAPs_USER";
                    break;
                case "Practice User":
                    appUser.DesignationId = 4;
                    appUser.UserDesignation = "PRACTICE_USER";
                    break;
                default:
                    appUser.UserDesignation = string.Empty;
                    break;
            }

            IdentityResult addUserResult = null;
            try
            {
                ApplicationDbContext appContxt = new ApplicationDbContext();
                addUserResult = await this.UserManager.CreateAsync(appUser, tempPassword);

                if (addUserResult.Succeeded)
                {
                    // selected user role
                    this.UserManager.AddToRole(appUser.Id, role.Name);
                    appContxt.UserRoles.Add(new UserRole { RoleId = role.Id, UserId = appUser.Id });
                    appContxt.SaveChanges();
                }

                // EmailService emailService = new EmailService();
                // ApplicationUser currentUser = this.UserManager.FindByName(HttpContext.Current.User.Identity.Name);
                // ApplicationUser currentUser = this.UserManager.FindByName("SysAdmin");
                // emailService.SendEmail(EmailMessageType.UserCreated, appUser, appUser, tempPassword);
            }
            catch (Exception ex)
            {
                ch.LogMethodEndAPI(System.Reflection.MethodBase.GetCurrentMethod().Name, DateTime.Now);
            }

            if (!addUserResult.Succeeded)
            {
                ch.LogMethodEndAPI(System.Reflection.MethodBase.GetCurrentMethod().Name, DateTime.Now);
                return GetErrorResult(addUserResult);
            }
            ch.LogMethodEndAPI(System.Reflection.MethodBase.GetCurrentMethod().Name, DateTime.Now);
            return this.StatusCode(HttpStatusCode.Created);
        }

        /// <summary>
        /// Gets the users.
        /// </summary>
        /// <returns></returns>
        [Route("api/Account/users/{firstName}/{lastName}/{userId}/{practiceId}/{filter}")]
        [HttpGet]
        // [Authorize(Roles = "Administrator, HubAdmin")]
        // [GZIPCompress]
        public IHttpActionResult GetUsers(int take, int skip, int page, int pageSize, string firstName, string lastName, string userId, string practiceId, string filter)
        {
            ch.LogMethodStartAPI(System.Reflection.MethodBase.GetCurrentMethod().Name, DateTime.Now);

            bool isFirstNameBlank = (firstName == "NA");
            bool isLastNameBlank = (lastName == "NA");
            bool isUserIdBlank = (userId == "NA");
            bool isPracticeNameBlank = (practiceId == "NA");
            if (string.IsNullOrEmpty(filter))
            {
                filter = "NA";
            }
            ApplicationDbContext dbCntxt = new ApplicationDbContext();
            var modelMapper = new ViewModelMapperService();

            var users = this.UserManager.Users.ToList<ApplicationUser>()
                .Select(u => modelMapper.MapUserList(u)).Where(u => (isFirstNameBlank || u.FirstName.ToUpper().Contains(firstName.ToUpper()))
                            && ((filter.Equals("NA") || u.FirstName.ToUpper().Contains(filter.ToUpper()))
                            || (filter.Equals("NA") || u.LastName.ToUpper().Contains(filter.ToUpper())))
                            && (isLastNameBlank || u.LastName.ToUpper().Contains(lastName.ToUpper()))
                            && (isPracticeNameBlank || (u.PracticeName != null && u.PracticeName.ToUpper().Contains(practiceId.ToUpper())))
                            && (isUserIdBlank || u.UserId.ToUpper().Contains(userId.ToUpper())) && (u.DesignationId != 2 || !u.Roles.Contains("118c3eae-13b5-4f2e-9d47-2432118e522a"))).ToList();

            int TotalRows = users.Count;
            if (take == 0)
            {
                take = TotalRows;
                skip = 0;
            }

            users = users.OrderByDescending(x => x.IsActive).ThenBy(x => x.UserId).Skip(skip).Take(take).ToList();

            foreach (var user in users)
            {
                var roles = dbCntxt.UserRoles.Where(r => r.UserId == user.Id).Select(k => k.Role.Name).Take(1).ToList();
                user.Roles = roles;
                user.UserRolesDisplay = String.Join(", ", roles);
                user.LastModifiedDateDisplay = user.LastModifiedDate.ToString();

                user.UserType = dbCntxt.Designation.Where(o => o.Id == user.DesignationId).Select(u => u.DisplayName).First();
            }
            var result = new
            {
                Data = users,
                Total = TotalRows
            };
            ch.LogMethodEndAPI(System.Reflection.MethodBase.GetCurrentMethod().Name, DateTime.Now);
            return Ok(result);
        }


        /// <summary>
        /// Gets the user.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        [Route("api/Account/userlist/{userName}")]
        [HttpGet]
        public IHttpActionResult GetUserList(string userName)
        {
            ch.LogMethodStartAPI(System.Reflection.MethodBase.GetCurrentMethod().Name, DateTime.Now);

            ApplicationDbContext dbCntxt = new ApplicationDbContext();

            //IdentityDbContext dbCotxtIdentity = new IdentityDbContext();

            var modelMapper = new ViewModelMapperService();

            var dbUser = this.UserManager.Users.FirstOrDefault();

            if (dbUser == null)
            {
                return this.StatusCode(HttpStatusCode.NotFound);
            }
            else
            {

                var users = this.UserManager.Users.ToList<ApplicationUser>().Select(u => modelMapper.MapUserList(u));

                var result = new
                {
                    data = users
                };


                return Ok(result);

            }
        }

        private IHttpActionResult Ok<T>(IEnumerable<CreateUserViewModel> users)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        [Route("api/Account/user")]
        [HttpPost]
        //[Authorize(Roles = "Administrator, HubAdmin")]
        public IHttpActionResult GetUser(CreateUserViewModel user)
        {
            ch.LogMethodStartAPI(System.Reflection.MethodBase.GetCurrentMethod().Name, DateTime.Now);

            ApplicationDbContext dbCntxt = new ApplicationDbContext();

            var modelMapper = new ViewModelMapperService();

            var dbUser = this.UserManager.Users.Where(u => u.UserName == user.UserId).FirstOrDefault();

            if (dbUser == null)
            {
                ch.LogMethodEndAPI(System.Reflection.MethodBase.GetCurrentMethod().Name, DateTime.Now);
                return this.StatusCode(HttpStatusCode.NotFound);
            }
            else
            {
                CreateUserViewModel userVM = modelMapper.MapUser(dbUser);
                var roles = dbCntxt.UserRoles.Where(r => r.UserId == dbUser.Id).Select(k => k.Role.Name).ToList();
                userVM.Roles = roles;//this.UserManager.GetRoles(dbUser.Id).ToList();
                if (userVM.Roles.Count > 0)
                {
                    userVM.RoleId = RoleManager.FindByName(userVM.Roles.FirstOrDefault()).Id;
                }

                ch.LogMethodEndAPI(System.Reflection.MethodBase.GetCurrentMethod().Name, DateTime.Now);
                return this.Ok<CreateUserViewModel>(userVM);
            }
        }

        [Route("api/Account/userById/{userId}")]
        [HttpGet]
        //[Authorize(Roles = "Administrator, HubAdmin")]
        public IHttpActionResult GetUserById(string userId)
        {
            ch.LogMethodStartAPI(System.Reflection.MethodBase.GetCurrentMethod().Name, DateTime.Now);

            ApplicationDbContext dbCntxt = new ApplicationDbContext();

            var modelMapper = new ViewModelMapperService();

            var dbUser = this.UserManager.Users.Where(u => u.Id == userId).FirstOrDefault();

            if (dbUser == null)
            {
                ch.LogMethodEndAPI(System.Reflection.MethodBase.GetCurrentMethod().Name, DateTime.Now);
                return this.StatusCode(HttpStatusCode.NotFound);
            }
            else
            {
                CreateUserViewModel userVM = modelMapper.MapUser(dbUser);
                var roles = dbCntxt.UserRoles.Where(r => r.UserId == dbUser.Id).Select(k => k.Role.Name).ToList();
                userVM.Roles = roles;//this.UserManager.GetRoles(dbUser.Id).ToList();
                if (userVM.Roles.Count > 0)
                {
                    userVM.RoleId = RoleManager.FindByName(userVM.Roles.FirstOrDefault()).Id;
                }

                ch.LogMethodEndAPI(System.Reflection.MethodBase.GetCurrentMethod().Name, DateTime.Now);
                return this.Ok<CreateUserViewModel>(userVM);
            }
        }


        /// <summary>
        /// Savedigitals the signature.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        [Route("api/Account/digitalSignature")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult savedigitalSignature(DigitalSignatureViewModel user)
        {
            ch.LogMethodStartAPI(System.Reflection.MethodBase.GetCurrentMethod().Name, DateTime.Now);

            using (var ms = new MemoryStream(user.DigitalSignature))
            {
                var image = Image.FromStream(ms);
                if (image.Height > 60 || image.Width > 200)
                {
                    ch.LogMethodEndAPI(System.Reflection.MethodBase.GetCurrentMethod().Name, DateTime.Now);
                    return BadRequest(IMAGE_SIZE_VALIDATION);
                }
            }
            ApplicationDbContext dbCntxt = new ApplicationDbContext();
            ApplicationUser dbUser;
            if (user.isChange)
            {
                ch.LogMethodEndAPI(System.Reflection.MethodBase.GetCurrentMethod().Name, DateTime.Now);
                dbUser = this.UserManager.Find(user.UserId, user.Password);
            }
            else
            {
                ch.LogMethodEndAPI(System.Reflection.MethodBase.GetCurrentMethod().Name, DateTime.Now);
                dbUser = this.UserManager.Users.Where(u => u.UserName == user.UserId).FirstOrDefault();
            }
            if (dbUser == null)
            {
                ch.LogMethodEndAPI(System.Reflection.MethodBase.GetCurrentMethod().Name, DateTime.Now);
                return this.StatusCode(HttpStatusCode.NotFound);
            }
            else
            {
                if (user.PIN != null)
                {
                    dbUser.PIN = user.PIN;
                }
                dbUser.DigitalSignature = user.DigitalSignature;
                dbUser.ModifiedBy = User.Identity.Name;
                dbUser.ModifiedDateTime = DateTime.UtcNow;

                IdentityResult updateUserResult = null;
                try
                {
                    updateUserResult = this.UserManager.Update(dbUser);

                }
                catch (Exception ex)
                {
                    ch.LogMethodEndAPI(System.Reflection.MethodBase.GetCurrentMethod().Name, DateTime.Now);
                }

                if (!updateUserResult.Succeeded)
                {
                    ch.LogMethodEndAPI(System.Reflection.MethodBase.GetCurrentMethod().Name, DateTime.Now);
                    return GetErrorResult(updateUserResult);
                }
                ch.LogMethodEndAPI(System.Reflection.MethodBase.GetCurrentMethod().Name, DateTime.Now);
                return this.StatusCode(HttpStatusCode.Created);

            }
        }

        /// <summary>
        /// Gets the digital signature.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        [Route("api/Account/digitalSignature")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetDigitalSignature(string userId)
        {
            ch.LogMethodStartAPI(System.Reflection.MethodBase.GetCurrentMethod().Name, DateTime.Now);

            DigitalSignatureViewModel user = new DigitalSignatureViewModel();
            ApplicationDbContext dbCntxt = new ApplicationDbContext();

            var dbUser = this.UserManager.Users.Where(u => u.UserName == userId).FirstOrDefault();

            if (dbUser == null)
            {
                ch.LogMethodEndAPI(System.Reflection.MethodBase.GetCurrentMethod().Name, DateTime.Now);
                return this.StatusCode(HttpStatusCode.NotFound);
            }
            else
            {
                user.DigitalSignature = dbUser.DigitalSignature;
                user.PIN = dbUser.PIN;
                user.ConfirmPIN = dbUser.PIN;
                ch.LogMethodEndAPI(System.Reflection.MethodBase.GetCurrentMethod().Name, DateTime.Now);
                return this.Ok<DigitalSignatureViewModel>(user);
            }
        }

        //[Authorize]
        /// <summary>
        /// Updates the user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        [Route("api/Account/update")]
        [HttpPost]
        //[Authorize(Roles = "Administrator, HubAdmin")]
        public IHttpActionResult UpdateUser(CreateUserViewModel user)
        {
            ch.LogMethodStartAPI(System.Reflection.MethodBase.GetCurrentMethod().Name, DateTime.Now);

            ApplicationDbContext appContxt = new ApplicationDbContext();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var exUser = this.UserManager.FindByEmail(user.Email);
            if (exUser != null && user.Id != exUser.Id)
            {
                return BadRequest(DUPLICATE_EMAIL_ID);
            }

            if (user.RoleId == null) // ? user.Roles.Count == 0 : false
            {
                return BadRequest(NO_ROLES_CREATE);
            }

            ApplicationUser appUser = this.UserManager.Users.Where<ApplicationUser>(a => a.UserName == user.UserId).FirstOrDefault();

            appUser.Email = user.Email;
            appUser.FirstName = user.FirstName;
            appUser.LastName = user.LastName;
            appUser.OrganizationId = user.OrganizationId;
            appUser.StatusId = user.StatusId;
            appUser.ModifiedBy = "SysAdmin";
            // appUser.ModifiedBy = User.Identity.Name;
            appUser.ModifiedDateTime = DateTime.UtcNow;
            appUser.DesignationId = user.DesignationId;
            appUser.UserDesignation = user.UserDesignation;

            IdentityResult updateUserResult = null;

            try
            {
                updateUserResult = this.UserManager.Update(appUser);

                if (updateUserResult.Succeeded)
                {
                    var rolesInDbForUser = this.UserManager.GetRoles(appUser.Id).ToList();

                    var changes1 = rolesInDbForUser.Except(user.Roles); // remove roles
                    // var changes2 = user.Roles.Except(rolesInDbForUser); // add roles

                    if (changes1.Count() > 0)
                    {
                        this.UserManager.RemoveFromRoles(appUser.Id, changes1.ToArray()); // remove roles
                    }

                    //if (changes2.Count() > 0)
                    //{
                    //    this.UserManager.AddToRoles(appUser.Id, changes2.ToArray()); // add roles
                    //}

                    var currentRoles = appContxt.UserRoles.Where(u => u.UserId == appUser.Id);
                    appContxt.UserRoles.RemoveRange(appContxt.UserRoles.Where(u => u.UserId == appUser.Id));

                    var role = this.RoleManager.Roles.Single(k => k.Id == user.RoleId);

                    this.UserManager.AddToRole(appUser.Id, role.Name);
                    appContxt.UserRoles.Add(new UserRole { RoleId = role.Id, UserId = appUser.Id });

                    if (role.Name.Equals("ASAPs User"))
                    {
                        appUser.PracticeId = 1;
                        this.UserManager.Update(appUser);
                    }

                    appContxt.SaveChanges();

                    var coreServiceUrl = WebConfigurationManager.AppSettings["Anzu.AnnPortal.Core.Service"];

                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(coreServiceUrl);
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        HttpResponseMessage response = client.PostAsJsonAsync("Portal/api/Practice/UpdatePracticeUser", new
                        PracticeUserUpdateDTO
                        {
                            UserId = appUser.Id,
                            FirstName = appUser.FirstName,
                            LastName = appUser.LastName
                        }).Result;

                        if (response.IsSuccessStatusCode)
                        {
                            var source = response.Content.ReadAsStringAsync();
                            dynamic result = source.Result;
                        }
                    }

                    if (changes1.Any() && changes1.FirstOrDefault().Equals("Practice User"))
                    {
                        appUser.PracticeName = null;

                        updateUserResult = this.UserManager.Update(appUser);
                        bool updatePracticeUserResult = false;

                        using (var client = new HttpClient())
                        {
                            client.BaseAddress = new Uri(coreServiceUrl);
                            client.DefaultRequestHeaders.Accept.Clear();
                            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                            HttpResponseMessage response = client.GetAsync(string.Format("Portal/api/Practice/RemovePracticeUser/{0}/", appUser.UserName)).Result;
                            if (response.IsSuccessStatusCode)
                            {
                                var source = response.Content.ReadAsStringAsync();
                                dynamic result = JObject.Parse(source.Result);
                                if (result != null)
                                {
                                    updatePracticeUserResult = result;
                                }
                            }

                        }
                    }
                    //foreach (var roleName in user.Roles)
                    //{
                    //    var role = this.RoleManager.Roles.Single(k => k.Name == roleName);
                    //    appContxt.UserRoles.Add(new UserRole { RoleId = role.Id, UserId = appUser.Id });
                    //}

                    // appContxt.UserOrganizations.RemoveRange(appContxt.UserOrganizations.Where(u => u.ApplicationUserId == appUser.Id));

                    //foreach (var hub in user.SecondaryHubIds)
                    //{
                    //    appContxt.UserOrganizations.Add(new UserOrganization { OrganizationId = hub, ApplicationUserId = appUser.Id });
                    //    appContxt.PreviousHubs.Add(new PreviousHubs { UserId = appUser.Id, HubId = hub, CreatedDate = DateTime.UtcNow, CreatedBy = appUser.ModifiedBy });
                    //}

                    // appContxt.PreviousHubs.Add(new PreviousHubs { UserId = appUser.Id, HubId = appUser.OrganizationId, CreatedDate = DateTime.UtcNow, CreatedBy = appUser.ModifiedBy });


                    appContxt.SaveChanges();
                    if (changes1.Count() > 0)
                    {
                        EmailService emailService = new EmailService();
                        // ApplicationUser currentUser = this.UserManager.FindByName(HttpContext.Current.User.Identity.Name);
                        emailService.SendEmail(EmailMessageType.RoleChanged, appUser, appUser, null, 0, changes1.FirstOrDefault(), role.Name);
                    }
                }
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        // raise a new exception nesting
                        // the current instance as InnerException
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
            }
            catch (Exception ex)
            {
                ch.LogMethodEndAPI(System.Reflection.MethodBase.GetCurrentMethod().Name, DateTime.Now);
            }

            if (!updateUserResult.Succeeded)
            {
                ch.LogMethodEndAPI(System.Reflection.MethodBase.GetCurrentMethod().Name, DateTime.Now);
                return GetErrorResult(updateUserResult);
            }
            ch.LogMethodEndAPI(System.Reflection.MethodBase.GetCurrentMethod().Name, DateTime.Now);
            return this.StatusCode(HttpStatusCode.Created);
        }

        /// <summary>
        /// Resets the user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        [Route("api/Account/reset")]
        [HttpPost]
        //[Authorize(Roles = "Administrator, HubAdmin")]
        public IHttpActionResult ResetUser(CreateUserViewModel user)
        {
            ch.LogMethodStartAPI(System.Reflection.MethodBase.GetCurrentMethod().Name, DateTime.Now);
            var result = this.ResetUserLogin(user.UserId);
            ch.LogMethodEndAPI(System.Reflection.MethodBase.GetCurrentMethod().Name, DateTime.Now);
            return result;
        }

        /// <summary>
        /// Resets the user login.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        private IHttpActionResult ResetUserLogin(string userId)
        {
            ch.LogMethodStartAPI(System.Reflection.MethodBase.GetCurrentMethod().Name, DateTime.Now);

            ApplicationUser appUser = this.UserManager.Users.Where<ApplicationUser>(a => a.UserName == userId).FirstOrDefault();

            if (appUser == null)
            {
                return this.StatusCode(HttpStatusCode.NotFound);
            }
            else
            {
                AccountReset reset = new AccountReset(this.UserManager, this.RoleManager);
                var updateUserResult = reset.ResetUserLogin(appUser);

                if (!updateUserResult.Succeeded)
                {
                    ch.LogMethodEndAPI(System.Reflection.MethodBase.GetCurrentMethod().Name, DateTime.Now);
                    return GetErrorResult(updateUserResult);
                }
                ch.LogMethodEndAPI(System.Reflection.MethodBase.GetCurrentMethod().Name, DateTime.Now);
                return this.StatusCode(HttpStatusCode.Created);
            }
        }

        /// <summary>
        /// Deactivates the user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        [Route("api/Account/deactivate")]
        [HttpPost]
        //[Authorize(Roles = "Administrator, HubAdmin")]
        public IHttpActionResult DeactivateUser(CreateUserViewModel user)
        {
            ch.LogMethodStartAPI(System.Reflection.MethodBase.GetCurrentMethod().Name, DateTime.Now);

            ApplicationUser appUser = this.UserManager.Users.Where<ApplicationUser>(a => a.UserName == user.UserId).FirstOrDefault();

            if (appUser == null)
            {
                return this.StatusCode(HttpStatusCode.NotFound);
            }
            else
            {
                appUser.StatusId = (int)StatusType.Deactivate;

                IdentityResult updateUserResult = null;

                try
                {
                    updateUserResult = this.UserManager.Update(appUser);

                    if (updateUserResult.Succeeded)
                    {

                        System.Runtime.Caching.ObjectCache cache = System.Runtime.Caching.MemoryCache.Default;
                        object cacheItem = cache.Get("DeactivatedUsers");

                        List<string> deactivatedUsers;

                        if (cacheItem != null)
                        {
                            deactivatedUsers = cacheItem as List<string>;

                            if (deactivatedUsers != null)
                            {
                                deactivatedUsers.Add(appUser.UserName);
                            }
                        }
                        else
                        {
                            deactivatedUsers = new List<string>();
                            deactivatedUsers.Add(appUser.UserName);
                        }

                    }
                }
                catch (Exception ex)
                {
                    ch.LogMethodEndAPI(System.Reflection.MethodBase.GetCurrentMethod().Name, DateTime.Now);
                }

                if (!updateUserResult.Succeeded)
                {
                    ch.LogMethodEndAPI(System.Reflection.MethodBase.GetCurrentMethod().Name, DateTime.Now);
                    return GetErrorResult(updateUserResult);
                }
                ch.LogMethodEndAPI(System.Reflection.MethodBase.GetCurrentMethod().Name, DateTime.Now);
                return this.StatusCode(HttpStatusCode.Created);
            }
        }

        /// <summary>
        /// Activates the user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        [Route("api/Account/activate")]
        [HttpPost]
        //[Authorize(Roles = "Administrator, HubAdmin")]
        public IHttpActionResult ActivateUser(CreateUserViewModel user)
        {
            ch.LogMethodStartAPI(System.Reflection.MethodBase.GetCurrentMethod().Name, DateTime.Now);

            ApplicationUser appUser = this.UserManager.Users.Where<ApplicationUser>(a => a.UserName == user.UserId).FirstOrDefault();
            ApplicationDbContext appContxt = new ApplicationDbContext();

            if (appUser == null)
            {
                return this.StatusCode(HttpStatusCode.NotFound);
            }
            else
            {
                appUser.StatusId = (int)StatusType.Activate;
                string tempPwd = System.Web.Security.Membership.GeneratePassword(9, 4);
                appUser.IsForceToChangePassword = true;
                appUser.PasswordHash = new PasswordHasher().HashPassword(tempPwd);

                IdentityResult updateUserResult = null;

                try
                {
                    updateUserResult = this.UserManager.Update(appUser);

                    if (updateUserResult.Succeeded)
                    {
                        UserManager.ResetAccessFailedCount(appUser.Id);

                        System.Runtime.Caching.ObjectCache cache = System.Runtime.Caching.MemoryCache.Default;
                        object cacheItem = cache.Get("DeactivatedUsers");

                        List<string> deactivatedUsers;

                        if (cacheItem != null)
                        {
                            deactivatedUsers = cacheItem as List<string>;

                            if (deactivatedUsers != null)
                            {
                                deactivatedUsers.RemoveAll(a => a.Equals(appUser.UserName));
                            }
                        }

                        appContxt.PreviousPasswords.Add(new PreviousPassword { UserId = appUser.Id, PasswordHash = new PasswordHasher().HashPassword(tempPwd), CreateDate = DateTime.UtcNow });
                        appContxt.SaveChanges();

                        EmailService emailService = new EmailService();
                        // ApplicationUser currentUser = this.UserManager.FindByName(HttpContext.Current.User.Identity.Name);
                        emailService.SendEmail(EmailMessageType.ActivateUser, appUser, appUser, tempPwd);
                    }
                }
                catch (Exception ex)
                {
                    ch.LogMethodEndAPI(System.Reflection.MethodBase.GetCurrentMethod().Name, DateTime.Now);
                }

                if (!updateUserResult.Succeeded)
                {
                    ch.LogMethodEndAPI(System.Reflection.MethodBase.GetCurrentMethod().Name, DateTime.Now);
                    return GetErrorResult(updateUserResult);
                }
                ch.LogMethodEndAPI(System.Reflection.MethodBase.GetCurrentMethod().Name, DateTime.Now);
                return this.StatusCode(HttpStatusCode.Created);
            }
        }

        /// <summary>
        /// Checks the user identifier.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        [Route("api/Account/CheckUserId")]
        [HttpGet]
        [Authorize(Roles = "Administrator, HubAdmin")]
        public IHttpActionResult CheckUserId(string userName)
        {
            ch.LogMethodStartAPI(System.Reflection.MethodBase.GetCurrentMethod().Name, DateTime.Now);

            IdentityUser user = this.UserManager.FindByName(userName);

            if (user != null)
            {
                ch.LogMethodEndAPI(System.Reflection.MethodBase.GetCurrentMethod().Name, DateTime.Now);
                return this.BadRequest(DUPLICATE_USER_ID);
            }
            else
            {
                ch.LogMethodEndAPI(System.Reflection.MethodBase.GetCurrentMethod().Name, DateTime.Now);
                return Ok();
            }
        }

        /// <summary>
        /// Checks the email identifier.
        /// </summary>
        /// <param name="emailId">The email identifier.</param>
        /// <returns></returns>
        [Route("api/Account/CheckEmailId")]
        [HttpGet]
        [Authorize(Roles = "Administrator, HubAdmin")]
        public IHttpActionResult CheckEmailId(string emailId)
        {
            ch.LogMethodStartAPI(System.Reflection.MethodBase.GetCurrentMethod().Name, DateTime.Now);

            IdentityUser user = this.UserManager.FindByEmail(emailId);

            if (user != null)
            {
                ch.LogMethodEndAPI(System.Reflection.MethodBase.GetCurrentMethod().Name, DateTime.Now);
                return this.BadRequest(DUPLICATE_EMAIL_ID);
            }
            else
            {
                ch.LogMethodEndAPI(System.Reflection.MethodBase.GetCurrentMethod().Name, DateTime.Now);
                return Ok();
            }
        }

        /// <summary>
        /// Gets the specified take.
        /// </summary>
        /// <param name="take">The take.</param>
        /// <param name="skip">The skip.</param>
        /// <param name="page">The page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="searchByUserId">The search by user identifier.</param>
        /// <param name="searchByUserName">Name of the search by user.</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public IHttpActionResult Get(int take, int skip, int page, int pageSize, string searchByUserId, string searchByUserName)
        {
            ch.LogMethodStartAPI(System.Reflection.MethodBase.GetCurrentMethod().Name, DateTime.Now);

            if (!string.IsNullOrEmpty(searchByUserId) || !string.IsNullOrEmpty(searchByUserName))
            {
                skip = 0;
            }

            List<CreateUserViewModel> createUserVMList = new List<CreateUserViewModel>();

            var modelMapper = new ViewModelMapperService();

            var users = new List<ApplicationUser>();

            var queryable = this.UserManager.Users;
            var queryableCount = this.UserManager.Users;

            var aaa = queryable.FirstOrDefault().Roles;

            if (!string.IsNullOrEmpty(searchByUserId))
            {
                queryable = queryable.Where(u => u.UserName.Contains(searchByUserId));
                queryableCount = queryableCount.Where(u => u.UserName.Contains(searchByUserId));
            }

            if (!string.IsNullOrEmpty(searchByUserName))
            {
                queryable = queryable.Where(u => u.FirstName.Contains(searchByUserName));
                queryableCount = queryableCount.Where(u => u.FirstName.Contains(searchByUserName));
            }

            int totalCount = queryableCount.Count();

            users = queryable.OrderBy(a => a.UserName).ToList();

            int noOfUsersRemoved = 0;
            List<ApplicationUser> appUsersToRemove = new List<ApplicationUser>();

            foreach (ApplicationUser appUser in users)
            {
                CreateUserViewModel userVM = new CreateUserViewModel();
                var roleList = this.UserManager.GetRoles(appUser.Id).ToList();

                if (roleList.Count > 0)
                {
                    if (roleList.Count == 1 && roleList.Remove(roleAdminName))
                    {
                        noOfUsersRemoved++;
                        appUsersToRemove.Add(appUser);
                    }
                    else
                    {
                        roleList.Remove(roleAdminName);

                        string userRoleList = string.Empty;

                        foreach (string roleName in roleList)
                        {
                            if (!string.IsNullOrEmpty(userRoleList))
                            {
                                userRoleList = string.Format("{0}, {1}", userRoleList, roleName);
                            }
                            else
                            {
                                userRoleList = string.Format("{0}", roleName);
                            }
                        }

                        userVM = modelMapper.MapUser(appUser);
                        userVM.UserRolesDisplay = userRoleList;
                        createUserVMList.Add(userVM);
                    }
                }
            }

            totalCount = totalCount - noOfUsersRemoved;
            var userIdsToRemove = appUsersToRemove.Select(u => u.Id).ToList();
            users.RemoveAll(u => userIdsToRemove.Contains(u.Id));

            var result = new
            {
                Data = createUserVMList.Skip(skip).Take(take).ToList(),
                Total = totalCount
            };
            ch.LogMethodEndAPI(System.Reflection.MethodBase.GetCurrentMethod().Name, DateTime.Now);
            return this.Ok(result);
        }

        /// <summary>
        /// Checks the user activate or deactivated.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Account/IsActiveUser")]
        public IHttpActionResult CheckUserActivateOrDeactivated()
        {
            ch.LogMethodStartAPI(System.Reflection.MethodBase.GetCurrentMethod().Name, DateTime.Now);

            System.Runtime.Caching.ObjectCache cache = System.Runtime.Caching.MemoryCache.Default;
            object cacheItem = cache.Get("DeactivatedUsers");

            List<string> deactivatedUsers;

            if (cacheItem != null)
            {
                deactivatedUsers = cacheItem as List<string>;

                if (deactivatedUsers != null)
                {
                    bool isExists = deactivatedUsers.Exists(a => a.Equals(User.Identity.Name));

                    if (isExists)
                    {
                        ch.LogMethodEndAPI(System.Reflection.MethodBase.GetCurrentMethod().Name, DateTime.Now);
                        return Ok("false");
                    }
                    else
                    {
                        ch.LogMethodEndAPI(System.Reflection.MethodBase.GetCurrentMethod().Name, DateTime.Now);
                        return Ok("true");
                    }
                }
                else
                {
                    ch.LogMethodEndAPI(System.Reflection.MethodBase.GetCurrentMethod().Name, DateTime.Now);
                    return Ok("true");
                }
            }
            else
            {
                return Ok("true");
            }
        }

        /// <summary>
        /// Authenticates the user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        [Route("api/Account/AuthenticateUser")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult AuthenticateUser(LoginViewModel user)
        {
            ch.LogMethodStartAPI(System.Reflection.MethodBase.GetCurrentMethod().Name, DateTime.Now);

            ApplicationDbContext dbCntxt = new ApplicationDbContext();
            ApplicationUser dbUser;

            dbUser = UserManager.Find(user.UserName, user.Password);
            if (dbUser != null)
            {
                ch.LogMethodEndAPI(System.Reflection.MethodBase.GetCurrentMethod().Name, DateTime.Now);
                return Ok("true");
            }
            else
            {
                ch.LogMethodEndAPI(System.Reflection.MethodBase.GetCurrentMethod().Name, DateTime.Now);
                return Ok("false");
            }
        }

        [Route("api/Account/AuthenticateUserPIN")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult AuthenticateUserPIN(LoginViewModel user)
        {
            ch.LogMethodStartAPI(System.Reflection.MethodBase.GetCurrentMethod().Name, DateTime.Now);

            ApplicationDbContext dbCntxt = new ApplicationDbContext();
            ApplicationUser dbUser;

            dbUser = this.UserManager.FindByName(user.UserName);
            if (dbUser != null && dbUser.PIN != null && dbUser.PIN == user.PIN)
            {
                ch.LogMethodEndAPI(System.Reflection.MethodBase.GetCurrentMethod().Name, DateTime.Now);
                return Ok("true");
            }
            else
            {
                ch.LogMethodEndAPI(System.Reflection.MethodBase.GetCurrentMethod().Name, DateTime.Now);
                return Ok("false");
            }
        }

        /// <summary>
        /// Gets the user.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        [Route("api/Account/user/{userName}")]
        [HttpGet]
        public IHttpActionResult GetUser(string userName)
        {
            ch.LogMethodStartAPI(System.Reflection.MethodBase.GetCurrentMethod().Name, DateTime.Now);

            ApplicationDbContext dbCntxt = new ApplicationDbContext();

            var modelMapper = new ViewModelMapperService();

            var dbUser = this.UserManager.Users.Where(u => u.UserName == userName).FirstOrDefault();

            if (dbUser == null)
            {
                return this.StatusCode(HttpStatusCode.NotFound);
            }
            else
            {
                CreateUserViewModel userVM = modelMapper.MapUser(dbUser);
                var designation = this.UserManager.GetUserDesignation(userVM.DesignationId);

                if (designation != null)
                {
                    userVM.UserType = designation.Name;
                }
                userVM.Roles = this.UserManager.GetRoles(dbUser.Id).ToList();

                userVM.SecondaryHubIds = dbCntxt.UserOrganizations.Where(r => r.ApplicationUserId == dbUser.Id).Select(k => k.Organization.Id).ToList();
                var oPrimaryHub = dbCntxt.Organizations.Where(o => o.Id == userVM.OrganizationId).First();
                userVM.PrimaryHub = oPrimaryHub.Name;

                userVM.SecondaryHubs = dbCntxt.UserOrganizations.Where(r => r.ApplicationUserId == dbUser.Id).Select(k => new HubMaster { Id = k.Organization.Id, Name = k.Organization.Name }).ToList();

                userVM.SecondaryHubs.Add(new HubMaster() { Id = oPrimaryHub.Id, Name = oPrimaryHub.Name });
                userVM.SecondaryHubs = userVM.SecondaryHubs.GroupBy(hub => hub.Id).Select(group => group.First()).ToList();

                ch.LogMethodEndAPI(System.Reflection.MethodBase.GetCurrentMethod().Name, DateTime.Now);
                return this.Ok<CreateUserViewModel>(userVM);
            }
        }

        [Route("api/Account/ChangeUserHub/{HubId}")]
        [HttpGet]
        public IHttpActionResult ChangeUserHub(string HubId)
        {
            ch.LogMethodStartAPI(System.Reflection.MethodBase.GetCurrentMethod().Name, DateTime.Now);

            var AuthenticationManager = HttpContext.Current.GetOwinContext().Authentication;
            var Identity = new ClaimsIdentity(User.Identity);
            Identity.RemoveClaim(Identity.FindFirst("CurrentHubId"));
            Identity.AddClaim(new Claim("CurrentHubId", HubId));

            Identity.RemoveClaim(Identity.FindFirst("MainColour"));
            Identity.RemoveClaim(Identity.FindFirst("MainColourDark"));
            Identity.RemoveClaim(Identity.FindFirst("MainColourDarkHover"));
            Identity.RemoveClaim(Identity.FindFirst("MainColourHover"));

            Identity.RemoveClaim(Identity.FindFirst("AccentColour"));


            //HubConfigDTO selectedHubConfig = new HubConfigDTO();
            using (var client = new HttpClient())
            {
                int hub = Int32.Parse(HubId);
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["Anzu.AnnPortal.Core.Service"]);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.GetAsync(string.Format("api/masterdata/gethubconfig/{0}", hub)).Result;
                if (response.IsSuccessStatusCode)
                {
                    //selectedHubConfig = response.Content.ReadAsAsync<HubConfigDTO>();

                    var source = response.Content.ReadAsStringAsync();
                    if (source.Result != "null")
                    {
                        dynamic selectedHubConfig = JObject.Parse(source.Result);


                        if (selectedHubConfig.MainColor_R != null && selectedHubConfig.MainColor_G != null && selectedHubConfig.MainColor_B != null)
                        {
                            var r = selectedHubConfig.MainColor_R.Value;
                            var g = selectedHubConfig.MainColor_G.Value;
                            var b = selectedHubConfig.MainColor_B.Value;

                            var rDark = (Int32.Parse(r) - 14);
                            var gDark = (Int32.Parse(g) - 15);
                            var bDark = (Int32.Parse(b) - 14);

                            var rDarkHover = (Int32.Parse(r) - 22);
                            var gDarkHover = (Int32.Parse(g) - 29);
                            var bDarkHover = (Int32.Parse(b) - 25);

                            var rHover = (Int32.Parse(r) - 14);
                            var gHover = (Int32.Parse(g) - 15);
                            var bHover = (Int32.Parse(b) - 14);


                            var mainC = "rgb(" + r + "," + g + "," + b + ")";
                            var mainDarkC = "rgb(" + rDark + "," + gDark + "," + bDark + ")";
                            var mainDarkHoverC = "rgb(" + rDarkHover + "," + gDarkHover + "," + bDarkHover + ")";
                            var mainCHover = "rgb(" + rHover + "," + gHover + "," + bHover + ")";

                            Identity.AddClaim(new Claim("MainColour", mainC)); /// RGB Main
                            Identity.AddClaim(new Claim("MainColourDark", mainDarkC)); /// RGB Main
                            Identity.AddClaim(new Claim("MainColourDarkHover", mainDarkHoverC)); /// RGB Main
                            Identity.AddClaim(new Claim("MainColourHover", mainCHover)); /// RGB Main
                        }
                        else
                        {
                            Identity.AddClaim(new Claim("MainColour", "NULL")); /// RGB Main
                            Identity.AddClaim(new Claim("MainColourDark", "NULL")); /// RGB Main
                            Identity.AddClaim(new Claim("MainColourDarkHover", "NULL")); /// RGB Main
                            Identity.AddClaim(new Claim("MainColourHover", "NULL")); /// RGB Main
                        }

                        if (selectedHubConfig.AccentColor_R != null && selectedHubConfig.AccentColor_G != null && selectedHubConfig.AccentColor_B != null)
                        {
                            var accentr = selectedHubConfig.AccentColor_R.Value;
                            var accentg = selectedHubConfig.AccentColor_G.Value;
                            var accentb = selectedHubConfig.AccentColor_B.Value;
                            var accentC = "rgb(" + accentr + "," + accentg + "," + accentb + ")";
                            Identity.AddClaim(new Claim("AccentColour", accentC)); /// RGB Main
                        }
                        else
                        {
                            Identity.AddClaim(new Claim("AccentColour", "NULL")); /// RGB Main
                        }
                    }
                    else
                    {
                        Identity.AddClaim(new Claim("MainColour", "NULL")); /// RGB Main
                        Identity.AddClaim(new Claim("MainColourDark", "NULL")); /// RGB Main
                        Identity.AddClaim(new Claim("MainColourDarkHover", "NULL")); /// RGB Main
                        Identity.AddClaim(new Claim("MainColourHover", "NULL")); /// RGB Main

                        Identity.AddClaim(new Claim("AccentColour", "NULL")); /// RGB Main
                    }

                }
                else
                {
                    Identity.AddClaim(new Claim("MainColour", "NULL")); /// RGB Main
                    Identity.AddClaim(new Claim("MainColourDark", "NULL")); /// RGB Main
                    Identity.AddClaim(new Claim("MainColourDarkHover", "NULL")); /// RGB Main
                    Identity.AddClaim(new Claim("MainColourHover", "NULL")); /// RGB Main

                    Identity.AddClaim(new Claim("AccentColour", "NULL")); /// RGB Main
                }
            }


            string name = SetHubDetails(Convert.ToInt32(HubId));
            Identity.RemoveClaim(Identity.FindFirst("CurrentHubName"));

            Identity.AddClaim(new Claim("CurrentHubName", name)); /// Add Hub Id to the Identity 
                                                                  /// 
            var coreServiceUrl = WebConfigurationManager.AppSettings["Anzu.AnnPortal.Core.Service"];
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(coreServiceUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.GetAsync(string.Format("api/masterdata/getselectedhub/{0}", HubId)).Result;
                if (response.IsSuccessStatusCode)
                {
                    var source = response.Content.ReadAsStringAsync();
                    dynamic hubData = JObject.Parse(source.Result);
                    if (hubData != null)
                    {
                        string hubTimeZone = hubData.TimeZone;
                        Identity.RemoveClaim(Identity.FindFirst("CurrentHubTimeZone"));
                        Identity.AddClaim(new Claim("CurrentHubTimeZone", hubTimeZone)); /// Add Hub Time zone to the Identity 
                    }
                }
            }

            AuthenticationManager.AuthenticationResponseGrant = new AuthenticationResponseGrant(new ClaimsPrincipal(Identity), new AuthenticationProperties { IsPersistent = true });

            ch.LogMethodEndAPI(System.Reflection.MethodBase.GetCurrentMethod().Name, DateTime.Now);
            return this.Ok();
        }

        [Route("api/Account/DistinctUsers")]
        [HttpGet]
        public IHttpActionResult GetDistinctUserName()
        {
            ch.LogMethodStartAPI(System.Reflection.MethodBase.GetCurrentMethod().Name, DateTime.Now);

            ApplicationDbContext dbCntxt = new ApplicationDbContext();
            var modelMapper = new ViewModelMapperService();

            var users = this.UserManager.Users.ToList<ApplicationUser>().Select(u => modelMapper.MapUserList(u));
            users = users.ToList();
            var result = this.Ok(users);
            ch.LogMethodEndAPI(System.Reflection.MethodBase.GetCurrentMethod().Name, DateTime.Now);
            return result;
        }

        private string SetHubDetails(int HubId)
        {
            ch.LogMethodStartAPI(System.Reflection.MethodBase.GetCurrentMethod().Name, DateTime.Now);

            ApplicationDbContext dbCntxt = new ApplicationDbContext();
            var result = dbCntxt.Organizations.Where(o => o.Id == HubId).Select(o => o.Name).FirstOrDefault();
            ch.LogMethodEndAPI(System.Reflection.MethodBase.GetCurrentMethod().Name, DateTime.Now);
            return result;
        }
        [Route("api/Account/GetUsersForRolesAndHubs")]
        [HttpPost]
        public IHttpActionResult GetUsersForRoles(NotificationHubRoleDTO notificationHubRoleDTO)
        {
            ch.LogMethodStartAPI(System.Reflection.MethodBase.GetCurrentMethod().Name, DateTime.Now);
            ApplicationDbContext dbCntxt = new ApplicationDbContext();
            var modelMapper = new ViewModelMapperService();

            var roleUsers = dbCntxt.UserRoles.Where(ur => notificationHubRoleDTO.Roles.Contains(ur.Role.Name)).Select(ur => ur.User.UserName);
            var hubUsers = dbCntxt.UserOrganizations.Where(ur => notificationHubRoleDTO.Hubs.Contains(ur.OrganizationId)).Select(ur => ur.ApplicationUser.UserName);
            var users = roleUsers.Union(hubUsers).Distinct();
            var result = this.Ok(users);
            ch.LogMethodEndAPI(System.Reflection.MethodBase.GetCurrentMethod().Name, DateTime.Now);
            return result;
        }

        /// <summary>
        /// Gets the user.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        [Route("api/account/getrnlist/{careProvider}/{hubid}")]
        [AllowAnonymous]
        [HttpGet]
        public List<UserCompressedViewModel> GetRNList(string careProvider, int hubid)
        {
            ch.LogMethodStartAPI(System.Reflection.MethodBase.GetCurrentMethod().Name, DateTime.Now);

            ApplicationDbContext dbCntxt = new ApplicationDbContext();

            var modelMapper = new ViewModelMapperService();


            var dbUser = this.UserManager.Users.Where(u => (u.DesignationId == (int)Anzu.AnnPortal.Identity.Common.Model.Enum.DesignationTypes.HubRN || u.DesignationId == (int)Anzu.AnnPortal.Identity.Common.Model.Enum.DesignationTypes.Pharmacist)
                                                     && u.StatusId == (int)Anzu.AnnPortal.Identity.Common.Model.Enum.StatusType.Activate && u.OrganizationId == hubid).ToList();

            if (dbUser == null)
            {
                //return this.StatusCode(HttpStatusCode.NotFound);
                return new List<UserCompressedViewModel>();
            }
            else
            {
                //var users = dbUser.ToList<ApplicationUser>().Select(u => modelMapper.MapUserList(u));
                //var result = new
                //{
                //    data = users
                //};
                //return Ok(result);

                var users = dbUser.ToList<ApplicationUser>().Select(u => modelMapper.MapUserListToCompressedProperties(u));

                return users.ToList(); ;

            }
        }


        /// <summary>
        /// Gets the user.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        [Route("api/account/getphycisanlist/{careProvider}/{hubid}")]
        [AllowAnonymous]
        [HttpGet]
        public List<UserCompressedViewModel> GetPhycisanList(string careProvider, int hubid)
        {
            ch.LogMethodStartAPI(System.Reflection.MethodBase.GetCurrentMethod().Name, DateTime.Now);

            ApplicationDbContext dbCntxt = new ApplicationDbContext();

            var modelMapper = new ViewModelMapperService();

            var dbUser = this.UserManager.Users.Where(u => u.DesignationId == (int)Anzu.AnnPortal.Identity.Common.Model.Enum.DesignationTypes.HubProvider
                                                     && u.StatusId == (int)Anzu.AnnPortal.Identity.Common.Model.Enum.StatusType.Activate && u.OrganizationId == hubid).ToList();

            if (dbUser == null)
            {
                //return this.StatusCode(HttpStatusCode.NotFound);
                return new List<UserCompressedViewModel>();
            }
            else
            {
                //var users = dbUser.ToList<ApplicationUser>().Select(u => modelMapper.MapUserList(u));
                //var result = new
                //{
                //    data = users
                //};
                //return Ok(result);
                var users = dbUser.ToList<ApplicationUser>().Select(u => modelMapper.MapUserListToCompressedProperties(u));

                return users.ToList(); ;

            }
        }

        /// <summary>
        /// Gets the user.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        [Route("api/Account/getuserlist")]
        [HttpPost]
        public List<UserCompressedViewModel> GetUserListByName(List<string> userlist)
        {
            ch.LogMethodStartAPI(System.Reflection.MethodBase.GetCurrentMethod().Name, DateTime.Now);

            ApplicationDbContext dbCntxt = new ApplicationDbContext();

            //IdentityDbContext dbCotxtIdentity = new IdentityDbContext();

            var modelMapper = new ViewModelMapperService();

            var dbUser = this.UserManager.Users.FirstOrDefault();

            if (dbUser == null)
            {
                //return this.StatusCode(HttpStatusCode.NotFound);
                return new List<UserCompressedViewModel>();
            }
            else
            {
                var users = UserManager.Users.Where(e => userlist.Contains(e.UserName)).ToList<ApplicationUser>().Select(u => modelMapper.MapUserListToCompressedProperties(u));

                return users.ToList();
                //var result = new
                //{
                //    data = users
                //};
                //return Ok(result);

            }
        }

        /// <summary>
        /// Gets the user.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        [Route("api/Account/updateUserPractice")]
        [HttpPost]
        public IHttpActionResult UpdateUserPractice(PracticeUsersViewModel data)
        {
            ApplicationDbContext dbCntxt = new ApplicationDbContext();
            var modelMapper = new ViewModelMapperService();
            var isSuccess = false;

            // remove current assgined practice names
            var currentList = this.UserManager.Users.Where(x => x.PracticeName == data.practiceName).ToList();
            foreach (var currentUser in currentList)
            {
                currentUser.PracticeName = string.Empty;
                currentUser.PracticeId = 0;
                this.UserManager.Update(currentUser);
            }

            // update new practice name
            foreach (var id in data.userIdList)
            {
                var dbUser = this.UserManager.Users.Where(u => u.Id == id).FirstOrDefault();

                if (dbUser == null)
                {
                    ch.LogMethodEndAPI(System.Reflection.MethodBase.GetCurrentMethod().Name, DateTime.Now);
                    return this.StatusCode(HttpStatusCode.NotFound);
                }
                else
                {
                    dbUser.PracticeName = data.practiceName;
                    dbUser.PracticeId = data.practiceId;
                    dbUser.ModifiedDateTime = DateTime.UtcNow;

                    var updateUserResult = this.UserManager.Update(dbUser);
                    isSuccess = updateUserResult.Succeeded;
                }
            }

            if (data.userIdList.Count == 0 || isSuccess)
            {
                dbCntxt.SaveChanges();
                return this.StatusCode(HttpStatusCode.Created);
            }
            else
            {
                return this.StatusCode(HttpStatusCode.BadRequest);
            }
        }

        /// <summary>
        /// Updates profile image of the user
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("api/Account/UpdateProfilePicture")]
        [HttpPost]
        public IHttpActionResult UpdateProfilePicture(ProfileImageHandlerDTO data)
        {
            try
            {
                ApplicationUser appUser = this.UserManager.Users.Where<ApplicationUser>(a => a.Id == data.UserId).FirstOrDefault();

                appUser.DocumentContent = Convert.FromBase64String(data.ImageData);
                this.UserManager.Update(appUser);
            }
            catch (Exception Ex)
            {
                return this.StatusCode(HttpStatusCode.BadRequest);
            }

            return this.StatusCode(HttpStatusCode.Created);
        }
    }
}