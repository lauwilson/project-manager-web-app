using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Hemlock.App_Start.Startup))]

namespace Hemlock.App_Start
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}