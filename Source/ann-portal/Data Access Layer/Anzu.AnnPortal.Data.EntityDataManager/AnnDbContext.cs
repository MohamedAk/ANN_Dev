using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Anzu.AnnPortal.Data.Model.Core;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Anzu.AnnPortal.Data.EntityDataManager
{
    public class AnnDbContext : IdentityDbContext<ApplicationUser>
    {
        public AnnDbContext()
            : base("AnnPortalDatabase", throwIfV1Schema: false)
        {
            
        }
        public static AnnDbContext Create()
    {
        return new AnnDbContext();
    } 
    }
} 