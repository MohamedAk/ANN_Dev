//----------------------------------------------------------------------- 
// <copyright file="HubMaster.cs" company="Brandix i3"> 
//     Copyright Brandix i3 2015. All rights reserved. 
// </copyright>
// <summary>
// The HubMaster class. target CLR version 4.0.30319.34209.
// Created by : 
// Created date time: 
// </summary>
//-----------------------------------------------------------------------

namespace Anzu.AnnPortal.Identity.Common.Model.DTO
{
    public class HubMaster
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the chief complaint.
        /// </summary>
        /// <value>
        /// The chief complaint.
        /// </value> 
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the health systems identifier.
        /// </summary>
        /// <value>
        /// The health systems identifier.
        /// </value> 
        public long HealthSystemsId { get; set; }

        /// <summary>
        /// Gets or sets the time zone.
        /// </summary>
        /// <value>
        /// The time zone.
        /// </value> 
        public string TimeZone { get; set; }

    }
}
