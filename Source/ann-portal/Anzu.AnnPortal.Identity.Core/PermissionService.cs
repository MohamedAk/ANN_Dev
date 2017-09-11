using Anzu.AnnPortal.Identity.Common.Model.Enum;
using Anzu.AnnPortal.Identity.Data.Model;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IdentityDataModels = Anzu.AnnPortal.Identity.Data.Model.Models;

namespace Anzu.AnnPortal.Identity.Core
{
    public class PermissionService
    {
        private const string PermissionError = "Permission could not be found. Permission Code: {0}";
        private const string UserError = "ApplicationUser could not be found. Username: {0}";

        /// <summary>
        /// Gets the permissions.
        /// </summary>
        /// <param name="roleList">The role list.</param>
        /// <returns></returns>
        public List<string> GetPermissions(List<string> roleList)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();
            var permissionList = dbContext.RolePermissions.Include("Permission")
                                                            .Where(r => roleList.Contains(r.ApplicationRole.Name)
                                                                    && r.ApplicationRole.StatusId == (int)StatusType.Activate)
                                                            .ToList().Select(r => r.Permission.Name);

            return roleList;
        }

        /// <summary>
        /// Gets the permission codes.
        /// </summary>
        /// <param name="roleList">The role list.</param>
        /// <returns></returns>
        public List<string> GetPermissionCodes(List<string> roleList)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();
            var rolepermissions = dbContext.RolePermissions.Include("Permission").Include("ApplicationRole")
                                                            .Where(r => r.ApplicationRole.StatusId == (int)StatusType.Activate)
                                                            .ToList();


            dbContext.RolePermissions.Include("Permission").Include("ApplicationRole")
                                                            .Where(r => roleList.Contains(r.ApplicationRole.Name)
                                                                    && r.ApplicationRole.StatusId == (int)StatusType.Activate)
                                                            .ToList().Select(r => r.Permission.Code).ToList();

            List<string> permissionList = new List<string>();

            foreach(var item in rolepermissions)
            {
                if(roleList.Contains(item.ApplicationRole.Name))
                {
                    permissionList.Add(item.Permission.Code);
                }
            }

            return permissionList;
        }

        /// <summary>
        /// Determines whether [has moduel permission] [the specified permission module].
        /// </summary>
        /// <param name="permissionModule">The permission module.</param>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        public bool HasModuelPermission(PermissionModule permissionModule, string userName)
        {
            bool isHaveRight = false;

            ApplicationDbContext dbContext = new ApplicationDbContext();

            // get permissions in the module
            var permissionsInModule = dbContext.Set<IdentityDataModels.Module>().Include("Permissions")
                                                .Where(m => m.PermissionCategoryId == (int)permissionModule)
                                                .SelectMany(p => p.Permissions).ToList();

            // get logged in user
            var selectUserQuery = dbContext.Set<IdentityDataModels.ApplicationUser>()
                                            .Where(a => a.UserName.Equals(userName) && a.StatusId == 1)
                                            .FirstOrDefault();

            // get roles for user
            var userRoles = dbContext.Set<IdentityUserRole>()
                                    .Where(u => u.UserId.Equals(selectUserQuery.Id))
                                    .Select(r => r.RoleId)
                                    .ToList();

            List<IdentityDataModels.Permission> rolePermissions = new List<IdentityDataModels.Permission>();

            foreach (string roleId in userRoles)
            {
                var aa = dbContext.Set<IdentityDataModels.RolePermission>().Include("Permission")
                                    .Where(rp => rp.ApplicationRoleId.Equals(roleId)).Select(a => a.Permission).ToList();

                foreach (IdentityDataModels.Permission permission in aa)
                {
                    rolePermissions.Add(permission);
                }
            }

            rolePermissions = rolePermissions.Distinct().ToList();

            var hasPermissions = from p in rolePermissions
                                 join q in permissionsInModule on p.Id equals q.Id
                                 select p;

            if (hasPermissions.Any())
                isHaveRight = true;

            return isHaveRight;
        }

        /// <summary>
        /// Determines whether the specified permission code has permission.
        /// </summary>
        /// <param name="permissionCode">The permission code.</param>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        public bool HasPermission(string permissionCode, string userName)
        {
            bool hasPermission = false;

            ApplicationDbContext dbContext = new ApplicationDbContext();

            // get the associated permission
            IdentityDataModels.Permission permission = dbContext.Set<IdentityDataModels.Permission>().Where(p => p.Code.Equals(permissionCode)).FirstOrDefault();

            if (permission == null)
            {
                throw new ApplicationException(String.Format(PermissionError, permissionCode));
            }

            // get the user
            IdentityDataModels.ApplicationUser appUser = dbContext.Set<IdentityDataModels.ApplicationUser>().Where(u => u.UserName.Equals(userName) && u.StatusId == 1).FirstOrDefault();

            if (appUser == null)
            {
                throw new ApplicationException(String.Format(UserError, userName));
            }

            // get roles for user
            var userRoles = dbContext.Set<IdentityUserRole>()
                                    .Where(u => u.UserId.Equals(appUser.Id))
                                    .Select(r => r.RoleId)
                                    .ToList();

            List<IdentityDataModels.Permission> rolePermissions = new List<IdentityDataModels.Permission>();

            foreach (string roleId in userRoles)
            {
                var aa = dbContext.Set<IdentityDataModels.RolePermission>().Include("Permission")
                                    .Where(rp => rp.ApplicationRoleId.Equals(roleId)).Select(a => a.Permission).ToList();

                foreach (IdentityDataModels.Permission p in aa)
                {
                    rolePermissions.Add(p);
                }
            }

            rolePermissions = rolePermissions.Distinct().ToList();

            if (permission != null && rolePermissions.Exists(r => r.Id == permission.Id))
            {
                hasPermission = true;
            }

            return hasPermission;
        }

        /// <summary>
        /// Determines whether [has admin rights] [the specified user name].
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        public bool HasAdminRights(string userName)
        {
            bool hasAdminRights = false;

            string roleName = "Admin";

            ApplicationDbContext appDbContext = new ApplicationDbContext();
            var adminRole = appDbContext.Set<IdentityRole>()
                                        .Where(r => r.Name.Equals(roleName))
                                        .FirstOrDefault();

            if(adminRole != null)
            {
                string adminRoleId = adminRole.Id;

                var appUser = appDbContext.Set<IdentityUser>().Where(u => u.UserName.Equals(userName)).FirstOrDefault();

                if(appUser != null)
                {
                    var roleAdminUserRole = appDbContext.Set<IdentityUserRole>()
                                                    .Where(u => u.RoleId.Equals(adminRoleId) && u.UserId.Equals(appUser.Id))
                                                    .FirstOrDefault();

                    if (roleAdminUserRole != null)
                    {
                        hasAdminRights = true;
                    }
                }
            }

            return hasAdminRights;
        }

        /// <summary>
        /// Determines whether [has multiple permissions] [the specified username].
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="codeList">The role list.</param>
        /// <returns></returns>
        public bool HasMultiplePermissions(string userName, List<string> codeList)
        {
            bool hasPermission = false;

            ApplicationDbContext dbContext = new ApplicationDbContext();

            // get the associated permission
            List<IdentityDataModels.Permission> permissions = dbContext.Set<IdentityDataModels.Permission>().Where(p => codeList.Contains(p.Code)).ToList();

            if (permissions == null)
            {
                throw new ApplicationException(String.Format(PermissionError, string.Empty));
            }

            // get the user
            IdentityDataModels.ApplicationUser appUser = dbContext.Set<IdentityDataModels.ApplicationUser>().Where(u => u.UserName.Equals(userName) && u.StatusId == 1).FirstOrDefault();

            if (appUser == null)
            {
                throw new ApplicationException(String.Format(UserError, userName));
            }

            // get roles for user
            var userRoles = dbContext.Set<IdentityUserRole>()
                                    .Where(u => u.UserId.Equals(appUser.Id))
                                    .Select(r => r.RoleId)
                                    .ToList();

            List<IdentityDataModels.Permission> rolePermissions = new List<IdentityDataModels.Permission>();

            foreach (string roleId in userRoles)
            {
                var rolePermissionsInDb = dbContext.Set<IdentityDataModels.RolePermission>().Include("Permission")
                                        .Where(rp => rp.ApplicationRoleId.Equals(roleId)).Select(a => a.Permission).ToList();

                foreach (IdentityDataModels.Permission permissionObj in rolePermissionsInDb)
                {
                    rolePermissions.Add(permissionObj);
                }
            }

            rolePermissions = rolePermissions.Distinct().ToList();

            if(permissions != null && permissions.Any())
            {
                if(permissions.Except(rolePermissions).Count() == 0)
                {
                    hasPermission = true;
                }
            }

            return hasPermission;
        }

        /// <summary>
        /// Determines whether [has one permission from list] [the specified user name].
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="codeList">The code list.</param>
        /// <returns></returns>
        /// <exception cref="System.ApplicationException">
        /// </exception>
        public bool HasOnePermissionFromList(string userName, List<string> codeList)
        {
            bool hasPermission = false;

            ApplicationDbContext dbContext = new ApplicationDbContext();

            // get the associated permission
            List<IdentityDataModels.Permission> permissions = dbContext.Set<IdentityDataModels.Permission>().Where(p => codeList.Contains(p.Code)).ToList();

            if (permissions == null)
            {
                throw new ApplicationException(String.Format(PermissionError, string.Empty));
            }

            // get the user
            IdentityDataModels.ApplicationUser appUser = dbContext.Set<IdentityDataModels.ApplicationUser>().Where(u => u.UserName.Equals(userName) && u.StatusId == 1).FirstOrDefault();

            if (appUser == null)
            {
                throw new ApplicationException(String.Format(UserError, userName));
            }

            // get roles for user
            var userRoles = dbContext.Set<IdentityUserRole>()
                                    .Where(u => u.UserId.Equals(appUser.Id))
                                    .Select(r => r.RoleId)
                                    .ToList();

            List<IdentityDataModels.Permission> rolePermissions = new List<IdentityDataModels.Permission>();

            foreach (string roleId in userRoles)
            {
                var rolePermissionsInDb = dbContext.Set<IdentityDataModels.RolePermission>().Include("Permission")
                                        .Where(rp => rp.ApplicationRoleId.Equals(roleId)).Select(a => a.Permission).ToList();

                foreach (IdentityDataModels.Permission permissionObj in rolePermissionsInDb)
                {
                    rolePermissions.Add(permissionObj);
                }
            }

            rolePermissions = rolePermissions.Distinct().ToList();

            if (permissions != null && permissions.Any())
            {
                if (permissions.Except(rolePermissions).Count() < permissions.Count)
                {
                    hasPermission = true;
                }
            }

            return hasPermission;
        }
    }
}
