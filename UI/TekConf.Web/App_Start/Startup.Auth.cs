using Owin;

namespace TekConf.Web
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            app.UseSignInCookies();

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