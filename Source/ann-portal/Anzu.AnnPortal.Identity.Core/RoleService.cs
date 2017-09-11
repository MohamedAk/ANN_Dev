using Anzu.AnnPortal.Identity.Common.Model;
using Anzu.AnnPortal.Identity.Common.Model.Enum;
using Anzu.AnnPortal.Identity.Data.Model;
using Anzu.AnnPortal.Identity.Data.Model.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anzu.AnnPortal.Identity.Core
{
    public class RoleService
    {
        /// <summary>
        /// Gets the active roles.
        /// </summary>
        /// <returns></returns>
        public List<CreateRoleViewModel> GetActiveRoles()
        {
            List<CreateRoleViewModel> activeRoles = new List<CreateRoleViewModel>();

            ApplicationDbContext appDbCntxt = new ApplicationDbContext();

            var dbRoles = appDbCntxt.Set<ApplicationRole>()
                                    .Where(r => r.StatusId == (int)StatusType.Activate && !r.Name.Equals("Admin")).OrderBy(o => o.Name)
                                    .ToList();

            ViewModelMapperService vmMapper = new ViewModelMapperService();

            foreach(ApplicationRole appRole in dbRoles)
            {
                activeRoles.Add(vmMapper.MapRole(appRole));
            }

            return activeRoles;
        }

        /// <summary>
        /// Gets all roles.
        /// </summary>
        /// <returns></returns>
        public List<CreateRoleViewModel> GetAllRoles()
        {
            List<CreateRoleViewModel> activeRoles = new List<CreateRoleViewModel>();

            ApplicationDbContext appDbCntxt = new ApplicationDbContext();

            var dbRoles = appDbCntxt.Set<ApplicationRole>().OrderBy(o => o.Name).ToList();

            ViewModelMapperService vmMapper = new ViewModelMapperService();

            foreach (ApplicationRole appRole in dbRoles)
            {
                activeRoles.Add(vmMapper.MapRole(appRole));
            }

            return activeRoles;
        }

        /// <summary>
        /// Searches the name of the role by.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public List<CreateRoleViewModel> SearchRoleByName(string text)
        {
            ApplicationDbContext dbCntxt = new ApplicationDbContext();

            List<ApplicationRole> appRoles = dbCntxt.Set<ApplicationRole>()
                                                    .Where(a => a.StatusId == (int)StatusType.Activate && a.Name.Contains(text) && !a.Name.Equals("Admin"))
                                                    .ToList();

            List<CreateRoleViewModel> activeRoles = new List<CreateRoleViewModel>();
            ViewModelMapperService vmMapper = new ViewModelMapperService();

            foreach (ApplicationRole appRole in appRoles)
            {
                activeRoles.Add(vmMapper.MapRole(appRole));
            }

            return activeRoles;
        }

        /// <summary>
        /// Gets the active roles.
        /// </summary>
        /// <returns></returns>
        public List<CreateRoleViewModel> FindRolesByUserName(string userName)
        {
            List<CreateRoleViewModel> activeRoles = new List<CreateRoleViewModel>();

            ApplicationDbContext appDbCntxt = new ApplicationDbContext();

            ApplicationUser appUser = appDbCntxt.Set<ApplicationUser>()
                                                .Where(u => u.UserName.Equals(userName))
                                                .FirstOrDefault();

            var dbRoles = appDbCntxt.Set<IdentityUserRole>()
                                    .Where(u => u.UserId.Equals(appUser.Id))
                                    .ToList();

            ViewModelMapperService vmMapper = new ViewModelMapperService();

            foreach (IdentityUserRole userRole in dbRoles)
            {
                activeRoles.Add(
                                vmMapper.MapRole(
                                                appDbCntxt.Set<ApplicationRole>()
                                                            .Where(r => r.Id.Equals(userRole.RoleId))
                                                            .FirstOrDefault()
                                                )
                                );
            }

            return activeRoles;
        }
    }
}
