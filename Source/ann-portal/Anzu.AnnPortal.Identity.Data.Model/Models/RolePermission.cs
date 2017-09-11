using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anzu.AnnPortal.Identity.Data.Model.Models
{
    public class RolePermission
    {
        public int Id { get; set; }

        [Required]
        public int PermissionId { get; set; }

        [Column(TypeName = "nvarchar")]
        [Required]
        public string ApplicationRoleId { get; set; }

        [ForeignKey("PermissionId")]
        public virtual Permission Permission { get; set; }

        [ForeignKey("ApplicationRoleId")]
        public virtual ApplicationRole ApplicationRole { get; set; }
    }
}
