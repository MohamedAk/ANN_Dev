using Anzu.AnnPortal.Business.API.Core;
using Anzu.AnnPortal.Common.IocContainer;
using Anzu.AnnPortal.Common.Model.Common;
using Anzu.AnnPortal.Common.Model.Enum;
using Anzu.AnnPortal.Common.Model.Portal;
using Anzu.AnnPortal.Common.Notification;
using Anzu.AnnPortal.Data.EntityManager;
using Anzu.AnnPortal.Data.Model;
using Anzu.AnnPortal.Data.Model.Common;
using Anzu.AnnPortal.Data.Repository;
using AutoMapper;
using Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Anzu.AnnPortal.Business.Core.Core
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="BaseService" />
    /// <seealso cref="Anzu.AnnPortal.Business.API.Core.IPracticeService" />
    public class PracticeService : BaseService, IPracticeService
    {
        /// <summary>
        /// The repository
        /// </summary>
        // private IRepository repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="PracticeService"/> class.
        /// </summary>
        public PracticeService()
        {
            // this.repository = IocContainer.Resolve<IRepository>();
        }


        /// <summary>
        /// Des the activate practice.
        /// </summary>
        /// <param name="practiceId">The practice identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public bool DeActivatePractice(long practiceId, string userName, bool deleteData = false, bool refreshCube = false)
        {
            BaseRepository repository = new BaseRepository();
            var practice = repository.Find<Practice>(p => p.Id == practiceId).FirstOrDefault();

            if (practice.IsActive)
            {
                practice.IsActive = false;
            }
            else
            {
                practice.IsActive = true;

                bool isImplantsExists = IsPracticeContainImplants(practiceId);
                if (isImplantsExists == false)
                {
                    PracticeBrestImplant implants = new PracticeBrestImplant();
                    implants.PracticeId = practiceId;
                    implants.BIId= Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["UnspecifiedBreastImplantId"]);
                    implants.FromDate = Convert.ToDateTime("2000-01-01 00:00:00.000");
                    implants.RecordStatusId = (int)(Anzu.AnnPortal.Common.Model.Enum.RecordStatus.Active);
                    implants.CreatedBy = userName;
                    implants.CreatedDate = DateTimeOffset.Now;

                    repository.Insert<PracticeBrestImplant>(implants);
                }
            }
            practice.ModifiedBy = userName;
            practice.ModifiedDate = DateTimeOffset.Now;
            var result = repository.Update<Practice>(practice);

            // log to PracticeActivationLog table
            var tempLog = new PracticeActivationLog();
            tempLog.EmrId = practice.EmrId;
            tempLog.IsActivated = practice.IsActive;
            tempLog.IsDataDeleted = deleteData;
            tempLog.UpdatedDate = DateTime.Now;
            tempLog.UserId = userName;
            tempLog.RecordStatusId = (int)(Anzu.AnnPortal.Common.Model.Enum.RecordStatus.Active);
            tempLog.CreatedDate = DateTime.Now;
            tempLog.CreatedBy = userName;
            //repository.Insert<PracticeActivationLog>(tempLog);

            var updatePEITable = false;

            if (!practice.IsActive)
            {    //Deactivated
                if (deleteData && !refreshCube)
                {
                    updatePEITable = true;
                    tempLog.IsRefreshLater = true;
                    tempLog.IsCubeRefreshed = false;
                }                
            }
            else
            {
                //Activated
                updatePEITable = true;
            }

            repository.Insert<PracticeActivationLog>(tempLog);

            if (updatePEITable)
            {
                var instance = this.GetPracticeEditInformation(practice.Id, practice.EmrId, DateTime.Today);
                if (instance == null)
                {
                    PracticeEditInformation practiceEditInformation = new PracticeEditInformation();
                    practiceEditInformation.PracticeId = practice.Id;
                    practiceEditInformation.EmrId = practice.EmrId;
                    practiceEditInformation.EMRMappingUpdatedTime = DateTime.Today;
                    practiceEditInformation.IsEMRMappingUpdated = true;
                    this.CreatePracticeEditInformation(practiceEditInformation, userName);
                }
                else
                {
                    instance.IsCubeUpdated = null;
                    this.UpdatePracticeEditInformation(instance, userName);
                }
            }

            return result.IsActive;
        }

        /// <summary>
        /// Creates the practice.
        /// </summary>
        /// <param name="practice">The practice.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public PracticeDTO CreatePractice(PracticeDTO practice, string userName)
        {
            BaseRepository repository = new BaseRepository();

            //Check practice name is already exist
            bool practiceExist = IsPracticeNameUnique(practice.Name);
            if (practiceExist)
            {
                return null;
            }

            //Adding practice
            Practice p = Mapper.Map<Practice>(practice);
            //
            if (p.State != null && p.StateId != null)
            {
                p.RegionId = GetRegionIdByState(p.StateId);
            }
            //

            //if (p.State != null && p.State.Region != null)
            //{
            //    p.RegionId = (int)p.State.Region.Id;
            //}

            p.CreatedBy = userName;
            p.CreatedDate = DateTime.Now;
            p.RecordStatusId = (int)(Anzu.AnnPortal.Common.Model.Enum.RecordStatus.Active);
            p.State = null;
            Practice result = repository.Insert<Practice>(p);

            if (practice.PracticeUserList != null && practice.PracticeUserList.Count() > 0)
            {
                //Adding users to practice
                // BaseRepository r2 = new BaseRepository();
                foreach (ExternalUserDTO eu in practice.PracticeUserList)
                {
                    //Skip adding same user to the practice

                    //Give error when adding already practice assigned user to the practice

                    //UserRole ur = repository.Find<UserRole>(role => role.Name == "PRACTICE_USER").SingleOrDefault();
                    //if (ur != null)
                    //{
                    //    User u = Mapper.Map<User>(eu);
                    //    u.PracticeId = result.Id;
                    //    u.CreatedBy = userName;
                    //    u.CreatedDate = DateTime.Now;
                    //    u.RecordStatusId = (int)(Anzu.AnnPortal.Common.Model.Enum.RecordStatus.Active);
                    //    u.UserRoleId = ur.Id;
                    //    r2.Insert<User>(u);
                    //}

                    User u = Mapper.Map<User>(eu);
                    u.RRUserId = eu.RRUserId;
                    u.PracticeId = result.Id;
                    u.CreatedBy = userName;
                    u.CreatedDate = DateTime.Now;
                    u.RecordStatusId = (int)(Anzu.AnnPortal.Common.Model.Enum.RecordStatus.Active);
                    u.UserRoleId = null;
                    repository.Insert<User>(u);
                }
            }

            if (practice.BrestImplants != null && practice.BrestImplants.Count() > 0)
            {
                BaseRepository r3 = new BaseRepository();
                //Adding products
                foreach (PracticeBrestImplantDTO bid in practice.BrestImplants)
                {
                    PracticeBrestImplant bi = Mapper.Map<PracticeBrestImplant>(bid);
                    bi.PracticeId = result.Id;
                    ///bi.Practice = result;
                    bi.RecordStatusId = (int)(Anzu.AnnPortal.Common.Model.Enum.RecordStatus.Active);
                    bi.CreatedBy = userName;
                    bi.CreatedDate = DateTime.Now;
                    bi.BIId = bid.BrestImplant.Id;
                    r3.Insert<PracticeBrestImplant>(bi);
                }
            }
            if (result != null)
            {
                return GetPracticeById(result.Id);
            }
            else
            {
                return null;
            }
        }

        public PracticeDTO UpdatePractice(PracticeDTO practice, string userName)
        {
            try
            {
                BaseRepository repository = new BaseRepository();
                Practice p = repository.Find<Practice>(r => r.Id == practice.Id).SingleOrDefault();

                p.AddressLine1 = practice.AddressLine1;
                p.AddressLine2 = practice.AddressLine2;
                p.City = practice.City;
                p.ContactNumber = practice.ContactNumber;
                p.ContactPerson = practice.ContactPerson;
                p.StateId = practice.State.Id;
                p.ZipCodeId = practice.ZipCode.Id;
                p.Name = practice.Name;
                if (p.State != null && p.State.Region != null)
                {
                    p.RegionId = (int)p.State.Region.Id;
                }
                p.ModifiedBy = userName;
                p.ModifiedDate = DateTime.Now;
                repository.Update<Practice>(p);

                if (practice.PracticeUserList.Any())
                {
                    //Adding users to practice
                    foreach (ExternalUserDTO eu in practice.PracticeUserList)
                    {
                        //update if exist
                        User user = repository.Find<User>(u => u.RRUserId == eu.RRUserId && u.RecordStatusId != 2).SingleOrDefault();
                        if (user != null)
                        {
                            user.RRUserId = eu.RRUserId;
                            user.RecordStatusId = eu.RecordStatusId;
                            user.ModifiedBy = userName;
                            user.PracticeId = p.Id;
                            user.ModifiedDate = DateTime.Now;
                            user.UserRolesDisplay = eu.UserRolesDisplay;
                            repository.Update<User>(user);
                        }
                        else
                        {
                            if (eu.RecordStatusId != 2)
                            {
                                User u = Mapper.Map<User>(eu);
                                u.RRUserId = eu.RRUserId;
                                u.PracticeId = p.Id;
                                u.CreatedBy = userName;
                                u.CreatedDate = DateTime.Now;
                                u.RecordStatusId = (int)(Anzu.AnnPortal.Common.Model.Enum.RecordStatus.Active);
                                u.UserRoleId = null;
                                u.UserRolesDisplay = eu.UserRolesDisplay;
                                repository.Insert<User>(u);
                            }
                            //UserRole ur = repository.Find<UserRole>(role => role.Name == "PRACTICE_USER").SingleOrDefault();
                            //if (ur != null)
                            //{
                            //    User u = Mapper.Map<User>(eu);
                            //    u.PracticeId = practice.Id;
                            //    u.CreatedBy = userName;
                            //    u.CreatedDate = DateTime.Now;
                            //    u.RecordStatusId = (int)(Anzu.AnnPortal.Common.Model.Enum.RecordStatus.Active);
                            //    u.UserRoleId = ur.Id;
                            //    repository.Insert<User>(u);
                            //}
                            //else
                            //{
                            //    throw new Exception("User Role is null");
                            //}
                        }
                    }
                }

                if (practice.BrestImplants != null && practice.BrestImplants.Count() > 0)
                {
                    var instance = this.GetPracticeEditInformation(practice.Id, practice.EmrId, DateTime.Today);

                    //Adding products
                    foreach (PracticeBrestImplantDTO bid in practice.BrestImplants)
                    {
                        PracticeEditInformation practiceEditInformation = new PracticeEditInformation();
                        practiceEditInformation.PracticeId = practice.Id;
                        practiceEditInformation.EmrId = practice.EmrId;
                        practiceEditInformation.EMRMappingUpdatedTime = DateTime.Today;

                        PracticeBrestImplant implant = repository.Find<PracticeBrestImplant>(u => u.Id == bid.Id).SingleOrDefault();
                        if (implant != null)
                        {
                            if (practice.IsActive && implant.RecordStatusId != bid.RecordStatusId)
                            {
                                if (instance == null)
                                {
                                    practiceEditInformation.IsBreastImplantUpdated = true;
                                    this.CreatePracticeEditInformation(practiceEditInformation, userName);
                                }
                                else
                                {
                                    instance.IsCubeUpdated = null;
                                    var updatedInstance = this.UpdatePracticeEditInformation(instance, userName);
                                }
                            }

                            implant.RecordStatusId = bid.RecordStatusId;
                            implant.ModifiedBy = userName;
                            implant.ModifiedDate = DateTime.Now;
                            //
                            if (practice.BrestImplants != null && practice.BrestImplants.Count() > 1 &&  implant.BIId == Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["UnspecifiedBreastImplantId"]))
                            {
                                implant.RecordStatusId = (int)(Anzu.AnnPortal.Common.Model.Enum.RecordStatus.Delete);
                            }
                            //
                            repository.Update<PracticeBrestImplant>(implant);

                        }
                        else
                        {
                            if (practice.IsActive)
                            {
                                if (instance == null)
                                {
                                    practiceEditInformation.IsBreastImplantUpdated = true;
                                    this.CreatePracticeEditInformation(practiceEditInformation, userName);
                                }
                                else
                                {
                                    instance.IsCubeUpdated = null;
                                    var updatedInstance = this.UpdatePracticeEditInformation(instance, userName);
                                }
                            }

                            if (bid.RecordStatusId != 2)
                            {
                                PracticeBrestImplant bi = Mapper.Map<PracticeBrestImplant>(bid);
                                bi.PracticeId = practice.Id;
                                bi.RecordStatusId = (int)(Anzu.AnnPortal.Common.Model.Enum.RecordStatus.Active);
                                bi.CreatedBy = userName;
                                bi.CreatedDate = DateTime.Now;
                                bi.BIId = bid.BrestImplant.Id;
                                //
                                if (!(practice.BrestImplants != null && practice.BrestImplants.Count() > 1 && bid.BrestImplant.Id == Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["UnspecifiedBreastImplantId"])))
                                {
                                    repository.Insert<PracticeBrestImplant>(bi);
                                }
                                //
                                //repository.Insert<PracticeBrestImplant>(bi);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return GetPracticeById(practice.Id);
        }

        public int? GetRegionIdByState(int? stateId)
        {
            int regionId = 0;
            try
            {
                BaseRepository repository = new BaseRepository();
                regionId =  (int)repository.Find<State>(p => p.Id == stateId && p.RecordStatusId == (int)(Anzu.AnnPortal.Common.Model.Enum.RecordStatus.Active)).Select(a => a.RegionId).FirstOrDefault();
                return regionId;
            }
            catch (Exception)
            {
                return regionId;
            }
        }

        public bool UpdatePracticeUser(string firstName, string lastName, string userId)
        {
            try
            {
                BaseRepository repository = new BaseRepository();
                foreach (var userItem in repository.Find<User>(x => x.RRUserId == userId))
                {
                    userItem.FirstName = firstName;
                    userItem.LastName = lastName;
                    repository.Update<User>(userItem);
                }
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// Gets this instance.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public ICollection<PracticeDTO> Practices()
        {
            BaseRepository repository = new BaseRepository();
            var results = repository.Find<Practice>(p => p.RecordStatusId == (int)(Anzu.AnnPortal.Common.Model.Enum.RecordStatus.Active));
            var practiceList = Mapper.Map<List<PracticeDTO>>(results);
            var returnList = new List<PracticeDTO>();
            foreach (var practice in practiceList)
            {
                practice.LastUpdatedDate = GetLastUpdatedDate(practice.EmrId);
                practice.HasData = !PracticeHasDataCheck(practice.EmrId);
                returnList.Add(practice);
            }
            return returnList;
        }

        public ICollection<PracticeDTO> SearchPractices(string searchOption, string filter)
        {
            BaseRepository repository = new BaseRepository();
            if (searchOption == "address")
            {
                filter = filter.Replace(" , ", ",").Replace(", ", ",");
            }
            var results = repository.Find<Practice>(p => p.RecordStatusId == (int)(Anzu.AnnPortal.Common.Model.Enum.RecordStatus.Active)
            && ((searchOption == "practiceId" && p.EmrId.ToUpper().Contains(filter))
            || (searchOption == "practiceName" && p.Name.ToUpper().Contains(filter))
            || (searchOption == "contactPerson" && p.ContactPerson.ToUpper().Contains(filter))
            || (searchOption == "address" && (p.AddressLine1 + "," + p.AddressLine2 + "," + p.City + "," + p.State.Name + "," + p.ZipCode.Code).ToUpper().Contains(filter.ToUpper()))
            || (searchOption == "city" && p.City.ToUpper().Contains(filter))
            || (searchOption == "state" && p.State.Name.ToUpper().Contains(filter))
            || (searchOption == "contactNo" && p.ContactNumber.ToUpper().Contains(filter))
            ));

            var practiceList = Mapper.Map<List<PracticeDTO>>(results);

            var returnList = new List<PracticeDTO>();
            foreach (var practice in practiceList)
            {
                practice.LastUpdatedDate = GetLastUpdatedDate(practice.EmrId);
                practice.HasData = !PracticeHasDataCheck(practice.EmrId);
                returnList.Add(practice);
            }
            return returnList;
        }

        // (CheckAddress(p, filter) == true
        //private bool CheckAddress(Practice p, string filter)
        //{
        //    var address = new StringBuilder();
        //    if (!string.IsNullOrEmpty(p.AddressLine1)) address.Append(p.AddressLine1 + ", ");
        //    if (!string.IsNullOrEmpty(p.AddressLine2)) address.Append(p.AddressLine2 + ", ");
        //    if (!string.IsNullOrEmpty(p.City)) address.Append(p.City + ", ");
        //    if (!string.IsNullOrEmpty(p.State.Name)) address.Append(p.State.Name + ", ");
        //    if (!string.IsNullOrEmpty(p.ZipCode.Code)) address.Append(p.ZipCode.Code + ", ");
        //    // p.AddressLine1.ToUpper().Contains(filter) || p.AddressLine2.ToUpper().Contains(filter))

        //    return address.ToString().Contains(filter);
        //}

        /// <summary>
        /// Gets the practice by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public PracticeDTO GetPracticeById(long id)
        {
            BaseRepository repository = new BaseRepository();
            var results = repository.Find<Practice>(p => p.Id == id).SingleOrDefault();
            var practice = Mapper.Map<PracticeDTO>(results);

            var userResult = repository.Find<User>(p => p.PracticeId == id && p.RecordStatusId == (int)(Anzu.AnnPortal.Common.Model.Enum.RecordStatus.Active));
            var practiceUsers = Mapper.Map<List<ExternalUserDTO>>(userResult);
            practice.PracticeUserList = practiceUsers;

            var brestImplantResult = repository.Find<PracticeBrestImplant>(b => b.PracticeId == id && b.RecordStatusId == (int)(Anzu.AnnPortal.Common.Model.Enum.RecordStatus.Active)).OrderByDescending(i => i.FromDate);
            var brestImplants = Mapper.Map<List<PracticeBrestImplantDTO>>(brestImplantResult);

            var zipcode = repository.Find<ZipCode>(z => z.Id == results.ZipCodeId).SingleOrDefault();
            var zipDTO = Mapper.Map<ZipCodeDTO>(zipcode);
            practice.ZipCode = zipDTO;

            var state = repository.Find<State>(s => s.Id == results.StateId).SingleOrDefault();
            var stateDTO = Mapper.Map<StateDTO>(state);
            practice.State = stateDTO;

            foreach (PracticeBrestImplantDTO pbi in brestImplants)
            {
                var biResult = repository.Find<BrestImplant>(b => b.Id == pbi.BIId).SingleOrDefault();
                var bi = Mapper.Map<BrestImplantDTO>(biResult);
                pbi.BrestImplant = bi;
            }

            practice.BrestImplants = brestImplants;
            return practice;
        }


        /// <summary>
        /// Gets this instance.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public System.Collections.Generic.ICollection<RegionDTO> Regions()
        {
            BaseRepository repository = new BaseRepository();
            var results = repository.Find<Region>(z => z.RecordStatusId == (int)(Anzu.AnnPortal.Common.Model.Enum.RecordStatus.Active));
            return Mapper.Map<List<RegionDTO>>(results);
        }

        /// <summary>
        /// Statuses this instance.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public System.Collections.Generic.ICollection<Common.Model.Common.StateDTO> States()
        {
            BaseRepository repository = new BaseRepository();
            var results = repository.Find<State>(z => z.RecordStatusId == (int)(Anzu.AnnPortal.Common.Model.Enum.RecordStatus.Active));
            return Mapper.Map<List<StateDTO>>(results);

        }

        /// <summary>
        /// Zips the codes.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public System.Collections.Generic.ICollection<ZipCodeDTO> ZipCodes()
        {
            BaseRepository repository = new BaseRepository();
            var results = repository.Find<ZipCode>(z => z.RecordStatusId == (int)(Anzu.AnnPortal.Common.Model.Enum.RecordStatus.Active));
            return Mapper.Map<ICollection<ZipCodeDTO>>(results);

        }

        /// <summary>
        /// Zips the code filter by text.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public List<ZipCodeDTO> ZipCodeFilterByText(string filter)
        {
            BaseRepository repository = new BaseRepository();
            List<ZipCodeDTO> ZipCodeDTO = new List<ZipCodeDTO>();
            List<ZipCode> zipCode = new List<ZipCode>();
            if (filter == "")
            {
                zipCode = repository.Take<ZipCode>(5, false).ToList();
            }
            else
            {
                // substring = filter;
                // if (substring[1].Count() > 2)
                // {
                zipCode = repository.GetAll<ZipCode>().Where(e => e.Code.StartsWith(filter)).ToList();
                //}
            }
            ZipCodeDTO = Mapper.Map<List<ZipCodeDTO>>(zipCode);

            return ZipCodeDTO;
        }

        /// <summary>
        /// Externals the users.
        /// </summary>
        /// <returns></returns>
        public List<ExternalUserDTO> Users()
        {
            BaseRepository repository = new BaseRepository();
            var resultSet = repository.Find<User>(u => u.RecordStatusId == (int)(Anzu.AnnPortal.Common.Model.Enum.RecordStatus.Active));
            List<ExternalUserDTO> userList = Mapper.Map<List<ExternalUserDTO>>(resultSet);
            return userList;

        }

        /// <summary>
        /// Validates EMR Status
        /// </summary>
        /// <param name="emrId"></param>
        /// <returns>true if has any updates, false is doesn't have any updates</returns>
        public bool ValidateEmrStatus(string emrId)
        {
            try
            {
                BaseRepository repository = new BaseRepository();
                return repository.ValidateEmrStatus(emrId).Count > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool PracticeReActivatedCheck(string emrId)
        {
            try
            {
                BaseRepository repository = new BaseRepository();
                return repository.PracticeReActivatedCheck(emrId) > 0;
            }
            catch (Exception Ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="emrList"></param>
        /// <returns></returns>
        public bool StartProcessCube(List<string> emrList, string userId, int timeout)
        {
            try
            {
                BaseRepository repository = new BaseRepository();

                var jobQueue = new JobQueue();
                jobQueue.UserId = userId;
                jobQueue.StartTime = DateTime.Now.AddMinutes(timeout);
                jobQueue.JobMode = 1;
                jobQueue.JobStatusId = 1;
                jobQueue.RecordStatusId = (int)Common.Model.Enum.RecordStatus.Active;
                jobQueue.CreatedDate = DateTime.Now;
                repository.Insert<JobQueue>(jobQueue);

                foreach (var emr in emrList)
                {
                    var tempEmr = GetPracticeByEMRId(emr);
                    var jobQueuePractice = new JobQueuePractice();
                    jobQueuePractice.JobQueueId = jobQueue.Id;
                    jobQueuePractice.EmrId = emr;
                    jobQueuePractice.RecordStatusId = (int)Common.Model.Enum.RecordStatus.Active;
                    jobQueuePractice.CreatedDate = DateTime.Now;
                    jobQueuePractice.PracticeId = tempEmr.Id;
                    repository.Insert<JobQueuePractice>(jobQueuePractice);
                }

                return true;
            }
            catch (Exception EX)
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="emrId"></param>
        /// <returns></returns>
        public bool PracticeHasDataCheck(string emrId)
        {
            try
            {
                BaseRepository repository = new BaseRepository();
                //var practiceStatus = repository.GetAll<PracticeActivationLog>().Where(x => x.EmrId == emrId).OrderByDescending(x => x.Id);
                //if (practiceStatus != null)
                //{
                //    var tempPracticeStatus = practiceStatus.FirstOrDefault();

                //    if (tempPracticeStatus.IsActivated)
                //    {
                //        // practice is activated
                //        return true;
                //    }
                //    else
                //    {
                //        // returns true if data delete flag is false
                //        return !(tempPracticeStatus.IsDataDeleted);
                //    }
                //}
                //else
                //{
                //    return false;
                //}

                var hasData = repository.CheckPracticeData(emrId);
                return (hasData == 1);
            }
            catch (Exception EX)
            {
                return false;
            }
        }

        public List<string> GetServiceEmailList()
        {
            try
            {
                BaseRepository repository = new BaseRepository();
                return repository.GetServiceEmailList();
            }
            catch (Exception)
            {
                return new List<string>();
            }
        }

        public bool CancelPendingJob(string userName, int timeOut = 5)
        {
            try
            {
                BaseRepository repository = new BaseRepository();
                var currentJobQueue = repository.GetAll<JobQueue>().Where(x => x.JobStatusId == 1);
                foreach (var jobs in currentJobQueue)
                {
                    jobs.JobStatusId = 3;
                    repository.Update<JobQueue>(jobs);

                    // Get job practices
                    var practicesByJobs = repository.GetAll<JobQueuePractice>().Where(x => x.JobQueueId == jobs.Id);
                    foreach (var practice in practicesByJobs)
                    {
                        // check for valida activate / deactivate scenario
                        var activationStatus = repository.GetAll<PracticeActivationLog>().Where(x => x.EmrId == practice.EmrId).OrderByDescending(x => x.Id).FirstOrDefault();
                        var fromTime = jobs.StartTime.AddMinutes(-(timeOut + 1)).LocalDateTime;
                        var toTime = jobs.StartTime.LocalDateTime;
                        var checkTime = activationStatus.UpdatedDate;
                        if (DateTime.Compare(fromTime, checkTime) < 0 && DateTime.Compare(checkTime, toTime) < 0)
                        {
                            // UPDATE PracticeEditInformation Table
                            var instance = this.GetPracticeEditInformation(practice.Id, practice.EmrId, DateTime.Today);
                            if (instance == null)
                            {
                                PracticeEditInformation practiceEditInformation = new PracticeEditInformation();
                                practiceEditInformation.PracticeId = practice.Id;
                                practiceEditInformation.EmrId = practice.EmrId;
                                practiceEditInformation.EMRMappingUpdatedTime = DateTime.Today;
                                practiceEditInformation.IsEMRMappingUpdated = true;
                                this.CreatePracticeEditInformation(practiceEditInformation, userName);
                            }
                            else
                            {
                                instance.IsCubeUpdated = null;
                                this.UpdatePracticeEditInformation(instance, userName);
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception EX)
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<JobStatusDTO> GetJobRunningStatus()
        {
            try
            {
                BaseRepository repository = new BaseRepository();
                return repository.GetJobRunningStatus();
            }
            catch (Exception)
            {
                return new List<JobStatusDTO>();
            }
        }

        public string GetLastUpdatedDate(string emrId)
        {
            try
            {
                BaseRepository repository = new BaseRepository();
                var jobQueuePractices = repository.GetAll<JobQueuePractice>().Where(x => x.EmrId == emrId).Select(x => x.JobQueue).Where(x => x.JobStatusId == 3).OrderByDescending(x => x.StartTime).FirstOrDefault();

                DateTimeOffset s = DateTimeOffset.Now.AddDays(-1);
                TimeSpan ts = new TimeSpan(02, 00, 0);
                s = s.Date + ts;

                if (jobQueuePractices != null)
                {
                    return (jobQueuePractices.EndTime != null) ? jobQueuePractices.EndTime.Value.ToString() : s.ToString();
                }
                else
                {
                    // validate practice
                    if (PracticeReActivatedCheck(emrId))
                    {
                        return s.ToString();
                    }
                    else
                    {
                        return "N/A";
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the product list.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public ICollection<BrestImplantDTO> GetProductList()
        {
            BaseRepository repository = new BaseRepository();
            var resultSet = repository.Find<BrestImplant>(u => u.RecordStatusId == (int)(Anzu.AnnPortal.Common.Model.Enum.RecordStatus.Active));
            List<BrestImplantDTO> products = Mapper.Map<List<BrestImplantDTO>>(resultSet);
            return products;
        }

        /// <summary>
        /// Determines whether [is date range correct] [the specified from date].
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <param name="practiceId">The practice identifier.</param>
        /// <returns></returns>
        public bool IsDateRangeCorrect(DateTime fromDate, DateTime toDate, long practiceId)
        {
            BaseRepository repository = new BaseRepository();
            bool result = repository.Find<PracticeBrestImplant>(p => p.PracticeId == practiceId && p.RecordStatusId == (int)(Anzu.AnnPortal.Common.Model.Enum.RecordStatus.Active) && (p.FromDate > fromDate && p.ToDate < fromDate) && (p.FromDate > toDate && p.ToDate < toDate)).Any();
            return result;
        }

        /// <summary>
        /// Determines whether [is practice name unique] [the specified practice name].
        /// </summary>
        /// <param name="practiceName">Name of the practice.</param>
        /// <returns></returns>
        public bool IsPracticeNameUnique(string practiceName)
        {
            if (practiceName != null)
            {
                BaseRepository repository = new BaseRepository();
                bool result = repository.Find<Practice>(p => p.Name != null && p.Name.ToLower() == practiceName.ToLower() && p.RecordStatusId == (int)(Anzu.AnnPortal.Common.Model.Enum.RecordStatus.Active)).Any();
                return result;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Adds the procedure level.
        /// </summary>
        /// <param name="level">The level.</param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public List<ProcedureLevelDTO> AddProcedureLevels(List<ProcedureLevelDTO> levels, string userName)
        {
            BaseRepository repository = new BaseRepository();
            foreach (ProcedureLevelDTO level in levels)
            {
                if (level != null)
                {
                    ProcedureLevel pl = Mapper.Map<ProcedureLevel>(level);
                    pl.CreatedBy = userName;
                    pl.CreatedDate = DateTime.Now;
                    pl.RecordStatusId = (int)(Anzu.AnnPortal.Common.Model.Enum.RecordStatus.Active);

                    ProcedureLevel result = repository.Insert<ProcedureLevel>(pl);

                }
            }

            return null;
        }

        /// <summary>
        /// Gets the user.
        /// </summary>
        /// <param name="rrUserId">The rr user identifier.</param>
        /// <returns></returns>
        public ExternalUserDTO GetUser(string rrUserId)
        {
            if (!string.IsNullOrEmpty(rrUserId))
            {
                BaseRepository repository = new BaseRepository();
                var user = repository.Find<User>(u => u.UserName == rrUserId && u.RecordStatusId == (int)(Anzu.AnnPortal.Common.Model.Enum.RecordStatus.Active)).SingleOrDefault();
                if (user != null)
                {
                    ExternalUserDTO eu = Mapper.Map<ExternalUserDTO>(user);
                    var practice = repository.Find<Practice>(p => p.Id == user.PracticeId).SingleOrDefault();
                    if (practice != null)
                    {
                        eu.Practice = Mapper.Map<PracticeDTO>(practice);
                    }

                    var role = repository.Find<UserRole>(r => r.Id == user.UserRoleId).SingleOrDefault();
                    if (role != null)
                    {
                        eu.UserRole = Mapper.Map<UserRoleDTO>(role);
                    }

                    return eu;

                }
            }
            return null;
        }

        public ExternalUserDTO GetUser(string rrUserId, string userName)
        {
            if (!string.IsNullOrEmpty(rrUserId))
            {
                BaseRepository repository = new BaseRepository();
                var user = repository.Find<User>(u => u.UserName == userName && u.RRUserId == rrUserId && u.RecordStatusId == (int)(Anzu.AnnPortal.Common.Model.Enum.RecordStatus.Active)).SingleOrDefault();
                if (user != null)
                {
                    ExternalUserDTO eu = Mapper.Map<ExternalUserDTO>(user);
                    var practice = repository.Find<Practice>(p => p.Id == user.PracticeId).SingleOrDefault();
                    if (practice != null)
                    {
                        eu.Practice = Mapper.Map<PracticeDTO>(practice);
                    }

                    var role = repository.Find<UserRole>(r => r.Id == user.UserRoleId).SingleOrDefault();
                    if (role != null)
                    {
                        eu.UserRole = Mapper.Map<UserRoleDTO>(role);
                    }

                    return eu;
                }
            }
            return null;
        }

        /// <summary>
        /// Determines whether [is user already added] [the specified user identifier].
        /// </summary>
        /// <param name="rrUserId">The user identifier.</param>
        /// <returns></returns>
        public bool IsUserAlreadyAdded(string rrUserId)
        {
            if (!string.IsNullOrEmpty(rrUserId))
            {
                BaseRepository repository = new BaseRepository();
                bool isAdded = repository.Find<User>(u => u.UserName.Trim() == rrUserId.Trim() && u.RecordStatusId == (int)(Anzu.AnnPortal.Common.Model.Enum.RecordStatus.Active)).Any();
                return isAdded;
            }
            return false;
        }

        /// <summary>
        /// Creates the procedure mapping.
        /// </summary>
        /// <param name="procedure">The procedure mapping.</param>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        public ProcedureDTO CreateProcedure(ProcedureDTO procedure, string userName)
        {
            try
            {
                if (procedure != null)
                {
                    BaseRepository repository = new BaseRepository();
                    Procedure p = Mapper.Map<Procedure>(procedure);
                    p.CreatedBy = userName;
                    p.CreatedDate = DateTime.Now;
                    p.RecordStatusId = (int)(Anzu.AnnPortal.Common.Model.Enum.RecordStatus.Active);

                    Procedure result = repository.Insert<Procedure>(p);
                    return Mapper.Map<ProcedureDTO>(result);

                }

                return null;
            }
            catch (Exception)
            {

                throw;
            }

        }

        /// <summary>
        /// Gets the procedure by text.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        public List<ProcedureDTO> GetProcedureByText(string filter)
        {
            try
            {
                BaseRepository repository = new BaseRepository();
                List<ProcedureDTO> procedureDTO = new List<ProcedureDTO>();
                List<Procedure> procedure = new List<Procedure>();

                if (string.IsNullOrEmpty(filter))
                {
                    procedure = repository.Take<Procedure>(5, false).ToList();
                }
                else
                {
                    procedure = repository.GetAll<Procedure>().Where(e => e.ProcedureLevel1.Name.StartsWith(filter) || e.ProcedureLevel2.Name.StartsWith(filter) || e.ProcedureLevel3.Name.StartsWith(filter) || e.ProcedureLevel4.Name.StartsWith(filter)).ToList();
                }

                procedureDTO = Mapper.Map<List<ProcedureDTO>>(procedure);
                return this.PopulateProcedureDisplayName(procedureDTO);
            }
            catch (Exception)
            {

                throw;
            }
        }


        /// <summary>
        /// Gets the procedure by text.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        public ProcedureDTO GetProcedureById(long procedureId)
        {
            try
            {
                BaseRepository repository = new BaseRepository();
                ProcedureDTO procedureDTO = new ProcedureDTO();
                Procedure procedure = new Procedure();

                procedure = repository.Find<Procedure>(p => p.Id == procedureId && p.RecordStatusId == (int)(Anzu.AnnPortal.Common.Model.Enum.RecordStatus.Active)).FirstOrDefault();
                //procedure = repository.Find<Procedure>(p => p.Id == procedureId).FirstOrDefault();
                if (procedure == null)
                {
                    return null;
                }
                procedureDTO = Mapper.Map<ProcedureDTO>(procedure);

                return this.PopulateProcedureDisplayName(procedureDTO);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Creates the asap user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">
        /// User already assigned to practice.
        /// or
        /// User is already an ASAP user.
        /// </exception>
        public ExternalUserDTO CreateASAPUser(ExternalUserDTO user, string userName)
        {
            try
            {
                ExternalUserDTO userResult = null;
                if (user != null)
                {
                    userResult = this.GetUserByUserName(user.UserName);
                }

                if (userResult != null)
                {
                    if (userResult.UserRoleId == (int)UserRoles.PRACTICE_USER)
                    {
                        userResult.ErrorMessage = "User already assigned to practice.";
                        return userResult;
                    }
                    else if (userResult.UserRoleId == (int)UserRoles.ASAP_USER)
                    {
                        userResult.ErrorMessage = "User is already an ASAP user.";
                        return userResult;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    if (user != null)
                    {
                        User u = Mapper.Map<User>(user);
                        BaseRepository repository = new BaseRepository();
                        u.CreatedBy = userName;
                        u.CreatedDate = DateTime.Now;
                        u.RecordStatusId = (int)(Anzu.AnnPortal.Common.Model.Enum.RecordStatus.Active);
                        u.UserRoleId = (int)UserRoles.ASAP_USER;
                        repository.Insert<User>(u);
                    }

                    return user;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the name of the user by user.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        public ExternalUserDTO GetUserByUserName(string userName)
        {
            try
            {
                if (!string.IsNullOrEmpty(userName))
                {
                    BaseRepository repository = new BaseRepository();
                    var user = repository.Find<User>(u => u.UserName.Trim() == userName.Trim() && u.RecordStatusId == (int)(Anzu.AnnPortal.Common.Model.Enum.RecordStatus.Active)).FirstOrDefault();
                    return Mapper.Map<ExternalUserDTO>(user);
                }

                return null;
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Gets the asap users.
        /// </summary>
        /// <returns></returns>
        public ICollection<ExternalUserDTO> GetASAPUsers()
        {
            try
            {
                BaseRepository repository = new BaseRepository();
                var results = repository.Find<User>(p => p.RecordStatusId == (int)(Anzu.AnnPortal.Common.Model.Enum.RecordStatus.Active) && p.UserRoleId == (int)UserRoles.ASAP_USER);
                var asapUsers = Mapper.Map<List<ExternalUserDTO>>(results);
                return asapUsers;
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Deactivates the asap user.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        public bool DeactivateASAPUser(long id, string userName)
        {
            try
            {
                BaseRepository repository = new BaseRepository();
                var practice = repository.Find<User>(p => p.Id == id).FirstOrDefault();
                practice.RecordStatusId = (int)Anzu.AnnPortal.Common.Model.Enum.RecordStatus.Delete;
                practice.ModifiedBy = userName;
                practice.ModifiedDate = DateTimeOffset.Now;
                var result = repository.Update<User>(practice);
                if (result != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the latest emr identifier.
        /// </summary>
        /// <returns></returns>
        public string GetLatestEMRId()
        {
            try
            {
                BaseRepository repository = new BaseRepository();
                var latestEmr = repository.GetAll<Practice>().OrderByDescending(i => i.Id).FirstOrDefault();
                long latestExistingEmrId = (latestEmr != null) ? latestEmr.Id : 0;

                string result = string.Empty;
                if (latestExistingEmrId >= 0 && latestExistingEmrId < 9)
                {
                    result = string.Format("EMR0{0}", latestExistingEmrId + 1);
                }
                else
                {
                    result = string.Format("EMR{0}", latestExistingEmrId + 1);
                }

                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Updates the user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        public ExternalUserDTO UpdateUser(ExternalUserDTO user)
        {

            try
            {
                BaseRepository repository = new BaseRepository();
                User dbInstance = Mapper.Map<User>(user);
                dbInstance = repository.Update<User>(dbInstance);
                var result = Mapper.Map<ExternalUserDTO>(dbInstance);
                return result;
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
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        public List<PracticeProcedureDTO> AddPracticeProcedures(List<PracticeProcedureDTO> procedures, string userName)
        {
            try
            {
                BaseRepository repository = new BaseRepository();
                List<PracticeProcedure> savedlist = new List<PracticeProcedure>();

                #region practice edit information
                if (procedures.Any())
                {
                    var instance = this.GetPracticeEditInformation((long)procedures.FirstOrDefault().PracticeId, procedures.FirstOrDefault().EmrId, DateTime.Today);
                    if (instance == null)
                    {
                        PracticeEditInformation practiceEditInformation = new PracticeEditInformation();
                        practiceEditInformation.PracticeId = (long)procedures.FirstOrDefault().PracticeId;
                        practiceEditInformation.EmrId = procedures.FirstOrDefault().EmrId;
                        practiceEditInformation.EMRMappingUpdatedTime = DateTime.Today;
                        practiceEditInformation.IsEMRMappingUpdated = true;
                        this.CreatePracticeEditInformation(practiceEditInformation, userName);
                    }
                    else
                    {
                        instance.IsCubeUpdated = null;
                        var updatedInstance = this.UpdatePracticeEditInformation(instance, userName);
                    }
                }
                #endregion

                foreach (PracticeProcedureDTO procedure in procedures)
                {
                    if (procedure != null)
                    {
                        PracticeProcedure practiceProcedure = Mapper.Map<PracticeProcedure>(procedure);

                        practiceProcedure.RecordStatusId = (int)(Anzu.AnnPortal.Common.Model.Enum.RecordStatus.Active);
                        practiceProcedure.Procedure = null;
                        practiceProcedure.ProductType = null;
                        practiceProcedure.Company = null;
                        if (procedure.Id > 0)
                        {
                            PracticeProcedure practiceProcedureToUpdate = null;
                            if (procedure.ProcedureId != null)
                            {
                                practiceProcedureToUpdate = GetPracticeProcedureByPracticeaAndEMRProcedure(practiceProcedure.PracticeId, procedure.EmrProcedure);
                                practiceProcedureToUpdate.IsProductSale = false;
                            }
                            else
                            {
                                practiceProcedureToUpdate = GetPracticeProcedureByPracticeaAndEMRProcedure(practiceProcedure.PracticeId, procedure.EmrProcedure);
                                if (procedure.IsDiscarded == true && procedure.IsProductSale != true)
                                {
                                    practiceProcedureToUpdate.IsProductSale = false;
                                }
                                else
                                {
                                    practiceProcedureToUpdate.IsProductSale = procedure.IsProductSale;
                                }

                            }

                            practiceProcedureToUpdate.ProcedureId = procedure.ProcedureId;
                            practiceProcedureToUpdate.CompanyId = procedure.CompanyId;
                            practiceProcedureToUpdate.ProductTypeId = procedure.ProductTypeId;
                            practiceProcedureToUpdate.IsDiscarded = procedure.IsDiscarded == true ? true : procedure.IsDiscarded = null;
                            practiceProcedureToUpdate.ModifiedBy = userName;
                            practiceProcedureToUpdate.ModifiedDate = DateTime.Now;
                            savedlist.Add(repository.Update<PracticeProcedure>(practiceProcedureToUpdate));
                        }
                        else
                        {
                            practiceProcedure.CreatedBy = userName;
                            practiceProcedure.CreatedDate = DateTime.Now;
                            savedlist.Add(repository.Insert<PracticeProcedure>(practiceProcedure));
                        }

                    }
                }

                return Mapper.Map<List<PracticeProcedureDTO>>(savedlist);
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        /// <summary>
        /// Gets the procedures.
        /// </summary>
        /// <returns></returns>
        public List<ProcedureDTO> GetProcedures()
        {
            try
            {
                BaseRepository repository = new BaseRepository();
                List<ProcedureDTO> procedureDTO = new List<ProcedureDTO>();
                List<Procedure> procedure = new List<Procedure>();


                procedure = repository.GetAllQuery<Procedure>().ToList();

                procedureDTO = Mapper.Map<List<ProcedureDTO>>(procedure);

                return this.PopulateProcedureDisplayName(procedureDTO);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Populates the display name of the procedure.
        /// </summary>
        /// <param name="procedures">The procedures.</param>
        /// <returns></returns>
        private List<ProcedureDTO> PopulateProcedureDisplayName(List<ProcedureDTO> procedures)
        {
            foreach (var item in procedures)
            {
                var level1 = item.ProcedureLevel1 != null ? item.ProcedureLevel1 : null;
                var level2 = item.ProcedureLevel2 != null ? item.ProcedureLevel2 : null;
                var level3 = item.ProcedureLevel3 != null ? item.ProcedureLevel3 : null;
                var level4 = item.ProcedureLevel4 != null ? item.ProcedureLevel4 : null;

                item.DisplayName = string.Format("{0} {1} {2} {3}",
                                    level1 != null ? string.Format("{0} {1}", "L1 -", level1.Name) : string.Empty,
                                    level2 != null ? string.Format("{0} {1}", "L2 -", level2.Name) : string.Empty,
                                    level3 != null ? string.Format("{0} {1}", "L3 -", level3.Name) : string.Empty,
                                    level4 != null ? string.Format("{0} {1}", "L4 -", level4.Name) : string.Empty);
            }

            return procedures;
        }

        /// <summary>
        /// Adds the practice procedure.
        /// </summary>
        /// <param name="procedure">The procedure.</param>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        public PracticeProcedureDTO AddPracticeProcedure(PracticeProcedureDTO procedure, string userName)
        {
            try
            {
                BaseRepository repository = new BaseRepository();

                if (procedure != null)
                {
                    PracticeProcedure practiceProcedure = Mapper.Map<PracticeProcedure>(procedure);
                    practiceProcedure.CreatedBy = userName;
                    practiceProcedure.CreatedDate = DateTime.Now;
                    practiceProcedure.RecordStatusId = (int)(Anzu.AnnPortal.Common.Model.Enum.RecordStatus.Active);
                    practiceProcedure.Procedure = null;
                    practiceProcedure.ProductType = null;
                    practiceProcedure.Company = null;
                    practiceProcedure = repository.Insert<PracticeProcedure>(practiceProcedure);
                    return Mapper.Map<PracticeProcedureDTO>(practiceProcedure);
                }
                else
                {
                    return null;
                }

            }
            catch (Exception)
            {

                throw;
            }

        }

        public PracticeDTO GetPracticeByEMRId(String emrId)
        {
            BaseRepository repository = new BaseRepository();
            var result = repository.Find<Practice>(p => p.EmrId == emrId && p.RecordStatusId == (int)(Anzu.AnnPortal.Common.Model.Enum.RecordStatus.Active)).FirstOrDefault();
            var practice = Mapper.Map<PracticeDTO>(result);

            return practice;
        }

        private ProcedureDTO PopulateProcedureDisplayName(ProcedureDTO procedure)
        {

            var level1 = procedure.ProcedureLevel1 != null ? procedure.ProcedureLevel1 : null;
            var level2 = procedure.ProcedureLevel2 != null ? procedure.ProcedureLevel2 : null;
            var level3 = procedure.ProcedureLevel3 != null ? procedure.ProcedureLevel3 : null;
            var level4 = procedure.ProcedureLevel4 != null ? procedure.ProcedureLevel4 : null;

            procedure.DisplayName = string.Format("{0} {1} {2} {3}",
                                    level1 != null ? string.Format("{0} {1}", "L1 -", level1.Name) : string.Empty,
                                    level2 != null ? string.Format("{0} {1}", "L2 -", level2.Name) : string.Empty,
                                    level3 != null ? string.Format("{0} {1}", "L3 -", level3.Name) : string.Empty,
                                    level4 != null ? string.Format("{0} {1}", "L4 -", level4.Name) : string.Empty);

            return procedure;
        }

        /// <summary>
        /// Gets the practice procedure by practicea and procedure.
        /// </summary>
        /// <param name="practiceId">The practice identifier.</param>
        /// <param name="procedureId">The procedure identifier.</param>
        /// <returns></returns>
        public PracticeProcedure GetPracticeProcedureByPracticeaAndEMRProcedure(long? practiceId, string emrProcedure)
        {
            try
            {
                BaseRepository repository = new BaseRepository();
                var practiceProcedure = repository.Find<PracticeProcedure>(u => u.PracticeId == practiceId && u.EmrProcedure == emrProcedure).FirstOrDefault();
                return practiceProcedure;
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Creates the practice edit information.
        /// </summary>
        /// <param name="practiceEditInformation">The practice edit information.</param>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        public PracticeEditInformation CreatePracticeEditInformation(PracticeEditInformation practiceEditInformation, string userName)
        {
            try
            {
                BaseRepository repository = new BaseRepository();

                if (practiceEditInformation != null)
                {
                    practiceEditInformation.CreatedBy = userName;
                    practiceEditInformation.CreatedDate = DateTime.Now;
                    practiceEditInformation.RecordStatusId = (int)(Anzu.AnnPortal.Common.Model.Enum.RecordStatus.Active);
                    var result = repository.Insert<PracticeEditInformation>(practiceEditInformation);
                    return result;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception)
            {

                throw;
            }

        }

        /// <summary>
        /// Gets the practice edit information.
        /// </summary>
        /// <param name="practiceEditInformation">The practice edit information.</param>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        public PracticeEditInformation GetPracticeEditInformation(long practiceId, string emrId, DateTime date)
        {
            try
            {
                BaseRepository repository = new BaseRepository();
                var result = repository.Find<PracticeEditInformation>(p => p.PracticeId == practiceId && p.EmrId == emrId && p.EMRMappingUpdatedTime == date).FirstOrDefault();
                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// Updates the practice edit information.
        /// </summary>
        /// <param name="practiceEditInformation">The practice edit information.</param>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        public PracticeEditInformation UpdatePracticeEditInformation(PracticeEditInformation practiceEditInformation, string userName)
        {
            try
            {
                BaseRepository repository = new BaseRepository();

                if (practiceEditInformation != null)
                {
                    practiceEditInformation.ModifiedBy = userName;
                    practiceEditInformation.ModifiedDate = DateTime.Now;
                    var result = repository.Update<PracticeEditInformation>(practiceEditInformation);
                    return result;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception)
            {

                throw;
            }

        }

        /// <summary>
        /// Determines whether [is user already assigned] [the specified rr user identifier].
        /// </summary>
        /// <param name="rrUserId">The rr user identifier.</param>
        /// <returns></returns>
        public ExternalUserDTO IsUserAlreadyAssigned(string rrUserId)
        {
            if (!string.IsNullOrEmpty(rrUserId))
            {
                BaseRepository repository = new BaseRepository();
                var user = repository.Find<User>(u => u.UserName.Trim() == rrUserId.Trim() && u.RecordStatusId == (int)(Anzu.AnnPortal.Common.Model.Enum.RecordStatus.Active)).FirstOrDefault();
                ExternalUserDTO tempUserDTO = Mapper.Map<ExternalUserDTO>(user);
                if (tempUserDTO != null)
                {
                    if (tempUserDTO.PracticeId != null)
                    {
                        tempUserDTO.IsUserAlreadyExsistInPractice = true;
                    }
                    else
                    {
                        tempUserDTO.IsUserASAPUser = true;
                    }
                }
                return tempUserDTO;
            }
            return null;
        }

        public bool EditPracticeBreastImplants(long PracticeId, string EmrId, string UserName)
        {
            PracticeEditInformation practiceEditInformation = new PracticeEditInformation();
            practiceEditInformation.PracticeId = PracticeId;
            practiceEditInformation.EmrId = EmrId;
            practiceEditInformation.EMRMappingUpdatedTime = DateTime.Today;
            practiceEditInformation.IsEMRMappingUpdated = true;
            this.CreatePracticeEditInformation(practiceEditInformation, UserName);

            if (practiceEditInformation.Id != 0)
                return true;
            else
                return false;
        }

        public bool RemovePracticeUser(string username)
        {
            BaseRepository repository = new BaseRepository();
            var user = repository.Find<User>(u => u.UserName.Trim() == username.Trim() && u.RecordStatusId == (int)(Anzu.AnnPortal.Common.Model.Enum.RecordStatus.Active)).FirstOrDefault();
            if (user != null)
            {
                user.RecordStatusId = (int)Anzu.AnnPortal.Common.Model.Enum.RecordStatus.Delete;
                repository.Update(user);
            }

            return true;
        }

        public int GetRecordStatusByRRUserId(string userId)
        {
            try
            {
                BaseRepository repository = new BaseRepository();
                var result = repository.Find<User>(x => x.RRUserId == userId).FirstOrDefault();
                return (result != null) ? result.RecordStatusId : 1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool IsPracticeContainImplants(long id)
        {
            bool isImplantExists = false;
            try
            {
                BaseRepository repository = new BaseRepository();
                isImplantExists =  repository.Find<PracticeBrestImplant>(u => u.PracticeId == id && u.RecordStatusId == (int)(Anzu.AnnPortal.Common.Model.Enum.RecordStatus.Active)).Any();
                return isImplantExists;
            }
            catch (Exception)
            {
                return isImplantExists;
            }
        }
    }
}