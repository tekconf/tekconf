using System.Threading.Tasks;
using System.Web.Mvc;
using TekConf.Web.ViewModels;

namespace TekConf.Web.Controllers
{
    using AutoMapper;
    using System.Collections.Generic;
    using TekConf.Common.Entities;
    using TekConf.RemoteData.Dtos.v1;
    using TekConf.RemoteData.v1;

	public class ScheduleController : Controller
	{
		private readonly IRemoteDataRepository _remoteDataRepository;
        private readonly IConferenceRepository _conferenceRepository;

		public ScheduleController(IRemoteDataRepository remoteDataRepository,
            IConferenceRepository conferenceRepository)
		{
			_remoteDataRepository = remoteDataRepository;
            _conferenceRepository = conferenceRepository;
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

            IList<FullConferenceDto> newestConferences = null;
            var getNewestConferencesTask = Task.Factory.StartNew(() =>
            {
                var nconferences = _conferenceRepository.GetNewestConferences();

                newestConferences = Mapper.Map<List<FullConferenceDto>>(nconferences);
            });
            await getNewestConferencesTask;
            ViewBag.NewestConferences = newestConferences;

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