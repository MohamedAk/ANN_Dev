using Anzu.AnnPortal.Business.API.Core;
using Anzu.AnnPortal.Common.Model.Portal;
using Anzu.AnnPortal.Data.EntityManager;
using Anzu.AnnPortal.Data.Model;
using Anzu.AnnPortal.Data.Repository;
using AutoMapper;
using Core;
using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Data.Objects.SqlClient;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Anzu.AnnPortal.Business.Core.Core
{
    /// <summary>
    /// 
    /// </summary>
    public class MetadataService : BaseService, IMetadataService
    {
        /// <summary>
        /// The _repository
        /// </summary>
        private IRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="MetadataService"/> class.
        /// </summary>
        public MetadataService()
        {
            _repository = new BaseRepository();
        }

        /// <summary>
        /// Gets the procedures for level.
        /// </summary>
        /// <param name="levelId">The level identifier.</param>
        /// <returns></returns>
        public ICollection<ProcedureLevelDTO> GetProcedureLevel(int levelId)
        {
            try
            {
                BaseRepository repository = new BaseRepository();
                var results = repository.Find<ProcedureLevel>(p => p.LevelNumber == levelId && p.RecordStatusId == (int)(Anzu.AnnPortal.Common.Model.Enum.RecordStatus.Active));
                return Mapper.Map<List<ProcedureLevelDTO>>(results);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Get the selected procedure
        /// </summary>
        /// <param name="procedureId">The procedure ID</param>
        /// <returns></returns>
        public ProcedureDTO GetProcedureById(int procedureId)
        {
            BaseRepository repository = new BaseRepository();
            var resultSet = repository.Find<Procedure>(u => u.Id == procedureId && u.RecordStatusId == (int)(Anzu.AnnPortal.Common.Model.Enum.RecordStatus.Active)).FirstOrDefault();
            ProcedureDTO procedure = Mapper.Map<ProcedureDTO>(resultSet);
            return procedure;
        }

        /// <summary>
        /// Get the related procudure associted for a procedure
        /// </summary>
        /// <param name="procedureId">The procedure Id</param>
        /// <returns></returns>
        //public ICollection<RelatedProcedureDTO> GetRelatedProcedureByProcedureId(int procedureId)
        public ICollection<ProcedureDTO> GetRelatedProcedureByProcedureId(int procedureId)
        {
            List<ProcedureDTO> procedureList = new List<ProcedureDTO>();

            BaseRepository repository = new BaseRepository();
            //var resultSet = repository.Find<RelatedProcedure>(u => u.ProcedureId == procedureId && u.RecordStatusId == (int)(Anzu.AnnPortal.Common.Model.Enum.RecordStatus.Active));
            var resultSet = repository.Find<RelatedProcedure>(u => u.ProcedureId == procedureId && u.RecordStatusId == (int)(Anzu.AnnPortal.Common.Model.Enum.RecordStatus.Active));
            List<RelatedProcedureDTO> relatedList = new List<RelatedProcedureDTO>();
            relatedList = Mapper.Map<List<RelatedProcedureDTO>>(resultSet);

            foreach (var item in relatedList)
            {
                procedureList.Add(GetProcedureById((int)item.RelatedProcedureId));

            }
            return procedureList;

            //return Mapper.Map<List<RelatedProcedureDTO>>(resultSet);

            //return Mapper.Map<List<ProcedureDTO>>(resultSet);
        }

        /// <summary>
        /// Inserts the specified instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        public ProcedureLevelDTO Insert(ProcedureLevelDTO instance)
        {
            ProcedureLevel dbInstance = Mapper.Map<ProcedureLevel>(instance);
            dbInstance = _repository.Insert<ProcedureLevel>(dbInstance);
            var result = Mapper.Map<ProcedureLevelDTO>(dbInstance);
            return result;
        }

        /// <summary>
        /// Updates the specified instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        public ProcedureLevelDTO Update(ProcedureLevelDTO instance)
        {
            ProcedureLevel dbInstance = Mapper.Map<ProcedureLevel>(instance);
            dbInstance = _repository.Update<ProcedureLevel>(dbInstance);
            var result = Mapper.Map<ProcedureLevelDTO>(dbInstance);
            return result;
        }

        /// <summary>
        /// Gets the product list.
        /// </summary>
        /// <returns></returns>
        public ICollection<BrestImplantDTO> GetBreastImplants()
        {
            BaseRepository repository = new BaseRepository();
            var resultSet = repository.Find<BrestImplant>(u => u.RecordStatusId == (int)(Anzu.AnnPortal.Common.Model.Enum.RecordStatus.Active)).OrderByDescending(i => i.Id);
            List<BrestImplantDTO> products = Mapper.Map<List<BrestImplantDTO>>(resultSet);
            return products;
        }

        /// <summary>
        /// Inserts the specified instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        public BrestImplantDTO Insert(BrestImplantDTO instance)
        {
            BrestImplant dbInstance = Mapper.Map<BrestImplant>(instance);
            dbInstance = _repository.Insert<BrestImplant>(dbInstance);
            var result = Mapper.Map<BrestImplantDTO>(dbInstance);
            return result;
        }

        /// <summary>
        /// Updates the specified instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        public BrestImplantDTO Update(BrestImplantDTO instance)
        {
            BrestImplant dbInstance = Mapper.Map<BrestImplant>(instance);
            dbInstance = _repository.Update<BrestImplant>(dbInstance);
            var result = Mapper.Map<BrestImplantDTO>(dbInstance);
            return result;
        }

        /// <summary>
        /// Procedures the level filter by text.
        /// </summary>
        /// <param name="levelId">The level identifier.</param>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        public List<ProcedureLevelDTO> ProcedureLevelFilterByText(long levelId, string filter)
        {
            try
            {
                BaseRepository repository = new BaseRepository();
                List<ProcedureLevelDTO> ProcedureLevelDTO = new List<ProcedureLevelDTO>();
                IEnumerable<ProcedureLevel> procedureLevel;
                if (String.IsNullOrEmpty(filter))
                {
                    procedureLevel = repository.GetAll<ProcedureLevel>().Where(e => e.LevelNumber == levelId && e.RecordStatusId == (int)Anzu.AnnPortal.Common.Model.Enum.RecordStatus.Active).ToList().Take(10);

                }
                else
                {
                    procedureLevel = repository.GetAll<ProcedureLevel>().Where(e => e.RecordStatusId == (int)Anzu.AnnPortal.Common.Model.Enum.RecordStatus.Active && e.LevelNumber == levelId && e.Name.ToLower().StartsWith(filter)).ToList();
                }

                ProcedureLevelDTO = Mapper.Map<List<ProcedureLevelDTO>>(procedureLevel);

                return ProcedureLevelDTO;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<ProcedureLevelDTO> ProcedureLevelFilterByText(long levelId, string filter, long procedureId)
        {
            try
            {
                BaseRepository repository = new BaseRepository();
                List<ProcedureLevelDTO> ProcedureLevelDTO = new List<ProcedureLevelDTO>();

                var procedureLevelNo = Convert.ToInt64(0);
                switch (levelId)
                {
                    case 1:
                        procedureLevelNo = Convert.ToInt64(repository.Find<Procedure>(p => p.Id == procedureId && p.RecordStatusId == (int)(Anzu.AnnPortal.Common.Model.Enum.RecordStatus.Active)).Select(p => p.ProcedureLevelOne).FirstOrDefault());
                        break;
                    case 2:
                        procedureLevelNo = Convert.ToInt64(repository.Find<Procedure>(p => p.Id == procedureId && p.RecordStatusId == (int)(Anzu.AnnPortal.Common.Model.Enum.RecordStatus.Active)).Select(p => p.ProcedureLevelTwo).FirstOrDefault());
                        break;
                    case 3:
                        procedureLevelNo = Convert.ToInt64(repository.Find<Procedure>(p => p.Id == procedureId && p.RecordStatusId == (int)(Anzu.AnnPortal.Common.Model.Enum.RecordStatus.Active)).Select(p => p.ProcedureLevelThree).FirstOrDefault());
                        break;
                    case 4:
                        procedureLevelNo = Convert.ToInt64(repository.Find<Procedure>(p => p.Id == procedureId && p.RecordStatusId == (int)(Anzu.AnnPortal.Common.Model.Enum.RecordStatus.Active)).Select(p => p.ProcedureLevelFour).FirstOrDefault());
                        break;
                    default:
                        break;
                }

                //procedureLevelNo =  repository.Find<Procedure>(p => p.Id == procedureId && p.RecordStatusId == (int)(Anzu.AnnPortal.Common.Model.Enum.RecordStatus.Active)).Select(p => p.ProcedureLevelTwo).FirstOrDefault();

                IEnumerable<ProcedureLevel> tempProcedureLevel = repository.Find<ProcedureLevel>(p => p.Id == procedureLevelNo && p.LevelNumber == levelId && p.RecordStatusId == (int)(Anzu.AnnPortal.Common.Model.Enum.RecordStatus.Active));

                IEnumerable<ProcedureLevel> procedureLevel;
                if (String.IsNullOrEmpty(filter))
                {
                    procedureLevel = repository.GetAll<ProcedureLevel>().Where(e => e.LevelNumber == levelId && e.RecordStatusId == (int)Anzu.AnnPortal.Common.Model.Enum.RecordStatus.Active).ToList().Take(10);
                    //procedureLevel = repository.GetAll<ProcedureLevel>().Where(e => e.LevelNumber == levelId && e.RecordStatusId == (int)Anzu.AnnPortal.Common.Model.Enum.RecordStatus.Active).ToList().Take(9);

                    if (tempProcedureLevel != null && tempProcedureLevel.Count() != 0)
                    {
                        if (!procedureLevel.Contains(tempProcedureLevel.FirstOrDefault()))
                        {
                            procedureLevel = procedureLevel.Concat(tempProcedureLevel);
                        }
                    }
                }
                else
                {
                    procedureLevel = repository.GetAll<ProcedureLevel>().Where(e => e.RecordStatusId == (int)Anzu.AnnPortal.Common.Model.Enum.RecordStatus.Active && e.LevelNumber == levelId && e.Name.ToLower().StartsWith(filter)).ToList();
                }

                ProcedureLevelDTO = Mapper.Map<List<ProcedureLevelDTO>>(procedureLevel);

                return ProcedureLevelDTO;
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// Procedures the filter by text.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        public List<ProcedureDTO> ProcedureFilterByText(string filter)
        {
            try
            {
                BaseRepository repository = new BaseRepository();
                List<ProcedureDTO> procedureDTO = new List<ProcedureDTO>();
                List<Procedure> procedure = new List<Procedure>();

                if (string.IsNullOrEmpty(filter))
                {
                    procedure = repository.Take<Procedure>(10, false).Distinct().OrderBy(i => i.Name).ToList();
                }
                else
                {
                    procedure = repository.GetAll<Procedure>().Where(p => p.Name.ToLower().StartsWith(filter)).Distinct().OrderBy(i => i.Name).ToList();
                }

                procedureDTO = Mapper.Map<List<ProcedureDTO>>(procedure);

                return procedureDTO;
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
        public ICollection<ProcedureDTO> GetAllProcedureLevels()
        {
            try
            {
                BaseRepository repository = new BaseRepository();
                IEnumerable<Procedure> results = repository.GetAll<Procedure>().Where(e => e.RecordStatusId == (int)Anzu.AnnPortal.Common.Model.Enum.RecordStatus.Active).OrderByDescending(i => i.Id).ToList();
                return Mapper.Map<List<ProcedureDTO>>(results);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Gets the services from staging by emr identifier.
        /// </summary>
        /// <param name="take">The take.</param>
        /// <param name="skip">The skip.</param>
        /// <param name="page">The page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="emrId">The emr identifier.</param>
        /// <returns></returns>
        //public ICollection<ServiceDTO> GetServicesFromStagingByEMRId(string emrId)
        //{
        //    try
        //    {
        //        BaseRepository repository = new BaseRepository();
        //        var results = repository.GetServicesFromStagingByEMRId(emrId);
        //        return Mapper.Map<List<ServiceDTO>>(results);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        public ICollection<PracticeProcedureDTO> GetServicesFromStagingByEMRIdToGrid(int take, int skip, int page, int pageSize, string emrId, out int recordCount, string sortOption = "", string searchOption = "", string searchText = "")
        {
            BaseRepository repository = new BaseRepository();
            var queryable = repository.GetServicesFromStagingByEMRId(emrId);

            PracticeService practiceService = new PracticeService();

            var practice = practiceService.GetPracticeByEMRId(emrId);

            // Filter data
            switch (searchOption)
            {
                case "1":
                    // EMR Service Name
                    if (!string.IsNullOrEmpty(searchText))
                    {
                        queryable = queryable.Where(x => x.ServiceName.ToUpper().Contains(searchText.ToUpper())).ToList();
                    }
                    break;
                case "2":
                    // Procedure Name
                    queryable = queryable.Where(x => x.ProcedureName != null && x.ProcedureName.ToUpper().Contains(searchText.ToUpper())).ToList();
                    break;
                case "3":
                    // Product Name
                    queryable = queryable.Where(x => x.ProductName != null && x.ProductName.ToUpper().Contains(searchText.ToUpper())).ToList();
                    break;
                case "4":
                    // Company Name
                    queryable = queryable.Where(x => x.CompanyName != null && x.CompanyName.ToUpper().Contains(searchText.ToUpper())).ToList();
                    break;
                default:
                    break;
            }

            // Sort By
            switch (sortOption)
            {
                case "1":
                    // EMR Service Name
                    queryable = queryable.OrderBy(x => x.ServiceName).ToList();
                    break;
                case "2":
                    // Procedure Name
                    queryable = queryable.OrderBy(x => x.ProcedureName == null ? "1" : "0").ThenBy(x => x.ProcedureName).ToList();
                    break;
                case "3":
                    // Product Name
                    queryable = queryable.OrderBy(x => x.ProductName == null ? "1" : "0").ThenBy(x => x.ProductName).ToList();
                    break;
                case "4":
                    // Status
                    queryable = queryable.OrderBy(x => x.IsDiscarded).ThenBy(x => x.ServiceName).ToList();
                    break;
                case "5":
                    // Unmapped
                    queryable = queryable.OrderBy(x => x.IsProductSale).ThenBy(x => x.ProcedureId).ToList();
                    break;
                case "6":
                    // Last updated
                    queryable = queryable.OrderByDescending(x => x.CreatedDate > x.ModifiedDate ? x.CreatedDate : x.ModifiedDate).ToList();
                    break;
                default:
                    break;
            }

            recordCount = queryable.Count;

            var result = queryable.Skip(skip).Take(take).ToList();

            List<PracticeProcedureDTO> procedureList = new List<PracticeProcedureDTO>();
            foreach (var item in result)
            {
                PracticeProcedureDTO procedure = new PracticeProcedureDTO();

                procedure.EMRServiceId = item.EMRServiceId;
                procedure.ServiceName = item.ServiceName;
                procedure.EmrProcedure = item.EMRServiceId.ToString();
                procedure.Id = item.Id ?? 0;
                procedure.PracticeId = item.PracticeId != null ? item.PracticeId : practice.Id;
                long? a = item.ProcedureId;
                procedure.ProcedureId = item.ProcedureId ?? null;

                procedure.Procedure = item.ProcedureId != null ? practiceService.GetProcedureById(item.ProcedureId.Value) : null;
                procedure.CompanyId = item.CompanyId;
                procedure.Company = item.CompanyId != null ? GetCompanyById(item.CompanyId.Value) : null;
                procedure.ProductTypeId = item.ProductTypeId;
                procedure.ProductType = item.ProductTypeId != null ? GetProductById(item.ProductTypeId.Value) : null;

                procedure.IsProductSale = item.IsProductSale ?? false;
                procedure.IsDiscarded = item.IsDiscarded;
                procedureList.Add(procedure);
            }

            return procedureList.ToList();
        }

        //public bool IsRelProcedureExists(long procedureId)
        //{
        //    BaseRepository repository = new BaseRepository();
        //    bool result = repository.Find<RelatedProcedure>(p => p.ProcedureId == procedureId && p.RecordStatusId == (int)(Anzu.AnnPortal.Common.Model.Enum.RecordStatus.Active)).Any();
        //    return result;
        //}


        /// <summary>
        /// Determines whether [is procedure level sequence unique] [the specified name].
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="procedureLevel1">The procedure level1.</param>
        /// <param name="procedureLevel2">The procedure level2.</param>
        /// <param name="procedureLevel3">The procedure level3.</param>
        /// <param name="procedureLevel4">The procedure level4.</param>
        /// <returns></returns>
        public bool IsDuplicateProcedureLevelSequence(string name, int? procedureLevel1, int? procedureLevel2, int? procedureLevel3, int? procedureLevel4)
        {
            BaseRepository repository = new BaseRepository();
            bool result = repository.Find<Procedure>(p => p.Name == name && p.ProcedureLevelOne == procedureLevel1 && p.ProcedureLevelTwo == procedureLevel2 && p.ProcedureLevelThree == procedureLevel3 && p.ProcedureLevelFour == procedureLevel4 && p.RecordStatusId == (int)(Anzu.AnnPortal.Common.Model.Enum.RecordStatus.Active)).Any();
            return result;
        }

        /// <summary>
        /// Inserts the specified instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        public ProcedureDTO Insert(ProcedureDTO instance)
        {
            Procedure dbInstance = Mapper.Map<Procedure>(instance);
            dbInstance = _repository.Insert<Procedure>(dbInstance);
            var result = Mapper.Map<ProcedureDTO>(dbInstance);
            return result;
        }

        public RelatedProcedureDTO Insert(RelatedProcedureDTO instance)
        {
            RelatedProcedure dbInstance = Mapper.Map<RelatedProcedure>(instance);
            dbInstance = _repository.Insert<RelatedProcedure>(dbInstance);
            var result = Mapper.Map<RelatedProcedureDTO>(dbInstance);
            return result;
        }

        public List<RelatedProcedureDTO> DeleteOnlyRelatedProcedure(long procedureId)
        {
            BaseRepository repository = new BaseRepository();
            var resultSet = repository.Find<RelatedProcedure>(u => u.ProcedureId == procedureId && u.RecordStatusId == (int)(Anzu.AnnPortal.Common.Model.Enum.RecordStatus.Active));
            resultSet = resultSet.Select(c => { c.RecordStatusId = (int)(Anzu.AnnPortal.Common.Model.Enum.RecordStatus.Delete); return c; }).ToList();

            repository.Delete<RelatedProcedure>(u => u.ProcedureId == procedureId);

            List<RelatedProcedureDTO> result = new List<RelatedProcedureDTO>();
            return result;
        }

        public List<RelatedProcedureDTO> DeleteRelatedProcedure(long procedureId)
        {
            BaseRepository repository = new BaseRepository();
            var resultSet = repository.Find<RelatedProcedure>(u => u.ProcedureId == procedureId && u.RecordStatusId == (int)(Anzu.AnnPortal.Common.Model.Enum.RecordStatus.Active));
            resultSet = resultSet.Select(c => { c.RecordStatusId = (int)(Anzu.AnnPortal.Common.Model.Enum.RecordStatus.Delete); return c; }).ToList();


            repository.Delete<RelatedProcedure>(u => u.ProcedureId == procedureId);
            repository.Delete<RelatedProcedure>(u => u.RelatedProcedureId == procedureId);

            List<RelatedProcedureDTO> result = new List<RelatedProcedureDTO>();
            return result;
            //var dbInstance = _repository.Update<RelatedProcedure>(resultSet);
            //var result = Mapper.Map<List<RelatedProcedureDTO>>(dbInstance);
            //return result;
        }

        public List<long> GetExistingRelatedProcedureIds(long procedureId)
        {
            BaseRepository repository = new BaseRepository();
            var resultSet = repository.Find<RelatedProcedure>(u => u.ProcedureId == procedureId && u.RecordStatusId == (int)(Anzu.AnnPortal.Common.Model.Enum.RecordStatus.Active)).Select(a => a.RelatedProcedureId);
            return resultSet.ToList();

        }

        public bool GetBreastImplantState(long procedureId)
        {
            BaseRepository repository = new BaseRepository();
            bool practiceProductFlag = false;
            practiceProductFlag = repository.Find<Procedure>(u => u.Id == procedureId && u.RecordStatusId == (int)(Anzu.AnnPortal.Common.Model.Enum.RecordStatus.Active)).Select(u => u.PracticeProductFlag).FirstOrDefault();
            return practiceProductFlag;
        }


        public bool AddPracticeEditInfoOnProcedureChange(int procedureId, string userName)
        {

            DateTime todayMidnight = DateTime.Today;
            DateTime tommorowMidnight = DateTime.Today.AddDays(1);

            BaseRepository repository = new BaseRepository();
            //GetPracticeId in PracticeProcedures
            IEnumerable<long?> practiceIds = GetPracticeProceduresByProcedureId(procedureId);
            foreach (var item in practiceIds)
            {
                //var currentRecord = repository.Find<PracticeEditInformation>(u => u.PracticeId == item && (u.EMRMappingUpdatedTime >= todayMidnight && u.EMRMappingUpdatedTime <= tommorowMidnight) && u.RecordStatusId == (int)(Anzu.AnnPortal.Common.Model.Enum.RecordStatus.Active)).FirstOrDefault();
                var currentRecord = repository.Find<PracticeEditInformation>(u => u.PracticeId == item && (u.IsCubeUpdated == false || u.IsCubeUpdated == null) && (u.EMRMappingUpdatedTime >= todayMidnight && u.EMRMappingUpdatedTime <= tommorowMidnight) && u.RecordStatusId == (int)(Anzu.AnnPortal.Common.Model.Enum.RecordStatus.Active)).FirstOrDefault();

                if (currentRecord == null)
                {
                    PracticeEditInformation practiceEditInformation = new PracticeEditInformation();
                    practiceEditInformation.PracticeId = Convert.ToInt32(item);
                    practiceEditInformation.EmrId = GetEmrId(Convert.ToInt32(item));
                    practiceEditInformation.EMRMappingUpdatedTime = DateTime.Today;
                    practiceEditInformation.IsBreastImplantUpdated = true;
                    practiceEditInformation.IsEMRMappingUpdated = true;

                    PracticeService service = new PracticeService();
                    service.CreatePracticeEditInformation(practiceEditInformation, userName);
                }
            }
            return true;
        }

        public ProcedureDTO DeleteProcedure(long id, string userName)
        {
            BaseRepository repository = new BaseRepository();
            var existingResult = repository.Find<Procedure>(u => u.Id == id && u.RecordStatusId == (int)(Anzu.AnnPortal.Common.Model.Enum.RecordStatus.Active)).FirstOrDefault();
            existingResult.RecordStatusId = (int)(Anzu.AnnPortal.Common.Model.Enum.RecordStatus.Delete);
            existingResult.ModifiedBy = userName;
            existingResult.ModifiedDate = DateTime.Today;

            var dbInstance = _repository.Update<Procedure>(existingResult);
            var result = Mapper.Map<ProcedureDTO>(dbInstance);

            DateTime todayMidnight = DateTime.Today;
            DateTime tommorowMidnight = DateTime.Today.AddDays(1);

            List<RelatedProcedureDTO> deletedRelatedProcedures = DeleteRelatedProcedure(id);

            //GetPracticeId in PracticeProcedures
            IEnumerable<long?> practiceIds = GetPracticeProceduresByProcedureId(id);

            //Delete records from PracticeProcedure table
            DeletePracticeProceduresByProcedureId(id);

            foreach (var item in practiceIds)
            {
                //var currentRecord = repository.Find<PracticeEditInformation>(u => u.PracticeId == item && (u.EMRMappingUpdatedTime >= todayMidnight && u.EMRMappingUpdatedTime <= tommorowMidnight) && u.RecordStatusId == (int)(Anzu.AnnPortal.Common.Model.Enum.RecordStatus.Active)).FirstOrDefault();
                var currentRecord = repository.Find<PracticeEditInformation>(u => u.PracticeId == item && (u.IsCubeUpdated == false || u.IsCubeUpdated == null) && (u.EMRMappingUpdatedTime >= todayMidnight && u.EMRMappingUpdatedTime <= tommorowMidnight) && u.RecordStatusId == (int)(Anzu.AnnPortal.Common.Model.Enum.RecordStatus.Active)).FirstOrDefault();

                if (currentRecord == null)
                {

                    PracticeEditInformation practiceEditInformation = new PracticeEditInformation();
                    practiceEditInformation.PracticeId = Convert.ToInt32(item);
                    practiceEditInformation.EmrId = GetEmrId(Convert.ToInt32(item));
                    practiceEditInformation.EMRMappingUpdatedTime = DateTime.Today;
                    practiceEditInformation.IsBreastImplantUpdated = true;
                    practiceEditInformation.IsEMRMappingUpdated = true;

                    PracticeService service = new PracticeService();
                    service.CreatePracticeEditInformation(practiceEditInformation, userName);
                }
            }

            return result;
        }

        public IEnumerable<long?> GetPracticeProceduresByProcedureId(long procedureId)
        {
            BaseRepository repository = new BaseRepository();
            var resultSet = repository.Find<PracticeProcedure>(u => u.ProcedureId == procedureId && u.IsDiscarded != true && u.RecordStatusId == (int)(Anzu.AnnPortal.Common.Model.Enum.RecordStatus.Active)).Select(a => a.PracticeId).Distinct().ToList();

            return resultSet;
        }

        public IEnumerable<long?> DeletePracticeProceduresByProcedureId(long procedureId)
        {
            BaseRepository repository = new BaseRepository();
            IEnumerable<long?> practiceIds = GetPracticeProceduresByProcedureId(procedureId);
            foreach (var item in practiceIds)
            {
                repository.Delete<PracticeProcedure>(u => u.PracticeId == item && u.ProcedureId == procedureId);
            }

            return practiceIds;
        }

        public string GetEmrId(long id)
        {
            BaseRepository repositoy = new BaseRepository();
            var resultSet = repositoy.Find<Practice>(u => u.Id == id && u.RecordStatusId == (int)(Anzu.AnnPortal.Common.Model.Enum.RecordStatus.Active)).Select(a => a.EmrId).FirstOrDefault();
            return resultSet;
        }

        /// <summary>
        /// Updates the specified instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        public ProcedureDTO Update(ProcedureDTO instance)
        {
            Procedure dbInstance = Mapper.Map<Procedure>(instance);
            dbInstance = _repository.Update<Procedure>(dbInstance);
            var result = Mapper.Map<ProcedureDTO>(dbInstance);
            return result;
        }

        /// <summary>
        /// Gets the companies.
        /// </summary>
        /// <returns></returns>
        public List<CompanyDTO> GetCompanies()
        {
            BaseRepository repository = new BaseRepository();
            var resultSet = repository.Find<Company>(u => u.RecordStatusId == (int)(Anzu.AnnPortal.Common.Model.Enum.RecordStatus.Active)).OrderByDescending(i => i.Id);
            List<CompanyDTO> products = Mapper.Map<List<CompanyDTO>>(resultSet);
            return products;
        }

        public CompanyDTO GetCompanyById(long companyId)
        {
            BaseRepository repository = new BaseRepository();
            var resultSet = repository.Find<Company>(u => u.Id == companyId).FirstOrDefault();
            CompanyDTO company = Mapper.Map<CompanyDTO>(resultSet);
            return company;
        }

        /// <summary>
        /// Inserts the specified instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        public CompanyDTO Insert(CompanyDTO instance)
        {
            Company dbInstance = Mapper.Map<Company>(instance);
            dbInstance = _repository.Insert<Company>(dbInstance);
            var result = Mapper.Map<CompanyDTO>(dbInstance);
            return result;
        }

        /// <summary>
        /// Updates the specified instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        public CompanyDTO Update(CompanyDTO instance)
        {
            Company dbInstance = Mapper.Map<Company>(instance);
            dbInstance = _repository.Update<Company>(dbInstance);
            var result = Mapper.Map<CompanyDTO>(dbInstance);
            return result;
        }

        /// <summary>
        /// Gets the products.
        /// </summary>
        /// <returns></returns>
        public List<ProductTypeDTO> GetProducts()
        {
            BaseRepository repository = new BaseRepository();
            var resultSet = repository.Find<ProductType>(u => u.RecordStatusId == (int)(Anzu.AnnPortal.Common.Model.Enum.RecordStatus.Active)).OrderByDescending(i => i.Id);
            List<ProductTypeDTO> products = Mapper.Map<List<ProductTypeDTO>>(resultSet);
            return products;
        }

        public ProductTypeDTO GetProductById(long prodcutId)
        {
            BaseRepository repository = new BaseRepository();
            var resultSet = repository.Find<ProductType>(u => u.Id == prodcutId).FirstOrDefault();
            ProductTypeDTO product = Mapper.Map<ProductTypeDTO>(resultSet);
            return product;
        }

        /// <summary>
        /// Inserts the specified instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        public ProductTypeDTO Insert(ProductTypeDTO instance)
        {
            ProductType dbInstance = Mapper.Map<ProductType>(instance);
            dbInstance = _repository.Insert<ProductType>(dbInstance);
            var result = Mapper.Map<ProductTypeDTO>(dbInstance);
            return result;
        }

        /// <summary>
        /// Updates the specified instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        public ProductTypeDTO Update(ProductTypeDTO instance)
        {
            ProductType dbInstance = Mapper.Map<ProductType>(instance);
            dbInstance = _repository.Update<ProductType>(dbInstance);
            var result = Mapper.Map<ProductTypeDTO>(dbInstance);
            return result;
        }

        /// <summary>
        /// Gets the procedure levels.
        /// </summary>
        /// <param name="take">The take.</param>
        /// <param name="skip">The skip.</param>
        /// <param name="page">The page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns></returns>
        public IEnumerable<ProcedureDTO> GetProcedureLevels(int take, int skip, int page, int pageSize, out int recordCount)
        {
            BaseRepository repository = new BaseRepository();
            var queryable = repository.GetAllQuery<Procedure>().Where(p => p.RecordStatusId == (int)(Anzu.AnnPortal.Common.Model.Enum.RecordStatus.Active));
            queryable = queryable.OrderByDescending(a => a.CreatedDate);
            recordCount = queryable.Count();
            queryable = queryable.Skip(skip).Take(take);

            IEnumerable<ProcedureDTO> result = Mapper.Map<IEnumerable<Procedure>, IEnumerable<ProcedureDTO>>(queryable.ToList());
            result = result.Select(c => { c.IsRelatedProcedureExists = IsRelatedProcedureExists(c.Id); return c; }).ToList();

            return result;
        }

        public bool IsRelatedProcedureExists(long procedureId)
        {
            BaseRepository repository = new BaseRepository();
            bool result = repository.Find<RelatedProcedure>(p => p.ProcedureId == procedureId && p.RecordStatusId == (int)(Anzu.AnnPortal.Common.Model.Enum.RecordStatus.Active)).Any();
            return result;
        }

        public IEnumerable<ProcedureDTO> GetProcedureLevelsFilter(int take, int skip, int page, int pageSize, string filter, out int recordCount, string sortOption = "", string searchOption = "", int procedureId = 0)
        {
            BaseRepository repository = new BaseRepository();
            var queryable = repository.GetAllQuery<Procedure>().Where(x => x.RecordStatusId == (int)(Anzu.AnnPortal.Common.Model.Enum.RecordStatus.Active));
            filter = (filter == "NA") ? "" : filter;

            // Filter data
            switch (searchOption)
            {
                case "1":
                    // Procedure Name
                    if (!string.IsNullOrEmpty(filter))
                    {
                        queryable = queryable.Where(x => x.Name.ToUpper().Contains(filter.ToUpper()));
                    }
                    break;
                case "2":
                    // Level 1                      
                    queryable = queryable.Where(x => x.ProcedureLevel1.Name.ToUpper().Contains(filter.ToUpper()));
                    break;
                case "3":
                    // Level 2                      
                    queryable = queryable.Where(x => x.ProcedureLevel2.Name.ToUpper().Contains(filter.ToUpper()));
                    break;
                case "4":
                    // Level 3                      
                    queryable = queryable.Where(x => x.ProcedureLevel3.Name.ToUpper().Contains(filter.ToUpper()));
                    break;
                case "5":
                    // Level 4                      
                    queryable = queryable.Where(x => x.ProcedureLevel4.Name.ToUpper().Contains(filter.ToUpper()));
                    break;
                default:
                    break;
            }

            // Sort data
            switch (sortOption)
            {
                case "1":
                    // Procedure Name
                    queryable = queryable.OrderBy(x => x.Name);
                    break;
                case "2":
                    // Breast Implant Use
                    queryable = queryable.OrderByDescending(x => x.PracticeProductFlag == true);
                    break;
                case "3":
                    // Related Procedures
                    queryable = queryable.OrderBy(x => x.Id);
                    break;
                case "4":
                    // Last Updated
                    queryable = queryable.OrderByDescending(x => x.ModifiedDate != null ? x.ModifiedDate : x.CreatedDate);
                    break;
                case "5":
                    // Level 1
                    // (x => x.ProcedureLevel2 != null ? x.ProcedureLevel2.Name : string.Empty).ThenBy(x => x.ProcedureLevel3 != null ? x.ProcedureLevel3.Name : string.Empty).ThenBy(x => x.ProcedureLevel4 != null ? x.ProcedureLevel4.Name : string.Empty);
                    queryable = queryable.OrderBy(x => x.ProcedureLevel1 != null ? x.ProcedureLevel1.Name : string.Empty).ThenByDescending(x => x.ModifiedDate != null ? x.ModifiedDate : x.CreatedDate);
                    break;
                case "6":
                    // Level 2
                    queryable = queryable.OrderBy(x => x.ProcedureLevel2 != null ? x.ProcedureLevel2.Name : string.Empty).ThenByDescending(x => x.ModifiedDate != null ? x.ModifiedDate : x.CreatedDate);
                    break;
                case "7":
                    // Level 3
                    queryable = queryable.OrderBy(x => x.ProcedureLevel3 != null ? x.ProcedureLevel3.Name : string.Empty).ThenByDescending(x => x.ModifiedDate != null ? x.ModifiedDate : x.CreatedDate);
                    break;
                case "8":
                    // Level 4
                    queryable = queryable.OrderBy(x => x.ProcedureLevel4 != null ? x.ProcedureLevel4.Name : string.Empty).ThenByDescending(x => x.ModifiedDate != null ? x.ModifiedDate : x.CreatedDate); ;
                    break;
                default:
                    break;
            }

            if (procedureId != 0)
            {
                queryable = queryable.Where(x => x.Id != procedureId);
                recordCount = queryable.Count();
                // queryable = queryable.Skip(skip).Take(take);
            }
            else
            {
                recordCount = queryable.Count();
                // queryable = queryable.Skip(skip).Take(take);
            }

            IEnumerable<ProcedureDTO> result = Mapper.Map<IEnumerable<Procedure>, IEnumerable<ProcedureDTO>>(queryable.ToList());

            // get related procedure data if sort option is 3, get data for all items
            if (sortOption == "3")
            {
                result = result.Select(c => { c.IsRelatedProcedureExists = IsRelatedProcedureExists(c.Id); return c; }).ToList();
                result = result.Where(x => x.IsRelatedProcedureExists == true);
            }



            // if sortoption equals 3, already has the related procedure data, only to the skipped list
            if (sortOption != "3")
            {
                result = result.Select(c => { c.IsRelatedProcedureExists = IsRelatedProcedureExists(c.Id); return c; }).ToList();
            }
            return result.Skip(skip).Take(take);
        }

        /// <summary>
        /// Determines whether [is procedure level unique] [the specified level number].
        /// </summary>
        /// <param name="levelNumber">The level number.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public bool IsProcedureLevelUnique(int levelNumber, string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                BaseRepository repository = new BaseRepository();
                bool result = repository.Find<ProcedureLevel>(p => p.LevelNumber == levelNumber && p.Name != null && p.Name.ToLower() == name.ToLower() && p.RecordStatusId == (int)(Anzu.AnnPortal.Common.Model.Enum.RecordStatus.Active)).Any();
                return result;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Determines whether [is brest implant unique] [the specified name].
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public bool IsBreastImplantUnique(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                BaseRepository repository = new BaseRepository();
                bool result = repository.Find<BrestImplant>(p => p.Name != null && p.Name.ToLower() == name.ToLower() && p.RecordStatusId == (int)(Anzu.AnnPortal.Common.Model.Enum.RecordStatus.Active)).Any();
                return result;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Determines whether [is product type unique] [the specified name].
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public bool IsProductTypeUnique(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                BaseRepository repository = new BaseRepository();
                bool result = repository.Find<ProductType>(p => p.Name != null && p.Name.ToLower() == name.ToLower() && p.RecordStatusId == (int)(Anzu.AnnPortal.Common.Model.Enum.RecordStatus.Active)).Any();
                return result;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Determines whether [is company unique] [the specified name].
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public bool IsCompanyUnique(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                BaseRepository repository = new BaseRepository();
                bool result = repository.Find<Company>(p => p.Name != null && p.Name.ToLower() == name.ToLower() && p.RecordStatusId == (int)(Anzu.AnnPortal.Common.Model.Enum.RecordStatus.Active)).Any();
                return result;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Determines the SQL job status
        /// </summary>
        /// <returns></returns>
        public List<SqlJobDTO> GetSqlJobStatus()
        {
            try
            {
                BaseRepository repository = new BaseRepository();
                var data = repository.GetSqlJobStatus();

                return data;
            }
            catch (Exception)
            {
                return new List<SqlJobDTO>();
            }
        }
    }
}