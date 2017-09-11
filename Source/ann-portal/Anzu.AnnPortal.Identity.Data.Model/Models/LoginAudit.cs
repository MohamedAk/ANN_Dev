using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anzu.AnnPortal.Identity.Data.Model.Models
{
    public class LoginAudit
    {
        public long Id { get; set; }

        public string LoginName { get; set; }

        public int LoginAudiStatus { get; set; }

        public DateTime LoginDateTime { get; set; }
    }
}
