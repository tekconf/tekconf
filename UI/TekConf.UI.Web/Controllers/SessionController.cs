using System.Configuration;
using System.Threading.Tasks;
using System.Web.Mvc;
using TekConf.UI.Web.App_Start;

namespace TekConf.UI.Web.Controllers
{
	using TekConf.RemoteData.v1;

	public class SessionController : Controller
	{
		private readonly IRemoteDataRepository _remoteDataRepository;

		public SessionController(IRemoteDataRepository remoteDataRepository)
		{
			_remoteDataRepository = remoteDataRepository;
		}

		[CompressFilter]
		public async Task<ActionResult> Index(string conferenceSlug)
		{
			var sessionsTask = _remoteDataRepository.GetSessionsAsync(conferenceSlug);
			await sessionsTask;
			return View(sessionsTask.Result);
		}

		[CompressFilter]
		public async Task<ActionResult> Detail(string conferenceSlug, string sessionSlug)
		{
			var sessionDto = await _remoteDataRepository.GetSession(conferenceSlug, sessionSlug);

			if (sessionDto == null)
			{
				return RedirectToAction("NotFound", "Error");
			}

			return View(sessionDto);
		}
	}
}
