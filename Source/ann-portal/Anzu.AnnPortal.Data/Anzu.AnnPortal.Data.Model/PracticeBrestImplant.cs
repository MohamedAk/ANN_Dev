using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anzu.AnnPortal.Data.Model
{
    /// <summary>
    /// 
    /// </summary>
     [Table("PracticeBrestImplants", Schema = "annPortal")]
    public class PracticeBrestImplant : Base
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
       // [ForeignKey("Practice")]
        public long? PracticeId { get; set; }

        /// <summary>
        /// Gets or sets the role.
        /// </summary>
        /// <value>
        /// The role.
        /// </value>
      //  public virtual Practice Practice { get; set; }

        /// <summary>
        /// Gets or sets the bi identifier.
        /// </summary>
        /// <value>
        /// The bi identifier.
        /// </value>
       // [ForeignKey("BrestImplant")]
        public long? BIId { get; set; }

        /// <summary>
        /// Gets or sets the Brest implant.
        /// </summary>
        /// <value>
        /// The Brest implant.
        /// </value>
        //public virtual BrestImplant BrestImplant { get; set; }

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
    }
}
