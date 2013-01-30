
using System.Timers;
using System.Web.Mvc;
using Microsoft.AspNet.SignalR;

namespace TekConf.UI.Web.Controllers
{
	public class EventsController : Controller
	{
		public ActionResult Index()
		{
			Timer timer = new Timer(2000);
			timer.Elapsed += delegate
				{
					var context = GlobalHost.ConnectionManager.GetHubContext<EventsHub>();
					context.Clients.All.broadcastMessage("rob", "test");
				};
			timer.Start();

			return View();
		}

	}
}