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
    public class AuthenticateUserResultDTO
    {
        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        /// <value>
        /// The user.
        /// </value>
        public UserDTO User { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="AuthenticateUserResultDTO"/> is authentication.
        /// </summary>
        /// <value>
        ///   <c>true</c> if authentication; otherwise, <c>false</c>.
        /// </value>
        public bool authentication { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public string message { get; set; }
    }
}
