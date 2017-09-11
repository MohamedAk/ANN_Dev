using Anzu.AnnPortal.Identity.Common.Model;
using Anzu.AnnPortal.Identity.Common.Model.Enum;
using Anzu.AnnPortal.Identity.Core;
using Anzu.AnnPortal.Identity.Data.Model;
using Anzu.AnnPortal.Identity.Data.Model.Models;
using Anzu.AnnPortal.Identity.Service.Filters;
using Anzu.AnnPortal.Identity.Service.IdentityExtentions;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Anzu.AnnPortal.Identity.Service.Controllers
{
    [Authorize]
    public class RoleController : BaseApiController
    {
        public ApplicationRoleManager RoleManager { get; private set; }

        private const string DUPLICATE_ROLE_NAME = "Entered role name exists in the system";
        private const string PERMISSION_CHECK = "Please select at least one permission to create the role";
        private const string USERS_EXIST = "Role cannot be deactivated when assigned to users";
        private const string EMPTY_ROLE = "Role name cannot be empty";
        private const string roleAdminName = "Admin";
        CommonHelper ch;

        public RoleController()
            : this(new ApplicationRoleManager())
        {
            this.ch = new CommonHelper();
        }

        public RoleController(ApplicationRoleManager roleManager)
        {
            this.RoleManager = roleManager;
            this.ch = new CommonHelper();
        }

        [Route("api/Role/activeRoles")]
        [HttpGet]
        [AllowAnonymous]
        public IHttpActionResult GetRoles()
        {
            ApplicationDbContext dbCntxt = new ApplicationDbContext();

            List<CreateRoleViewModel> roleViewModelList = new List<CreateRoleViewModel>();
            ViewModelMapperService vmMapper = new ViewModelMapperService();

            List<IdentityRole> identityRoleList = this.RoleManager.Roles.Where(r => r.StatusId == (int)StatusType.Activate).ToList<IdentityRole>();

            foreach (ApplicationRole role in identityRoleList)
            {
                CreateRoleViewModel roleVM = vmMapper.MapRole(role);
                roleVM.UserCount = dbCntxt.Set<IdentityUserRole>()
                                            .Count(r => r.RoleId == role.Id);
                roleViewModelList.Add(roleVM);
            }
            return this.Ok<IEnumerable<CreateRoleViewModel>>(roleViewModelList);
        }

        [Route("api/Role/allRoles")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetAllRoles()
        {
            ApplicationDbContext dbCntxt = new ApplicationDbContext();

            List<CreateRoleViewModel> roleViewModelList = new List<CreateRoleViewModel>();
            ViewModelMapperService vmMapper = new ViewModelMapperService();

            List<IdentityRole> identityRoleList = this.RoleManager.Roles.ToList<IdentityRole>();

            foreach (ApplicationRole role in identityRoleList)
            {
                CreateRoleViewModel roleVM = vmMapper.MapRole(role);
                roleVM.UserCount = dbCntxt.Set<IdentityUserRole>()
                                            .Count(r => r.RoleId == role.Id);
                roleViewModelList.Add(roleVM);
            }
            return this.Ok<IEnumerable<CreateRoleViewModel>>(roleViewModelList);
        }

        [Route("api/Role/activateRole")]
        [HttpPut]
        [Authorize]
        public async Task<IHttpActionResult> ActivateRole(CreateRoleViewModel role)
        {


            ApplicationRole appRole = await this.RoleManager.FindByNameAsync(role.Name);

            appRole.StatusId = (int)StatusType.Activate;

            IdentityResult updateResult = null;

            try
            {
                updateResult = await this.RoleManager.UpdateAsync(appRole);
            }
            catch (Exception ex)
            {

            }

            if (!updateResult.Succeeded)
            {

                return GetErrorResult(updateResult);
            }

            return this.StatusCode(HttpStatusCode.Created);
        }

        [Route("api/Role/deactivateRole")]
        [HttpPut]
        [Authorize]
        public async Task<IHttpActionResult> DeactivateRole(CreateRoleViewModel role)
        {

            if (role.UserCount > 0)
            {
                return this.BadRequest(USERS_EXIST);
            }
            ApplicationRole appRole = await this.RoleManager.FindByNameAsync(role.Name);

            appRole.StatusId = (int)StatusType.Deactivate;

            IdentityResult updateResult = null;

            try
            {
                updateResult = await this.RoleManager.UpdateAsync(appRole);
            }
            catch (Exception ex)
            {

            }

            if (!updateResult.Succeeded)
            {

                return GetErrorResult(updateResult);
            }
            return this.StatusCode(HttpStatusCode.Created);
        }

        [Route("api/Role/create")]
        [HttpPost]
        [Authorize]
        public async Task<IHttpActionResult> CreateRole(CreateRoleViewModel role)
        {

            ApplicationRole appRole = new ApplicationRole();

            if (role.Name == null || role.Name == "")
            {
                return this.BadRequest(EMPTY_ROLE);
            }

            if (this.RoleManager.RoleExists(role.Name))
            {
                return this.BadRequest(DUPLICATE_ROLE_NAME);
            }

            if (role.PermissionIds != null ? role.PermissionIds.Count == 0 : true)
            {
                return this.BadRequest(PERMISSION_CHECK);
            }

            appRole.Id = Guid.NewGuid().ToString();
            appRole.StatusId = (int)StatusType.Activate;
            appRole.Name = role.Name;
            appRole.CreatedBy = User.Identity.Name;
            //appRole.CreatedBy = "SysAdmin";
            appRole.CreatedDateTime = DateTime.UtcNow;

            IdentityResult roleResult = null;

            try
            {
                roleResult = await this.RoleManager.CreateAsync(appRole);

                ApplicationDbContext appContxt = new ApplicationDbContext();

                foreach (int permissionId in role.PermissionIds)
                {
                    appContxt.RolePermissions.Add(new RolePermission { ApplicationRoleId = appRole.Id, PermissionId = permissionId });
                }

                appContxt.SaveChanges();
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

            }

            if (!roleResult.Succeeded)
            {

                return GetErrorResult(roleResult);
            }

            return this.StatusCode(HttpStatusCode.Created);
        }

        //[Authorize]
        [Route("api/Role/update")]
        [HttpPut]
        [Authorize]
        public async Task<IHttpActionResult> UpdateRole(CreateRoleViewModel role)
        {


            ApplicationRole appRole = this.RoleManager.FindById(role.Id);

            if (appRole == null)
            {
                return this.StatusCode(HttpStatusCode.NotFound);
            }
            if (appRole.Name != role.Name)
            {
                if (this.RoleManager.RoleExists(role.Name))
                {

                    return this.BadRequest(DUPLICATE_ROLE_NAME);
                }
            }

            if (role.PermissionIds != null ? role.PermissionIds.Count == 0 : true)
            {
                return this.BadRequest(PERMISSION_CHECK);
            }
            if (role.Name == null || role.Name == "")
            {
                return this.BadRequest(EMPTY_ROLE);
            }
            appRole.Name = role.Name;
            //appRole.ModifiedBy = "SysAdmin";
            appRole.ModifiedBy = User.Identity.Name;
            appRole.ModifiedDateTime = DateTime.UtcNow;

            IdentityResult roleResult = null;

            try
            {
                roleResult = await this.RoleManager.UpdateAsync(appRole);

                ApplicationDbContext appContxt = new ApplicationDbContext();

                // permission ids of the role
                var appRolePermissions = appContxt.RolePermissions.Where(r => r.ApplicationRoleId == appRole.Id).Select(a => a.PermissionId).ToList();

                var removeChanges = appRolePermissions.Except(role.PermissionIds); // removed permissions
                var addedChanges = role.PermissionIds.Except(appRolePermissions); // added permissions

                // remove permissions from role
                foreach (var permissionToRemove in removeChanges)
                {
                    appContxt.RolePermissions.Remove(appContxt.RolePermissions.Where(a => a.ApplicationRoleId == appRole.Id && a.PermissionId == permissionToRemove).FirstOrDefault());
                }

                // add permissions to role
                foreach (var permissionToAdd in addedChanges)
                {
                    appContxt.RolePermissions.Add(new RolePermission { ApplicationRoleId = appRole.Id, PermissionId = permissionToAdd });
                }

                appContxt.SaveChanges();

                if (addedChanges.Count() > 0 || removeChanges.Count() > 0)
                {
                    // send mail to users for role permission changes
                    var userIdsForRole = appContxt.Set<IdentityUserRole>()
                                                    .Where(userroles => userroles.RoleId == role.Id)
                                                    .Select(u => u.UserId)
                                                    .Distinct()
                                                    .ToList();

                    EmailService emailService = new EmailService();

                    ApplicationUserManager userManager = new ApplicationUserManager();
                    var sender = userManager.FindByName(User.Identity.Name);

                    foreach (string userId in userIdsForRole)
                    {
                        var appUser = userManager.FindById(userId);

                        if (appUser.StatusId == (int)StatusType.Activate)
                        {
                            emailService.SendEmail(EmailMessageType.RoleChanged, appUser, sender);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }

            if (!roleResult.Succeeded)
            {

                return GetErrorResult(roleResult);
            }

            return this.StatusCode(HttpStatusCode.Created);
        }

        [Route("api/Role/roleNameCheck")]
        [HttpPut]
        [Authorize]
        public async Task<IHttpActionResult> RoleNameCheck(string roleName)
        {


            ApplicationRole role = await this.RoleManager.FindByNameAsync(roleName);

            if (role != null)
            {

                return this.BadRequest(DUPLICATE_ROLE_NAME);
            }
            else
            {

                return this.Ok();
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IHttpActionResult> Get(string roleId)
        {

            ApplicationDbContext dbCntxt = new ApplicationDbContext();
            ApplicationRole role = await this.RoleManager.FindByIdAsync(roleId);
            role.RolePermissions = dbCntxt.RolePermissions.Where(r => r.ApplicationRoleId == role.Id).ToList();

            //ApplicationRole role = this.RoleManager.Roles.Include(r => r.Id == roleId);
            if (role != null)
            {
                ViewModelMapperService vmMapper = new ViewModelMapperService();
                CreateRoleViewModel roleVM = vmMapper.MapRole(role);

                roleVM.UserCount = dbCntxt.Set<IdentityUserRole>()
                                            .Count(r => r.RoleId == role.Id);

                var result = this.Ok<CreateRoleViewModel>(roleVM);

                return result;
            }
            else
            {

                return this.StatusCode(HttpStatusCode.NotFound);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public object Get(int take, int skip, int page, int pageSize)
        {


            List<CreateRoleViewModel> roleViewModelList = new List<CreateRoleViewModel>();
            var roles = this.RoleManager.Roles.ToList();

            // remove role admin to hide it
            var roleAdminObj = roles.Where(r => r.Name.Equals(roleAdminName)).FirstOrDefault();
            roles.Remove(roleAdminObj);

            List<ApplicationRole> identityRoleList = roles.OrderBy(r => r.Name).Skip(skip).Take(take).ToList();

            ViewModelMapperService vmMapper = new ViewModelMapperService();

            ApplicationDbContext dbCntxt = new ApplicationDbContext();

            ApplicationUserManager userManager = new ApplicationUserManager();
            List<string> activeUserIds = userManager.Users.Where(user => user.StatusId == (int)StatusType.Activate).Select(a => a.Id).ToList();

            foreach (ApplicationRole appRole in identityRoleList)
            {
                appRole.RolePermissions = dbCntxt.RolePermissions.Where(r => r.ApplicationRoleId == appRole.Id).ToList();

                CreateRoleViewModel roleVM = vmMapper.MapRole(appRole);
                roleVM.UserCount = dbCntxt.Set<IdentityUserRole>()
                                            .Count(r => r.RoleId == appRole.Id && activeUserIds.Contains(r.UserId));

                roleViewModelList.Add(roleVM);
            }
            var result = new
            {
                Data = roleViewModelList,
                Total = roles.Count
            };

            return result;
        }
    }
}
