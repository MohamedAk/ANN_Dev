using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anzu.AnnPortal.Common.Model.Portal
{
    public class UserDTO
    {
        public string email { get; set; }
        public string rrUserId { get; set; }
        public int sessionTimeOut { get; set; }
        public string userRole { get; set; }

    }
}
