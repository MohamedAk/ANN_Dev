using Anzu.AnnPortal.Business.API.Core;
using Anzu.AnnPortal.Business.Core.Core;
using Anzu.AnnPortal.Common.IocContainer;
using Anzu.AnnPortal.Common.Model.Common;
using Anzu.AnnPortal.Common.Model.Portal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Anzu.AnnPortal.Business.Service.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    public class PracticeController : ApiController
    {

        /// <summary>
        /// The practice service
        /// </summary>
        IPracticeService practiceService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PracticeController"/> class.
        /// </summary>
        public PracticeController()
        {
            this.practiceService = IocContainer.Resolve<IPracticeService>();
        }

        /// <summary>
        /// Practices this instance.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        [HttpGet]
        public ICollection<PracticeDTO> Practice()
        {
            return null;
        }

        /// <summary>
        /// Des the activate practice.
        /// </summary>
        /// <param name="practiceId">The practice identifier.</param>
        /// <returns></returns>
        [HttpPost]
        public bool DeActivatePractice(long practiceId)
        {
            return practiceService.DeActivatePractice(practiceId);
        }


        /// <summary>
        /// Creates the practice.
        /// </summary>
        /// <param name="practice">The practice.</param>
        /// <returns></returns>
        [HttpPost]
        public bool CreatePractice(PracticeDTO practice)
        {

            return practiceService.CreatePractice(practice);
        }





        /// <summary>
        /// Searches the users.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        [HttpGet]
        public ICollection<ExternalUserDTO> SearchUsers(string query)
        {

            return practiceService.SearchUsers(query);
        }


        /// <summary>
        /// Regionses this instance.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public System.Collections.Generic.ICollection<RegionDTO> Regions()
        {

            return practiceService.Regions();
        }


        /// <summary>
        /// Stateses this instance.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public System.Collections.Generic.ICollection<Common.Model.Common.StateDTO> States()
        {

            return practiceService.States();
        }


        /// <summary>
        /// Zips the codes.
        /// </summary>
        /// <returns></returns>
        [Route("api/ZipCodes")]
        [HttpGet]
        public System.Collections.Generic.ICollection<ZipCodeDTO> ZipCodes()
        {
            return practiceService.ZipCodes();
        }
    }
}
