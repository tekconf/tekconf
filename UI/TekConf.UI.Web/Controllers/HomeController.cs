using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using TekConf.Common.Entities.Repositories;
using TekConf.RemoteData.Dtos.v1;
using TekConf.RemoteData.v1;
using TekConf.UI.Api;
using TekConf.UI.Web.App_Start;
using System.Linq;

namespace TekConf.UI.Web.Controllers
{
	using TekConf.Common.Entities;

	public class HomeController : Controller
	{
		//private RemoteDataRepositoryAsync _asyncRepository;
		private RemoteDataRepository _repository;
		private readonly IConferenceRepository _conferenceRepository;
		private readonly IScheduleRepository _scheduleRepository;
		private readonly IRepository<SubscriptionEntity> _subscriptionRepository;
		private readonly IRemoteDataRepository _remoteDataRepository;

		public HomeController(IConferenceRepository conferenceRepository, IScheduleRepository scheduleRepository, IRepository<SubscriptionEntity> subscriptionRepository, IRemoteDataRepository remoteDataRepository)
		{
			_conferenceRepository = conferenceRepository;
			_scheduleRepository = scheduleRepository;
			_subscriptionRepository = subscriptionRepository;
			_remoteDataRepository = remoteDataRepository;
		}

		[CompressFilter]
		public async Task<ActionResult> Index()
		{
			try
			{
				List<FullSpeakerDto> featuredSpeakers = null;
				var getFeaturedSpeakersTask = Task.Factory.StartNew(() =>
				{
					var speakers = _conferenceRepository.GetFeaturedSpeakers().ToList();
					featuredSpeakers = Mapper.Map<List<FullSpeakerDto>>(speakers);
				});

				IList<FullConferenceDto> featuredConferences = null;
				var getFeaturedConferencesTask = Task.Factory.StartNew(() =>
						{
							var conferences = _conferenceRepository.GetFeaturedConferences();

							featuredConferences = Mapper.Map<List<FullConferenceDto>>(conferences);
						});

				int totalCount = 0;
				var getConferencesCountTask = Task.Factory.StartNew(() =>
				{
					totalCount = _conferenceRepository.GetConferenceCount(searchTerm: null, showPastConferences: false);
				});

				IList<FullConferenceDto> scheduledConferences = new List<FullConferenceDto>();
				IList<FullConferenceDto> allConferences = new List<FullConferenceDto>();

				var getScheduledConferencesTask = Task.Factory.StartNew(() =>
				{
					if (Request.IsAuthenticated)
					{
						var schedules = _scheduleRepository.GetSchedules(User.Identity.Name);
						var conferences = new List<ConferenceEntity>();
						foreach (var schedule in schedules)
						{
							var conference = _conferenceRepository
									.AsQueryable()
									.SingleOrDefault(c => c.slug == schedule.ConferenceSlug);
							conferences.Add(conference);
						}
						conferences = conferences.OrderBy(c => c.start).Where(x => x.end >= DateTime.Now).Take(4).ToList();
						foreach (var conference in conferences)
						{
							var conferenceDto = Mapper.Map<ConferenceEntity, FullConferenceDto>(conference);
							scheduledConferences.Add(conferenceDto);
						}
					}
				});

				await Task.WhenAll(getConferencesCountTask, getFeaturedSpeakersTask, getFeaturedConferencesTask, getScheduledConferencesTask);

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
					FeaturedConferences = allConferences.ToList(),
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

		[HttpPost]
		public ActionResult Subscribe(string emailAddress)
		{
			_repository.AddSubscription(emailAddress, subscription =>
			{
				
			});

			return RedirectToAction("Index");

		}
	}
}
