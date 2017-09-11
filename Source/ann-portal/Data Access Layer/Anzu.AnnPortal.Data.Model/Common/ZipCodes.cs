using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Anzu.AnnPortal.Data.Model.Common
{
    /// <summary>
    /// Zip Code class
    /// </summary>

    [Table("ZipCode", Schema = "metadata")]
    public class ZipCode : Base
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
        [Required]
        [MaxLength(32)]
        public string Code { get; set; }
    }
}