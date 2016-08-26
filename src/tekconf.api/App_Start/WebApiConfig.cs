using System.Data.Entity.Infrastructure.Interception;
using System.Web.Http;
using CacheCow.Server;
using TekConf.Api.App_Start;
using TekConf.Api.Data;
using TekConf.Api.Infrastructure.DataAccess;

namespace TekConf.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
       
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            GlobalConfiguration.Configuration.MessageHandlers.Add(new CachingHandler(GlobalConfiguration.Configuration));

            //config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            DbInterception.Add(new TekConfInterceptorTransientErrors());
            DbInterception.Add(new TekConfInterceptorLogging());
            StructuremapWebApi.Start();
            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}