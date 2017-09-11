namespace Anzu.AnnPortal.Identity.Service.Controllers
{
    using System;
    using System.Web.Http;
    using Anzu.AnnPortal.Identity.Core;
    using Filters;

    /// <summary>
    /// Designation api controller
    /// </summary>

    //[Authorize]
    public class DesignationController : BaseApiController
    {
        CommonHelper ch = new CommonHelper();

        /// <summary>
        /// Gets this instance.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult Get()
        {
            ch.LogMethodStartAPI(System.Reflection.MethodBase.GetCurrentMethod().Name, DateTime.Now);
            DesignationService designationService = new DesignationService();
            var result = this.Json(designationService.GetAll());
            ch.LogMethodEndAPI(System.Reflection.MethodBase.GetCurrentMethod().Name, DateTime.Now);
            return result;
        }
    }
}
