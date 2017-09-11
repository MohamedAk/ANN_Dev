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
using Anzu.AnnPortal.Web.UI.Helper;
using System.Security.Claims;
using WebApi.OutputCache.V2;
using System.Web.Configuration;

namespace Anzu.AnnPortal.Web.UI.ApiControllers.Core
{
    /// <summary>
    /// Class Metadata Controller.
    /// </summary>
    /// <seealso cref="Anzu.AnnPortal.Web.UI.ApiControllers.BaseController" />
    [Authorize]
    [RoutePrefix("api/Metadata")]
    public class MetadataController : BaseController
    {
        private MetadataService _metadataService;

        public MetadataController()
        {
            _metadataService = new MetadataService();
        }
        /// <summary>
        /// Gets the procedures for level.
        /// </summary>
        /// <param name="levelId">The level identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetProcedureLevel/{levelId}")]
        public ICollection<ProcedureLevelDTO> GetProcedureLevel(int levelId)
        {
            try
            {
                return _metadataService.GetProcedureLevel(levelId);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        [Route("SaveProcedureLevels/")]
        public ICollection<ProcedureLevelDTO> SaveProcedureLevels(List<ProcedureLevelDTO> procedureLevels)
        {
            try
            {
                var userName = IdentityHelper.GetUserNameFromIdentity(((ClaimsIdentity)HttpContext.Current.User.Identity).Claims).Value;
                List<ProcedureLevelDTO> savedlist = new List<ProcedureLevelDTO>();
                foreach (var pL in procedureLevels)
                {
                    if (pL.Name != null)
                    {
                        if (pL.Id > 0)
                        {
                            pL.ModifiedBy = userName;
                            pL.ModifiedDate = DateTime.Now;
                            savedlist.Add(_metadataService.Update(pL));
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(pL.Name))
                            {
                                pL.CreatedBy = userName;
                                pL.CreatedDate = DateTime.Now;
                                pL.RecordStatusId = (int)(Anzu.AnnPortal.Common.Model.Enum.RecordStatus.Active);
                                pL.Id = _metadataService.Insert(pL).Id;
                                savedlist.Add(pL);
                            }
                        }
                    }
                }

                return savedlist.Where(l => l.RecordStatusId == (int)(Anzu.AnnPortal.Common.Model.Enum.RecordStatus.Active)).ToList();
            }
            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// Gets the breast implants.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetBreastImplants/")]
        public ICollection<BrestImplantDTO> GetBreastImplants()
        {
            try
            {
                return _metadataService.GetBreastImplants();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Saves the breast implants.
        /// </summary>
        /// <param name="breastImplants">The breast implants.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("SaveBreastImplants/")]
        public ICollection<BrestImplantDTO> SaveBreastImplants(List<BrestImplantDTO> breastImplants)
        {
            try
            {
                var userName = IdentityHelper.GetUserNameFromIdentity(((ClaimsIdentity)HttpContext.Current.User.Identity).Claims).Value;
                List<BrestImplantDTO> savedlist = new List<BrestImplantDTO>();
                foreach (var pL in breastImplants)
                {
                    if (pL.Name != null)
                    {
                        if (pL.Id > 0)
                        {
                            pL.ModifiedBy = userName;
                            pL.ModifiedDate = DateTime.Now;
                            savedlist.Add(_metadataService.Update(pL));
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(pL.Name))
                            {
                                pL.CreatedBy = userName;
                                pL.CreatedDate = DateTime.Now;
                                pL.RecordStatusId = (int)(Anzu.AnnPortal.Common.Model.Enum.RecordStatus.Active);
                                pL.Id = _metadataService.Insert(pL).Id;
                                savedlist.Add(pL);
                            }
                        }
                    }
                }

                return savedlist.Where(l => l.RecordStatusId == (int)(Anzu.AnnPortal.Common.Model.Enum.RecordStatus.Active)).ToList();
            }
            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// Procedures the level filter by text.
        /// </summary>
        /// <param name="sort">The sort.</param>
        /// <param name="group">The group.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="levelId">The level identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("ProcedureLevelFilterByText/{levelId}/{procedureId}/")]
        //[Route("ProcedureLevelFilterByText/{levelId}")]
        public IHttpActionResult ProcedureLevelFilterByText(string sort = "", string group = "", string filter = "", long levelId = 0, long procedureId = 0)
        {
            try
            {
                //var result = _metadataService.ProcedureLevelFilterByText(levelId, filter);
                var result = _metadataService.ProcedureLevelFilterByText(levelId, filter, procedureId);
                return Ok(result);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        [Route("ProcedureFilterByText")]
        public IHttpActionResult ProcedureFilterByText(string sort = "", string group = "", string filter = "")
        {
            try
            {
                var result = _metadataService.ProcedureFilterByText(filter);
                return Ok(result);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Gets all procedure levels.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAllProcedureLevels/")]
        public IHttpActionResult GetAllProcedureLevels()
        {
            try
            {
                var result = _metadataService.GetAllProcedureLevels();
                return Ok(result);
            }
            catch (Exception)
            {
                throw;
            }
        }

        //[HttpGet]
        //[Route("GetServicesFromStagingByEMRId/{emrId}")]
        //public ICollection<ServiceDTO> GetServicesFromStagingByEMRId(string emrId)
        //{
        //    try
        //    {
        //        return _metadataService.GetServicesFromStagingByEMRId(emrId);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        /// <summary>
        /// Get SQL job status
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetSqlJobStatus/")]
        [AllowAnonymous]
        public IHttpActionResult GetSqlJobStatus()
        {
            var data = _metadataService.GetSqlJobStatus().FirstOrDefault();
            var cubeProcessingStep = Convert.ToInt32((WebConfigurationManager.AppSettings["cubeProcessingStep"]));
            var status = false;
            var message = "ETL is complete";
            if (data != null && data.current_step == cubeProcessingStep)
            {
                status = true;
                message = "Processing Cube";
            }

            return Ok(new { status = status, message = message });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="emrId"></param>
        /// <param name="take"></param>
        /// <param name="skip"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [Route("GetServicesFromStagingByEMRIdToGrid/{emrId}/{sortOption}")]
        [HttpGet]
        public IHttpActionResult GetServicesFromStagingByEMRIdToGrid(string emrId, string sortOption, int take, int skip, int page, int pageSize)
        {
            int TotalRows;
            var result = new
            {
                Data = _metadataService.GetServicesFromStagingByEMRIdToGrid(take, skip, page, pageSize, emrId, out TotalRows, sortOption).ToList<PracticeProcedureDTO>(),
                Total = TotalRows
            };

            return Ok(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="emrId"></param>
        /// <param name="serviceName"></param>
        /// <param name="procedureName"></param>
        /// <param name="product"></param>
        /// <param name="company"></param>
        /// <param name="take"></param>
        /// <param name="skip"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [Route("GetServicesFromStagingByEMRIdToGrid/{emrId}/{sortOption}/{searchOption}/{searchText}/")]
        [HttpGet]
        public IHttpActionResult GetServicesFromStagingByEMRIdToGrid(string emrId, string searchOption, string searchText, string sortOption, int take, int skip, int page, int pageSize)
        {
            int TotalRows;
            searchText = (searchText != "NA") ? searchText : "";
            var result = new
            {
                Data = _metadataService.GetServicesFromStagingByEMRIdToGrid(take, skip, page, pageSize, emrId, out TotalRows, sortOption, searchOption, searchText.Trim()).ToList<PracticeProcedureDTO>(),
                Total = TotalRows
            };

            return Ok(result);
        }

        /// <summary>
        /// Determines whether [is procedure level sequence unique] [the specified sequence].
        /// </summary>
        /// <param name="sequence">The sequence.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("IsDuplicateProcedureLevelSequence/{name}/{procedureLevel1}/{procedureLevel2}/{procedureLevel3}/{procedureLevel4}")]
        public bool IsDuplicateProcedureLevelSequence(string name = "", int? procedureLevel1 = null, int? procedureLevel2 = null, int? procedureLevel3 = null, int? procedureLevel4 = null)
        {
            if (procedureLevel3 == null)
            {
                procedureLevel3 = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LevelThreeUnspecifiedId"]);
            }
            if (procedureLevel4 == null)
            {
                procedureLevel4 = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LevelFourUnspecifiedId"]);
            }

            return _metadataService.IsDuplicateProcedureLevelSequence(name, procedureLevel1, procedureLevel2, procedureLevel3, procedureLevel4);
        }

        [HttpPost]
        [Route("ProcedureLevelSequences/")]
        public ICollection<ProcedureDTO> ProcedureLevelSequences(List<ProcedureDTO> sequence)
        {
            try
            {
                var userName = IdentityHelper.GetUserNameFromIdentity(((ClaimsIdentity)HttpContext.Current.User.Identity).Claims).Value;
                List<ProcedureDTO> savedlist = new List<ProcedureDTO>();
                foreach (var pL in sequence)
                {
                    if (pL.Name != null)
                    {
                        if (pL.Id > 0)
                        {
                            pL.ModifiedBy = userName;
                            pL.ModifiedDate = DateTime.Now;
                            savedlist.Add(_metadataService.Update(pL));
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(pL.Name))
                            {
                                pL.CreatedBy = userName;
                                pL.CreatedDate = DateTime.Now;
                                pL.RecordStatusId = (int)(Anzu.AnnPortal.Common.Model.Enum.RecordStatus.Active);
                                pL.ProcedureLevel1 = pL.ProcedureLevel2 = pL.ProcedureLevel3 = pL.ProcedureLevel4 = null;
                                pL.Id = _metadataService.Insert(pL).Id;
                                savedlist.Add(pL);
                            }
                        }
                    }
                }

                return savedlist.Where(l => l.RecordStatusId == (int)(Anzu.AnnPortal.Common.Model.Enum.RecordStatus.Active)).ToList();
            }
            catch (Exception)
            {
                throw;
            }

        }

        [HttpPost]
        [Route("DeleteProcedure/{procedureId}")]
        public ProcedureDTO DeleteProcedure(long procedureId)
        {
            try
            {
                var userName = IdentityHelper.GetUserNameFromIdentity(((ClaimsIdentity)HttpContext.Current.User.Identity).Claims).Value;
                return _metadataService.DeleteProcedure(procedureId, userName);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet]
        [Route("GetPracticeProceduresByProcedureId/{procedureId}")]
        public IEnumerable<long?> GetPracticeProceduresByProcedureId(long procedureId)
        {
            try
            {
                return _metadataService.GetPracticeProceduresByProcedureId(procedureId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [Route("ProcedureRelatedProcedure")]
        //public ICollection<RelatedProcedureDTO> ProcedureRelatedProcedure(ProcedureDTO proc, List<int> id)
        public ICollection<RelatedProcedureDTO> ProcedureRelatedProcedure(ProcedureDTO proc)
        {
            var userName = IdentityHelper.GetUserNameFromIdentity(((ClaimsIdentity)HttpContext.Current.User.Identity).Claims).Value;
            ProcedureDTO savedItem = new ProcedureDTO();
            List<RelatedProcedureDTO> savedList = new List<RelatedProcedureDTO>();

            if (proc.Name != null)
            {
                if (proc.Id > 0)
                {
                    proc.ModifiedBy = userName;
                    proc.ModifiedDate = DateTime.Now;

                    if (proc.ProcedureLevelThree == null)
                    {
                        proc.ProcedureLevelThree = Convert.ToInt64(System.Configuration.ConfigurationManager.AppSettings["LevelThreeUnspecifiedId"]);
                    }
                    if (proc.ProcedureLevelFour == null)
                    {
                        proc.ProcedureLevelFour = Convert.ToInt64(System.Configuration.ConfigurationManager.AppSettings["LevelFourUnspecifiedId"]);
                    }

                    bool exisitingBreastState = _metadataService.GetBreastImplantState(proc.Id);

                    List<long> exisitingRelatedProcedureIds = _metadataService.GetExistingRelatedProcedureIds(proc.Id);
                    List<int> convertexisitingRelatedProcedureIds = exisitingRelatedProcedureIds.Select(i => (int)i).ToList();

                    //Check related procedure Deleted??
                    var exceptList = convertexisitingRelatedProcedureIds.Except(proc.relatedProcedureId);

                    //Check related procedure Added??
                    var exceptListAdded = proc.relatedProcedureId.Except(convertexisitingRelatedProcedureIds);

                    //Add record to practice edit information
                    if (exceptList.Count() > 0 || exceptListAdded.Count() > 0 || exisitingBreastState != proc.PracticeProductFlag)
                    {
                        _metadataService.AddPracticeEditInfoOnProcedureChange(Convert.ToInt32(proc.Id), userName);
                    }

                    _metadataService.Update(proc);
                    savedItem = proc;

                    //Delete existing related procedures
                    //_metadataService.DeleteRelatedProcedure(proc.Id);
                    _metadataService.DeleteOnlyRelatedProcedure(proc.Id);

                    foreach (var relatedItem in proc.relatedProcedureId)
                    {
                        RelatedProcedureDTO relatedProcedureItem = new RelatedProcedureDTO();

                        relatedProcedureItem.RecordStatusId = (int)(Anzu.AnnPortal.Common.Model.Enum.RecordStatus.Active);
                        relatedProcedureItem.CreatedBy = userName;
                        relatedProcedureItem.CreatedDate = DateTime.Now;
                        relatedProcedureItem.RelatedProcedureId = relatedItem;
                        relatedProcedureItem.ProcedureID = savedItem.Id;

                        _metadataService.Insert(relatedProcedureItem);
                        savedList.Add(relatedProcedureItem);

                    }

                }
                else
                {
                    if (!string.IsNullOrEmpty(proc.Name))
                    {
                        proc.CreatedBy = userName;
                        proc.CreatedDate = DateTime.Now;
                        proc.RecordStatusId = (int)(Anzu.AnnPortal.Common.Model.Enum.RecordStatus.Active);
                        proc.ProcedureLevel1 = proc.ProcedureLevel2 = proc.ProcedureLevel3 = proc.ProcedureLevel4 = null;
                        if (proc.ProcedureLevelThree == null)
                        {
                            proc.ProcedureLevelThree = Convert.ToInt64(System.Configuration.ConfigurationManager.AppSettings["LevelThreeUnspecifiedId"]);
                        }
                        if (proc.ProcedureLevelFour == null)
                        {
                            proc.ProcedureLevelFour = Convert.ToInt64(System.Configuration.ConfigurationManager.AppSettings["LevelFourUnspecifiedId"]);
                        }

                        proc.Id = _metadataService.Insert(proc).Id;
                        savedItem = proc;

                        foreach (var relatedItem in proc.relatedProcedureId)
                        {
                            RelatedProcedureDTO relatedProcedureItem = new RelatedProcedureDTO();

                            relatedProcedureItem.RecordStatusId = (int)(Anzu.AnnPortal.Common.Model.Enum.RecordStatus.Active);
                            relatedProcedureItem.CreatedBy = userName;
                            relatedProcedureItem.CreatedDate = DateTime.Now;
                            relatedProcedureItem.RelatedProcedureId = relatedItem;
                            relatedProcedureItem.ProcedureID = savedItem.Id;

                            _metadataService.Insert(relatedProcedureItem);
                            savedList.Add(relatedProcedureItem);
                        }
                    }
                }
            }
            return savedList.Where(l => l.RecordStatusId == (int)(Anzu.AnnPortal.Common.Model.Enum.RecordStatus.Active)).ToList();
        }

        [HttpGet]
        [Route("GetProcedureById/{id}")]
        public ProcedureDTO GetProcedureById(int id)
        {
            try
            {
                ProcedureDTO procedure = _metadataService.GetProcedureById(id);
                return procedure;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        [Route("GetRelatedProcedureByProcedureId/{procedureId}")]
        //public ICollection<RelatedProcedureDTO> GetRelatedProcedureByProcedureId(int procedureId)
        public ICollection<ProcedureDTO> GetRelatedProcedureByProcedureId(int procedureId)
        {
            try
            {
                return _metadataService.GetRelatedProcedureByProcedureId(procedureId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the companies.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetCompanies/")]
        //[CacheOutput(ClientTimeSpan = 3600, ExcludeQueryStringFromCacheKey = true)]
        public IHttpActionResult GetCompanies()
        {
            try
            {
                var result = _metadataService.GetCompanies();
                return Ok(result);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Saves the companies.
        /// </summary>
        /// <param name="companies">The companies.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("SaveCompanies/")]
        public ICollection<CompanyDTO> SaveCompanies(List<CompanyDTO> companies)
        {
            try
            {
                var userName = IdentityHelper.GetUserNameFromIdentity(((ClaimsIdentity)HttpContext.Current.User.Identity).Claims).Value;
                List<CompanyDTO> savedlist = new List<CompanyDTO>();
                foreach (var pL in companies)
                {
                    if (pL.Name != null)
                    {
                        if (pL.Id > 0)
                        {
                            pL.ModifiedBy = userName;
                            pL.ModifiedDate = DateTime.Now;
                            savedlist.Add(_metadataService.Update(pL));
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(pL.Name))
                            {
                                pL.CreatedBy = userName;
                                pL.CreatedDate = DateTime.Now;
                                pL.RecordStatusId = (int)(Anzu.AnnPortal.Common.Model.Enum.RecordStatus.Active);
                                pL.Id = _metadataService.Insert(pL).Id;
                                savedlist.Add(pL);
                            }
                        }
                    }
                }

                return savedlist.Where(l => l.RecordStatusId == (int)(Anzu.AnnPortal.Common.Model.Enum.RecordStatus.Active)).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the products.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetProducts/")]
        //[CacheOutput(ClientTimeSpan = 3600, ExcludeQueryStringFromCacheKey = true)]
        public IHttpActionResult GetProducts()
        {
            try
            {
                var result = _metadataService.GetProducts();
                return Ok(result);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Saves the products.
        /// </summary>
        /// <param name="products">The products.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("SaveProducts/")]
        public ICollection<ProductTypeDTO> SaveProducts(List<ProductTypeDTO> products)
        {
            try
            {
                var userName = IdentityHelper.GetUserNameFromIdentity(((ClaimsIdentity)HttpContext.Current.User.Identity).Claims).Value;
                List<ProductTypeDTO> savedlist = new List<ProductTypeDTO>();
                foreach (var pL in products)
                {
                    if (pL.Name != null)
                    {
                        if (pL.Id > 0)
                        {
                            pL.ModifiedBy = userName;
                            pL.ModifiedDate = DateTime.Now;
                            savedlist.Add(_metadataService.Update(pL));
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(pL.Name))
                            {
                                pL.CreatedBy = userName;
                                pL.CreatedDate = DateTime.Now;
                                pL.RecordStatusId = (int)(Anzu.AnnPortal.Common.Model.Enum.RecordStatus.Active);
                                pL.Id = _metadataService.Insert(pL).Id;
                                savedlist.Add(pL);
                            }
                        }
                    }
                }

                return savedlist.Where(l => l.RecordStatusId == (int)(Anzu.AnnPortal.Common.Model.Enum.RecordStatus.Active)).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the procedure levels.
        /// </summary>
        /// <param name="take">The take.</param>
        /// <param name="skip">The skip.</param>
        /// <param name="page">The page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns></returns>
        [Route("GetProcedureLevels")]
        [HttpGet]
        public IHttpActionResult GetProcedureLevels(int take, int skip, int page, int pageSize)
        {
            int TotalRows;
            var result = new
            {
                Data = _metadataService.GetProcedureLevels(take, skip, page, pageSize, out TotalRows).ToList<ProcedureDTO>(),
                Total = TotalRows
            };

            return Ok(result);
        }

        [Route("GetProcedureLevels/{sortOption}")]
        [HttpGet]
        public IHttpActionResult GetProcedureLevelsFilter(int take, int skip, int page, int pageSize, string sortOption)
        {
            int TotalRows;
            var result = new
            {
                Data = _metadataService.GetProcedureLevelsFilter(take, skip, page, pageSize, "NA", out TotalRows, sortOption).ToList<ProcedureDTO>(),
                Total = TotalRows
            };

            return Ok(result);
        }

        [Route("GetProcedureLevels/{sortOption}/{searchOption}/{filter}")]
        [HttpGet]
        public IHttpActionResult GetProcedureLevelsFilter(int take, int skip, int page, int pageSize, string sortOption, string searchOption, string filter, int procedureId = 0)
        {
            filter = HttpUtility.HtmlDecode(filter);
            filter = filter.Replace("_!", "/");
            filter = filter.Replace("_~", ":");
            int TotalRows;
            var result = new
            {
                Data = _metadataService.GetProcedureLevelsFilter(take, skip, page, pageSize, filter.Trim(), out TotalRows, sortOption, searchOption, procedureId).ToList<ProcedureDTO>(),
                Total = TotalRows
            };

            return Ok(result);
        }

        /// <summary>
        /// Determines whether [is procedure level unique] [the specified level number].
        /// </summary>
        /// <param name="levelNumber">The level number.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("IsProcedureLevelUnique/{levelNumber}/{name}")]
        public bool IsProcedureLevelUnique(int levelNumber, string name)
        {
            return _metadataService.IsProcedureLevelUnique(levelNumber, name);
        }

        /// <summary>
        /// Determines whether [is brest implant unique] [the specified name].
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("IsBreastImplantUnique/{name}")]
        public bool IsBreastImplantUnique(string name)
        {
            return _metadataService.IsBreastImplantUnique(name);
        }

        /// <summary>
        /// Determines whether [is product type unique] [the specified name].
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("IsProductTypeUnique/{name}")]
        public bool IsProductTypeUnique(string name)
        {
            return _metadataService.IsProductTypeUnique(name);
        }

        /// <summary>
        /// Determines whether [is company unique] [the specified name].
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("IsCompanyUnique/{name}")]
        public bool IsCompanyUnique(string name)
        {
            return _metadataService.IsCompanyUnique(name);
        }

        [HttpGet]
        [Route("AuthorizeRouting/")]
        public IHttpActionResult AuthorizeRouting()
        {
            try
            {
                var path = Request.GetQueryNameValuePairs().FirstOrDefault(x => x.Key.Equals("p")).Value.Trim();

                var userPathList = new List<string>();
                var asapUserPathList = new List<string>();
                var administratorPathList = new List<string>();

                userPathList.Add("/dashboard/");
                userPathList.Add("/dashboard/Conversion");
                userPathList.Add("/dashboard/ANN Monitor");
                userPathList.Add("/dashboard/Return Patients");
                userPathList.Add("/dashboard/Repeat Procedures");

                asapUserPathList.Add("/dashboard/");
                asapUserPathList.Add("/dashboard/Conversion Admin");
                asapUserPathList.Add("/dashboard/ANN Monitor Admin");
                asapUserPathList.Add("/dashboard/Products Sold Admin");
                asapUserPathList.Add("/dashboard/Return Patients Admin");
                asapUserPathList.Add("/dashboard/Repeat Procedures Admin");

                administratorPathList.Add("/admin/practice-management");
                administratorPathList.Add("/admin/asap-user-management");
                administratorPathList.Add("/admin/procedure-filter-mappings");
                administratorPathList.Add("/admin/procedure-filter-meta-data");

                var currentUserRole = ((ClaimsIdentity)User.Identity).Claims.Where(c => c.Type == "UserRole").Select(c => c.Value).FirstOrDefault();
                var redirectUrl = "/#/Identity/Login/Login";
                switch (currentUserRole)
                {
                    case "ADMINISTRATOR":
                        redirectUrl = "/admin/practice-management";
                        return Json(new { isValid = administratorPathList.Contains(path), redirectUrl = redirectUrl });
                    case "ASAP_USER":
                        redirectUrl = "/dashboard/";
                        return Json(new { isValid = asapUserPathList.Contains(path), redirectUrl = redirectUrl });
                    case "PRACTICE_USER":
                        redirectUrl = "/dashboard/";
                        return Json(new { isValid = userPathList.Contains(path), redirectUrl = redirectUrl });
                    default:
                        return Json(new { isValid = false, redirectUrl = redirectUrl });
                }
            }
            catch (Exception Ex)
            {
                return Json(new { isValid = false, redirectUrl = "/#/Identity/Login/Login" });
            }
        }
    }
}