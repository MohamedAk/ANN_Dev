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
    /// Class Practice Edit Information.
    /// </summary>
    [Table("PracticeEditInformation", Schema = "annPortal")]
    public class PracticeEditInformation: Base
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
        /// Gets or sets the practice identifier.
        /// </summary>
        /// <value>
        /// The practice identifier.
        /// </value>
        public long PracticeId { get; set; }

        /// <summary>
        /// Gets or sets the emr identifier.
        /// </summary>
        /// <value>
        /// The emr identifier.
        /// </value>
        public string EmrId { get; set; }

        /// <summary>
        /// Gets or sets the is emr mapping updated.
        /// </summary>
        /// <value>
        /// The is emr mapping updated.
        /// </value>
        public bool? IsEMRMappingUpdated { get; set; }

        /// <summary>
        /// Gets or sets the emr mapping updated time.
        /// </summary>
        /// <value>
        /// The emr mapping updated time.
        /// </value>
        public DateTimeOffset EMRMappingUpdatedTime { get; set; }

        /// <summary>
        /// Gets or sets the is cube updated.
        /// </summary>
        /// <value>
        /// The is cube updated.
        /// </value>
        public bool? IsCubeUpdated { get; set; }

        /// <summary>
        /// Gets or sets the cube updated time.
        /// </summary>
        /// <value>
        /// The cube updated time.
        /// </value>
        public DateTimeOffset? CubeUpdatedTime { get; set; }

        /// <summary>
        /// Gets or sets the is breast implant updated.
        /// </summary>
        /// <value>
        /// The is breast implant updated.
        /// </value>
        public bool? IsBreastImplantUpdated { get; set; }
    }
}
