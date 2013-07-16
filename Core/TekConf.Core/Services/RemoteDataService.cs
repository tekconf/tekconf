using System;
using System.Collections.Generic;
using Cirrious.MvvmCross.Plugins.File;
using Cirrious.MvvmCross.Plugins.Messenger;
using Cirrious.MvvmCross.Plugins.Network.Reachability;
using Cirrious.MvvmCross.Plugins.Sqlite;
using TekConf.Core.Models;
using TekConf.Core.Repositories;
using TekConf.RemoteData.Dtos.v1;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TekConf.Core.Entities;

namespace TekConf.Core.Services
{
	using Newtonsoft.Json;

	public class RemoteDataService : IRemoteDataService
	{
		private readonly IMvxFileStore _fileStore;
		private readonly ICacheService _cache;
		private readonly IAuthentication _authentication;
		private readonly ILocalConferencesRepository _localConferencesRepository;
		private readonly ISQLiteConnection _connection;
		private readonly IMvxMessenger _messenger;
		private readonly IRestService _restService;
		private readonly IMvxReachability _reachability;

		public RemoteDataService(IMvxFileStore fileStore, ICacheService cache, IAuthentication authentication, ILocalConferencesRepository localScheduleRepository, 
																										ILocalConferencesRepository localConferencesRepository, ISQLiteConnection connection, IMvxMessenger messenger, IRestService restService)
		{
			_fileStore = fileStore;
			_cache = cache;
			_authentication = authentication;
			_localConferencesRepository = localConferencesRepository;
			_connection = connection;
			_messenger = messenger;
			_restService = restService;
			_reachability = null;
		}



		public async Task<IEnumerable<ConferencesListViewDto>> GetConferencesAsync()
		{
			string conferencesUrl = App.ApiRootUri + "conferences?format=json";
			
			var token = new CancellationToken();
			var conferences = await _restService.GetAsync<List<FullConferenceDto>>(conferencesUrl, token);

			conferences = conferences.OrderBy(x => x.start).ToList();
			foreach (var conference in conferences)
			{
				var entity = new ConferenceEntity(conference);
				_localConferencesRepository.Save(entity);
				var conferenceEntity = _localConferencesRepository.Get(entity.Slug);

				foreach (var session in conference.sessions)
				{
					var sessionEntity = new SessionEntity(conferenceEntity.Id, session);
					_localConferencesRepository.AddSession(sessionEntity);
				}
			}

			var results = await _localConferencesRepository.ListAsync();

			var list = results.Select(entity => new ConferencesListViewDto(entity, _fileStore)).ToList();

			return list;
		}

		public async Task<IEnumerable<ConferencesListViewDto>> GetFavoritesAsync(string userName, bool isRefreshing)
		{
			IEnumerable<ConferencesListViewDto> favorites = null;
			string getSchedulesUrl = App.ApiRootUri + "conferences/schedules?userName={0}&format=json";
			string uri = string.Format(getSchedulesUrl, userName);
			var token = new CancellationToken();
			var scheduledConferences = await _restService.GetAsync<List<FullConferenceDto>>(uri, token);

			if (scheduledConferences != null)
			{
				scheduledConferences = scheduledConferences.OrderBy(x => x.start).ToList();
				foreach (var scheduleConference in scheduledConferences)
				{
					var conference = _localConferencesRepository.Get(scheduleConference.slug);
					if (conference != null)
					{
						conference.IsAddedToSchedule = true;
						_localConferencesRepository.Save(conference);
					}
					else
					{
						var entity = new ConferenceEntity(scheduleConference) { IsAddedToSchedule = true };
						_localConferencesRepository.Save(entity);
					}
				}

				var conferences = await _localConferencesRepository.ListFavoritesAsync();
				if (conferences != null && conferences.Any())
				{
					favorites = conferences.Select(c => new ConferencesListViewDto(c, _fileStore)).ToList();
				}
			}

			return favorites;
		}

		
		public void GetConferenceSessionsList(string slug, bool isRefreshing, Action<ConferenceSessionsListViewDto> success = null, Action<Exception> error = null)
		{
			ConferenceSessionsService.GetConferenceSessionsAsync(_fileStore, _localConferencesRepository, _reachability, slug, isRefreshing, null, _authentication, _connection, success, error);
		}

		public async Task<ConferenceDetailViewDto> GetConferenceDetailAsync(string slug, bool isRefreshing)
		{
			var uri = string.Format(App.ApiRootUri + "conferences/{0}?format=json", slug);
			var token = new CancellationToken();
			var conference = await _restService.GetAsync<FullConferenceDto>(uri, token);

			var conferenceEntity = new ConferenceEntity(conference);
			_localConferencesRepository.Save(conferenceEntity);

			var conferenceDetail = new ConferenceDetailViewDto(conferenceEntity);

			return conferenceDetail;
		}

		public void GetSchedule(string userName, string conferenceSlug, bool isRefreshing, ISQLiteConnection connection, Action<ScheduleDto> success = null, Action<Exception> error = null)
		{
			ScheduleService.GetScheduleAsync(_fileStore, _localConferencesRepository, userName, conferenceSlug, isRefreshing, _cache, connection, success, error);
		}

		public void LoginWithTekConf(string userName, string password, Action<bool, string> success = null, Action<Exception> error = null)
		{
			UserService.GetAuthenticationAsync(userName, password, _messenger, success, error);
		}

		public void AddToSchedule(string userName, string conferenceSlug, Action<ScheduleDto> success = null, Action<Exception> error = null)
		{
			ScheduleService.AddToScheduleAsync(_fileStore, _localConferencesRepository, userName, conferenceSlug, false, _cache, _connection, success, error);
		}

		public void AddSessionToSchedule(string userName, string conferenceSlug, string sessionSlug, Action<ScheduleDto> success = null, Action<Exception> error = null)
		{
			ScheduleService.AddSessionToScheduleAsync(_fileStore, _localConferencesRepository, userName, conferenceSlug, sessionSlug, false, _cache, _connection, success, error);
		}

		public void RemoveFromSchedule(string userName, string conferenceSlug, Action<ScheduleDto> success = null, Action<Exception> error = null)
		{
			ScheduleService.RemoveFromScheduleAsync(_fileStore, _localConferencesRepository, userName, conferenceSlug, false, _cache, _connection, success, error);
		}

		public void RemoveSessionFromSchedule(string userName, string conferenceSlug, string sessionSlug, Action<ScheduleDto> success = null, Action<Exception> error = null)
		{
			ScheduleService.RemoveSessionFromScheduleAsync(_fileStore, _localConferencesRepository, userName, conferenceSlug, sessionSlug, false, _cache, _connection, success, error);
		}

		public void GetSession(string conferenceSlug, string sessionSlug, bool isRefreshing, Action<SessionDetailDto> success, Action<Exception> error)
		{
			SessionService.GetSessionAsync(_fileStore, conferenceSlug, sessionSlug, isRefreshing, _localConferencesRepository, _cache, _connection, success, error);
		}

		public void GetIsOauthUserRegistered(string userId, Action<string> success, Action<Exception> error)
		{
			UserService.GetIsOauthUserRegisteredAsync(userId, _messenger, success, error);
		}

		public void CreateOauthUser(string userId, string userName, Action<string> success, Action<Exception> error)
		{
			UserService.CreateOauthUserAsync(userId, userName, _messenger, success, error);
		}
	}
}