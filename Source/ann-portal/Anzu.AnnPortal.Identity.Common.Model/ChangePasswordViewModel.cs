//----------------------------------------------------------------------- 
// <copyright file="ChangePasswordViewModel.cs" company="Brandix i3"> 
//     Copyright Brandix i3 2015. All rights reserved. 
// </copyright>
// <summary>
// The ChangePasswordViewModel class. target CLR version 4.0.30319.34209.
// Created by : 
// Created date time: 
// </summary>
//-----------------------------------------------------------------------
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Anzu.AnnPortal.Identity.Common.Model
{
    public class ChangePasswordViewModel : IValidatableObject
    {
        /// <summary>
        /// Gets or sets the old password.
        /// </summary>
        /// <value>
        /// The old password.
        /// </value>
        //[Required(ErrorMessage = "Current password cannot be empty")]
        //[DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        /// <summary>
        /// Gets or sets the new password.
        /// </summary>
        /// <value>
        /// The new password.
        /// </value>
        [Required(ErrorMessage = "New password cannot be empty")]
        [StringLength(100, ErrorMessage = "Minimum password length required is {2}.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        /// <summary>
        /// Gets or sets the confirm password.
        /// </summary>
        /// <value>
        /// The confirm password.
        /// </value>
        [Required(ErrorMessage = "Confirm new password cannot be empty")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

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

        public bool ShowOldPassword { get; set; }

        public string UserId { get; set; }

        /// <summary>
        /// Determines whether the specified object is valid.
        /// </summary>
        /// <param name="validationContext">The validation context.</param>
        /// <returns>
        /// A collection that holds failed-validation information.
        /// </returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!this.ISSecurityQuestionAnswered)
            {
                if (string.IsNullOrEmpty(this.SecurityQuestionAnswer))
                {
                    yield return new ValidationResult("Please answer Security Question");
                }
                if (this.SecurityQuestionId == 0)
                {
                    yield return new ValidationResult("Please select security question");
                }

            }
        }
    }
}