//----------------------------------------------------------------------- 
// <copyright file="CreateUserViewModel.cs" company="Brandix i3"> 
//     Copyright Brandix i3 2015. All rights reserved. 
// </copyright>
// <summary>
// The CreateUserViewModel class. target CLR version 4.0.30319.34209.
// Created by : 
// Created date time: 
// </summary>
//-----------------------------------------------------------------------
using Anzu.AnnPortal.Identity.Common.Model.DTO;
using Anzu.AnnPortal.Identity.Common.Model.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Anzu.AnnPortal.Identity.Common.Model
{
    public class CreateUserViewModel
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        //[Required]
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        /// <value>
        /// The first name.
        /// </value>
        [Required]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        /// <value>
        /// The last name.
        /// </value>
        [Required]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the organization identifier.
        /// </summary>
        /// <value>
        /// The organization identifier.
        /// </value>
        //[Required]
        public int OrganizationId { get; set; }

        /// <summary>
        /// Gets or sets the secondary hub ids.
        /// </summary>
        /// <value>
        /// The secondary hub ids.
        /// </value>
        public List<int> SecondaryHubIds { get; set; }

        /// <summary>
        /// Gets or sets the designation identifier.
        /// </summary>
        /// <value>
        /// The designation identifier.
        /// </value>
        // [Required]
        public int DesignationId { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        [Required(ErrorMessage = "Please enter a valid email")]
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the user designation.
        /// </summary>
        /// <value>
        /// The user designation.
        /// </value>
        // [Required]
        public string UserDesignation { get; set; }

        /// <summary>
        /// Gets or sets the roles.
        /// </summary>
        /// <value>
        /// The roles.
        /// </value>
        // [Required]
        public List<string> Roles { get; set; }
        public string RoleId { get; set; }

        /// <summary>
        /// Gets or sets the digital signature.
        /// </summary>
        /// <value>
        /// The digital signature.
        /// </value>
        public byte[] DigitalSignature { get; set; }

        /// <summary>
        /// Gets or sets the status identifier.
        /// </summary>
        /// <value>
        /// The status identifier.
        /// </value>
        public int StatusId { get; set; }

        /// <summary>
        /// Gets the full name.
        /// </summary>
        /// <value>
        /// The full name.
        /// </value>
        public string FullName
        {
            get { return String.Format("{0} {1}", FirstName, LastName); }
        }

        /// <summary>
        /// Gets or sets the last modified date.
        /// </summary>
        /// <value>
        /// The last modified date.
        /// </value>
        public DateTime? LastModifiedDate { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance is active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is active; otherwise, <c>false</c>.
        /// </value>
        public bool IsActive
        {
            get { return StatusId == (int)StatusType.Activate ? true : false; }
        }

        /// <summary>
        /// Gets or sets the created date time.
        /// </summary>
        /// <value>
        /// The created date time.
        /// </value>
        public DateTime CreatedDateTime { get; set; }

        /// <summary>
        /// Gets or sets the user roles display.
        /// </summary>
        /// <value>
        /// The user roles display.
        /// </value>
        public string UserRolesDisplay { get; set; }

        /// <summary>
        /// Gets or sets the hubs.
        /// </summary>
        /// <value>
        /// The hubs.
        /// </value>
        public string Hubs { get; set; }

        /// <summary>
        /// Gets or sets the primary hub.
        /// </summary>
        /// <value>
        /// The primary hub.
        /// </value>
        public string PrimaryHub { get; set; }

        /// <summary>
        /// Gets or sets the type of the user.
        /// </summary>
        /// <value>
        /// The type of the user.
        /// </value>
        public string UserType { get; set; }

        /// <summary>
        /// Gets or sets the secondary hubs.
        /// </summary>
        /// <value>
        /// The secondary hubs.
        /// </value>
        public List<HubMaster> SecondaryHubs { get; set; }

        /// <summary>
        /// Gets or sets the Last Modified Date to display.
        /// </summary>
        /// <value>
        /// The Last Modified Date String.
        /// </value>
        public string LastModifiedDateDisplay { get; set; }

        /// <summary>
        /// Gets or sets the Practice Name to display.
        /// </summary>
        /// <value>
        /// The Practice Name String.
        /// </value>
        public string PracticeName { get; set; }

        /// <summary>
        /// Gets or sets the content of the document.
        /// </summary>
        /// <value>
        /// The content of the document.
        /// </value>
        public byte[] DocumentContent { get; set; }


        /// <summary>
        /// Gets or sets the practice id.
        /// </summary>
        /// <value>
        /// The practice id.
        /// </value>
        public int? PracticeId { get; set; }
    }
}