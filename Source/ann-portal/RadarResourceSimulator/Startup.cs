using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;
using RadarResourceSimulator.Models;

[assembly: OwinStartup(typeof(RadarResourceSimulator.Startup))]

namespace RadarResourceSimulator
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            var context = new ApplicationDbContext();
            context.Seed(context);
        }
    }
}
