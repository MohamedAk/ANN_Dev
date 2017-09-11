using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Anzu.AnnPortal.Identity.Service.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult Index()
        {
            ViewBag.Code = 0000;
            ViewBag.Message = "Server encountered an error. Issue Reference #" + String.Format("#{0}", DateTime.Now.ToString("HHmmss"));
            return View();
        }

        public ActionResult Code(int code)
        {
            ViewBag.Message = String.Format("#{0}", DateTime.Now.ToString("HHmmss"));
            ViewBag.Code = code;

            if (400 <= code && code < 500)
            {
                ViewBag.Description = "The request contains bad syntax or cannot be fulfilled. Issue Reference #" + ViewBag.Message;
            }
            else if (500 <= code && code < 600)
            {
                ViewBag.Description = "The server failed to fulfill an apparently valid request. Issue Reference #" + ViewBag.Message;
            }
            else
            {
                ViewBag.Description = "The server failed to fullfill a valid request. Issue Reference #" + ViewBag.Message;
            }


            return View();
        }
    }
}