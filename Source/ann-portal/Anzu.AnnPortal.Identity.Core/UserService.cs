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
    public class UserService
    {
        /// <summary>
        /// Gets the active users.
        /// </summary>
        /// <param name="noOfRecords">The no of records.</param>
        /// <returns>if no parameter is passed all records are returned</returns>
        public List<CreateUserViewModel> GetActiveUsers(int noOfRecords = 0)
        {
            ApplicationDbContext dbCntxt = new ApplicationDbContext();
            List<ApplicationUser> appUsers = null;

            if (noOfRecords == 0)
            {
                appUsers = dbCntxt.Set<ApplicationUser>().Where(a => a.StatusId == (int)StatusType.Activate).ToList();
            }
            else
            {
                appUsers = dbCntxt.Set<ApplicationUser>().Where(a => a.StatusId == (int)StatusType.Activate).Take(noOfRecords).ToList();
            }

            List<CreateUserViewModel> activeUsers = new List<CreateUserViewModel>();
            ViewModelMapperService vmMapper = new ViewModelMapperService();

            foreach (ApplicationUser appUser in appUsers)
            {
                activeUsers.Add(vmMapper.MapUser(appUser));
            }

            return activeUsers;
        }

        /// <summary>
        /// Searches the active users.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public List<CreateUserViewModel> SearchActiveUsers(string text)
        {
            ApplicationDbContext dbCntxt = new ApplicationDbContext();

            List<ApplicationUser> appUsers = dbCntxt.Set<ApplicationUser>()
                                                    .Where(a => a.StatusId == (int)StatusType.Activate && (a.FirstName.Contains(text) || a.LastName.Contains(text)))
                                                    .ToList();

            List<CreateUserViewModel> activeUsers = new List<CreateUserViewModel>();
            ViewModelMapperService vmMapper = new ViewModelMapperService();

            foreach (ApplicationUser appUser in appUsers)
            {
                activeUsers.Add(vmMapper.MapUser(appUser));
            }

            return activeUsers;
        }

        /// <summary>
        /// Gets the user by identifier.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        public CreateUserViewModel GetUserById(string userName)
        {
            CreateUserViewModel userFound = null;

            ApplicationDbContext dbCntxt = new ApplicationDbContext();

            var appUser = dbCntxt.Set<ApplicationUser>().Where(u => u.UserName.Equals(userName)).FirstOrDefault();

            ViewModelMapperService mapper = new ViewModelMapperService();

            if (appUser != null)
                userFound = mapper.MapUser(appUser);

            return userFound;
        }

        /// <summary>
        /// Gets all users.
        /// </summary>
        /// <returns></returns>
        public List<CreateUserViewModel> GetAllUsers()
        {
            ApplicationDbContext dbCntxt = new ApplicationDbContext();
            List<ApplicationUser> appUsers = null;

            appUsers = dbCntxt.Set<ApplicationUser>().ToList();

            List<CreateUserViewModel> activeUsers = new List<CreateUserViewModel>();
            ViewModelMapperService vmMapper = new ViewModelMapperService();

            foreach (ApplicationUser appUser in appUsers)
            {
                activeUsers.Add(vmMapper.MapUser(appUser));
            }

            return activeUsers;
        }

        /// <summary>
        /// Gets all hub providers.
        /// </summary>
        /// <returns></returns>
        public List<CreateUserViewModel> GetAllHubProviders()
        {
            //List<CreateUserViewModel> hubProviders = new List<CreateUserViewModel>(); ;

            //ApplicationDbContext appDb = new ApplicationDbContext();

            ///**
            // * issue AV-2770
            // * changed get hub provider query to get clinical staff
            // * tables were not changed
            // * **/
            //var users = appDb.Set<ApplicationUser>()
            //                        .Where(u => u.DesignationId.Value == (int)Avera.Identity.Common.Model.Enum.DesignationTypes.Clinical
            //                            && u.StatusId == (int)Avera.Identity.Common.Model.Enum.StatusType.Activate)
            //                        .ToList<ApplicationUser>();

            //ViewModelMapperService vmMapper = new ViewModelMapperService();

            //foreach (ApplicationUser appUser in users)
            //{
            //    hubProviders.Add(vmMapper.MapUser(appUser));
            //}

            return null;
        }

        /// <summary>
        /// Gets all hub rn.
        /// </summary>
        /// <returns></returns>
        public List<CreateUserViewModel> GetAllHubRN()
        {
            //List<CreateUserViewModel> hubRN = new List<CreateUserViewModel>(); ;

            //ApplicationDbContext appDb = new ApplicationDbContext();
            //var users = appDb.Set<ApplicationUser>()
            //                        .Where(u => u.DesignationId.Value == (int)Avera.Identity.Common.Model.Enum.DesignationTypes.HubRN
            //                        && u.StatusId == (int)Avera.Identity.Common.Model.Enum.StatusType.Activate)
            //                        .ToList<ApplicationUser>();

            //ViewModelMapperService vmMapper = new ViewModelMapperService();

            //foreach (ApplicationUser appUser in users)
            //{
            //    hubRN.Add(vmMapper.MapUser(appUser));
            //}

            return null;
        }

        /// <summary>
        /// Gets all deactivated user ids.
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllDeactivatedUserIds()
        {
            ApplicationDbContext appDb = new ApplicationDbContext();

            var deActivateUsers = appDb.Set<ApplicationUser>()
                                    .Where(u => u.StatusId == (int)Anzu.AnnPortal.Identity.Common.Model.Enum.StatusType.Deactivate)
                                    .Select(u => u.UserName)
                                    .ToList<string>();

            return deActivateUsers;
        }

        /// <summary>
        /// Gets all hub providers.
        /// </summary>
        /// <returns></returns>
        public List<CreateUserViewModel> GetHubProviders()
        {
            List<CreateUserViewModel> hubProviders = new List<CreateUserViewModel>(); ;

            ApplicationDbContext appDb = new ApplicationDbContext();

            var users = appDb.Set<ApplicationUser>()
                                    .Where(u => u.DesignationId == (int)Anzu.AnnPortal.Identity.Common.Model.Enum.DesignationTypes.HubProvider
                                        && u.StatusId == (int)Anzu.AnnPortal.Identity.Common.Model.Enum.StatusType.Activate)
                                    .ToList<ApplicationUser>();

            ViewModelMapperService vmMapper = new ViewModelMapperService();

            foreach (ApplicationUser appUser in users)
            {
                hubProviders.Add(vmMapper.MapUser(appUser));
            }

            return hubProviders;
        }

        public ApplicationUser GetAdminUser()
        {
            ApplicationDbContext appDbCntxt = new ApplicationDbContext();

            ApplicationUser appUser = appDbCntxt.Set<ApplicationUser>()
                                                .Where(u => u.UserName.Equals("SysAdmin"))
                                                .FirstOrDefault();
            return appUser;
        }
    }
}
