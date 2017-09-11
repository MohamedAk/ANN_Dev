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
    public class ProcedureLevelDTO : BaseDTO
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
       
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the level number.
        /// </summary>
        /// <value>
        /// The level number.
        /// </value>
        public int LevelNumber { get; set; }

        /// <summary>
        /// Gets or sets the is product.
        /// </summary>
        /// <value>
        /// The is product.
        /// </value>
        public bool? IsProduct { get; set; }
    }
}
