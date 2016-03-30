using System;
using System.Collections.Generic;
using System.Net;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Akavache;
using Fusillade;
using Polly;
using Tekconf.DTO;
using Plugin.Connectivity;
using System.Linq;

namespace TekConf.Mobile.Core.Services
{

	public class ConferencesService : IConferencesService
	{
		private readonly IApiService _apiService;
	    private readonly ISchedulesService _schedulesService;

	    public ConferencesService(IApiService apiService, ISchedulesService schedulesService)
	    {
	        _apiService = apiService;
	        _schedulesService = schedulesService;
	    }

	    public async Task<List<ConferenceModel>> GetConferences()
		{
			throw new NotImplementedException();
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
			var cachedConference = BlobCache.LocalMachine.GetAndFetchLatest(slug, () => GetRemoteConference(priority, slug), offset =>
				{
					TimeSpan elapsed = DateTimeOffset.Now - offset;
					return elapsed > new TimeSpan(hours: 0, minutes: 30, seconds: 0);
				});

			var conference = await cachedConference.FirstOrDefaultAsync();

			return conference;
		}


		private async Task<List<Conference>> GetRemoteConferencesAsync(Priority priority)
		{
			List<Conference> conferences = null;
			Task<List<Conference>> getConferencesTask;
			switch (priority)
			{
			case Priority.Background:
				getConferencesTask = _apiService.Background.GetConferences();
				break;
			case Priority.UserInitiated:
				getConferencesTask = _apiService.UserInitiated.GetConferences();
				break;
			case Priority.Speculative:
				getConferencesTask = _apiService.Speculative.GetConferences();
				break;
			default:
				getConferencesTask = _apiService.UserInitiated.GetConferences();
				break;
			}

			if (CrossConnectivity.Current.IsConnected)
			{
				conferences = await Policy
					.Handle<WebException>()
					.WaitAndRetryAsync
					(
						retryCount:5, 
						sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
					)
					.ExecuteAsync(async () => await getConferencesTask);
			}
			return conferences;
		}

		public async Task<Conference> GetRemoteConference(Priority priority, string slug)
		{
			Conference conference = null;

			Task<Conference> getConferenceTask;
			switch (priority)
			{
			case Priority.Background:
				getConferenceTask = _apiService.Background.GetConference(slug);
				break;
			case Priority.UserInitiated:
				getConferenceTask = _apiService.UserInitiated.GetConference(slug);
				break;
			case Priority.Speculative:
				getConferenceTask = _apiService.Speculative.GetConference(slug);
				break;
			default:
				getConferenceTask = _apiService.UserInitiated.GetConference(slug);
				break;
			}

			if (CrossConnectivity.Current.IsConnected)
			{
				conference = await Policy
					.Handle<Exception>()
					.RetryAsync(retryCount: 5)
					.ExecuteAsync(async () => await getConferenceTask);
			}

			return conference;
		}

	}

}