using Anzu.AnnPortal.Common.Model.Common;
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
    public class PracticeDTO : BaseDTO
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the emr identifier.
        /// </summary>
        /// <value>
        /// The emr identifier.
        /// </value>
        public string EmrId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the region identifier.
        /// </summary>
        /// <value>
        /// The region identifier.
        /// </value>
        public RegionDTO Region { get; set; }

        /// <summary>
        /// Gets or sets the zip code identifier.
        /// </summary>
        /// <value>
        /// The zip code identifier.
        /// </value>
        public ZipCodeDTO ZipCode { get; set; }

        /// <summary>
        /// Gets or sets the city.
        /// </summary>
        /// <value>
        /// The city.
        /// </value>
        public string City { get; set; }

        /// <summary>
        /// Gets or sets the state identifier.
        /// </summary>
        /// <value>
        /// The state identifier.
        /// </value>
        public StateDTO State { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is active; otherwise, <c>false</c>.
        /// </value>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the address line1.
        /// </summary>
        /// <value>
        /// The address line1.
        /// </value>
        public string AddressLine1 { get; set; }

        /// <summary>
        /// Gets or sets the address line2.
        /// </summary>
        /// <value>
        /// The address line2.
        /// </value>
        public string AddressLine2 { get; set; }

        /// <summary>
        /// Gets or sets the practice user list.
        /// </summary>
        /// <value>
        /// The practice user list.
        /// </value>
        public List<ExternalUserDTO> PracticeUserList { get; set; }

        /// <summary>
        /// Gets or sets the brest implants.
        /// </summary>
        /// <value>
        /// The brest implants.
        /// </value>
        public List<PracticeBrestImplantDTO> BrestImplants { get; set; }

        /// <summary>
        /// Gets or sets the contact number.
        /// </summary>
        /// <value>
        /// The contact number.
        /// </value>
        public string ContactNumber { get; set; }

        /// <summary>
        /// Gets or sets the contact person.
        /// </summary>
        /// <value>
        /// The contact person.
        /// </value>
        public string ContactPerson { get; set; }

        /// <summary>
        /// Gets or sets the last updated date.
        /// </summary>
        /// <value>
        /// The last updated date.
        /// </value>
        public string LastUpdatedDate { get; set; }

        /// <summary>
        /// Gets or sets flag delete data.
        /// </summary>
        /// <value>
        /// Delete data flag.
        /// </value>
        public bool DeleteData { get; set; }

        /// <summary>
        /// Gets or sets flag Refresh cube now.
        /// </summary>
        /// <value>
        /// Refresh cube now flag.
        /// </value>
        public bool RefreshCube { get; set; }

        /// <summary>
        /// Gets or sets flag has practice data.
        /// </summary>
        /// <value>
        /// Has practice data flag.
        /// </value>
        public bool HasData { get; set; }
    }
}