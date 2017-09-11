namespace Anzu.AnnPortal.Identity.Data.Model.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Designation
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int? StatusId { get; set; }

        [ForeignKey("StatusId")]
        public virtual Status Status { get; set; }

        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        /// <value>
        /// The display name.
        /// </value>
        [MaxLength(200)]
        public string  DisplayName { get; set; }

    }
}
