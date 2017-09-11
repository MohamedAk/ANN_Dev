using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anzu.AnnPortal.Common.Model.Portal
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Anzu.AnnPortal.Common.Model.BaseDTO" />
    public class ExternalUserDTO : BaseDTO
    {

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public long Id { get; set; }
        /// <summary>
        /// Gets or sets the unique identifier.
        /// </summary>
        /// <value>
        /// The uninque identifier mapped with the Identity DB.
        /// </value>
        public string RRUserId { get; set; }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        /// <value>
        /// The first name.
        /// </value>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        /// <value>
        /// The last name.
        /// </value>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the password hash.
        /// </summary>
        /// <value>
        /// The password hash.
        /// </value>
        public string PasswordHash { get; set; }

        /// <summary>
        /// Gets or sets the security stamp.
        /// </summary>
        /// <value>
        /// The security stamp.
        /// </value>
        public string SecurityStamp { get; set; }

        /// <summary>
        /// Gets or sets the practice.
        /// </summary>
        /// <value>
        /// The practice.
        /// </value>
        public PracticeDTO Practice { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is deleted.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is deleted; otherwise, <c>false</c>.
        /// </value>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Gets or sets the user role identifier.
        /// </summary>
        /// <value>
        /// The user role identifier.
        /// </value>
        public long UserRoleId { get; set; }

        /// <summary>
        /// Gets or sets the user role.
        /// </summary>
        /// <value>
        /// The user role.
        /// </value>
        public UserRoleDTO UserRole { get; set; }

        /// <summary>
        /// Gets or sets the content of the document.
        /// </summary>
        /// <value>
        /// The content of the document.
        /// </value>
        public byte[] DocumentContent { get; set; }

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        /// <value>
        /// The error message.
        /// </value>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Gets the display name.
        /// </summary>
        /// <value>
        /// The display name.
        /// </value>
        public string DisplayName
        {
            get
            {
                return String.Format("{0} {1}", FirstName, LastName);
            }
        }

        /// <summary>
        /// Gets or sets the is user already exsist in practice.
        /// </summary>
        /// <value>
        /// The is user already exsist in practice.
        /// </value>
        public bool IsUserAlreadyExsistInPractice { get; set; }

        /// <summary>
        /// Gets or sets the is user asap user.
        /// </summary>
        /// <value>
        /// The is user asap user.
        /// </value>
        public bool IsUserASAPUser { get; set; }

        /// <summary>
        /// Gets or sets the practice identifier.
        /// </summary>
        /// <value>
        /// The practice identifier.
        /// </value>
        public long? PracticeId { get; set; }

        /// <summary>
        /// Gets and Sets the display user role name.
        /// </summary>
        /// <value>
        /// The display user role  name.
        /// </value>
        public string UserRolesDisplay { get; set; }

        /// <summary>
        /// Gets and Sets the display user status name.
        /// </summary>
        /// <value>
        /// The display user status name.
        /// </value>

        public string UserStatusDisplay
        {
            get
            {
                return (this.RecordStatusId == 1) ? "Active" : "Deactive";
            }
        }
    }
}
