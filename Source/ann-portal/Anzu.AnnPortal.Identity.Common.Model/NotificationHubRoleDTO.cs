using Anzu.AnnPortal.Identity.Common.Model.DTO;
using Anzu.AnnPortal.Identity.Common.Model.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Anzu.AnnPortal.Identity.Common.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class NotificationHubRoleDTO
    {

        /// <summary>
        /// The roles
        /// </summary>
        public List<string> Roles { get; set; }
        /// <summary>
        /// The hubs
        /// </summary>
        public List<int> Hubs { get; set; }
    } 
}