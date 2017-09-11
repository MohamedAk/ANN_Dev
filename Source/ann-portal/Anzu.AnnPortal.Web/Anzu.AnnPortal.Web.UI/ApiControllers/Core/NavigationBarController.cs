using Anzu.AnnPortal.Business.Core.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Anzu.AnnPortal.Web.UI.ApiControllers.Core
{
    /// <summary>
    /// Class Navigation Bar Contoller.
    /// </summary>
    /// <seealso cref="Anzu.AnnPortal.Web.UI.ApiControllers.BaseController" />
    [Authorize]
    [RoutePrefix("api/NavigationBar")]
    public class NavigationBarController : BaseController
    {
        /// <summary>
        /// The _navigation barervice
        /// </summary>
        private NavigationBarService _navigationBarService;

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationBarController"/> class.
        /// </summary>
        public NavigationBarController()
        {
            _navigationBarService = new NavigationBarService();
        }

        /// <summary>
        /// Gets the navigation bar information.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetNavigationBarInformation")]
        public IHttpActionResult GetNavigationBarInformation()
        {
            var result = _navigationBarService.GetNavigationBarInformation();
            return Ok(result);
        }
    }
}