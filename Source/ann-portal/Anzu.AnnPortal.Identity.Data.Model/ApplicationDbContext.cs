//----------------------------------------------------------------------- 
// <copyright file="ApplicationDbContext.cs" company="Brandix i3"> 
//     Copyright Brandix i3 2015. All rights reserved. 
// </copyright>
// <summary>
// The ApplicationDbContext class. target CLR version 4.0.30319.34209.
// Created by : 
// Created date time: 
// </summary>
//-----------------------------------------------------------------------
using Anzu.AnnPortal.Identity.Data.Model.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Anzu.AnnPortal.Identity.Data.Model
{
    /// <seealso cref="Microsoft.AspNet.Identity.EntityFramework.IdentityDbContext" />
    public class ApplicationDbContext : IdentityDbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationDbContext" /> class.
        /// </summary>
        public ApplicationDbContext()
            : base("ApplicationSecurityDatabase")
        {
        }

        /// <summary>
        /// Creates this instance.
        /// </summary>
        /// <returns></returns>
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        /// <summary>
        /// This method is called when the model for a derived context has been initialized, but
        /// before the model has been locked down and used to initialize the context.  The default
        /// implementation of this method does nothing, but it can be overridden in a derived class
        /// such that the model can be further configured before it is locked down.
        /// </summary>
        /// <param name="modelBuilder">The builder that defines the model for the context being created.</param>
        /// <remarks>
        /// Typically, this method is called only once when the first instance of a derived context
        /// is created.  The model for that context is then cached and is for all further instances of
        /// the context in the app domain.  This caching can be disabled by setting the ModelCaching
        /// property on the given ModelBuilder, but note that this can seriously degrade performance.
        /// More control over caching is provided through use of the Database ModelBuilder and Database ContextFactory
        /// classes directly.
        /// </remarks>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            base.OnModelCreating(modelBuilder);
        }

        /// <summary>
        /// Gets or sets the permissions.
        /// </summary>
        /// <value>
        /// The permissions.
        /// </value>
        public DbSet<Permission> Permissions { get; set; }

        /// <summary>
        /// Gets or sets the role permissions.
        /// </summary>
        /// <value>
        /// The role permissions.
        /// </value>
        public DbSet<RolePermission> RolePermissions { get; set; }

        /// <summary>
        /// Gets or sets the organizations.
        /// </summary>
        /// <value>
        /// The organizations.
        /// </value>
        public DbSet<Organization> Organizations { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public DbSet<Status> Status { get; set; }
        
        /// <summary>
        /// Gets or sets the module.
        /// </summary>
        /// <value>
        /// The module.
        /// </value>
        public DbSet<Module> Module { get; set; }

        /// <summary>
        /// Gets or sets the login audit.
        /// </summary>
        /// <value>
        /// The login audit.
        /// </value>
        public DbSet<LoginAudit> LoginAudit { get; set; }

        /// <summary>
        /// Gets or sets the login audit status.
        /// </summary>
        /// <value>
        /// The login audit status.
        /// </value>
        public DbSet<LoginAuditStatus> LoginAuditStatus { get; set; }

        /// <summary>
        /// Gets or sets the designation.
        /// </summary>
        /// <value>
        /// The designation.
        /// </value>
        public DbSet<Designation> Designation { get; set; }

        /// <summary>
        /// Gets or sets the user roles.
        /// </summary>
        /// <value>
        /// The user roles.
        /// </value>
        public DbSet<UserRole> UserRoles { get; set; }

        /// <summary>
        /// Gets or sets the user organizations.
        /// </summary>
        /// <value>
        /// The user organizations.
        /// </value>
        public DbSet<UserOrganization> UserOrganizations { get; set; }

        /// <summary>
        /// Gets or sets the security question.
        /// </summary>
        /// <value>
        /// The security question.
        /// </value>
        public DbSet<SecurityQuestion> SecurityQuestion { get; set; }

        /// <summary>
        /// Gets or sets the user security question.
        /// </summary>
        /// <value>
        /// The user security question.
        /// </value>
        public DbSet<ApplicationUserSecurityQuestion> UserSecurityQuestion { get; set; }

        /// <summary>
        /// Gets or sets the previous passwords.
        /// </summary>
        /// <value>
        /// The previous passwords.
        /// </value>
        public DbSet<PreviousPassword> PreviousPasswords { get; set; }
    }
}
