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
    /// 
    /// </summary>
    [Table("ProcedureLevels", Schema = "annPortal")]
    public class ProcedureLevel : Base
    { 
       
        [Key]
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the level number.
        /// </summary>
        /// <value>
        /// The level number.
        /// </value>
        public long LevelNumber { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the is product.
        /// </summary>
        /// <value>
        /// The is product.
        /// </value>
        public bool? IsProduct { get; set; }

    }
}
