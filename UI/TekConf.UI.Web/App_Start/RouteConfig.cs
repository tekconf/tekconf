using System.Web.Mvc;
using System.Web.Routing;

namespace TekConf.UI.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(name: "Detail", url: "Conferences/{conferenceSlug}",
                defaults: new { controller = "Conferences", action = "Detail" });

            routes.MapRoute(name: "SessionDetail", url: "Conferences/{conferenceSlug}/{sessionSlug}",
                defaults: new { controller = "Session", action = "Detail" });

            routes.MapRoute(name: "SessionSpeakerDetail", url: "Conferences/{conferenceSlug}/{sessionSlug}/{speakerSlug}",
                defaults: new { controller = "Speaker", action = "Detail" });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}