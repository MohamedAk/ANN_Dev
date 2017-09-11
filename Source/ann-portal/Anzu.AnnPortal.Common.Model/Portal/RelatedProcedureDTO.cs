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
    /// <seealso cref="Anzu.AnnPortal.Common.Model.Base.BaseDTO" />
    public class RelatedProcedureDTO : BaseDTO
    {

        /// <summary>
        /// Gets or sets the related procedure identifier.
        /// </summary>
        /// <value>
        /// The related procedure identifier.
        /// </value>
        public long RelatedProcedureId { get; set; }

        public long ProcedureID { get; set; }

        /// <summary>
        /// Gets or sets the procedure.
        /// </summary>
        /// <value>
        /// The procedure.
        /// </value>
        public ProcedureDTO Procedure { get; set; }

    }
}
