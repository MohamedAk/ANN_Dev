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
    /// <seealso cref="Anzu.AnnPortal.Common.Model.BaseDTO" />
    public class PracticeBrestImplantDTO : BaseDTO
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the state identifier.
        /// </summary>
        /// <value>
        /// The state identifier.
        /// </value>     
        public long? PracticeId { get; set; }

        /// <summary>
        /// Gets or sets the bi identifier.
        /// </summary>
        /// <value>
        /// The bi identifier.
        /// </value>
        public long? BIId { get; set; }

        /// <summary>
        /// Gets or sets the brest implant.
        /// </summary>
        /// <value>
        /// The brest implant.
        /// </value>
        public BrestImplantDTO BrestImplant { get; set; }

        /// <summary>
        /// Gets or sets from date.
        /// </summary>
        /// <value>
        /// From date.
        /// </value>
        public DateTime FromDate { get; set; }

        /// <summary>
        /// Gets or sets to date.
        /// </summary>
        /// <value>
        /// To date.
        /// </value>
        public DateTime? ToDate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is delete.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is delete; otherwise, <c>false</c>.
        /// </value>
        public bool IsDelete { get; set; }

    }
}
