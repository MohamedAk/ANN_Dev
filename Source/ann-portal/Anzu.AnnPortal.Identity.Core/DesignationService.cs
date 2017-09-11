using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IdentityDataModel = Anzu.AnnPortal.Identity.Data.Model.Models;
using Anzu.AnnPortal.Identity.Data.Model;
using Anzu.AnnPortal.Identity.Common.Model.Enum;

namespace Anzu.AnnPortal.Identity.Core
{
    public class DesignationService
    {
        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns></returns>
        public List<IdentityDataModel.Designation> GetAll()
        {
            ApplicationDbContext appDb = new ApplicationDbContext();
            var designations = appDb.Set<IdentityDataModel.Designation>().Where(d => d.StatusId == (int)StatusType.Activate).ToList();

            return designations;
        }

        /// <summary>
        /// Gets the designation by identifier.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns></returns>
        public Anzu.AnnPortal.Identity.Common.Model.DesignationViewModel GetDesignationById(int Id)
        {
            Anzu.AnnPortal.Identity.Common.Model.DesignationViewModel designation = null;

            ApplicationDbContext appDb = new ApplicationDbContext();
            var designationData = appDb.Set<IdentityDataModel.Designation>().Where<IdentityDataModel.Designation>(i => i.Id == Id).FirstOrDefault();

            designation = new ViewModelMapperService().MapDesignation(designationData);

            return designation;
        }
    }
}
