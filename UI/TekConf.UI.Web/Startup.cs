using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TekConf.UI.Web.Startup))]
namespace TekConf.UI.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
