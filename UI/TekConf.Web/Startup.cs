
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using Owin.Security.Providers.GitHub;
using Owin.Security.Providers.LinkedIn;
using Owin.Security.Providers.Yahoo;

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
			app.UseMicrosoftAccountAuthentication(
					clientId: "0000000040112827",
					clientSecret: "dOusz11kRzfBWUShkNpbf465EPEbhRE5");

			app.UseTwitterAuthentication(
							consumerKey: "3mYNb4Jt33Ttdgw4Q4Ppjw",
							consumerSecret: "RwRTOPu6tYoP2Yh0RBLOfQeWiTKPBjlfDv7IbNJ3G7k");

			app.UseFacebookAuthentication(
							appId: "417883241605228",
							appSecret: "c2df6f0a2ed2a01f6f0553b3e58ad715");

			app.UseYahooAuthentication(
				consumerKey:
					"dj0yJmk9QXpLck5Vc2ZQdnRUJmQ9WVdrOWRVNVRRVVpvTkhFbWNHbzlNVEEzT1RRMk5UTTJNZy0tJnM9Y29uc3VtZXJzZWNyZXQmeD1jMQ--",
				consumerSecret: "224c0e0e5c3129e43b5c87273aa925a1ba089195");

			app.UseLinkedInAuthentication(clientId: "777d7tm6j83h3g", clientSecret: "qA05N3l3VldticeX");

			app.UseGitHubAuthentication(clientId: "5173718457d21c487d19", clientSecret: "df237e7acbf23a144c88ab697450350d7dae9f08");

			app.UseGoogleAuthentication();
		}
	}
}
