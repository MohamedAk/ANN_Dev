using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anzu.AnnPortal.Common.Model.Portal
{
    /// <summary>
    /// 
    /// </summary>
    public class PracticeProcedureDTO : BaseDTO
    {

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the practice.
        /// </summary>
        /// <value>
        /// The practice.
        /// </value>
        public PracticeDTO Practice { get; set; }

        /// <summary>
        /// Gets or sets the practice identifier.
        /// </summary>
        /// <value>
        /// The practice identifier.
        /// </value>
        public long? PracticeId { get; set; }

        /// <summary>
        /// Gets or sets the emr procedure.
        /// </summary>
        /// <value>
        /// The emr procedure.
        /// </value>
        public string EmrProcedure { get; set; }

        /// <summary>
        /// Gets or sets the procedure.
        /// </summary>
        /// <value>
        /// The procedure.
        /// </value>
        public ProcedureDTO Procedure { get; set; }

        /// <summary>
        /// Gets or sets the procedure identifier.
        /// </summary>
        /// <value>
        /// The procedure identifier.
        /// </value>
        public long? ProcedureId { get; set; }

        /// <summary>
        /// Gets or sets the company identifier.
        /// </summary>
        /// <value>
        /// The company identifier.
        /// </value>
        public long? CompanyId { get; set; }

        /// <summary>
        /// Gets or sets the company.
        /// </summary>
        /// <value>
        /// The company.
        /// </value>
        public virtual CompanyDTO Company { get; set; }

        /// <summary>
        /// Gets or sets the product type identifier.
        /// </summary>
        /// <value>
        /// The product type identifier.
        /// </value>
        public long? ProductTypeId { get; set; }

        /// <summary>
        /// Gets or sets the type of the product.
        /// </summary>
        /// <value>
        /// The type of the product.
        /// </value>
        public virtual ProductTypeDTO ProductType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is product sale.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is product sale; otherwise, <c>false</c>.
        /// </value>
        public bool IsProductSale { get; set; }

        /// <summary>
        /// Gets or sets the emr service identifier.
        /// </summary>
        /// <value>
        /// The emr service identifier.
        /// </value>
        public long EMRServiceId { get; set; }

        /// <summary>
        /// Gets or sets the name of the service.
        /// </summary>
        /// <value>
        /// The name of the service.
        /// </value>
        public string ServiceName { get; set; }

        /// <summary>
        /// Gets or sets the is discarded.
        /// </summary>
        /// <value>
        /// The is discarded.
        /// </value>
        public bool? IsDiscarded { get; set; }

        /// <summary>
        /// Gets or sets the previous procedure identifier.
        /// </summary>
        /// <value>
        /// The previous procedure identifier.
        /// </value>
        public long? PreviousProcedureId { get; set; }

        /// <summary>
        /// Gets or sets the previous company identifier.
        /// </summary>
        /// <value>
        /// The previous company identifier.
        /// </value>
        public long? PreviousCompanyId { get; set; }

        /// <summary>
        /// Gets or sets the previous product type identifier.
        /// </summary>
        /// <value>
        /// The previous product type identifier.
        /// </value>
        public long? PreviousProductTypeId { get; set; }

        /// <summary>
        /// Gets or sets the emr identifier.
        /// </summary>
        /// <value>
        /// The emr identifier.
        /// </value>
        public string EmrId { get; set; }
    }
}
