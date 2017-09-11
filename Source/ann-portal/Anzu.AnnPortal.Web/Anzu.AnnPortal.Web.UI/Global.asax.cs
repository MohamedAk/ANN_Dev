using Anzu.AnnPortal.Business.Core.AutoMapperConf;
using Anzu.AnnPortal.Common.Log;
using System;
using System.Security.Claims;
using System.Web;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Anzu.AnnPortal.Web.UI
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.NameIdentifier;
            AutoMapperConf.Configure();
            MvcHandler.DisableMvcResponseHeader = true;

        }

        protected void Application_PreSendRequestHeaders(object sender, EventArgs e)
        {
            HttpApplication app = sender as HttpApplication;
            if (app != null && app.Context != null)
            {
                app.Context.Response.Headers.Remove("Server");
            }
        }

        protected void Application_Error()
        {
            Exception exception = Server.GetLastError();
            LogHelper.LogException(exception, String.Format("Apllication Error Occurred - #{0}", DateTime.Now.ToString("HHmmss")));
        }
    }
}
