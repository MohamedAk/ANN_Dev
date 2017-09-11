using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;

namespace Anzu.AnnPortal.Identity.Service.Filters
{
    public class AddCustomHttpHeaderFilter : ActionFilterAttribute
    {
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            if ((actionExecutedContext != null) && (actionExecutedContext.Response != null) && (actionExecutedContext.Response.Headers != null))
            {
                actionExecutedContext.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN");
            }

            base.OnActionExecuted(actionExecutedContext);
        }
    }
}