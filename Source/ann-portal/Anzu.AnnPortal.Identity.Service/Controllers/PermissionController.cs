using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity;
using Newtonsoft.Json;
using Anzu.AnnPortal.Identity.Data.Model;
using Anzu.AnnPortal.Identity.Core;
using Anzu.AnnPortal.Identity.Service.Filters;

namespace Anzu.AnnPortal.Identity.Service.Controllers
{
    [Authorize]
    public class PermissionController : BaseApiController
    {
        CommonHelper ch = new CommonHelper();
        
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult Get()
        {
            ch.LogMethodStartAPI(System.Reflection.MethodBase.GetCurrentMethod().Name, DateTime.Now);

            ApplicationDbContext appDb = new ApplicationDbContext();

            var permissionList = appDb.Module;
            var group = permissionList.Select(a => new
            {
                ModuleName = a.Name,
                Permissions = a.Permissions.Select(p => new { PermissionId = p.Id, PermissionCode = p.Code, PermissionName = p.Name })
            });
            ch.LogMethodEndAPI(System.Reflection.MethodBase.GetCurrentMethod().Name, DateTime.Now);
            return this.Json(group);
        }

        [HttpGet]
        [Route("api/Permission/CheckPermission")]
        public IHttpActionResult CheckPermission(string permissionCode)
        {
            ch.LogMethodStartAPI(System.Reflection.MethodBase.GetCurrentMethod().Name, DateTime.Now);

            bool hasPermission = false;

            PermissionService permissionService = new PermissionService();
            hasPermission = permissionService.HasPermission(permissionCode, User.Identity.Name);
            var result = this.Json(hasPermission);
            ch.LogMethodEndAPI(System.Reflection.MethodBase.GetCurrentMethod().Name, DateTime.Now);
            return result;
        }

        [HttpGet]
        [Route("api/Permission/CheckMultiplePermissions")]
        public IHttpActionResult CheckMultiplePermissions([FromUri] List<string> permissionCodes)
        {
            ch.LogMethodStartAPI(System.Reflection.MethodBase.GetCurrentMethod().Name, DateTime.Now);

            bool hasPermission = false;

            if (permissionCodes != null && permissionCodes.Any())
            {
                permissionCodes = SplitByComma(permissionCodes[0]);
            }

            PermissionService permissionService = new PermissionService();
            hasPermission = permissionService.HasMultiplePermissions(User.Identity.Name, permissionCodes);
            var result = this.Json(hasPermission);
            ch.LogMethodEndAPI(System.Reflection.MethodBase.GetCurrentMethod().Name, DateTime.Now);
            return result;
        }

        [HttpGet]
        [Route("api/Permission/CheckOnePermissionFormList")]
        public IHttpActionResult CheckOnePermissionFormList([FromUri] List<string> permissionCodes)
        {
            ch.LogMethodStartAPI(System.Reflection.MethodBase.GetCurrentMethod().Name, DateTime.Now);

            bool hasPermission = false;

            if (permissionCodes != null && permissionCodes.Any())
            {
                permissionCodes = SplitByComma(permissionCodes[0]);
            }

            PermissionService permissionService = new PermissionService();
            hasPermission = permissionService.HasOnePermissionFromList(User.Identity.Name, permissionCodes);
            var result = this.Json(hasPermission);
            ch.LogMethodEndAPI(System.Reflection.MethodBase.GetCurrentMethod().Name, DateTime.Now);
            return result;
        }

        [NonAction]
        private List<string> SplitByComma(string values)
        {
            ch.LogMethodStartAPI(System.Reflection.MethodBase.GetCurrentMethod().Name, DateTime.Now);

            List<string> separatedValues = new List<string>();

            string[] splittedValues = values.Split(',');

            if(splittedValues.Length > 0)
            {
                separatedValues = splittedValues.ToList();

                separatedValues.RemoveAll(a => a.Equals(string.Empty));
            }
            ch.LogMethodEndAPI(System.Reflection.MethodBase.GetCurrentMethod().Name, DateTime.Now);
            return separatedValues;
        }
    }
}
