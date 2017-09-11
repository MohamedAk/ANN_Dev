using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anzu.AnnPortal.Identity.Data.Model.Models
{
    public class SecurityQuestion
    {
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the question.
        /// </summary>
        /// <value>
        /// The question.
        /// </value>
        [MaxLength(250)]
        public string Question { get; set; }
    }
}
