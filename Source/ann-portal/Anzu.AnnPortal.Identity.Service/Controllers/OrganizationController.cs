using Anzu.AnnPortal.Identity.Common.Model;
using Anzu.AnnPortal.Identity.Common.Model.Enum;
using Anzu.AnnPortal.Identity.Data.Model;
using Anzu.AnnPortal.Identity.Data.Model.Models;
using Anzu.AnnPortal.Identity.Service.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Anzu.AnnPortal.Identity.Service.Controllers
{
    //[Authorize]
    public class OrganizationController : ApiController
    {
        CommonHelper ch = new CommonHelper();

        [Route("api/Organization/activeOrganizations/{id}")]
        [HttpPost]
        public IHttpActionResult GetActiveOrganizations(int id)
        {
            ch.LogMethodStartAPI(System.Reflection.MethodBase.GetCurrentMethod().Name, DateTime.Now);
            ApplicationDbContext context = new ApplicationDbContext();
            var result = this.Ok<IEnumerable<Organization>>(context.Organizations.Where(o => o.StatusId == (int)StatusType.Activate && o.Id != id).ToList());
            ch.LogMethodEndAPI(System.Reflection.MethodBase.GetCurrentMethod().Name, DateTime.Now);
            return result;
        }

        /// <summary>
        /// Inserts the organization.
        /// </summary>
        /// <param name="organizationName">Name of the organization.</param>
        /// <returns></returns>
        [Route("api/Organization/insertOrganizations")]
        [HttpPost]
        public IHttpActionResult InsertOrganization([FromBody]string[] parameters)
        {
            ch.LogMethodStartAPI(System.Reflection.MethodBase.GetCurrentMethod().Name, DateTime.Now);

            if (parameters == null)
            {
                return NotFound();
            }

            string organizationName = parameters[0];
            string Key = parameters[1];
            int Id = 0;
            if (string.IsNullOrEmpty(organizationName) || string.IsNullOrEmpty(Key) || !int.TryParse(Key, out Id))
            {
                return NotFound();
            }

            Organization dbcontext = new Organization();
            dbcontext.Id = Id;
            dbcontext.Name = organizationName;
            dbcontext.StatusId = (int)StatusType.Activate;

            ApplicationDbContext context = new ApplicationDbContext();
            var newOrganization = context.Organizations.Add(dbcontext);
            context.SaveChanges();
            ch.LogMethodEndAPI(System.Reflection.MethodBase.GetCurrentMethod().Name, DateTime.Now);
            return Ok(newOrganization);
        }
        [Route("api/Organization/updateOrganizations")]
        [HttpPost]
        public IHttpActionResult UpdateOrganization([FromBody]string[] parameters)
        {
            ch.LogMethodStartAPI(System.Reflection.MethodBase.GetCurrentMethod().Name, DateTime.Now);
            if (parameters == null)
            {
                ch.LogMethodEndAPI(System.Reflection.MethodBase.GetCurrentMethod().Name, DateTime.Now);
                return NotFound();
            }

            string organizationName = parameters[0];
            string Key = parameters[1];
            string status = parameters[2];

            int Id, statusId = 0;
            if (string.IsNullOrEmpty(organizationName) || string.IsNullOrEmpty(Key) || !int.TryParse(Key, out Id) || !int.TryParse(status, out statusId))
            {
                ch.LogMethodEndAPI(System.Reflection.MethodBase.GetCurrentMethod().Name, DateTime.Now);
                return NotFound();
            }

            ApplicationDbContext context = new ApplicationDbContext();
            var existingOrganization = context.Organizations.Where(org => org.Id == Id).SingleOrDefault();
            if (existingOrganization == null)
            {
                ch.LogMethodEndAPI(System.Reflection.MethodBase.GetCurrentMethod().Name, DateTime.Now);
                return NotFound();
            }

            existingOrganization.Name = organizationName;
            existingOrganization.StatusId = statusId;

            context.SaveChanges();
            ch.LogMethodEndAPI(System.Reflection.MethodBase.GetCurrentMethod().Name, DateTime.Now);
            return Ok(existingOrganization);
        }

        /// <summary>
        /// Gets the active organizations.
        /// </summary>
        /// <returns></returns>
        [Route("api/Organization/activeOrganizations")]
        [HttpPost]
        public IHttpActionResult GetActiveOrganizations()
        {
            ch.LogMethodStartAPI(System.Reflection.MethodBase.GetCurrentMethod().Name, DateTime.Now);
            ApplicationDbContext context = new ApplicationDbContext();
            var result = this.Ok<IEnumerable<Organization>>(context.Organizations.Where(o => o.StatusId == (int)StatusType.Activate).ToList());
            ch.LogMethodEndAPI(System.Reflection.MethodBase.GetCurrentMethod().Name, DateTime.Now);
            return result;
       }
    }
}


