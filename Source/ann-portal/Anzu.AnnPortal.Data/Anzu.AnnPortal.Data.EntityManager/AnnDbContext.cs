using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Anzu.AnnPortal.Data.Model;
using Anzu.AnnPortal.Data.Model.Common;
using Anzu.AnnPortal.Data.Model.Core;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Anzu.AnnPortal.Data.EntityManager
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Microsoft.AspNet.Identity.EntityFramework.IdentityDbContext{Anzu.AnnPortal.Data.Model.Core.ApplicationUser}"/>
    public class AnnDbContext : IdentityDbContext<ApplicationUser>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AnnDbContext"/> class.
        /// </summary>
        public AnnDbContext()
            : base("AnnPortalDatabase", throwIfV1Schema: false)
        {

        }

        /// <summary>
        /// Creates this instance.
        /// </summary>
        /// <returns></returns>
        public static AnnDbContext Create()
        {
            return new AnnDbContext();
        }

        /// <summary>
        /// Called when [model creating].
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            base.OnModelCreating(modelBuilder);
        }

        /// <summary>
        /// Seeds the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        public void Seed(AnnDbContext context)
        {
            #region Seed RecordStatuses

            foreach (var status in new[] { "Active", "Deleted" })
            {
                context.RecordStatuses.AddOrUpdate(i => i.Name, new RecordStatus
                {
                    Name = status
                });
            }
            context.SaveChanges();

            #endregion


            context.SaveChanges();
        }



        /// <summary>
        /// Gets or sets the practices.
        /// </summary>
        /// <value>
        /// The practices.
        /// </value>
        public DbSet<Practice> Practices { get; set; }

        /// <summary>
        /// Gets or sets the record statuses.
        /// </summary>
        /// <value>
        /// The record statuses.
        /// </value>
        public DbSet<RecordStatus> RecordStatuses { get; set; }

        /// <summary>
        /// Gets or sets the brest implants.
        /// </summary>
        /// <value>
        /// The brest implants.
        /// </value>
        public DbSet<BrestImplant> BrestImplants { get; set; }

        /// <summary>
        /// Gets or sets the dashboards.
        /// </summary>
        /// <value>
        /// The dashboards.
        /// </value>
        public DbSet<Dashboard> Dashboards { get; set; }

        /// <summary>
        /// Gets or sets the practice brest implants.
        /// </summary>
        /// <value>
        /// The practice brest implants.
        /// </value>
        public DbSet<PracticeBrestImplant> PracticeBrestImplants { get; set; }

        /// <summary>
        /// Gets or sets the practice procedures.
        /// </summary>
        /// <value>
        /// The practice procedures.
        /// </value>
        public DbSet<PracticeProcedure> PracticeProcedures { get; set; }

        /// <summary>
        /// Gets or sets the procedures.
        /// </summary>
        /// <value>
        /// The procedures.
        /// </value>
        public DbSet<Procedure> Procedures { get; set; }

        /// <summary>
        /// Gets or sets the procedure levels.
        /// </summary>
        /// <value>
        /// The procedure levels.
        /// </value>
        public DbSet<ProcedureLevel> ProcedureLevels { get; set; }

        /// <summary>
        /// Gets or sets the related procedures.
        /// </summary>
        /// <value>
        /// The related procedures.
        /// </value>
        public DbSet<RelatedProcedure> RelatedProcedures { get; set; }

        /// <summary>
        /// Gets or sets the user roles.
        /// </summary>
        /// <value>
        /// The user roles.
        /// </value>
        public DbSet<UserRole> UserRoles { get; set; }

        /// <summary>
        /// Gets or sets the role dashboards.
        /// </summary>
        /// <value>
        /// The role dashboards.
        /// </value>
        public DbSet<RoleDashboard> RoleDashboards { get; set; }

        /// <summary>
        /// Gets or sets the users.
        /// </summary>
        /// <value>
        /// The users.
        /// </value>
        public DbSet<User> ExternalUsers { get; set; }

        /// <summary>
        /// Gets or sets the total procedures.
        /// </summary>
        /// <value>
        /// The total procedures.
        /// </value>
        public DbSet<TotalProcedure> TotalProcedures { get; set; }

        /// <summary>
        /// Gets or sets the total revenues.
        /// </summary>
        /// <value>
        /// The total revenues.
        /// </value>
        public DbSet<TotalRevenue> TotalRevenues { get; set; }

        /// <summary>
        /// Gets or sets the practice edit information.
        /// </summary>
        /// <value>
        /// The practice edit information.
        /// </value>
        public DbSet<PracticeEditInformation> PracticeEditInformation { get; set; }

        /// <summary>
        /// Gets or sets the job status information.
        /// </summary>
        /// <value>
        /// The job status information.
        /// </value>
        public DbSet<JobStatus> JobStatus { get; set; }

        /// <summary>
        /// Gets or sets the job queue
        /// </summary>
        /// <value>
        /// job queue information
        /// </value>
        public DbSet<JobQueue> JobQueue { get; set; }

        /// <summary>
        /// Gets or sets the job queue practice
        /// </summary>
        /// <value>
        /// The job queue practice information.
        /// </value>
        public DbSet<JobQueuePractice> JobQueuePractice { get; set; }


        /// <summary>
        /// Logs practive activation / deactivation
        /// </summary>
        /// <value>
        /// Practive activation / deactivation information.
        /// </value>
        public DbSet<PracticeActivationLog> PracticeActivationLog { get; set; }
    }
}