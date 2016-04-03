using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Fusillade;
using Polly;
using Tekconf.DTO;
using Plugin.Connectivity;

namespace TekConf.Mobile.Core.Services
{
	public class RemoteConferencesService : IRemoteConferencesService
	{
		private readonly IApiService _apiService;
		public RemoteConferencesService(IApiService apiService)
		{
			_apiService = apiService;
		}

		public async Task<Conference> GetConference(Priority priority, string slug)
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

		public async Task<List<Conference>> GetConferences(Priority priority)
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
						retryCount: 5,
						sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
					)
					.ExecuteAsync(async () => await getConferencesTask);
			}
			return conferences;
		}
	}
}