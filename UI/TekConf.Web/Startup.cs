using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TekConf.Web.Startup))]
namespace TekConf.Web
{
    public partial class Startup 
    {
        public void Configuration(IAppBuilder app) 
        {
            ConfigureAuth(app);
        }
    }
}
