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
				var stopWaitHandle = new AutoResetEvent(false);

				List<FullSpeakerDto> featuredSpeakers = null;
				_repository.GetFeaturedSpeakers(callback: speakers =>
				{
					featuredSpeakers = speakers;
					stopWaitHandle.Set();
				});
				stopWaitHandle.WaitOne();


				IList<FullConferenceDto> featuredConferences = null;
				_repository.GetFeaturedConferences(conferences =>
				{
					featuredConferences = conferences;
					stopWaitHandle.Set();
				});
				stopWaitHandle.WaitOne();

				IList<FullConferenceDto> scheduledConferences = new List<FullConferenceDto>();
				if (Request.IsAuthenticated)
				{
					_repository.GetSchedules(User.Identity.Name, conferences =>
						{
							scheduledConferences = conferences.Where(x => x.end >= DateTime.Now).Take(4).ToList();
							stopWaitHandle.Set();
						});

					stopWaitHandle.WaitOne();
				}

				int totalCount = 0;
				_repository.GetConferencesCount(showPastConferences: false, search: null, callback: count =>
				{
					totalCount = count;
					stopWaitHandle.Set();
				});
				stopWaitHandle.WaitOne();

				IList<FullConferenceDto> allConferences = new List<FullConferenceDto>();
				if (scheduledConferences.Any())
				{
					if (scheduledConferences.Count < 4)
					{
						allConferences = scheduledConferences;
						allConferences.Add(featuredConferences.Take(1).Single());
						allConferences.Add(featuredConferences.Skip(1).Take(1).Single());

					}
					else
					{
						allConferences = scheduledConferences;
					}
				}
				else
				{
					allConferences = featuredConferences.Take(4).ToList();
				}

				var vm = new HomePageViewModel()
				{
					FeaturedConferences = allConferences.ToList(), // featuredConferences == null ? new List<FullConferenceDto>() : featuredConferences.ToList(),
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
							FeaturedConferences = new List<FullConferenceDto>() { new FullConferenceDto() { name = ex.Message } },
							FeaturedSpeakers = new List<FullSpeakerDto>(),
							TotalCount = 0
						});
			}

		}
	}
}
