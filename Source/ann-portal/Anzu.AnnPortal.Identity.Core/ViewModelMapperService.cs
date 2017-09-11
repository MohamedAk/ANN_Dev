using Anzu.AnnPortal.Identity.Common.Model;
using Anzu.AnnPortal.Identity.Data.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anzu.AnnPortal.Identity.Core
{
    public class ViewModelMapperService
    {
        public CreateUserViewModel MapUser(ApplicationUser appUser)
        {
            return new CreateUserViewModel
            {
                Email = appUser.Email,
                FirstName = appUser.FirstName,
                LastName = appUser.LastName,
                UserDesignation = appUser.UserDesignation,
                OrganizationId = appUser.OrganizationId,
                Roles = appUser.Roles.Select(r => r.RoleId).ToList(),
                UserId = appUser.UserName,
                LastModifiedDate = appUser.ModifiedDateTime != null ? appUser.ModifiedDateTime : appUser.CreatedDateTime,
                StatusId = appUser.StatusId,
                DesignationId = appUser.DesignationId,
                CreatedDateTime = appUser.CreatedDateTime,
                Id = appUser.Id,
                DigitalSignature = appUser.DigitalSignature,
                PracticeName = appUser.PracticeName,
                PracticeId = appUser.PracticeId,
                DocumentContent = appUser.DocumentContent
            };
        }

        public CreateUserViewModel MapUserList(ApplicationUser appUser)
        {
            return new CreateUserViewModel
            {
                Email = appUser.Email,
                FirstName = appUser.FirstName,
                LastName = appUser.LastName,
                UserDesignation = appUser.UserDesignation,
                OrganizationId = appUser.OrganizationId,
                Roles = appUser.Roles.Select(r => r.RoleId).ToList(),
                UserId = appUser.UserName,
                LastModifiedDate = appUser.ModifiedDateTime != null ? appUser.ModifiedDateTime : appUser.CreatedDateTime,
                StatusId = appUser.StatusId,
                DesignationId = appUser.DesignationId,
                CreatedDateTime = appUser.CreatedDateTime,
                Id = appUser.Id,
                PracticeName = appUser.PracticeName,
                PracticeId = appUser.PracticeId,
                DocumentContent = appUser.DocumentContent
            };
        }

        public CreateRoleViewModel MapRole(ApplicationRole appRole)
        {
            return new CreateRoleViewModel
            {
                LastUpdateDateTime = appRole.ModifiedDateTime != null ? appRole.ModifiedDateTime.Value : appRole.CreatedDateTime,
                Id = appRole.Id,
                Name = appRole.Name,
                StatusId = appRole.StatusId,
                PermissionIds = appRole.RolePermissions != null ? appRole.RolePermissions.Select(r => r.PermissionId).ToList() : new List<int>(),
                CreatedDateTime = appRole.CreatedDateTime
            };
        }

        public DesignationViewModel MapDesignation(Designation designation)
        {
            DesignationViewModel designationVM = null;

            if(designation != null)
            {
                designationVM = new DesignationViewModel();

                designationVM.Id = designation.Id;
                designationVM.Name = designation.Name;
            }

            return designationVM;
        }

        public UserCompressedViewModel MapUserListToCompressedProperties(ApplicationUser appUser)
        {
            return new UserCompressedViewModel
            {
                UserId = appUser.UserName,
                FirstName = appUser.FirstName,
                LastName = appUser.LastName,
                UserDesignation = appUser.UserDesignation,
                
            };
        }
    }
}
