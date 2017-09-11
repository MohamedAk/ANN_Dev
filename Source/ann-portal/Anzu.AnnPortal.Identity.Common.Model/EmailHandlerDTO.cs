using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anzu.AnnPortal.Identity.Common.Model
{
    public class EmailHandlerDTO
    {
        public List<string> EmrList { get; set; }
        public List<string> RecieverList { get; set; }
        public int EmailType { get; set; }
        public string JobCreatorUserId { get; set; }
    }
}
