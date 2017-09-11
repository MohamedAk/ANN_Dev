using Anzu.AnnPortal.Common.Model.Common;
using Anzu.AnnPortal.Common.Model.Portal;
using Anzu.AnnPortal.Data.Model;
using Anzu.AnnPortal.Data.Model.Common;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anzu.AnnPortal.Business.Core.AutoMapperConf
{
    public class AutoMapperConf
    {
        /// <summary>
        /// Configures this instance.
        /// </summary>
        public static void Configure()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<ZipCode, ZipCodeDTO>();
                cfg.CreateMap<ZipCodeDTO, ZipCode>();

                cfg.CreateMap<Region, RegionDTO>();
                cfg.CreateMap<RegionDTO, Region>();

                cfg.CreateMap<State, StateDTO>();
                cfg.CreateMap<StateDTO, State>();

                cfg.CreateMap<Dashboard, DashboardDTO>();
                cfg.CreateMap<DashboardDTO, Dashboard>();

                cfg.CreateMap<Practice, PracticeDTO>();
                cfg.CreateMap<PracticeDTO, Practice>();

                cfg.CreateMap<Procedure, ProcedureDTO>();
                cfg.CreateMap<ProcedureDTO, Procedure>();

                cfg.CreateMap<ProcedureLevel, ProcedureLevelDTO>();
                cfg.CreateMap<ProcedureLevelDTO, ProcedureLevel>();

                cfg.CreateMap<RelatedProcedure, RelatedProcedureDTO>();
                cfg.CreateMap<RelatedProcedureDTO, RelatedProcedure>();

                cfg.CreateMap<Role, RoleDTO>();
                cfg.CreateMap<RoleDTO, Role>();

                cfg.CreateMap<PracticeProcedure, PracticeProcedureDTO>();
                cfg.CreateMap<PracticeProcedureDTO, PracticeProcedure>();

                cfg.CreateMap<User, ExternalUserDTO>();
                cfg.CreateMap<ExternalUserDTO, User>();

                cfg.CreateMap<BrestImplant, BrestImplantDTO>();
                cfg.CreateMap<BrestImplantDTO, BrestImplant>();

                cfg.CreateMap<PracticeBrestImplant, PracticeBrestImplantDTO>();
                cfg.CreateMap<PracticeBrestImplantDTO, PracticeBrestImplant>();

                cfg.CreateMap<UserRole, UserRoleDTO>();
                cfg.CreateMap<UserRoleDTO, UserRole>();

                cfg.CreateMap<Company, CompanyDTO>();
                cfg.CreateMap<CompanyDTO, Company>();

                cfg.CreateMap<ProductType, ProductTypeDTO>();
                cfg.CreateMap<ProductTypeDTO, ProductType>();
           
            });
        }
    }
}
