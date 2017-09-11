using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anzu.AnnPortal.Common.Model.Portal
{
    /// <summary>
    /// Class ServiceDTO.
    /// </summary>
    /// <seealso cref="Anzu.AnnPortal.Common.Model.BaseDTO" />
    public class ServiceDTO : BaseDTO
    {
        /// <summary>
        /// Gets or sets the emr identifier.
        /// </summary>
        /// <value>
        /// The emr identifier.
        /// </value>
        public string EMRId { get; set; }

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

        public long? Id { get; set; }

        public long? PracticeId { get; set; }

        public string EmrProcedure { get; set; }

        public long? ProcedureId { get; set; }

        public string ProcedureName { get; set; }

        public long? CompanyId { get; set; }

        public string CompanyName { get; set; }

        public string ProductName { get; set; }

        public long? ProductTypeId { get; set; }

        public bool? IsProductSale { get; set; }

        public bool? IsDiscarded { get; set; }

    }
}
