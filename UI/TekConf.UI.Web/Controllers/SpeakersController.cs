using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Ploeh.AutoFixture;
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
			//var openCallsConferencesTask = _repository.GetConferencesWithOpenCalls();
			var conferencesTask = _repository.GetConferences(sortBy: string.Empty, showPastConferences: true, search: string.Empty);


			await Task.WhenAll(conferencesTask);

			//var openCallConferences = openCallsConferencesTask.Result == null ? new List<ConferencesDto>() : openCallsConferencesTask.Result.ToList();
			var allConferences = conferencesTask.Result == null ? new List<ConferencesDto>() : conferencesTask.Result.ToList();
			
			//var filteredConferences = openCallConferences
			//														.Where(c => c.start >= DateTime.Now.AddDays(-2))
			//														.OrderBy(c => c.start)
			//														.Take(4)
			//														.ToList();

			IFixture fixture = new Fixture()
						.Customize(new MultipleCustomization());
			var openConferences = fixture
				.Build<FullConferenceDto>()
				.Without(x => x.imageUrl)
				.CreateMany<FullConferenceDto>()
				.ToList();

			foreach (var o in openConferences)
				o.imageUrl = GetRandomConferenceImage();

			var vm = new SpeakersViewModel()
			{
				OpenConferences = openConferences,
				Presentations = fixture.Build<PresentationDto>()
												.With(x => x.ImagePath, "img/Presentations/Sample/Sample.png")
												.CreateMany()
												.ToList(),
				MyConferences = allConferences.Skip(20).Take(6).ToList(),
			};

			return View(vm);
		}

		private string GetRandomConferenceImage()
		{
			Thread.Sleep(10);
			var images = new List<string>()
				{
					"img/conferences/mountainjs-2013.png",
					"img/conferences/cloud-develop-2012.png",
					"img/conferences/BackboneConf.png",
					"img/conferences/codemash-2013.png",
					"img/conferences/devoxx-2012.png",
					"img/conferences/js-conf-downunder-2012.png",
					"img/conferences/jsconf-2013.png",
					"img/conferences/m3-conference-2012.png",
					"img/conferences/aspConf.png",
					"img/conferences/cascadiajs-2012.png",
					"img/conferences/xamarin-evolve-2013.png",
					"img/conferences/wwdc-2012.png",
					"img/conferences/RailsConf.png",
				};
			
			images.Shuffle();
			var image = images.FirstOrDefault();

			return image;
		}
	}

	public static class Extensions
	{
		public static void Shuffle<T>(this IList<T> list)
		{
			Thread.Sleep(10);
			Random rng = new Random(DateTime.Now.Millisecond);
			int n = list.Count;
			while (n > 1)
			{
				n--;
				int k = rng.Next(n + 1);
				T value = list[k];
				list[k] = list[n];
				list[n] = value;
			}
		}
	}
}
