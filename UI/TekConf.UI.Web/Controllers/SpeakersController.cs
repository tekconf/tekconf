using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
			//var openCallsConferencesTask = _repository.GetConferencesWithOpenCalls();
			var openCallsConferencesTask = _repository.GetConferences(showOnlyOpenCalls: true);

			await Task.WhenAll(openCallsConferencesTask);

			var openCallConferences = openCallsConferencesTask.Result == null ? new List<ConferencesDto>() : openCallsConferencesTask.Result.ToList();
		
			var vm = new SpeakersViewModel()
			{
				OpenConferences = openCallConferences.OrderBy(x => x.callForSpeakersCloses).ToList(),
				Presentations = new List<PresentationDto>(),
				MyConferences = new List<ConferencesDto>(),
			};

			return View(vm);
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
