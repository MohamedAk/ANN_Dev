using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anzu.AnnPortal.Common.Model.Common
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Anzu.AnnPortal.Common.Model.Base.BaseDTO" />
    public class StateDTO : BaseDTO
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the name of the code and.
        /// </summary>
        /// <value>
        /// The name of the code and.
        /// </value>
        public string CodeAndName { get; set; }
    }
}
