using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Anzu.AnnPortal.Business.API.Core;
using Anzu.AnnPortal.Business.Core.Core;
using Anzu.AnnPortal.Common.IocContainer;
using Anzu.AnnPortal.Common.Model.Portal;
using Anzu.AnnPortal.Common.Model.Common;
using System.Web;
using Newtonsoft.Json;
using System.Configuration;
using System.Security.Claims;
using Anzu.AnnPortal.Web.UI.Helper;
using WebApi.OutputCache.V2;
using System.Net.Http.Headers;

namespace Anzu.AnnPortal.Web.UI.ApiControllers.Core
{
    [Authorize]
    [RoutePrefix("api/Practice")]
    public class PracticeController : BaseController
    {
        /// <summary>
        /// The practice service
        /// </summary>
        // IPracticeService practiceService;

        /// <summary>
        /// Practices this instance.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        [HttpGet]
        [Route("Practices")]
        public ICollection<PracticeDTO> Practices()
        {
            PracticeService service = new PracticeService();
            return service.Practices();
        }

        [HttpGet]
        [Route("Practices/{searchOption}/{filter}")]
        public ICollection<PracticeDTO> Practices(string searchOption, string filter)
        {
            filter = HttpUtility.HtmlDecode(filter);
            filter = filter.Equals("NA") ? "" : filter;
            filter = filter.Replace("_!", "/");
            filter = filter.Replace("_~", ":");
            PracticeService service = new PracticeService();
            return service.SearchPractices(searchOption, filter.ToUpper());
        }

        /// <summary>
        /// Gets the practice by identifier.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetPracticeById/{id}")]
        public PracticeDTO GetPracticeById(long id)
        {
            PracticeService service = new PracticeService();
            return service.GetPracticeById(id);
        }

        /// <summary>
        /// Des the activate practice.
        /// </summary>
        /// <param name="practiceId">The practice identifier.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("DeActivatePractice")]
        public bool DeActivatePractice(PracticeDTO practice)
        {
            PracticeService service = new PracticeService();
            var userName = IdentityHelper.GetUserNameFromIdentity(((ClaimsIdentity)HttpContext.Current.User.Identity).Claims).Value;
            return service.DeActivatePractice(practice.Id, userName, practice.DeleteData, practice.RefreshCube);
        }

        /// <summary>
        /// Creates the practice.
        /// </summary>
        /// <param name="practice">The practice.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("CreatePractice")]
        public PracticeDTO CreatePractice(PracticeDTO practice)
        {
            var userName = IdentityHelper.GetUserNameFromIdentity(((ClaimsIdentity)HttpContext.Current.User.Identity).Claims).Value;
            PracticeService service = new PracticeService();
            return service.CreatePractice(practice, userName);
        }

        /// <summary>
        /// Updates the practice.
        /// </summary>
        /// <param name="practice">The practice.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("UpdatePractice")]
        public PracticeDTO UpdatePractice(PracticeDTO practice)
        {
            var userName = IdentityHelper.GetUserNameFromIdentity(((ClaimsIdentity)HttpContext.Current.User.Identity).Claims).Value;
            PracticeService service = new PracticeService();
            return service.UpdatePractice(practice, userName);
        }


        /// <summary>
        /// Searches the users.
        /// </summary>
        /// <param name="sort">The sort.</param>
        /// <param name="group">The group.</param>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("SearchUsers")]
        public IHttpActionResult SearchUsers(string sort = "", string group = "", string filter = "", bool practiceUsers = false)
        {
            HttpResponseMessage response;
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(ConfigurationManager.AppSettings["ExternalLoginEndpoint"]);
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            var identityDomain = System.Web.Configuration.WebConfigurationManager.AppSettings["identityService"].ToString();

            /// Get users from RADAR service
            //if (filter == "")
            //{
            //    response = client.GetAsync("/ANNRadar/ANNRadar.svc/SearchUser?searchKeyword=").Result;

            //}
            //else
            //{
            //    response = client.GetAsync("/ANNRadar/ANNRadar.svc/SearchUser?searchKeyword=" + filter).Result;
            //}

            /// Get users from identity
            if (string.IsNullOrEmpty(filter))
            {
                response = client.GetAsync(identityDomain + "api/Account/users/NA/NA/NA/NA/NA?take=0&skip=0&page=0&pageSize=0").Result;
            }
            else
            {
                response = client.GetAsync(identityDomain + "api/Account/users/NA/NA/NA/NA/" + filter + "?take=0&skip=0&page=0&pageSize=0").Result;
            }


            SearchUserResultDTO searchResut = new SearchUserResultDTO();
            searchResut.SearchUserResult = new List<ExternalUserDTO>();
            var jsonString = response.Content.ReadAsStringAsync().Result;
            var tempJson = JsonConvert.DeserializeObject<SearchIdentityUserResultDTO>(jsonString);

            foreach (var item in tempJson.Data)
            {
                //// get practice information
                //PracticeService practiceService = new PracticeService();
                //var tempUser = practiceService.GetUser(item.Id, item.UserId);   // get practice details from the user table
                if (practiceUsers && !item.UserRolesDisplay.Contains("Practice User"))
                {
                    continue;
                }

                searchResut.SearchUserResult.Add(new ExternalUserDTO
                {
                    FirstName = item.FirstName,
                    LastName = item.LastName,
                    UserName = item.UserId,
                    UserRoleId = (item.RoleId != null) ? Convert.ToInt64(item.RoleId) : 0,
                    RecordStatusId = (int)(Anzu.AnnPortal.Common.Model.Enum.RecordStatus.Active),
                    RRUserId = (item.Id != null) ? item.Id : Guid.NewGuid().ToString(),
                    UserRolesDisplay = item.UserRolesDisplay,
                    // UserStatusDisplay = 
                });
            }
            return Ok(searchResut.SearchUserResult);
        }

        private int GetRecordStatus(string id)
        {
            PracticeService service = new PracticeService();
            return service.GetRecordStatusByRRUserId(id);
        }

        [HttpGet]
        [Route("GetPracticeUsers")]
        public IHttpActionResult GetPracticeUsers(string sort = "", string group = "", string filter = "")
        {
            return SearchUsers(sort: sort, group: group, filter: filter, practiceUsers: true);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("UpdatePracticeUser")]
        public IHttpActionResult UpdatePracticeUser(PracticeUserUpdateDTO data)
        {
            PracticeService service = new PracticeService();
            return Ok(service.UpdatePracticeUser(data.FirstName, data.LastName, data.UserId));
        }


        /// <summary>
        /// Regionses this instance.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Regions")]
        public ICollection<RegionDTO> Regions()
        {
            PracticeService service = new PracticeService();
            return service.Regions();
        }


        /// <summary>
        /// Stateses this instance.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("States")]
        public ICollection<Common.Model.Common.StateDTO> States()
        {
            PracticeService service = new PracticeService();
            return service.States();
        }


        /// <summary>
        /// Zips the codes.
        /// </summary>
        /// <returns></returns>
        [Route("ZipCodes")]
        [HttpGet]
        public ICollection<ZipCodeDTO> ZipCodes()
        {
            PracticeService service = new PracticeService();
            return service.ZipCodes();
        }

        /// <summary>
        /// Zips the code filter by text.
        /// </summary>
        /// <param name="sort">The sort.</param>
        /// <param name="group">The group.</param>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("ZipCodeFilterByText")]
        public IHttpActionResult ZipCodeFilterByText(string sort = "", string group = "", string filter = "")
        {
            PracticeService service = new PracticeService();
            var result = service.ZipCodeFilterByText(filter);
            return Ok(result);

        }

        /// <summary>
        /// Userses this instance.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Users")]
        public ICollection<ExternalUserDTO> Users()
        {
            PracticeService service = new PracticeService();
            return service.Users();
        }

        /// <summary>
        /// Gets the product list.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetProductList")]
        public ICollection<BrestImplantDTO> GetProductList()
        {
            PracticeService service = new PracticeService();
            return service.GetProductList();
        }

        /// <summary>
        /// Determines whether [is date range correct] [the specified from date].
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <param name="practiceId">The practice identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetProIsDateRangeCorrect/{fromDate}/{toDate}/{practiceId}")]
        public bool IsDateRangeCorrect(DateTime fromDate, DateTime toDate, long practiceId)
        {
            PracticeService service = new PracticeService();
            return service.IsDateRangeCorrect(fromDate, toDate, practiceId);
        }

        /// <summary>
        /// Determines whether [is practice name unique] [the specified practice name].
        /// </summary>
        /// <param name="practiceName">Name of the practice.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("IsPracticeNameUnique/{practiceName}")]
        public bool IsPracticeNameUnique(string practiceName)
        {
            PracticeService service = new PracticeService();
            return service.IsPracticeNameUnique(practiceName);
        }

        /// <summary>
        /// Adds the procedure level.
        /// </summary>
        /// <param name="level">The level.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddProcedureLevels")]
        public List<ProcedureLevelDTO> AddProcedureLevels(List<ProcedureLevelDTO> levels)
        {
            PracticeService service = new PracticeService();
            var userName = IdentityHelper.GetUserNameFromIdentity(((ClaimsIdentity)HttpContext.Current.User.Identity).Claims).Value;
            return service.AddProcedureLevels(levels, userName);
        }

        /// <summary>
        /// Determines whether [is user already added] [the specified rr user identifier].
        /// </summary>
        /// <param name="rrUserId">The rr user identifier.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("IsUserAlreadyAdded")]
        public bool IsUserAlreadyAdded(ExternalUserDTO user)
        {
            if (user != null)
            {
                PracticeService service = new PracticeService();
                var result = service.IsUserAlreadyAdded(user.UserName);
                return result;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// Creates the procedure mapping.
        /// </summary>
        /// <param name="procedure">The procedure mapping.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("CreateProcedure")]
        public ProcedureDTO CreateProcedure(ProcedureDTO procedure)
        {
            PracticeService service = new PracticeService();
            var userName = IdentityHelper.GetUserNameFromIdentity(((ClaimsIdentity)HttpContext.Current.User.Identity).Claims).Value;
            return service.CreateProcedure(procedure, userName);
        }

        /// <summary>
        /// Gets the procedure by text.
        /// </summary>
        /// <param name="sort">The sort.</param>
        /// <param name="group">The group.</param>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetProcedureByText")]
        public IHttpActionResult GetProcedureByText(string sort = "", string group = "", string filter = "")
        {
            PracticeService service = new PracticeService();
            var result = service.GetProcedureByText(filter);
            return Ok(result);
        }

        /// <summary>
        /// Creates the asap user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("CreateASAPUser")]
        public ExternalUserDTO CreateASAPUser(ExternalUserDTO user)
        {
            PracticeService service = new PracticeService();
            var userName = IdentityHelper.GetUserNameFromIdentity(((ClaimsIdentity)HttpContext.Current.User.Identity).Claims).Value;
            return service.CreateASAPUser(user, userName);
        }

        /// <summary>
        /// Gets the asap users.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetASAPUsers")]
        public ICollection<ExternalUserDTO> GetASAPUsers()
        {
            PracticeService service = new PracticeService();
            return service.GetASAPUsers();
        }

        /// <summary>
        /// Deactivates the asap user.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("DeactivateASAPUser")]
        public bool DeactivateASAPUser(ExternalUserDTO user)
        {
            PracticeService service = new PracticeService();
            var userName = IdentityHelper.GetUserNameFromIdentity(((ClaimsIdentity)HttpContext.Current.User.Identity).Claims).Value;
            return service.DeactivateASAPUser(user.Id, userName);
        }

        /// <summary>
        /// Gets the latest emr identifier.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetLatestEMRId")]
        public string GetLatestEMRId()
        {
            PracticeService service = new PracticeService();
            return service.GetLatestEMRId();
        }

        /// <summary>
        /// Gets the name of the user by user.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetUserByUserName/")]
        public ExternalUserDTO GetUserByUserName()
        {
            try
            {
                var userName = IdentityHelper.GetUserNameFromIdentity(((ClaimsIdentity)HttpContext.Current.User.Identity).Claims).Value;
                PracticeService service = new PracticeService();
                return service.GetUserByUserName(userName);
            }
            catch (Exception)
            {

                throw;
            }

        }

        /// <summary>
        /// Adds the practice procedures.
        /// </summary>
        /// <param name="procedures">The procedures.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddPracticeProcedures")]
        public ICollection<PracticeProcedureDTO> AddPracticeProcedures(List<PracticeProcedureDTO> procedures)
        {
            PracticeService service = new PracticeService();
            var userName = IdentityHelper.GetUserNameFromIdentity(((ClaimsIdentity)HttpContext.Current.User.Identity).Claims).Value;
            return service.AddPracticeProcedures(procedures, userName);
        }

        /// <summary>
        /// Gets the procedures.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetProcedures")]
        [CacheOutput(ClientTimeSpan = 86400, ServerTimeSpan = 14400, ExcludeQueryStringFromCacheKey = true)]
        public IHttpActionResult GetProcedures()
        {
            PracticeService service = new PracticeService();
            var result = service.GetProcedures();
            return Ok(result);
        }

        /// <summary>
        /// Adds the practice procedures.
        /// </summary>
        /// <param name="procedure">The procedure.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddPracticeProcedure")]
        public PracticeProcedureDTO AddPracticeProcedure(PracticeProcedureDTO procedure)
        {
            PracticeService service = new PracticeService();
            var userName = IdentityHelper.GetUserNameFromIdentity(((ClaimsIdentity)HttpContext.Current.User.Identity).Claims).Value;
            return service.AddPracticeProcedure(procedure, userName);
        }

        /// <summary>
        /// Determines whether [is user already assigned] [the specified user].
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("IsUserAlreadyAssigned")]
        public ExternalUserDTO IsUserAlreadyAssigned(ExternalUserDTO user)
        {
            if (user != null)
            {
                PracticeService service = new PracticeService();
                var result = service.IsUserAlreadyAssigned(user.UserName);
                return result;
            }
            else
            {
                return null;
            }
        }

        [HttpPost]
        [Route("EditPracticeBreastImplants/{PracticeId}/{EmrId}")]
        public bool EditPracticeBreastImplants(long PracticeId, string EmrId)
        {
            PracticeService service = new PracticeService();
            var userName = IdentityHelper.GetUserNameFromIdentity(((ClaimsIdentity)HttpContext.Current.User.Identity).Claims).Value;

            return service.EditPracticeBreastImplants(PracticeId, EmrId, userName);
        }
        [HttpGet]
        [Route("UpdateDashboradIdForPreview/{id}")]
        public bool UpdateDashboradIdForPreview(int id)
        {
            // get current practice claim
            var practiceIdClaim = ((ClaimsIdentity)System.Web.HttpContext.Current.User.Identity).Claims
               .FirstOrDefault(c => c.Type == "practiceId");

            var emrIdClaim = ((ClaimsIdentity)System.Web.HttpContext.Current.User.Identity).Claims
                .FirstOrDefault(c => c.Type == "emrId");

            // remove current practice claim
            ((ClaimsIdentity)System.Web.HttpContext.Current.User.Identity).RemoveClaim(practiceIdClaim);
            ((ClaimsIdentity)System.Web.HttpContext.Current.User.Identity).RemoveClaim(emrIdClaim);

            // add new practiceClaim with new id
            ((ClaimsIdentity)System.Web.HttpContext.Current.User.Identity).AddClaim((new Claim("practiceId", id.ToString())));

            PracticeService service = new PracticeService();
            var practice = service.GetPracticeById(id);
            ((ClaimsIdentity)System.Web.HttpContext.Current.User.Identity).AddClaim((new Claim("emrId", practice.EmrId)));

            var authenticationManager = HttpContext.Current.GetOwinContext().Authentication;
            authenticationManager.AuthenticationResponseGrant = new Microsoft.Owin.Security.AuthenticationResponseGrant(new ClaimsPrincipal(((ClaimsIdentity)System.Web.HttpContext.Current.User.Identity)), new Microsoft.Owin.Security.AuthenticationProperties() { IsPersistent = true });

            return true;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("RemovePracticeUser/{username}")]
        public bool RemovePracticeUser(string username)
        {
            PracticeService service = new PracticeService();
            return service.RemovePracticeUser(username);
        }

        [HttpGet]
        [Route("ValidateCubeRefreshByPractice/{emrId}")]
        public bool ValidateCubeRefreshByPractice(string emrId)
        {
            PracticeService service = new PracticeService();
            return service.ValidateEmrStatus(emrId);
        }

        [HttpPost]
        [Route("ValidatePracticeList")]
        public List<string> ValidatePracticeList(List<string> emrList)
        {
            PracticeService service = new PracticeService();
            var returnList = new List<string>();
            foreach (var emr in emrList)
            {
                if (service.ValidateEmrStatus(emr))
                {
                    returnList.Add(emr);
                }
            }

            return returnList;
        }

        [HttpGet]
        [Route("PracticeReActivatedCheck/{emrId}")]
        public bool PracticeReActivatedCheck(string emrId)
        {
            PracticeService service = new PracticeService();
            return service.PracticeReActivatedCheck(emrId);
        }

        [HttpPost]
        [Route("ProcessCube")]
        public bool ProcessCube(ProcessCubeDTO data)
        {
            if (data.EmrList.Count > 0)
            {
                PracticeService service = new PracticeService();
                var userName = IdentityHelper.GetUserNameFromIdentity(((ClaimsIdentity)HttpContext.Current.User.Identity).Claims).Value;
                var isSuccess = service.StartProcessCube(data.EmrList, userName, Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["ManualCubeProcessTimeout"]));
                if (isSuccess)
                {
                    //// Send Email
                    var currentUserEmail = ((ClaimsIdentity)System.Web.HttpContext.Current.User.Identity).Claims
                                    .Where(c => c.Type == "UserEmail")
                                    .Select(c => c.Value).FirstOrDefault();
                    var jobCreatorFirstName = ((ClaimsIdentity)System.Web.HttpContext.Current.User.Identity).Claims
                                    .Where(c => c.Type == "FirstName")
                                    .Select(c => c.Value).FirstOrDefault();
                    var jobCreatorLastName = ((ClaimsIdentity)System.Web.HttpContext.Current.User.Identity).Claims
                                    .Where(c => c.Type == "LastName")
                                    .Select(c => c.Value).FirstOrDefault();

                    EmailService emailService = new EmailService();

                    var senderList = service.GetServiceEmailList();
                    senderList.Add(currentUserEmail);
                    var emailSenderList = string.Join(", ", senderList);
                    var emrList = new List<string>();
                    foreach (var emrId in data.EmrList)
                    {
                        emrList.Add(string.Format("{0} - {1}", emrId, service.GetPracticeByEMRId(emrId).Name));
                    }

                    emailService.SendEmail(emailSenderList, emailService.GetEmailSubject(data.JobType == 1 ? EmailMessageType.AllCubeRefreshStarted : EmailMessageType.SelectedCubeRefreshStarted), emailService.GetEmailBody(data.JobType == 1 ? EmailMessageType.AllCubeRefreshStarted : EmailMessageType.SelectedCubeRefreshStarted,
                        string.Format("{0} {1}", jobCreatorFirstName, jobCreatorLastName), emrList));
                }
                return isSuccess;
            }
            else
            {
                return false;
            }
        }

        [HttpGet]
        [Route("GetJobRunningStatus")]
        public List<JobStatusDTO> GetJobRunningStatus()
        {
            PracticeService service = new PracticeService();
            return service.GetJobRunningStatus();
        }

        [HttpGet]
        [Route("CancelPendingJob")]
        public bool CancelRunningJob()
        {
            PracticeService service = new PracticeService();
            var userName = IdentityHelper.GetUserNameFromIdentity(((ClaimsIdentity)HttpContext.Current.User.Identity).Claims).Value;
            return service.CancelPendingJob(userName, Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["ManualCubeProcessTimeout"]));
        }

        [HttpGet]
        [Route("PracticeHasDataCheck/{emrId}")]
        public bool PracticeHasDataCheck(string emrId)
        {
            PracticeService service = new PracticeService();
            return service.PracticeHasDataCheck(emrId);
        }

        [HttpGet]
        [Route("IsPracticeContainImplants/{id}")]
        public bool IsPracticeContainImplants(long id)
        {
            PracticeService service = new PracticeService();
            return service.IsPracticeContainImplants(id);
        }
    }
}