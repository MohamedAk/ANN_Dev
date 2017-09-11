//----------------------------------------------------------------------- 
// <copyright file="ValidateSecurityQuestionViewModel.cs" company="Brandix i3"> 
//     Copyright Brandix i3 2015. All rights reserved. 
// </copyright>
// <summary>
// The ValidateSecurityQuestionViewModel class. target CLR version 4.0.30319.34209.
// Created by : 
// Created date time: 
// </summary>
//-----------------------------------------------------------------------
using System.ComponentModel.DataAnnotations;

namespace Anzu.AnnPortal.Identity.Common.Model
{
    public class ValidateSecurityQuestionViewModel
    {
        /// <summary>
        /// Gets or sets the security question.
        /// </summary>
        /// <value>
        /// The security question.
        /// </value>
        [Display(Name = "Security Question")]
        public string SecurityQuestion { get; set; }

        /// <summary>
        /// Gets or sets the answer.
        /// </summary>
        /// <value>
        /// The answer.
        /// </value>
        [Required]
        [StringLength(30)]
        [Display(Name = "Answer")]
        public string Answer { get; set; }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>        
        public string UserId { get; set; }
    }
}
