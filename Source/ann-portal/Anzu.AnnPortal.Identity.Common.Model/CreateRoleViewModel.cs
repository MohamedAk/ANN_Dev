//----------------------------------------------------------------------- 
// <copyright file="CreateRoleViewModel.cs" company="Brandix i3"> 
//     Copyright Brandix i3 2015. All rights reserved. 
// </copyright>
// <summary>
// The CreateRoleViewModel class. target CLR version 4.0.30319.34209.
// Created by : 
// Created date time: 
// </summary>
//-----------------------------------------------------------------------
using Anzu.AnnPortal.Identity.Common.Model.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Anzu.AnnPortal.Identity.Common.Model
{
    public class CreateRoleViewModel
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the permission ids.
        /// </summary>
        /// <value>
        /// The permission ids.
        /// </value>
        public List<int> PermissionIds { get; set; }

        /// <summary>
        /// Gets or sets the last update date time.
        /// </summary>
        /// <value>
        /// The last update date time.
        /// </value>
        public DateTime LastUpdateDateTime { get; set; }

        /// <summary>
        /// Gets or sets the created date time.
        /// </summary>
        /// <value>
        /// The created date time.
        /// </value>
        public DateTime CreatedDateTime { get; set; }

        /// <summary>
        /// Gets or sets the status identifier.
        /// </summary>
        /// <value>
        /// The status identifier.
        /// </value>
        public int StatusId { get; set; }

        /// <summary>
        /// Gets or sets the user count.
        /// </summary>
        /// <value>
        /// The user count.
        /// </value>
        public int UserCount { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance is active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is active; otherwise, <c>false</c>.
        /// </value>
        public bool IsActive 
        { 
            get 
            {
                return StatusId == (int)StatusType.Activate ? true : false;
            } 
        }
    }
}