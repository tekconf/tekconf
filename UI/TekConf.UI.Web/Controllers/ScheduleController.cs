using System.Threading.Tasks;
using System.Web.Mvc;
using TekConf.UI.Web.ViewModels;

namespace TekConf.UI.Web.Controllers
{
	public class ScheduleController : Controller
	{
		private readonly IRemoteDataRepositoryAsync _remoteDataRepositoryAsync;

		public ScheduleController(IRemoteDataRepositoryAsync remoteDataRepositoryAsync)
		{
			_remoteDataRepositoryAsync = remoteDataRepositoryAsync;
		}

		[Authorize]
		public async Task<ActionResult> Index()
		{
			if (Request.IsAuthenticated)
			{
				if (System.Web.HttpContext.Current.User != null)
				{
					var conferencesTask = _remoteDataRepositoryAsync.GetSchedules(System.Web.HttpContext.Current.User.Identity.Name);
					await conferencesTask;

					var model = new ScheduleViewModel()
						{
							Conferences = conferencesTask.Result
						};

					return View(model);

				}
			}
			return View();
		}

		[HttpPost]
		[Authorize]
		public async Task<ActionResult> Delete(string conferenceSlug, string sessionSlug)
		{
			if (Request.IsAuthenticated)
			{
				if (System.Web.HttpContext.Current.User != null)
				{
					await _remoteDataRepositoryAsync.RemoveSessionFromSchedule(conferenceSlug, sessionSlug, System.Web.HttpContext.Current.User.Identity.Name, "");
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
					await _remoteDataRepositoryAsync.AddSessionToSchedule(conferenceSlug, sessionSlug, System.Web.HttpContext.Current.User.Identity.Name, "");
				}
			}

			return RedirectToAction("Detail", "Conferences", new { conferenceSlug });
		}
	}
}