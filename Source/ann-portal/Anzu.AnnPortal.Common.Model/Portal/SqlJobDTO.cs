using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anzu.AnnPortal.Common.Model.Portal
{
    public class SqlJobDTO : BaseDTO
    {
        public int running { get; set; }
        public int current_step { get; set; }
        public int last_run_date { get; set; }
        public int last_run_time { get; set; }
        public int job_state { get; set; }
    }
}