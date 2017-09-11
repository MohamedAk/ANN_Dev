using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anzu.AnnPortal.Data.Model
{
    [Table("JobStatus", Schema = "annPortal")]
    public class JobStatus
    {
        [Key]
        public int Id { get; set; }
        public string Status { get; set; }
    }
}
