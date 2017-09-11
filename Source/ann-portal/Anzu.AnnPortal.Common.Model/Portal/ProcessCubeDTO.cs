using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anzu.AnnPortal.Common.Model.Portal
{
    public class ProcessCubeDTO : BaseDTO
    {
        public List<string> EmrList { get; set; }
        public int JobType { get; set; }
    }
}
