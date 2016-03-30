using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Akavache;

using Fusillade;
using Polly;

using Tekconf.DTO;
using Plugin.Connectivity;
using TekConf.Mobile.Core.Messages;
using MvvmCross.Plugins.Messenger;

namespace TekConf.Mobile.Core.Services
{
	public class SchedulesService : ISchedulesService
	{
		private readonly IApiService _apiService;
	    private readonly ISettingsService _settingsService;
		private readonly IMvxMessenger _messenger;

		public SchedulesService(IApiService apiService, ISettingsService settingsService, IMvxMessenger messenger)
	    {
			_messenger = messenger;
			_apiService = apiService;
	        _settingsService = settingsService;
	    }

	    public async Task<List<Schedule>> GetSchedules(Priority priority)
		{
			var cache = BlobCache.LocalMachine;
			if (priority == Priority.UserInitiated) {
				BlobCache.LocalMachine.InvalidateAll ().Subscribe ();
			}

	        if (string.IsNullOrWhiteSpace(_settingsService.UserIdToken))
	        {
	            return Enumerable.Empty<Schedule>().ToList();
	        }

			var cachedSchedules = cache.GetAndFetchLatest("schedules", () => GetRemoteSchedulesAsync(priority),
				offset =>
				{
					TimeSpan elapsed = DateTimeOffset.Now - offset;
					return elapsed > new TimeSpan(hours: 24, minutes: 0, seconds: 0);
				});

			var schedules = await cachedSchedules.FirstOrDefaultAsync();

			return schedules;
		}

		public async Task<Schedule> GetSchedule(Priority priority, string slug)
		{
            if (string.IsNullOrWhiteSpace(_settingsService.UserIdToken))
            {
                return null;
            }
            var cachedSchedule = BlobCache.LocalMachine.GetAndFetchLatest($"schedule-{slug}", () => GetRemoteSchedule(priority, slug), offset =>
				{
					TimeSpan elapsed = DateTimeOffset.Now - offset;
					return elapsed > new TimeSpan(hours: 0, minutes: 30, seconds: 0);
				});

			var conference = await cachedSchedule.FirstOrDefaultAsync();

			return conference;
		}

		private async Task<List<Schedule>> GetRemoteSchedulesAsync(Priority priority)
		{
			List<Schedule> schedules = null;
			Task<List<Schedule>> getSchedulesTask;
			switch (priority)
			{
			case Priority.Background:
				getSchedulesTask = _apiService.Background.GetSchedules();
				break;
			case Priority.UserInitiated:
				getSchedulesTask = _apiService.UserInitiated.GetSchedules();
				break;
			case Priority.Speculative:
				getSchedulesTask = _apiService.Speculative.GetSchedules();
				break;
			default:
				getSchedulesTask = _apiService.UserInitiated.GetSchedules();
				break;
			}

			if (CrossConnectivity.Current.IsConnected)
			{
				schedules = await Policy
					.Handle<WebException>()
					.WaitAndRetryAsync
					(
						retryCount:5, 
						sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
					)
					.ExecuteAsync(async () => await getSchedulesTask);
			}
			return schedules;
		}

		public async Task<Schedule> GetRemoteSchedule(Priority priority, string slug)
		{
			Schedule schedule = null;

			Task<Schedule> getScheduleTask;
			switch (priority)
			{
			case Priority.Background:
				getScheduleTask = _apiService.Background.GetSchedule(slug);
				break;
			case Priority.UserInitiated:
				getScheduleTask = _apiService.UserInitiated.GetSchedule(slug);
				break;
			case Priority.Speculative:
				getScheduleTask = _apiService.Speculative.GetSchedule(slug);
				break;
			default:
				getScheduleTask = _apiService.UserInitiated.GetSchedule(slug);
				break;
			}

			if (CrossConnectivity.Current.IsConnected)
			{
				schedule = await Policy
					.Handle<Exception>()
					.RetryAsync(retryCount: 5)
					.ExecuteAsync(async () => await getScheduleTask);
			}

			return schedule;
		}

		public async Task<Schedule> AddToSchedule (Priority priority, string slug)
		{
			Schedule schedule = null;

			Task<Schedule> addToScheduleTask;
			switch (priority)
			{
			case Priority.Background:
				addToScheduleTask = _apiService.Background.AddToSchedule(slug);
				break;
			case Priority.UserInitiated:
				addToScheduleTask = _apiService.UserInitiated.AddToSchedule(slug);
				break;
			case Priority.Speculative:
				addToScheduleTask = _apiService.Speculative.AddToSchedule(slug);
				break;
			default:
				addToScheduleTask = _apiService.UserInitiated.AddToSchedule(slug);
				break;
			}

			if (CrossConnectivity.Current.IsConnected)
			{
				schedule = await Policy
					.Handle<Exception>()
					.RetryAsync(retryCount: 5)
					.ExecuteAsync(async () => await addToScheduleTask);
			}

			var conferenceAddedMessage = new ConferenceAddedToScheduleMessage(this, slug);
			_messenger.Publish(conferenceAddedMessage);
			return schedule;
		}

        public async Task RemoveFromSchedule(Priority priority, string slug)
        {
            Task removeFromScheduleTask;
            switch (priority)
            {
                case Priority.Background:
                    removeFromScheduleTask = _apiService.Background.RemoveFromSchedule(slug);
                    break;
                case Priority.UserInitiated:
                    removeFromScheduleTask = _apiService.UserInitiated.RemoveFromSchedule(slug);
                    break;
                case Priority.Speculative:
                    removeFromScheduleTask = _apiService.Speculative.RemoveFromSchedule(slug);
                    break;
                default:
                    removeFromScheduleTask = _apiService.UserInitiated.RemoveFromSchedule(slug);
                    break;
            }

            if (CrossConnectivity.Current.IsConnected)
            {
                await Policy
                    .Handle<Exception>()
                    .RetryAsync(retryCount: 5)
                    .ExecuteAsync(async () => await removeFromScheduleTask);
            }

			_messenger.Publish(new ConferenceRemovedFromScheduleMessage(this, slug));

        }


    }
}