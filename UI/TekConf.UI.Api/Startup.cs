
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(TekConf.UI.Api.Startup))]
 namespace TekConf.UI.Api
{
    public partial class Startup 
    {
        public void Configuration(IAppBuilder app) 
        {
            //ConfigureAuth(app);
            app.MapSignalR();
        }
    }
}
