using System.Configuration;
using System.Threading.Tasks;
using System.Web.Mvc;
using TekConf.UI.Web.ViewModels;

namespace TekConf.UI.Web.Controllers
{
	public class ScheduleController : Controller
	{
		private RemoteDataRepositoryAsync _repository;

		public ScheduleController()
		{
			var baseUrl = ConfigurationManager.AppSettings["BaseUrl"];

			_repository = new RemoteDataRepositoryAsync(baseUrl);
		}

		[Authorize]
		public async Task<ActionResult> Index()
		{
			if (Request.IsAuthenticated)
			{
				if (System.Web.HttpContext.Current.User != null && System.Web.HttpContext.Current.User.Identity != null)
				{
					var conferencesTask = _repository.GetSchedules(System.Web.HttpContext.Current.User.Identity.Name);
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
				if (System.Web.HttpContext.Current.User != null && System.Web.HttpContext.Current.User.Identity != null)
				{
					await _repository.RemoveSessionFromSchedule(conferenceSlug, sessionSlug, System.Web.HttpContext.Current.User.Identity.Name, "");
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
				if (System.Web.HttpContext.Current.User != null && System.Web.HttpContext.Current.User.Identity != null)
				{
					await _repository.AddSessionToSchedule(conferenceSlug, sessionSlug, System.Web.HttpContext.Current.User.Identity.Name, "");
				}
			}

			return RedirectToAction("Index");
		}
	}
}