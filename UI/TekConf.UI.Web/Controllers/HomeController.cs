using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using TekConf.RemoteData.Dtos.v1;
using TekConf.RemoteData.v1;
using TekConf.UI.Web.App_Start;
using System.Linq;

namespace TekConf.UI.Web.Controllers
{
	public class HomeController : Controller
	{
		//private RemoteDataRepositoryAsync _asyncRepository;
		private RemoteDataRepository _repository;
		public HomeController()
		{

			if (System.Web.HttpContext.Current.User != null && System.Web.HttpContext.Current.User.Identity != null)
			{
				var x = System.Web.HttpContext.Current.User.Identity.AuthenticationType;
				var y = System.Web.HttpContext.Current.User.Identity.Name;
			}

			var baseUrl = ConfigurationManager.AppSettings["BaseUrl"];

			//_asyncRepository = new RemoteDataRepositoryAsync(baseUrl);
			_repository = new RemoteDataRepository(baseUrl);
		}

		[CompressFilter]
		public ActionResult Index()
		{
			try
			{
				AutoResetEvent stopWaitHandle = new AutoResetEvent(false);
				//AsyncManager.OutstandingOperations.Increment();
				//AsyncManager.OutstandingOperations.Increment();
				//AsyncManager.OutstandingOperations.Increment();

				List<FullSpeakerDto> featuredSpeakers = null;
				_repository.GetFeaturedSpeakers(callback: speakers =>
				{
					featuredSpeakers = speakers;
					stopWaitHandle.Set();
					//AsyncManager.OutstandingOperations.Decrement();
				});
				stopWaitHandle.WaitOne();

				IList<ConferencesDto> featuredConferences = null;
				_repository.GetFeaturedConferences(callback: conferences =>
				{
					featuredConferences = conferences;
					stopWaitHandle.Set();
					//AsyncManager.OutstandingOperations.Decrement();
				});
				stopWaitHandle.WaitOne();

				int totalCount = 0;
				_repository.GetConferencesCount(showPastConferences: false, search: null, callback: count =>
				{
					totalCount = count;
					stopWaitHandle.Set();
					//AsyncManager.OutstandingOperations.Decrement();
				});
				stopWaitHandle.WaitOne();

				//var conferencesTask = _asyncRepository.GetFeaturedConferences();
				//var speakersTask = _asyncRepository.GetFeaturedSpeakers();
				//var conferencesCountTask = _asyncRepository.GetConferencesCount(showPastConferences: false, search: null);

				//await Task.WhenAll(conferencesTask, speakersTask, conferencesCountTask);

				//var featuredSpeakers = speakersTask.Result == null ? new List<FullSpeakerDto>() : speakersTask.Result.ToList();
				//var featuredConferences = conferencesTask.Result == null ? new List<ConferencesDto>() : conferencesTask.Result.ToList();
				//var totalCount = conferencesCountTask.Result;

				var vm = new HomePageViewModel()
				{
					FeaturedConferences = featuredConferences == null ? new List<ConferencesDto>() : featuredConferences.ToList(),
					FeaturedSpeakers = featuredSpeakers ?? new List<FullSpeakerDto>(),
					TotalCount = totalCount
				};

				return View(vm);
			}
			catch (Exception ex)
			{
	
				return
					View(new HomePageViewModel()
						{
							FeaturedConferences = new List<ConferencesDto>() { new ConferencesDto() { name = ex.Message }},
							FeaturedSpeakers = new List<FullSpeakerDto>(),
							TotalCount = 0
						});
			}
			
		}
	}
}
