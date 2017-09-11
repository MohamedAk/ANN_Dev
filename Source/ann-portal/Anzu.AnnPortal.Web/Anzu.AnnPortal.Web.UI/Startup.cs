using System;
using Anzu.AnnPortal.Data.EntityManager;
using Microsoft.Owin;
using Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.AspNet.Identity;
using System.Web.Configuration;

[assembly: OwinStartup(typeof(Anzu.AnnPortal.Web.UI.Startup))]

namespace Anzu.AnnPortal.Web.UI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=316888
            // Enable the application to use a cookie to store information for the signed in user
            var cookieAuthentiCationOption = new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Login/Login"),
                CookieHttpOnly = true,
                Provider = new CookieAuthenticationProvider { OnApplyRedirect = ApplyRedirect }
            };

            string domainName = WebConfigurationManager.AppSettings["domainname"];

            if (!domainName.Equals("localhost"))
            {
                cookieAuthentiCationOption.CookieDomain = domainName;
                cookieAuthentiCationOption.ExpireTimeSpan = TimeSpan.FromMinutes(Convert.ToInt32(WebConfigurationManager.AppSettings["CookieExpireTimeInMinutes"]));
            }

            app.UseCookieAuthentication(cookieAuthentiCationOption);

            // Use a cookie to temporarily store information about a user logging in with a third party login provider
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);
        }

        private static void ApplyRedirect(CookieApplyRedirectContext context)
        {
            Uri absoluteUri;
            if (Uri.TryCreate(context.RedirectUri, UriKind.Absolute, out absoluteUri))
            {
                string identityService = WebConfigurationManager.AppSettings["identityService"];

                var path = PathString.FromUriComponent(absoluteUri);
                if (path == context.OwinContext.Request.PathBase + context.Options.LoginPath)
                    context.RedirectUri = string.Format("{0}Login/Login", identityService); //"http://localhost:62524/Login/Login" +
                new QueryString(
                    context.Options.ReturnUrlParameter,
                    context.Request.Uri.AbsoluteUri);
            }

            context.Response.Redirect(context.RedirectUri);
        }
    }
}
