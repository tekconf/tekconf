using System.Web.Mvc;
using System.Web.Routing;

namespace TekConf.UI.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(name: "Detail", url: "conferences/{conferenceSlug}",
                defaults: new { controller = "conferences", action = "Detail" });

            routes.MapRoute(name: "SessionDetail", url: "conferences/{conferenceSlug}/{sessionSlug}",
                defaults: new { controller = "Session", action = "Detail" });

            routes.MapRoute(name: "SessionSpeakerDetail", url: "conferences/{conferenceSlug}/{sessionSlug}/{speakerSlug}",
                defaults: new { controller = "Speaker", action = "Detail" });

            routes.MapRoute(name: "AdminCreateConference", url: "admin/conferences/create",
                defaults: new { controller = "Admin", action = "CreateConference" });

            routes.MapRoute(name: "AdminAddSession", url: "admin/conferences/{conferenceSlug}/sessions/add",
                defaults: new { controller = "Admin", action = "AddSession" });

            routes.MapRoute(name: "AdminAddSpeaker", url: "admin/conferences/{conferenceSlug}/sessions/{sessionSlug}/speakers/add",
                defaults: new { controller = "Admin", action = "CreateSpeaker" });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}