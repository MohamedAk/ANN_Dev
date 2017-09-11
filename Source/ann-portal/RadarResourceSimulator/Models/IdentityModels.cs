using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace RadarResourceSimulator.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string Hometown { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        public void Seed(ApplicationDbContext context)
        {  
            var passwordHash = new PasswordHasher(); 
            foreach (var user in new[] { "testuser", "testasap", "testadmin", "millsuser", "riosuser", "wolinuser" })
            {
                if (context.Users.Any(u => u.UserName == user))
                {
                    continue;
                }
                var appUser = new ApplicationUser
                {
                    UserName = user,
                    Email = user+"@rr.com",
                    EmailConfirmed = true,
                    PasswordHash = passwordHash.HashPassword("test"), 
                    SecurityStamp = Guid.NewGuid().ToString()
                };
                context.Users.AddOrUpdate(u => u.UserName, appUser); 
            }
            context.SaveChanges();
        }
    }
}