using System;
using System.Collections.Generic;
using System.Net;
using Cirrious.MvvmCross.Plugins.File;
using Cirrious.MvvmCross.Plugins.Messenger;
using Cirrious.MvvmCross.Plugins.Network.Reachability;
using Cirrious.MvvmCross.Plugins.Sqlite;
using TekConf.Core.Messages;
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
			var list = new List<ConferencesListViewDto>();
			var token = new CancellationToken();

			try
			{
				var remoteConferences = await _restService.GetAsync<List<FullConferenceDto>>(conferencesUrl, token);

				if (remoteConferences != null)
				{
					remoteConferences = remoteConferences.OrderBy(x => x.start).ToList();
					foreach (var remoteConference in remoteConferences)
					{
						SaveFullRemoteConferenceToLocalEntities(remoteConference);
					}

					var localConferences = await _localConferencesRepository.ListAsync();
					foreach (var localConference in localConferences)
					{
						var existsInRemote = remoteConferences.Any(x => x.slug == localConference.Slug);
						if (!existsInRemote)
						{
							_localConferencesRepository.Delete(localConference);
						}
					}

					localConferences = await _localConferencesRepository.ListAsync();
					list = localConferences.Select(entity => new ConferencesListViewDto(entity, _fileStore)).ToList();
				}
			}
			catch (Exception)
			{

			}

			return list;
		}

		private void SaveFullRemoteConferenceToLocalEntities(FullConferenceDto remoteConference)
		{
			var localConference = new ConferenceEntity(remoteConference);
			var conferenceId = _localConferencesRepository.Save(localConference);
			var conferenceEntity = _localConferencesRepository.Get(localConference.Slug);

			foreach (var session in remoteConference.sessions)
			{
				SaveFullRemoteSessionToLocalEntities(conferenceEntity, session);
			}
		}

		private void SaveFullRemoteSessionToLocalEntities(ConferenceEntity conferenceEntity, FullSessionDto session)
		{
			var sessionEntity = new SessionEntity(conferenceEntity.Id, session);
			var sessionId = _localConferencesRepository.AddSession(sessionEntity);
			foreach (var speaker in session.speakers)
			{
				var speakerEntity = new SpeakerEntity(sessionId, speaker);
				_localConferencesRepository.AddSpeaker(speakerEntity);
			}
		}

		public async Task<IEnumerable<ConferencesListViewDto>> GetFavoritesAsync(string userName, bool isRefreshing)
		{
			IEnumerable<ConferencesListViewDto> favorites = new List<ConferencesListViewDto>();
			string getSchedulesUrl = App.ApiRootUri + "conferences/schedules?userName={0}&format=json";
			string uri = string.Format(getSchedulesUrl, userName);
			var token = new CancellationToken();

			try
			{
				var remoteFavorites = await _restService.GetAsync<List<FullConferenceDto>>(uri, token);

				if (remoteFavorites != null)
				{
					remoteFavorites = remoteFavorites.OrderBy(x => x.start).ToList();
					foreach (var scheduleConference in remoteFavorites)
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

					var localFavorites = await _localConferencesRepository.ListFavoritesAsync();
					foreach (var localConference in localFavorites)
					{
						var existsInRemote = remoteFavorites.Any(x => x.slug == localConference.Slug);
						if (!existsInRemote)
						{
							localConference.IsAddedToSchedule = false;
							_localConferencesRepository.Save(localConference);
						}
					}

					localFavorites = await _localConferencesRepository.ListFavoritesAsync();
					if (localFavorites != null && localFavorites.Any())
					{
						favorites = localFavorites.Select(c => new ConferencesListViewDto(c, _fileStore)).ToList();
					}
				}
			}
			catch (Exception)
			{

			}


			return favorites;
		}

		public async Task<ConferenceDetailViewDto> GetConferenceDetailAsync(string slug, bool isRefreshing)
		{
			var uri = string.Format(App.ApiRootUri + "conferences/{0}?format=json", slug);
			var token = new CancellationToken();

			var conferenceDetail = new ConferenceDetailViewDto();
			try
			{
				var conference = await _restService.GetAsync<FullConferenceDto>(uri, token);
				if (conference != null)
				{
					var conferenceEntity = new ConferenceEntity(conference);
					_localConferencesRepository.Save(conferenceEntity);

					conferenceDetail = new ConferenceDetailViewDto(conferenceEntity);
				}
			}
			catch (Exception)
			{
			}

			return conferenceDetail;
		}

		public async Task<ScheduleDto> RemoveFromScheduleAsync(string userName, string conferenceSlug)
		{
			string removeFromScheduleUrl = App.ApiRootUri + "conferences/{1}/schedule?userName={0}&conferenceSlug={1}&sessionSlug=&format=json";
			string uri = string.Format(removeFromScheduleUrl, userName, conferenceSlug);

			var token = new CancellationToken();

			var scheduleDto = new ScheduleDto();
			try
			{
				scheduleDto = await _restService.DeleteAsync<ScheduleDto>(uri, token);
			}
			catch (Exception)
			{
			}

			return scheduleDto;
		}

		public async Task<ScheduleDto> RemoveSessionFromScheduleAsync(string userName, string conferenceSlug, string sessionSlug)
		{
			string removeSessionFromScheduleUrl = App.ApiRootUri + "conferences/{1}/schedule?userName={0}&conferenceSlug={1}&sessionSlug={2}&format=json";
			string uri = string.Format(removeSessionFromScheduleUrl, userName, conferenceSlug, sessionSlug);

			var token = new CancellationToken();
			var scheduleDto = new ScheduleDto();
			try
			{
				scheduleDto = await _restService.DeleteAsync<ScheduleDto>(uri, token);
			}
			catch
			{

			}
			return scheduleDto;
		}

		public async Task<ScheduleDto> GetScheduleAsync(string userName, string conferenceSlug, bool isRefreshing, ISQLiteConnection connection)
		{
			string getScheduleUrl = App.ApiRootUri + "conferences/{1}/schedule?userName={0}&conferenceSlug={1}&format=json";

			string uri = string.Format(getScheduleUrl, userName, conferenceSlug);
			var token = new CancellationToken();

			var schedule = new ScheduleDto();
			try
			{
				schedule = await _restService.GetAsync<ScheduleDto>(uri, token);
				var conference = _localConferencesRepository.Get(conferenceSlug);
				if (conference != null)
				{
					conference.IsAddedToSchedule = true;
					_localConferencesRepository.Save(conference);
				}
			}
			catch (Exception)
			{

			}

			return schedule;
		}

		public async Task<string> GetIsOauthUserRegistered(string providerId)
		{
			string tekConfName = "";
			try
			{
				var token = new CancellationToken();

				string providerName = "";
				string userName = "";
				if (providerId.ToLower().Contains("twitter"))
				{
					providerName = "twitter";
					userName = providerId.ToLower().Replace("twitter:", "");
				}
				else if (providerId.ToLower().Contains("facebook"))
				{
					providerName = "facebook";
					userName = providerId.ToLower().Replace("facebook:", "");
				}
				else if (providerId.ToLower().Contains("google"))
				{
					providerName = "google";
					userName = providerId.ToLower().Replace("google:", "");
				}

				var uri = string.Format(App.WebRootUri + "account/IsOAuthUserRegistered?providerName={0}&userId={1}", providerName, userName);

				var oauthUser = await _restService.GetAsync<OAuthUserDto>(uri, token);
				if (oauthUser != null)
				{
					tekConfName = oauthUser.username;
				}
			}
			catch (Exception exception)
			{

			}

			return tekConfName;
		}

		public async Task<string> CreateOauthUser(string providerId, string userName)
		{
			string tekConfName = "";
			try
			{
				var token = new CancellationToken();

				string providerName = "";
				string userId = "";
				if (providerId.ToLower().Contains("twitter"))
				{
					providerName = "twitter";
					userId = providerId.ToLower().Replace("twitter:", "");
				}
				else if (providerId.ToLower().Contains("facebook"))
				{
					providerName = "facebook";
					userName = providerId.ToLower().Replace("facebook:", "");
				}
				else if (providerId.ToLower().Contains("google"))
				{
					providerName = "google";
					userName = providerId.ToLower().Replace("google:", "");
				}

				var uri = string.Format(App.WebRootUri + "account/CreateOauthUser?providerName={0}&userId={1}&userName={2}", providerName, userId, userName);

				var oAuthUser = await _restService.PostAsync<OAuthUserDto>(uri, null, token);
				if (oAuthUser != null)
				{
					tekConfName = oAuthUser.username;
				}
			}
			catch (Exception exception)
			{

			}

			return tekConfName;

		}

		public async Task<MobileLoginResultDto> LoginWithTekConf(string userName, string password)
		{
			var result = new MobileLoginResultDto();
			try
			{
				var token = new CancellationToken();
				var uri = string.Format(App.WebRootUri + "account/MobileLogin?UserName={0}&Password={1}", userName, password);
				result = await _restService.PostAsync<MobileLoginResultDto>(uri, null, token);
			}
			catch (Exception exception)
			{
			}

			return result;
		}

		public async Task<SessionDetailDto> GetSessionAsync(string conferenceSlug, string sessionSlug)
		{
			var uri = string.Format(App.ApiRootUri + "conferences/{0}/sessions/{1}?format=json", conferenceSlug, sessionSlug);
			var token = new CancellationToken();
			var fullSession = await _restService.GetAsync<FullSessionDto>(uri, token);
			var conference = _localConferencesRepository.Get(conferenceSlug);

			SaveFullRemoteSessionToLocalEntities(conference, fullSession);

			var sessionEntity = _localConferencesRepository.Get(conference.Slug, fullSession.slug);
			var sessionDetailDto = new SessionDetailDto(sessionEntity);

			return sessionDetailDto;
		}

		public async Task<ConferenceSessionsListViewDto> GetConferenceSessionsList(string slug)
		{
			var conference = _localConferencesRepository.Get(slug);

			var sessions = conference.Sessions(_connection);
			var conferenceSessionListView = new ConferenceSessionsListViewDto(sessions)
			{
				name = conference.Name,
				slug = conference.Slug
			};

			return conferenceSessionListView;
		}

		public void AddToSchedule(string userName, string conferenceSlug, Action<ScheduleDto> success = null, Action<Exception> error = null)
		{
			ScheduleService.AddToScheduleAsync(_fileStore, _localConferencesRepository, userName, conferenceSlug, false, _cache, _connection, success, error);
		}

		public void AddSessionToSchedule(string userName, string conferenceSlug, string sessionSlug, Action<ScheduleDto> success = null, Action<Exception> error = null)
		{
			ScheduleService.AddSessionToScheduleAsync(_fileStore, _localConferencesRepository, userName, conferenceSlug, sessionSlug, false, _cache, _connection, success, error);
		}

	}
}