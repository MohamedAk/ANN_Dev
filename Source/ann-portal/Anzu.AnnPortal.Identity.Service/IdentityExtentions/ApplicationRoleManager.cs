using Anzu.AnnPortal.Identity.Data.Model;
using Anzu.AnnPortal.Identity.Data.Model.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Anzu.AnnPortal.Identity.Service.IdentityExtentions
{
    public class ApplicationRoleManager : RoleManager<ApplicationRole>
    {        
        public ApplicationRoleManager()
            : base(new RoleStore<ApplicationRole>(new ApplicationDbContext()))
        {            
        }
    }
}