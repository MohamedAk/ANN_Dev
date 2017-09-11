using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anzu.AnnPortal.Identity.Data.Model.Models
{
    public class UserRole
    {
        public int Id { get; set; }

        [Column(TypeName = "nvarchar")]
        public string UserId { get; set; }
        [Column(TypeName = "nvarchar")]
        public string RoleId { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }

        [ForeignKey("RoleId")]
        public virtual ApplicationRole Role { get; set; }
    }
}
