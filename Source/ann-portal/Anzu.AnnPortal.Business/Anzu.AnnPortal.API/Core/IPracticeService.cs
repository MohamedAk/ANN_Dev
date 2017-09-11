using Anzu.AnnPortal.Common.Model;
using Anzu.AnnPortal.Common.Model.Common;
using Anzu.AnnPortal.Common.Model.Portal;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace Anzu.AnnPortal.Business.API.Core
{
    /// <summary>
    /// 
    /// </summary>
    public interface IPracticeService
    {
        /// <summary>
        /// Des the activate practice.
        /// </summary>
        /// <param name="practiceId">The practice identifier.</param>
        /// <returns></returns>
        bool DeActivatePractice(long practiceId, string userName, bool deleteData = false, bool refreshCube = false);

        /// <summary>
        /// Creates the practice.
        /// </summary>
        /// <param name="practice">The practice.</param>
        /// <returns></returns>
        PracticeDTO CreatePractice(PracticeDTO practice, string userName);

        /// <summary>
        /// Updates the practice.
        /// </summary>
        /// <param name="practice">The practice.</param>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        PracticeDTO UpdatePractice(PracticeDTO practice, string userName);

        /// <summary>
        /// Gets this instance.
        /// </summary>
        /// <returns></returns>
        ICollection<PracticeDTO> Practices();

        /// <summary>
        /// Gets the practice by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        PracticeDTO GetPracticeById(long id);

        /// <summary>
        /// Gets this instance.
        /// </summary>
        /// <returns></returns>
        ICollection<RegionDTO> Regions();

        /// <summary>
        /// Stateses this instance.
        /// </summary>
        /// <returns></returns>
        ICollection<StateDTO> States();

        /// <summary>
        /// Zips the codes.
        /// </summary>
        /// <returns></returns>
        ICollection<ZipCodeDTO> ZipCodes();

        /// <summary>
        /// Zips the code filter by text.
        /// </summary>
        /// <returns></returns>
        List<ZipCodeDTO> ZipCodeFilterByText(string filter);

        /// <summary>
        /// Gets all users.
        /// </summary>
        /// <returns></returns>
        List<ExternalUserDTO> Users();

        /// <summary>
        /// Gets the product list.
        /// </summary>
        /// <returns></returns>
        ICollection<BrestImplantDTO> GetProductList();

        /// <summary>
        /// Determines whether [is date range correct] [the specified from date].
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <param name="practiceId">The practice identifier.</param>
        /// <returns></returns>
        bool IsDateRangeCorrect(DateTime fromDate, DateTime toDate, long practiceId);

        /// <summary>
        /// Determines whether [is practice name unique] [the specified practice name].
        /// </summary>
        /// <param name="practiceName">Name of the practice.</param>
        /// <returns></returns>
        bool IsPracticeNameUnique(string practiceName);

        /// <summary>
        /// Adds the procedure level.
        /// </summary>
        /// <param name="level">The level.</param>
        /// <returns></returns>
        List<ProcedureLevelDTO> AddProcedureLevels(List<ProcedureLevelDTO> levels, string userName);

        /// <summary>
        /// Gets the practice identifier.
        /// </summary>
        /// <param name="rrUserId">The rr user identifier.</param>
        /// <returns></returns>
        ExternalUserDTO GetUser(string rrUserId);

        /// <summary>
        /// Gets the practice identifier.
        /// </summary>
        /// <param name="rrUserId"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        ExternalUserDTO GetUser(string rrUserId, string userName);

        /// <summary>
        /// Determines whether [is user already added] [the specified user identifier].
        /// </summary>
        /// <param name="rrUserId">The user identifier.</param>
        /// <returns></returns>
        bool IsUserAlreadyAdded(string rrUserId);

        /// <summary>
        /// Creates the procedure.
        /// </summary>
        /// <param name="procedure">The procedure.</param>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        ProcedureDTO CreateProcedure(ProcedureDTO procedure, string userName);

        /// <summary>
        /// Gets the procedure by text.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        List<ProcedureDTO> GetProcedureByText(string filter);

        /// <summary>
        /// Creates the asap user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        ExternalUserDTO CreateASAPUser(ExternalUserDTO user, string userName);

        /// <summary>
        /// Gets the asap users.
        /// </summary>
        /// <returns></returns>
        ICollection<ExternalUserDTO> GetASAPUsers();

        /// <summary>
        /// Deactivates the asap user.
        /// </summary>
        /// <param name="practiceId">The practice identifier.</param>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        bool DeactivateASAPUser(long practiceId, string userName);

        /// <summary>
        /// Gets the latest emr identifier.
        /// </summary>
        /// <returns></returns>
        string GetLatestEMRId();

        /// <summary>
        /// Saves the image.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="isProfileImageExist">if set to <c>true</c> [is profile image exist].</param>
        /// <returns></returns>
        ExternalUserDTO UpdateUser(ExternalUserDTO user);

        /// <summary>
        /// Gets the name of the user by user.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        ExternalUserDTO GetUserByUserName(string userName);

        /// <summary>
        /// Adds the practice procedures.
        /// </summary>
        /// <param name="procedures">The procedures.</param>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        List<PracticeProcedureDTO> AddPracticeProcedures(List<PracticeProcedureDTO> procedures, string userName);

        /// <summary>
        /// Gets the procedures.
        /// </summary>
        /// <returns></returns>
        List<ProcedureDTO> GetProcedures();

        /// <summary>
        /// Adds the practice procedure.
        /// </summary>
        /// <param name="procedure">The procedure.</param>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        PracticeProcedureDTO AddPracticeProcedure(PracticeProcedureDTO procedure, string userName);

        /// <summary>
        /// Gets the practice by emr identifier.
        /// </summary>
        /// <param name="emrId">The emr identifier.</param>
        /// <returns></returns>
        PracticeDTO GetPracticeByEMRId(String emrId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="emrId"></param>
        /// <returns></returns>
        bool ValidateEmrStatus(string emrId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="emrId"></param>
        /// <returns></returns>
        bool PracticeReActivatedCheck(string emrId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="timeOut"></param>
        /// <returns></returns>
        bool CancelPendingJob(string userName, int timeOut = 5);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        List<string> GetServiceEmailList();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="emrList"></param>
        /// <param name="userId"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        bool StartProcessCube(List<string> emrList, string userId, int timeout);
    }
}
