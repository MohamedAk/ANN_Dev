using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anzu.AnnPortal.Identity.Data.Model.Models
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Microsoft.AspNet.Identity.EntityFramework.IdentityUser" />
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// Gets or sets the status identifier.
        /// </summary>
        /// <value>
        /// The status identifier.
        /// </value>
        [Required]
        public int StatusId { get; set; }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        /// <value>
        /// The first name.
        /// </value>
        [Required]
        [MaxLength(20)]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        /// <value>
        /// The last name.
        /// </value>
        [Required]
        [MaxLength(20)]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the organization identifier.
        /// </summary>
        /// <value>
        /// The organization identifier.
        /// </value>
        [Required]
        public int OrganizationId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is force to change password.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is force to change password; otherwise, <c>false</c>.
        /// </value>
        [Required]
        public bool IsForceToChangePassword { get; set; }

        /// <summary>
        /// Gets or sets the designation identifier.
        /// </summary>
        /// <value>
        /// The designation identifier.
        /// </value>
        [Required]
        public int DesignationId { get; set; }

        /// <summary>
        /// Gets or sets the user designation.
        /// </summary>
        /// <value>
        /// The user designation.
        /// </value>
        [Required]
        [MaxLength(30)]
        public string UserDesignation { get; set; }

        /// <summary>
        /// Gets or sets the created by.
        /// </summary>
        /// <value>
        /// The created by.
        /// </value>
        [Required]
        public string CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the created date time.
        /// </summary>
        /// <value>
        /// The created date time.
        /// </value>
        [Required]
        public DateTime CreatedDateTime { get; set; }

        /// <summary>
        /// Gets or sets the digital signature.
        /// </summary>
        /// <value>
        /// The digital signature.
        /// </value>
        public byte[] DigitalSignature { get; set; }

        /// <summary>
        /// Gets or sets the modified by.
        /// </summary>
        /// <value>
        /// The modified by.
        /// </value>
        public string ModifiedBy { get; set; }

        /// <summary>
        /// Gets or sets the modified date time.
        /// </summary>
        /// <value>
        /// The modified date time.
        /// </value>
        public DateTime? ModifiedDateTime { get; set; }

        /// <summary>
        /// Gets or sets the previous user passwords.
        /// </summary>
        /// <value>
        /// The previous user passwords.
        /// </value>
        public virtual IList<PreviousPassword> PreviousUserPasswords { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        [ForeignKey("StatusId")]
        public virtual Status Status { get; set; }

        /// <summary>
        /// Gets or sets the organization.
        /// </summary>
        /// <value>
        /// The organization.
        /// </value>
        [ForeignKey("OrganizationId")]
        public virtual Organization Organization { get; set; }

        /// <summary>
        /// Gets or sets the secondary organizations.
        /// </summary>
        /// <value>
        /// The secondary organizations.
        /// </value>
        public virtual List<Organization> SecondaryOrganizations { get; set; }

        /// <summary>
        /// Gets or sets the designation.
        /// </summary>
        /// <value>
        /// The designation.
        /// </value>
        [ForeignKey("DesignationId")]
        public virtual Designation Designation { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is security question answered.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is security question answered; otherwise, <c>false</c>.
        /// </value>
        [Required]
        public bool IsSecurityQuestionAnswered { get; set; }

        /// <summary>
        /// Gets or sets the pin.
        /// </summary>
        /// <value>
        /// The pin.
        /// </value>
        public int? PIN { get; set; }

        /// <summary>
        /// Gets or sets the practice name.
        /// </summary>
        /// <value>
        /// The practice name.
        /// </value>
        public string PracticeName { get; set; }

        /// <summary>
        /// Gets or sets the practice id.
        /// </summary>
        /// <value>
        /// The practice id.
        /// </value>
        public int? PracticeId { get; set; }

        /// <summary>
        /// Gets or sets the content of the document.
        /// </summary>
        /// <value>
        /// The content of the document.
        /// </value>
        public byte[] DocumentContent { get; set; }
    }
}