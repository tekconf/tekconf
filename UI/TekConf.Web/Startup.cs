
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;

[assembly: OwinStartup("WebStartup", typeof(TekConf.Web.Startup))]
namespace TekConf.Web
{
    public partial class Startup 
    {
        public void Configuration(IAppBuilder app) 
        {
            //ConfigureAuth(app);
            app.MapSignalR();

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login")
            });
            // Use a cookie to temporarily store information about a user logging in with a third party login provider
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            app.UseTwitterAuthentication(
                    consumerKey: "3mYNb4Jt33Ttdgw4Q4Ppjw",
                    consumerSecret: "RwRTOPu6tYoP2Yh0RBLOfQeWiTKPBjlfDv7IbNJ3G7k");

            app.UseFacebookAuthentication(
                    appId: "417883241605228",
                    appSecret: "c2df6f0a2ed2a01f6f0553b3e58ad715");

            app.UseGoogleAuthentication();
        }
    }
}
