using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TekConf.RemoteData.Dtos.v1;
using TekConf.UI.Web.App_Start;
using TekConf.UI.Web.ViewModels;

namespace TekConf.UI.Web.Controllers
{
	public class SpeakersController : Controller
	{
		private RemoteDataRepositoryAsync _repository;

		public SpeakersController()
		{
			var baseUrl = ConfigurationManager.AppSettings["BaseUrl"];

			_repository = new RemoteDataRepositoryAsync(baseUrl);
		}

		[CompressFilter]
		public async Task<ActionResult> Index()
		{
			var conferencesTask = _repository.GetConferencesWithOpenCalls();
			//var speakersTask = _repository.GetFeaturedSpeakers();

			await Task.WhenAll(conferencesTask);

			//var featuredSpeakers = speakersTask.Result == null ? new List<FullSpeakerDto>() : speakersTask.Result.ToList();
			var featuredConferences = conferencesTask.Result == null ? new List<ConferencesDto>() : conferencesTask.Result.ToList();

			var filteredConferences = featuredConferences
																	.Where(c => c.start >= DateTime.Now.AddDays(-2))
																	.OrderBy(c => c.start)
																	.Take(4)
																	.ToList();

			var vm = new SpeakersViewModel()
			{
				OpenConferences = filteredConferences,
				//FeaturedSpeakers = featuredSpeakers
			};

			return View(vm);
		}

	}
}
