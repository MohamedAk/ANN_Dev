using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anzu.AnnPortal.Data.Model
{
    [Table("PracticeActivationLog", Schema = "annPortal")]
    public class PracticeActivationLog : Base
    {
        [Key]
        public long Id { get; set; }
        public string EmrId { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UserId { get; set; }
        public bool IsActivated { get; set; }
        public bool IsDataDeleted { get; set; }
        public bool IsRefreshLater { get; set; }
        public bool IsCubeRefreshed { get; set; }
    }
}
