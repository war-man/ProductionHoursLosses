using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin;
using Owin;
using ProductionHoursLosses.App_Start;

[assembly: OwinStartupAttribute(typeof(ProductionHoursLosses.Startup))]
namespace ProductionHoursLosses
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}