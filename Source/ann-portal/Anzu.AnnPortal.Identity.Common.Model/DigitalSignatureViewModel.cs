//----------------------------------------------------------------------- 
// <copyright file="DigitalSignatureViewModel.cs" company="Brandix i3"> 
//     Copyright Brandix i3 2015. All rights reserved. 
// </copyright>
// <summary>
// The DigitalSignatureViewModel class. target CLR version 4.0.30319.34209.
// Created by : 
// Created date time: 
// </summary>
//-----------------------------------------------------------------------
using System.ComponentModel.DataAnnotations;

namespace Anzu.AnnPortal.Identity.Common.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class DigitalSignatureViewModel
    {
        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        [Required(ErrorMessage = "Password cannot be empty")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the digital signature.
        /// </summary>
        /// <value>
        /// The digital signature.
        /// </value>
        public byte[] DigitalSignature { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is change.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is change; otherwise, <c>false</c>.
        /// </value>
        public bool isChange { get; set; }

        /// <summary>
        /// Gets or sets the pin.
        /// </summary>
        /// <value>
        /// The pin.
        /// </value>
        public int? PIN { get; set; }

        /// <summary>
        /// Gets or sets the confirm pin.
        /// </summary>
        /// <value>
        /// The confirm pin.
        /// </value>
        public int? ConfirmPIN { get; set; }

    }
}