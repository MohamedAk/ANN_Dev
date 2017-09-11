using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace Anzu.AnnPortal.Web.UI.Helper
{
    /// <summary>
    /// Class Identity Helper.
    /// </summary>
    public class IdentityHelper
    {
        /// <summary>
        /// Gets the user name from identity.
        /// </summary>
        /// <param name="claim">The claim.</param>
        /// <returns></returns>
        public static Claim GetUserNameFromIdentity(IEnumerable<Claim> claim)
        {
            if (claim != null)
            {
                return claim.Where(cl => cl.Type == "UserName").FirstOrDefault();
            }
            else
            {
                return null;
            }
        }
    }
}