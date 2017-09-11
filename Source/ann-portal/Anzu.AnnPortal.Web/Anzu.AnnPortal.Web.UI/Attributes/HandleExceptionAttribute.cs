using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;

namespace Anzu.AnnPortal.Web.UI.Attributes
{
    [AttributeUsage(AttributeTargets.All)]
    public class HandleExceptionAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            if (context.Exception != null)
            {
                //LogHelper.LogException(context.Exception, "Service Global Exception");
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent(String.Format("Message: {0} ST: {1}", context.Exception.Message, context.Exception.StackTrace)),
                    ReasonPhrase = "Exception"
                });
            }
        }
    }
}