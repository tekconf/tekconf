using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace TekConf.UI.Web
{
	public class MvcApplication : System.Web.HttpApplication
	{
		protected void Application_Start()
		{
			new AppHost().Init();

			ViewEngines.Engines.Clear();
			ViewEngines.Engines.Add(new RazorViewEngine());

			//AreaRegistration.RegisterAllAreas();
			RouteTable.Routes.MapHubs();
			//WebApiConfig.Register(GlobalConfiguration.Configuration);
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);
			AuthConfig.RegisterAuth();
		}
	}
}