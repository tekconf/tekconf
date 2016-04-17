using System;
using System.Collections.Generic;
using System.Net;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Fusillade;
using Polly;
using Tekconf.DTO;
using Plugin.Connectivity;
using System.Linq;
using AutoMapper;

namespace TekConf.Mobile.Core.Services
{
	public class ConferencesService : IConferencesService
	{
		private readonly ILocalConferencesService _localConferencesService;
		private readonly IRemoteConferencesService _remoteConferencesService;
		private readonly IMapper _mapper;

		//private readonly IApiService _apiService;
		//   private readonly ISchedulesService _schedulesService;

		//public ConferencesService(IApiService apiService, ISchedulesService schedulesService)
		//{
		//    _apiService = apiService;
		//    _schedulesService = schedulesService;
		//}

		public ConferencesService(IRemoteConferencesService remoteConferencesService, ILocalConferencesService localConferencesService, IMapper mapper)
		{
			_localConferencesService = localConferencesService;
			_remoteConferencesService = remoteConferencesService;
			_mapper = mapper;
		}

		public async Task<List<ConferenceModel>> GetConferences()
		{
			await _localConferencesService.Init();
			var remoteConferences = await _remoteConferencesService.GetConferences(Priority.Explicit);

			var models = _mapper.Map<List<ConferenceModel>>(remoteConferences);

			await _localConferencesService.Save(models);
			var savedModels = await _localConferencesService.GetConferences();
			return savedModels;

			//throw new NotImplementedException();
			//var cache = BlobCache.LocalMachine;
			//if (priority == Priority.UserInitiated) {
			//	BlobCache.LocalMachine.InvalidateAll ().Subscribe ();
			//}
			//var cachedConferences = cache.GetAndFetchLatest("conferences", () => GetRemoteConferencesAsync(priority),
			//	offset =>
			//	{
			//		TimeSpan elapsed = DateTimeOffset.Now - offset;
			//		return elapsed > new TimeSpan(hours: 24, minutes: 0, seconds: 0);
			//	});

			//var conferences = await cachedConferences.FirstOrDefaultAsync();
	  //      var schedules = await _schedulesService.GetSchedules(priority);
	  //      if (schedules != null)
	  //      {
	  //          foreach (var schedule in schedules)
	  //          {
	  //              var conference = conferences.SingleOrDefault(c => c.Slug == schedule.Conference.Slug);
	  //              if (conference != null)
	  //              {
	  //                  conference.IsAddedToSchedule = true;
	  //              }
	  //          }
	  //      }
	  //      return conferences;
		}

		public async Task<Conference> GetConference(Priority priority, string slug)
		{
			throw new NotImplementedException();
			//var cachedConference = BlobCache.LocalMachine.GetAndFetchLatest(slug, () => GetRemoteConference(priority, slug), offset =>
			//	{
			//		TimeSpan elapsed = DateTimeOffset.Now - offset;
			//		return elapsed > new TimeSpan(hours: 0, minutes: 30, seconds: 0);
			//	});

			//var conference = await cachedConference.FirstOrDefaultAsync();

			//return conference;
		}
	}
}