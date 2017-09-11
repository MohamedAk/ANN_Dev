using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anzu.AnnPortal.Data.Model
{
    [Table("JobQueuePractice", Schema = "annPortal")]
    public class JobQueuePractice : Base
    {
        [Key]
        public long Id { get; set; }

        [ForeignKey("JobQueue")]
        public long JobQueueId { get; set; }

        public virtual JobQueue JobQueue { get; set; }

        [ForeignKey("Practice")]
        public long PracticeId { get; set; }

        public virtual Practice Practice { get; set; }

        public string EmrId { get; set; }
    }
}