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
		[Authorize]
		public async Task<ActionResult> Index()
		{
			var x = User.Identity.Name;
			var conferencesTask = _repository.GetFeaturedConferences();
			var speakersTask = _repository.GetFeaturedSpeakers();

			await Task.WhenAll(conferencesTask, speakersTask);

			var featuredSpeakers = speakersTask.Result == null ? new List<FullSpeakerDto>() : speakersTask.Result.ToList();
			var featuredConferences = conferencesTask.Result == null ? new List<ConferencesDto>() : conferencesTask.Result.ToList();

			var filteredConferences = featuredConferences
																	.Where(c => c.start >= DateTime.Now.AddDays(-2))
																	.OrderBy(c => c.start)
																	.Take(4)
																	.ToList();

			var vm = new HomePageViewModel()
			{
				FeaturedConferences = filteredConferences,
				FeaturedSpeakers = featuredSpeakers
			};

			return View(vm);
		}
	}
}
