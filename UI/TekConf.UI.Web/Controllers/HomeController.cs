using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.Mvc;
using TekConf.RemoteData.Dtos.v1;
using TekConf.UI.Web.App_Start;
using System.Linq;

namespace TekConf.UI.Web.Controllers
{
	public class HomeController : Controller
	{
		private RemoteDataRepositoryAsync _repository;

		public HomeController()
		{
			var baseUrl = ConfigurationManager.AppSettings["BaseUrl"];

			_repository = new RemoteDataRepositoryAsync(baseUrl);
		}

		[CompressFilter]
		public async Task<ActionResult> Index()
		{
			var conferencesTask = _repository.GetFeaturedConferences();
			var speakersTask = _repository.GetFeaturedSpeakers();
			var conferencesCountTask = _repository.GetConferencesCount(showPastConferences: false, search: null);
			await Task.WhenAll(conferencesTask, speakersTask, conferencesCountTask);

			var featuredSpeakers = speakersTask.Result == null ? new List<FullSpeakerDto>() : speakersTask.Result.ToList();
			var featuredConferences = conferencesTask.Result == null ? new List<ConferencesDto>() : conferencesTask.Result.ToList();
			var totalCount = conferencesCountTask.Result;

			var vm = new HomePageViewModel()
			{
				FeaturedConferences = featuredConferences,
				FeaturedSpeakers = featuredSpeakers,
				TotalCount = totalCount
			};

			return View(vm);
		}
	}
}
