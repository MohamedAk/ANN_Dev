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
    public class ProcedureDTO : BaseDTO
    {

        public long Id { get; set; }

        public string Name { get; set; }

        public bool IsUseProduct { get; set; }

        public long? ProcedureLevelOne { get; set; }

        public virtual ProcedureLevelDTO ProcedureLevel1 { get; set; }

        public long? ProcedureLevelTwo { get; set; }

        public virtual ProcedureLevelDTO ProcedureLevel2 { get; set; }

        public long? ProcedureLevelThree { get; set; }

        public virtual ProcedureLevelDTO ProcedureLevel3 { get; set; }

        public long? ProcedureLevelFour { get; set; }

        public virtual ProcedureLevelDTO ProcedureLevel4 { get; set; }

        public long? ProcedureLevelFive { get; set; }

        public virtual ProcedureLevelDTO ProcedureLevel5 { get; set; }

        public long? ProcedureLevelSix { get; set; }

        public virtual ProcedureLevelDTO ProcedureLevel6 { get; set; }

        public long? ProcedureLevelSeven { get; set; }

        public virtual ProcedureLevelDTO ProcedureLevel7 { get; set; }

        public Boolean PracticeProductFlag { get; set; }

        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        /// <value>
        /// The display name.
        /// </value>
        public string DisplayName { get; set; }

        public List<int> relatedProcedureId { get; set; }

        public Boolean IsRelatedProcedureExists { get; set; }

    }
}
