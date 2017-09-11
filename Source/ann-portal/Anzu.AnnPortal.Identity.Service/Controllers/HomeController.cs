using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Anzu.AnnPortal.Identity.Service.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var con = HttpContext.User;
            ViewBag.Title = "Home Page";
            return View();
        }
    }
}
