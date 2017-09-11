using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.AspNet.Identity;
using System.Web.Configuration;

[assembly: OwinStartup(typeof(Anzu.AnnPortal.Identity.Service.Startup))]

namespace Anzu.AnnPortal.Identity.Service
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=316888

            var cookieAuthentiCationOption = new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Login/Login")
            };

            string domainName = WebConfigurationManager.AppSettings["domainname"];

            if (!domainName.Equals("localhost"))
            {
                cookieAuthentiCationOption.CookieDomain = domainName;
                cookieAuthentiCationOption.ExpireTimeSpan = TimeSpan.FromMinutes(Convert.ToInt32(WebConfigurationManager.AppSettings["CookieExpireTimeInMinutes"]));
            }

            app.UseCookieAuthentication(cookieAuthentiCationOption);

            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);
        }
    }
}
