using Anzu.AnnPortal.Data.EntityManager;
using Anzu.AnnPortal.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anzu.AnnPortal.Business.Core.Core
{
    /// <summary>
    /// 
    /// </summary>
    public class AccountService
    {
        /// <summary>
        /// Logins the specified user name.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        public bool Login(string userName, string password)
        {
            BaseRepository repository = new BaseRepository();
            var resultSet = repository.Find<User>(u => u.UserName.Trim().ToLower() == userName.Trim().ToLower() && u.PasswordHash == password && u.RecordStatusId == (int)(Anzu.AnnPortal.Common.Model.Enum.RecordStatus.Active)).Any();
            return resultSet;
        }
    }
}
