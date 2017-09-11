using Anzu.AnnPortal.Data.Model.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anzu.AnnPortal.Data.Model
{
    [Table("Practices", Schema = "annPortal")]
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
        public string EmrId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value> 
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the region identifier.
        /// </summary>
        /// <value>
        /// The region identifier.
        /// </value>
        public int? RegionId { get; set; }

        /// <summary>
        /// Gets or sets the zip code identifier.
        /// </summary>
        /// <value>
        /// The zip code identifier.
        /// </value>
       [ForeignKey("ZipCode")]
        public int? ZipCodeId { get; set; }

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
        public int? StateId { get; set; }

        /// <summary>
        /// Gets or sets the role.
        /// </summary>
        /// <value>
        /// The role.
        /// </value>
        public virtual State State { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is active; otherwise, <c>false</c>.
        /// </value>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the contact number.
        /// </summary>
        /// <value>
        /// The contact number.
        /// </value>
        public string ContactNumber { get; set; }

        /// <summary>
        /// Gets or sets the contact person.
        /// </summary>
        /// <value>
        /// The contact person.
        /// </value>
        public string ContactPerson { get; set; }


        /// <summary>
        /// Gets or sets the address1.
        /// </summary>
        /// <value>
        /// The address1.
        /// </value>
        public string AddressLine1 { get; set; }

        /// <summary>
        /// Gets or sets the address2.
        /// </summary>
        /// <value>
        /// The address2.
        /// </value>
        public string AddressLine2 { get; set; }
    }
}
