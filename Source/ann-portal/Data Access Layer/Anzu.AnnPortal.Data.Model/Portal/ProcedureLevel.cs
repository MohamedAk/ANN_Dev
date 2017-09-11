using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anzu.AnnPortal.Data.Model.Portal
{
    /// <summary>
    /// 
    /// </summary>
    [Table("ProcedureLevel", Schema = "annPortal")]
    public class ProcedureLevel : Base
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [Key]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the level number.
        /// </summary>
        /// <value>
        /// The level number.
        /// </value>
        [Required]
        public int LevelNumber { get; set; }
    }
}
