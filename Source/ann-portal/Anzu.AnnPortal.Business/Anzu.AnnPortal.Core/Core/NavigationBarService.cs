using Anzu.AnnPortal.Business.API.Core;
using Anzu.AnnPortal.Common.Model.Portal;
using Anzu.AnnPortal.Data.EntityManager;
using Anzu.AnnPortal.Data.Model;
using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anzu.AnnPortal.Business.Core.Core
{
    /// <summary>
    /// Class Navigation Bar Service.
    /// </summary>
    /// <seealso cref="Core.BaseService" />
    /// <seealso cref="Anzu.AnnPortal.Business.API.Core.INavigationService" />
    public class NavigationBarService : BaseService, INavigationService
    {
        /// <summary>
        /// The repository.
        /// </summary>
        private BaseRepository repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationBarService"/> class.
        /// </summary>
        public NavigationBarService()
        {
            repository = new BaseRepository();
        }

        /// <summary>
        /// Gets the navigation bar information.
        /// </summary>
        /// <returns></returns>
        public NavBarDTO GetNavigationBarInformation()
        {
            NavBarDTO result = new NavBarDTO();
            // get today's data
            //var totalRevenue = repository.Find<TotalRevenue>(p => p.Date == DateTime.Today).FirstOrDefault();
            //var totalProcedures = repository.Find<TotalProcedure>(p => p.Date == DateTime.Today).FirstOrDefault();

            // get latest available data
            var totalRevenue = repository.GetAll<TotalRevenue>().LastOrDefault();
            var totalProcedures = repository.GetAll<TotalProcedure>().LastOrDefault();

            result.TotalProcedures = totalProcedures != null ? totalProcedures.Value : 0;
            result.TotalRevenue = totalRevenue != null ? totalRevenue.Value : 0;
            return result;
        }
    }
}
