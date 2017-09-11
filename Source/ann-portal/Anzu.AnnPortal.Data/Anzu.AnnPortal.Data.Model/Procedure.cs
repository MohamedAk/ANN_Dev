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
    [Table("Procedures", Schema = "annPortal")]
    public class Procedure:Base
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
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [MaxLength(64)]
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is use product.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is use product; otherwise, <c>false</c>.
        /// </value>
        public bool IsUseProduct { get; set; }

        /// <summary>
        /// Gets or sets the procedure identifier.
        /// </summary>
        /// <value>
        /// The procedure identifier.
        /// </value>
        [ForeignKey("ProcedureLevel1")]
        public long? ProcedureLevelOne { get; set; }

        /// <summary>
        /// Gets or sets the procedure.
        /// </summary>
        /// <value>
        /// The procedure.
        /// </value>
        public virtual ProcedureLevel ProcedureLevel1 { get; set; }

        /// <summary>
        /// Gets or sets the procedure identifier.
        /// </summary>
        /// <value>
        /// The procedure identifier.
        /// </value>
        [ForeignKey("ProcedureLevel2")]
        public long? ProcedureLevelTwo { get; set; }

        /// <summary>
        /// Gets or sets the procedure.
        /// </summary>
        /// <value>
        /// The procedure.
        /// </value>
        public virtual ProcedureLevel ProcedureLevel2 { get; set; }

        /// <summary>
        /// Gets or sets the procedure identifier.
        /// </summary>
        /// <value>
        /// The procedure identifier.
        /// </value>
        [ForeignKey("ProcedureLevel3")]
        public long? ProcedureLevelThree { get; set; }

        /// <summary>
        /// Gets or sets the procedure.
        /// </summary>
        /// <value>
        /// The procedure.
        /// </value>
        public virtual ProcedureLevel ProcedureLevel3 { get; set; }

        /// <summary>
        /// Gets or sets the procedure identifier.
        /// </summary>
        /// <value>
        /// The procedure identifier.
        /// </value>
        [ForeignKey("ProcedureLevel4")]
        public long? ProcedureLevelFour { get; set; }

        /// <summary>
        /// Gets or sets the procedure.
        /// </summary>
        /// <value>
        /// The procedure.
        /// </value>
        public virtual ProcedureLevel ProcedureLevel4 { get; set; }

        /// <summary>
        /// Gets or sets the procedure identifier.
        /// </summary>
        /// <value>
        /// The procedure identifier.
        /// </value>
        [ForeignKey("ProcedureLevel5")]
        public long? ProcedureLevelFive { get; set; }

        /// <summary>
        /// Gets or sets the procedure.
        /// </summary>
        /// <value>
        /// The procedure.
        /// </value>
        public virtual ProcedureLevel ProcedureLevel5 { get; set; }

        /// <summary>
        /// Gets or sets the procedure identifier.
        /// </summary>
        /// <value>
        /// The procedure identifier.
        /// </value>
        [ForeignKey("ProcedureLevel6")]
        public long? ProcedureLevelSix { get; set; }

        /// <summary>
        /// Gets or sets the procedure.
        /// </summary>
        /// <value>
        /// The procedure.
        /// </value>
        public virtual ProcedureLevel ProcedureLevel6 { get; set; }

        /// <summary>
        /// Gets or sets the procedure identifier.
        /// </summary>
        /// <value>
        /// The procedure identifier.
        /// </value>
        [ForeignKey("ProcedureLevel7")]
        public long? ProcedureLevelSeven { get; set; }

        /// <summary>
        /// Gets or sets the procedure.
        /// </summary>
        /// <value>
        /// The procedure.
        /// </value>
        public virtual ProcedureLevel ProcedureLevel7 { get; set; }

        public Boolean PracticeProductFlag { get; set; }

    }
}
