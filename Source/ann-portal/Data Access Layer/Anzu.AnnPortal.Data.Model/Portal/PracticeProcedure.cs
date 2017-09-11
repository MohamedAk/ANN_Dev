using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anzu.AnnPortal.Data.Model.Portal
{
    [Table("PracticeProcedure", Schema = "annPortal")]
    class PracticeProcedure:Base
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
        [ForeignKey("Practice")]
        public long? PracticeId { get; set; }

        /// <summary>
        /// Gets or sets the practice.
        /// </summary>
        /// <value>
        /// The practice.
        /// </value>
        public virtual Practice Practice { get; set; }

        /// <summary>
        /// Gets or sets the emr procedure.
        /// </summary>
        /// <value>
        /// The emr procedure.
        /// </value>
        public string EmrProcedure { get; set; }

        /// <summary>
        /// Gets or sets the procedure identifier.
        /// </summary>
        /// <value>
        /// The procedure identifier.
        /// </value>
        [ForeignKey("Procedure")]
        public long? ProcedureId { get; set; }

        /// <summary>
        /// Gets or sets the procedure.
        /// </summary>
        /// <value>
        /// The procedure.
        /// </value>
        public virtual Procedure Procedure { get; set; }
    }
}
