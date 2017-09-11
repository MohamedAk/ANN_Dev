using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anzu.AnnPortal.Identity.Data.Model.Models
{
    public class Module
    {
        public int Id { get; set; }

        public int? PermissionCategoryId { get; set; }

        [Required]
        public string Name { get; set; }

        public ICollection<Permission> Permissions { get; set; } 
    }
}
