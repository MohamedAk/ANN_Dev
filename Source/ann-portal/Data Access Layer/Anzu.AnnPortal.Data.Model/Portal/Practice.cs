using Anzu.AnnPortal.Data.Model.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anzu.AnnPortal.Data.Model.Portal
{
    [Table("Practice", Schema = "annPortal")]
    public class Practice : Base
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [Key]
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the emr identifier.
        /// </summary>
        /// <value>
        /// The emr identifier.
        /// </value>
        public long? EmrId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [MaxLength(64)]
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the region identifier.
        /// </summary>
        /// <value>
        /// The region identifier.
        /// </value>
        public long RegionId { get; set; }

        /// <summary>
        /// Gets or sets the zip code identifier.
        /// </summary>
        /// <value>
        /// The zip code identifier.
        /// </value>
       [ForeignKey("ZipCode")]
        public long? ZipCodeId { get; set; }

       /// <summary>
       /// Gets or sets the zip code.
       /// </summary>
       /// <value>
       /// The zip code.
       /// </value>
        public virtual ZipCode ZipCode { get; set; }

        /// <summary>
        /// Gets or sets the city.
        /// </summary>
        /// <value>
        /// The city.
        /// </value>
        public string City { get; set; }

        /// <summary>
        /// Gets or sets the state identifier.
        /// </summary>
        /// <value>
        /// The state identifier.
        /// </value>
        [ForeignKey("State")]
        public long? StateId { get; set; }

        /// <summary>
        /// Gets or sets the role.
        /// </summary>
        /// <value>
        /// The role.
        /// </value>
        public virtual State State { get; set; }
    }
}
