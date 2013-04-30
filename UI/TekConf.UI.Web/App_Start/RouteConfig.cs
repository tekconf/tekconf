using System;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.AspNet.SignalR;

namespace TekConf.UI.Web
{
	public class RouteConfig
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
			routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon.ico(/.*)?" });
			routes.IgnoreRoute("{resource}.png/{*pathInfo}");

			routes.MapRoute(name: "Detail", url: "conferences/{conferenceSlug}",
					defaults: new { controller = "conferences", action = "Detail" });

			routes.MapRoute(name: "SessionDetail", url: "conferences/{conferenceSlug}/{sessionSlug}",
					defaults: new { controller = "Session", action = "Detail" });

			routes.MapRoute(name: "SessionSpeakerDetail", url: "conferences/{conferenceSlug}/speakers/{speakerSlug}",
					defaults: new { controller = "Speaker", action = "Detail" });

		
			routes.MapRoute(name: "Presentations", url: "profile/presentations",
				defaults: new { controller = "Presentations", action = "Index" });

			routes.MapRoute(name: "AddPresentation", url: "profile/presentations/add",
				defaults: new { controller = "Presentations", action = "Add" });

			routes.MapRoute(name: "PresentationHistory", url: "profile/presentations/{slug}/history",
				defaults: new { controller = "Presentations", action = "Detail" });

			routes.MapRoute(name: "AddPresentationHistory", url: "profile/presentations/{slug}/history/add",
				defaults: new { controller = "Presentations", action = "AddHistory" });

			routes.MapRoute(name: "PresentationDetail", url: "profile/presentations/{slug}",
				defaults: new { controller = "Presentations", action = "Detail" });

			routes.MapRoute(name: "AdminCreateConference", url: "admin/conferences/create",
					defaults: new { controller = "AdminConference", action = "CreateConference" });

			routes.MapRoute(name: "AdminEditConference", url: "admin/conferences/{conferenceSlug}/edit",
					defaults: new { controller = "AdminConference", action = "EditConference" });

			routes.MapRoute(name: "AdminEditConferences", url: "admin/conferences/",
					defaults: new { controller = "AdminConference", action = "EditConferencesIndex" });




			routes.MapRoute(name: "AdminAddSession", url: "admin/conferences/{conferenceSlug}/sessions/add",
					defaults: new { controller = "AdminSession", action = "AddSession" });

			routes.MapRoute(name: "AdminEditSession", url: "admin/conferences/{conferenceSlug}/sessions/{sessionSlug}/edit",
					defaults: new { controller = "AdminSession", action = "EditSession" });




			routes.MapRoute(name: "AdminAddSpeaker", url: "admin/conferences/{conferenceSlug}/sessions/{sessionSlug}/speakers/add",
					defaults: new { controller = "AdminSpeaker", action = "CreateSpeaker" });

			routes.MapRoute(name: "AdminEditSpeaker", url: "admin/conferences/{conferenceSlug}/speakers/{speakerSlug}/edit",
					defaults: new { controller = "AdminSpeaker", action = "EditSpeaker" });



			routes.MapRoute(
				name: "Default",
				url: "{controller}/{action}/{id}",
				defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional });

			

		}
	}
}