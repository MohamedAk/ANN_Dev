using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anzu.AnnPortal.Data.Model
{
    [Table("PracticeProcedures", Schema = "annPortal")]
    public class PracticeProcedure:Base
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

        /// <summary>
        /// Gets or sets the company identifier.
        /// </summary>
        /// <value>
        /// The company identifier.
        /// </value>
        [ForeignKey("Company")]
        public long? CompanyId { get; set; }

        /// <summary>
        /// Gets or sets the company.
        /// </summary>
        /// <value>
        /// The company.
        /// </value>
        public virtual Company Company { get; set; }

        /// <summary>
        /// Gets or sets the product identifier.
        /// </summary>
        /// <value>
        /// The product identifier.
        /// </value>
        [ForeignKey("ProductType")]
        public long? ProductTypeId { get; set; }

        /// <summary>
        /// Gets or sets the product.
        /// </summary>
        /// <value>
        /// The product.
        /// </value>
        public virtual ProductType ProductType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is product sale.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is product sale; otherwise, <c>false</c>.
        /// </value>
        public bool IsProductSale { get; set; }

        /// <summary>
        /// Gets or sets the is discarded.
        /// </summary>
        /// <value>
        /// The is discarded.
        /// </value>
        public bool? IsDiscarded { get; set; }
    }
}
