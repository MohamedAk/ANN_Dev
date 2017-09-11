using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Anzu.AnnPortal.Identity.Service.Filters
{
    public class AddCustomHeaderFilter : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if ((filterContext != null) && (filterContext.HttpContext != null) && (filterContext.HttpContext.Response.Headers != null))
            {
                filterContext.HttpContext.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN");
            }

            base.OnActionExecuted(filterContext);
        }
    }
}