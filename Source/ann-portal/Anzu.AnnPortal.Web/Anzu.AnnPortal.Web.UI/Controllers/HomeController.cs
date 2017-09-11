using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using Anzu.AnnPortal.Data.Model.Core;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Configuration;

namespace Anzu.AnnPortal.Web.UI.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    [Authorize]
    public class HomeController : Controller
    {
        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var user = HttpContext.User;

            ViewBag.CurrentUser = user;
            var roles = ((ClaimsIdentity)System.Web.HttpContext.Current.User.Identity).Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value);

            var userId = ((ClaimsIdentity)System.Web.HttpContext.Current.User.Identity).Claims
                .Where(c => c.Type == "UserId")
                .Select(c => c.Value).FirstOrDefault();

            var practiceId = ((ClaimsIdentity)System.Web.HttpContext.Current.User.Identity).Claims
                .Where(c => c.Type == "practiceId")
                .Select(c => c.Value).FirstOrDefault();

            var userRole = ((ClaimsIdentity)System.Web.HttpContext.Current.User.Identity).Claims
               .Where(c => c.Type == "UserRole")
               .Select(c => c.Value).FirstOrDefault();

            userRole = userRole.Trim().Replace(' ', '_');

            var firstName = ((ClaimsIdentity)System.Web.HttpContext.Current.User.Identity).Claims
               .Where(c => c.Type == "FirstName")
               .Select(c => c.Value).FirstOrDefault();

            var lastName = ((ClaimsIdentity)System.Web.HttpContext.Current.User.Identity).Claims
            .Where(c => c.Type == "LastName")
            .Select(c => c.Value).FirstOrDefault();

            var emrId = ((ClaimsIdentity)System.Web.HttpContext.Current.User.Identity).Claims
            .Where(c => c.Type == "emrId")
            .Select(c => c.Value).FirstOrDefault();

            ViewBag.loginUserId = userId;
            ViewBag.CurrentUserRoles = string.Join(",", roles);
            ViewBag.practiceId = practiceId;
            ViewBag.UserRole = userRole;
            ViewBag.FirstName = firstName;
            ViewBag.lastName = lastName;
            ViewBag.emrId = emrId != null ? emrId : "";
            ViewBag.coreDomain = ConfigurationManager.AppSettings["CoreServiceDomain"];
            ViewBag.identityDomain = ConfigurationManager.AppSettings["identityService"];

            return View();
        }

        public ActionResult User()
        {
            ViewBag.identityDomain = ConfigurationManager.AppSettings["identityService"];
            ViewBag.menuItem = "admin";
            return View();
        }
    }
}