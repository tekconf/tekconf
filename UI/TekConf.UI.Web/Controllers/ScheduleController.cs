using System.Threading.Tasks;
using System.Web.Mvc;
using TekConf.UI.Web.ViewModels;

namespace TekConf.UI.Web.Controllers
{
	using TekConf.RemoteData.v1;

	public class ScheduleController : Controller
	{
		private readonly IRemoteDataRepository _remoteDataRepository;

		public ScheduleController(IRemoteDataRepository remoteDataRepository)
		{
			_remoteDataRepository = remoteDataRepository;
		}

		[Authorize]
		public async Task<ActionResult> Index()
		{
			if (!Request.IsAuthenticated || System.Web.HttpContext.Current.User == null)
			{
				return View();
			}

			var conferences = await this._remoteDataRepository.GetSchedules(System.Web.HttpContext.Current.User.Identity.Name);

			var model = new ScheduleViewModel()
									{
										Conferences = conferences
									};

			return View(model);

		}

		[HttpPost]
		[Authorize]
		public async Task<ActionResult> Delete(string conferenceSlug, string sessionSlug)
		{
			if (Request.IsAuthenticated)
			{
				if (System.Web.HttpContext.Current.User != null)
				{
					await _remoteDataRepository.RemoveSessionFromSchedule(conferenceSlug, sessionSlug, System.Web.HttpContext.Current.User.Identity.Name);
				}
			}

			return RedirectToAction("Index");
		}

		[HttpPost]
		[Authorize]
		public async Task<ActionResult> Add(string conferenceSlug, string sessionSlug)
		{
			if (Request.IsAuthenticated)
			{
				if (System.Web.HttpContext.Current.User != null)
				{
					await _remoteDataRepository.AddSessionToSchedule(conferenceSlug, sessionSlug, System.Web.HttpContext.Current.User.Identity.Name);
				}
			}

			return RedirectToAction("Detail", "Conferences", new { conferenceSlug });
		}
	}
}