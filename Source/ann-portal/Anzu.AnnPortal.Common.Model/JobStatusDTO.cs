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
    public class JobStatusDTO : BaseDTO
    {
        /// <summary>
        /// Gets or sets the EMR identifier.
        /// </summary>
        /// <value>
        /// The EMR identifier.
        /// </value>
        public string EmrId { get; set; }

        /// <summary>
        /// Gets or sets the Job mode.
        /// </summary>
        /// <value>
        /// The Job mode.
        /// </value>
        public string JobMode { get; set; }

        /// <summary>
        /// Gets or sets the extract date.
        /// </summary>
        /// <value>
        /// The extract date.
        /// </value>
        public DateTime ExtractDate { get; set; }

        /// <summary>
        /// Gets or sets the can cancel.
        /// </summary>
        /// <value>
        /// The extract can cancel.
        /// </value>
        public int CanCancel { get; set; }
    }
}
