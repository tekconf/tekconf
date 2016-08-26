using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(TekConf.Api.Startup))]

namespace TekConf.Api
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
           ConfigureAuth(app);
        }
    }
}
