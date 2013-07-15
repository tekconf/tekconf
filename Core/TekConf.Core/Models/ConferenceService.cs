using System;
using System.IO;
using System.Net;
using Cirrious.CrossCore.Core;
using Cirrious.MvvmCross.Plugins.File;
using Cirrious.MvvmCross.Plugins.Network.Reachability;
using Newtonsoft.Json;
using TekConf.Core.Entities;
using TekConf.Core.Repositories;
using TekConf.Core.Services;
using TekConf.RemoteData.Dtos.v1;

namespace TekConf.Core.Models
{
	using Cirrious.MvvmCross.Plugins.Sqlite;

	public class ConferenceService
	{
		private readonly IMvxFileStore _fileStore;
		private readonly IMvxReachability _reachability;
		private readonly bool _isRefreshing;
		private readonly ICacheService _cache;

		private readonly ISQLiteConnection _connection;

		private readonly IAuthentication _authentication;
		private readonly ILocalConferencesRepository _localConferencesRepository;
		private readonly Action<ConferenceDetailViewDto> _success;
		private readonly Action<Exception> _error;
		private string _slug;

		private ConferenceService(IMvxFileStore fileStore, IMvxReachability reachability, bool isRefreshing, ICacheService cache, ISQLiteConnection connection, IAuthentication authentication, ILocalConferencesRepository localConferencesRepository, Action<ConferenceDetailViewDto> success, Action<Exception> error)
		{
			_fileStore = fileStore;
			_reachability = reachability;
			_isRefreshing = isRefreshing;
			_cache = cache;
			this._connection = connection;
			_authentication = authentication;
			_localConferencesRepository = localConferencesRepository;
			_success = success;
			_error = error;
		}

		public static void GetConferenceDetailAsync(IMvxFileStore fileStore, ILocalConferencesRepository localConferencesRepository, IMvxReachability reachability, string slug, bool isRefreshing, ICacheService cache, IAuthentication authentication, ISQLiteConnection connection, Action<ConferenceDetailViewDto> success, Action<Exception> error)
		{
			MvxAsyncDispatcher.BeginAsync(() =>
				DoAsyncGetConferenceDetail(fileStore, localConferencesRepository, reachability, slug, isRefreshing, cache, authentication,connection, success, error)
				);
		}

		private static void DoAsyncGetConferenceDetail(IMvxFileStore fileStore, ILocalConferencesRepository localConferencesRepository, IMvxReachability reachability, string slug, bool isRefreshing, ICacheService cache, IAuthentication authentication, ISQLiteConnection connection, Action<ConferenceDetailViewDto> success, Action<Exception> error)
		{
			var search = new ConferenceService(fileStore, reachability, isRefreshing, cache, connection, authentication, localConferencesRepository, success, error);
			search.StartGetConferenceDetail(slug);
		}

		private ConferenceDetailViewDto _conference;
		private void GetConferenceDetailSuccess(ConferenceDetailViewDto conference)
		{
			if (_authentication.IsAuthenticated)
			{
				_conference = conference;
				ScheduleService.GetScheduleAsync(_fileStore, _localConferencesRepository, _authentication.UserName, conference.slug, false, _cache, _connection, GetScheduleSuccess, GetScheduleError);
			}
			else
			{
				_conference = conference;
				_success(conference);
			}
		}

		private void GetScheduleError(Exception exception)
		{
			_success(_conference);
		}

		private void GetScheduleSuccess(ScheduleDto schedule)
		{
			if (schedule != null && !string.IsNullOrWhiteSpace(schedule.conferenceSlug))
			{
				_conference.isAddedToSchedule = true;
			}

			_success(_conference);
		}

		private void StartGetConferenceDetail(string slug)
		{
			_slug = slug;

			try
			{
				GetRemoteConference(slug);
			}
			catch (Exception exception)
			{
				_error(exception);
			}
		}

		private void GetRemoteConference(string slug)
		{
			var uri = string.Format(App.ApiRootUri + "conferences/{0}?format=json", slug);

			var request = (HttpWebRequest)WebRequest.Create(new Uri(uri));
			request.Accept = "application/json";
			request.BeginGetResponse(ReadGetConferenceCallback, request);
		}

		private void ReadGetConferenceCallback(IAsyncResult asynchronousResult)
		{
			try
			{
				var request = (HttpWebRequest)asynchronousResult.AsyncState;
				var response = (HttpWebResponse)request.EndGetResponse(asynchronousResult);
				using (var streamReader1 = new StreamReader(response.GetResponseStream()))
				{
					string resultString = streamReader1.ReadToEnd();
					HandleGetConferenceResponse(resultString);
				}
			}
			catch (Exception exception)
			{
				_error(exception);
			}
		}

		private void HandleGetConferenceResponse(string response)
		{
			var conference = JsonConvert.DeserializeObject<FullConferenceDto>(response);

			var conferenceEntity = new ConferenceEntity(conference);
			_localConferencesRepository.Save(conferenceEntity);

			var conferenceDetail = new ConferenceDetailViewDto(conferenceEntity);

			GetConferenceDetailSuccess(conferenceDetail);
		}

	}
}