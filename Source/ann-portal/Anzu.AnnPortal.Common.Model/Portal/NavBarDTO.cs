using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anzu.AnnPortal.Common.Model.Portal
{
    /// <summary>
    /// Class Nav Bar DTO.
    /// </summary>
    /// <seealso cref="Anzu.AnnPortal.Common.Model.BaseDTO" />
    public class NavBarDTO 
    {
        /// <summary>
        /// Gets or sets the total revenue.
        /// </summary>
        /// <value>
        /// The total revenue.
        /// </value>
        public decimal TotalRevenue { get; set; }

        /// <summary>
        /// Gets or sets the total procedures.
        /// </summary>
        /// <value>
        /// The total procedures.
        /// </value>
        public decimal TotalProcedures { get; set; }
    }
}
