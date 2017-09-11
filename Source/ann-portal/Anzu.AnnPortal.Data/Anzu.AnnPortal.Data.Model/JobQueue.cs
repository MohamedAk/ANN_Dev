using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anzu.AnnPortal.Data.Model
{
    /// <summary>
    /// Class JobQueue
    /// </summary>
    [Table("JobQueue", Schema = "annPortal")]
    public class JobQueue : Base
    {
        /// <summary>
        /// The Job Queue Id
        /// </summary>
        [Key]
        public long Id { get; set; }

        /// <summary>
        /// The User Id
        /// </summary>
        public string UserId { get; set; }
        
        /// <summary>
        /// The Start time of the job
        /// </summary>
        public DateTimeOffset StartTime { get; set; }

        /// <summary>
        /// The End time of the job
        /// </summary>
        public DateTimeOffset? EndTime { get; set; }

        /// <summary>
        /// The job mode
        /// </summary>
        public int JobMode { get; set; }

        /// <summary>
        /// The job status
        /// </summary>
        [ForeignKey("JobStatus")]
        public int JobStatusId { get; set; }

        public virtual JobStatus JobStatus { get; set; }
    }
}
