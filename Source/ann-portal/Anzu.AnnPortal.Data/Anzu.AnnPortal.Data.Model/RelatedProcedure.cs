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
    [Table("RelatedProcedures", Schema = "annPortal")]
    public class RelatedProcedure : Base
    {

        /// <summary>
        /// Gets or sets the related procedure identifier.
        /// </summary>
        /// <value>
        /// The related procedure identifier.
        /// </value>
        [Key, Column(Order = 0)]
        [ForeignKey("Procedure")]
        public long RelatedProcedureId { get; set; }

        /// <summary>
        /// Gets or sets the prodecure identifier.
        /// </summary>
        /// <value>
        /// The prodecure identifier.
        /// </value>
       [Key, Column(Order = 1)]
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
