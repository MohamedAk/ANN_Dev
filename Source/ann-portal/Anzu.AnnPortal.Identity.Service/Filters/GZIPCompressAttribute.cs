using System;
using System.Linq;
using System.Net.Http;
using System.Web.Http.Filters;

namespace Anzu.AnnPortal.Identity.Service.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class GZIPCompressAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(HttpActionExecutedContext context)
        {
            if (context.Response != null)
            {
                var acceptedEncoding = context.Response.RequestMessage.Headers.AcceptEncoding.First().Value;

                if (!acceptedEncoding.Equals("gzip", StringComparison.InvariantCultureIgnoreCase)
                    && !acceptedEncoding.Equals("deflate", StringComparison.InvariantCultureIgnoreCase))
                {
                    return;
                }

                context.Response.Content = new GZIPCompression(context.Response.Content, acceptedEncoding);
            }
            else
                return;
        }
    }
}