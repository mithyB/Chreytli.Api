﻿using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Chreytli.Api.Startup))]

namespace Chreytli.Api
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
