using System.Web.Mvc;
using System.Web.Routing;

namespace TekConf.Api
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Conferences",
                "",
                new { controller = "Conference", action = "Index"}
            );

            routes.MapRoute(
                "ConferencesDetails",
                "{slug}",
                new { controller = "Conference", action = "Details", Slug = "{slug}" }
            );

            routes.MapRoute(
                "ConferenceSpeakers",
                "{conference}/speakers",
                new { controller = "Speaker", action = "Index", Conference = "{conference}" }
            );

            routes.MapRoute(
                "ConferenceTags",
                "{conference}/tags",
                new { controller = "Tag", action = "Index", Conference = "{conference}" }
            );

            routes.MapRoute(
                "ConferenceTag",
                "{conference}/tags/{slug}",
                new { controller = "Tag", action = "Details", Conference = "{conference}", Slug = "{slug}" }
            );

            routes.MapRoute(
                "ConferenceSpeaker",
                "{conference}/speakers/{speaker}",
                new { controller = "Speaker", action = "Details", Conference = "{conference}", Speaker = "{speaker}" }
            );

            routes.MapRoute(
                "ConferenceSessions",
                "{conference}/sessions",
                new { controller = "Session", action = "Index", Conference = "{conference}" }
            );

            routes.MapRoute(
                "ConferenceSession",
                "{conference}/sessions/{session}",
                new { controller = "Session", action = "Details", Conference = "{conference}", Session = "{session}" }
            );



            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Conference", action = "Index" }
            );


        }
    }
}