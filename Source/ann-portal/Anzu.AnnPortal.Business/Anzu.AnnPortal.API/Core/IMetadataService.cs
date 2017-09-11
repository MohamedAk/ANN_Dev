using Anzu.AnnPortal.Common.Model.Portal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anzu.AnnPortal.Business.API.Core
{
    /// <summary>
    /// 
    /// </summary>
    public interface IMetadataService
    {
        /// Gets the procedures for level.
        /// </summary>
        /// <param name="levelId">The level identifier.</param>
        /// <returns></returns>
        ICollection<ProcedureLevelDTO> GetProcedureLevel(int levelId);

        /// <summary>
        /// Inserts the specified instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        ProcedureLevelDTO Insert(ProcedureLevelDTO instance);

        /// <summary>
        /// Updates the specified instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        ProcedureLevelDTO Update(ProcedureLevelDTO instance);

        /// <summary>
        /// Gets the breast implants.
        /// </summary>
        /// <returns></returns>
        ICollection<BrestImplantDTO> GetBreastImplants();

        /// <summary>
        /// Inserts the specified instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        BrestImplantDTO Insert(BrestImplantDTO instance);

        /// <summary>
        /// Updates the specified instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        BrestImplantDTO Update(BrestImplantDTO instance);

        /// <summary>
        /// Procedures the level filter by text.
        /// </summary>
        /// <param name="levelId">The level identifier.</param>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        List<ProcedureLevelDTO> ProcedureLevelFilterByText(long levelId, string filter);

        /// <summary>
        /// Procedures the filter by text.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        List<ProcedureDTO> ProcedureFilterByText(string filter);

        /// <summary>
        /// Gets all procedure levels.
        /// </summary>
        /// <returns></returns>
        ICollection<ProcedureDTO> GetAllProcedureLevels();

        /// <summary>
        /// Determines whether [is procedure level sequence unique] [the specified sequence].
        /// </summary>
        /// <param name="sequence">The sequence.</param>
        /// <returns></returns>
        bool IsDuplicateProcedureLevelSequence(string name, int? procedureLevel1, int? procedureLevel2, int? procedureLevel3, int? procedureLevel4);

        /// <summary>
        /// Inserts the specified instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        ProcedureDTO Insert(ProcedureDTO instance);

        /// <summary>
        /// Updates the specified instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        ProcedureDTO Update(ProcedureDTO instance);

        /// <summary>
        /// Gets the companies.
        /// </summary>
        /// <returns></returns>
        List<CompanyDTO> GetCompanies();

        /// <summary>
        /// Inserts the specified instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        CompanyDTO Insert(CompanyDTO instance);

        /// <summary>
        /// Updates the specified instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        CompanyDTO Update(CompanyDTO instance);

        /// <summary>
        /// Gets the products.
        /// </summary>
        /// <returns></returns>
        List<ProductTypeDTO> GetProducts();

        /// <summary>
        /// Inserts the specified instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        ProductTypeDTO Insert(ProductTypeDTO instance);

        /// <summary>
        /// Updates the specified instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        ProductTypeDTO Update(ProductTypeDTO instance);

        /// <summary>
        /// Gets the procedure levels.
        /// </summary>
        /// <param name="take">The take.</param>
        /// <param name="skip">The skip.</param>
        /// <param name="page">The page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="recordCount">The record count.</param>
        /// <returns></returns>
        IEnumerable<ProcedureDTO> GetProcedureLevels(int take, int skip, int page, int pageSize, out int recordCount);

        /// <summary>
        /// Gets the services from staging by emr identifier to grid.
        /// </summary>
        /// <param name="take">The take.</param>
        /// <param name="skip">The skip.</param>
        /// <param name="page">The page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="emrId">The emr identifier.</param>
        /// <param name="recordCount">The record count.</param>
        /// <returns></returns>
        ICollection<PracticeProcedureDTO> GetServicesFromStagingByEMRIdToGrid(int take, int skip, int page, int pageSize, string emrId, out int recordCount, string sortOption = "", string searchOption = "", string searchText = "");

        /// <summary>
        /// 
        /// </summary>
        /// <param name="take"></param>
        /// <param name="skip"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="filter"></param>
        /// <param name="recordCount"></param>
        /// <param name="searchOption"></param>
        /// <param name="sortOption"></param>
        /// <returns></returns>
        IEnumerable<ProcedureDTO> GetProcedureLevelsFilter(int take, int skip, int page, int pageSize, string filter, out int recordCount, string sortOption = "", string searchOption = "", int procedureId = 0);

        /// <summary>
        /// Determines whether [is procedure level unique] [the specified level number].
        /// </summary>
        /// <param name="levelNumber">The level number.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        bool IsProcedureLevelUnique(int levelNumber, string name);

        /// <summary>
        /// Determines whether [is brest implant unique] [the specified name].
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        bool IsBreastImplantUnique(string name);

        /// <summary>
        /// Determines whether [is product type unique] [the specified name].
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        bool IsProductTypeUnique(string name);

        /// <summary>
        /// Determines whether [is company unique] [the specified name].
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        bool IsCompanyUnique(string name);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prodcutId"></param>
        /// <returns></returns>
        ProductTypeDTO GetProductById(long prodcutId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        CompanyDTO GetCompanyById(long companyId);

        /// <summary>
        /// Determines the SQL job status
        /// </summary>
        /// <returns></returns>
        List<SqlJobDTO> GetSqlJobStatus();
    }
}
