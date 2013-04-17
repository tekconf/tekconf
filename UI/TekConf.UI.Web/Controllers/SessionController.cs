using System.Configuration;
using System.Threading.Tasks;
using System.Web.Mvc;
using TekConf.UI.Web.App_Start;

namespace TekConf.UI.Web.Controllers
{
	public class SessionController : Controller
	{
		private readonly IRemoteDataRepositoryAsync _remoteDataRepositoryAsync;

		public SessionController(IRemoteDataRepositoryAsync remoteDataRepositoryAsync)
		{
			_remoteDataRepositoryAsync = remoteDataRepositoryAsync;
		}

		[CompressFilter]
		public async Task<ActionResult> Index(string conferenceSlug)
		{
			var sessionsTask = _remoteDataRepositoryAsync.GetSessions(conferenceSlug);
			await sessionsTask;
			return View(sessionsTask.Result);
		}

		[CompressFilter]
		public async Task<ActionResult> Detail(string conferenceSlug, string sessionSlug)
		{
			var sessionDetailTask = _remoteDataRepositoryAsync.GetSessionDetail(conferenceSlug, sessionSlug);

			await sessionDetailTask;

			if (sessionDetailTask.Result == null)
			{
				return RedirectToAction("NotFound", "Error");
			}

			return View(sessionDetailTask.Result);
		}
	}
}
