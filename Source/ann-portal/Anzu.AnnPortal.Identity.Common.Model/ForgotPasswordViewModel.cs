//----------------------------------------------------------------------- 
// <copyright file="ForgotPasswordViewModel.cs" company="Brandix i3"> 
//     Copyright Brandix i3 2015. All rights reserved. 
// </copyright>
// <summary>
// The ForgotPasswordViewModel class. target CLR version 4.0.30319.34209.
// Created by : 
// Created date time: 
// </summary>
//-----------------------------------------------------------------------
using System.ComponentModel.DataAnnotations;

namespace Anzu.AnnPortal.Identity.Common.Model
{
    public class ForgotPasswordViewModel
    {
        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        [Required(ErrorMessage = "User ID cannot be empty")]
        [Display(Name = "User ID")]
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the security question identifier.
        /// </summary>
        /// <value>
        /// The security question identifier.
        /// </value>
        [Display(Name = "Select security question")]
        public int SecurityQuestionId { get; set; }

        /// <summary>
        /// Gets or sets the security question answer.
        /// </summary>
        /// <value>
        /// The security question answer.
        /// </value>
        [Display(Name = "Answer")]
        [StringLength(30)]
        public string SecurityQuestionAnswer { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is security question answered.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is security question answered; otherwise, <c>false</c>.
        /// </value>
        public bool ISSecurityQuestionAnswered { get; set; }
    }
}