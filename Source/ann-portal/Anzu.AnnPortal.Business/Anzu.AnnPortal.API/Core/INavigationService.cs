using Anzu.AnnPortal.Common.Model.Portal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anzu.AnnPortal.Business.API.Core
{
    /// <summary>
    /// Interface INavigationService.
    /// </summary>
    public interface INavigationService
    {
        /// <summary>
        /// Gets the navigation bar information.
        /// </summary>
        /// <returns></returns>
        NavBarDTO GetNavigationBarInformation();
    }
}
