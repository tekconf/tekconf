using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Cirrious.CrossCore.Core;
using Cirrious.MvvmCross.Plugins.File;
using Cirrious.MvvmCross.ViewModels;
using Newtonsoft.Json;
using TekConf.Core.Repositories;
using TekConf.Core.Services;
using TekConf.Core.ViewModels;
using TekConf.RemoteData.Dtos.v1;

namespace TekConf.Core.Models
{
	using Cirrious.MvvmCross.Plugins.Sqlite;

	using TekConf.Core.Entities;

	public class ScheduleService
	{

		private readonly string GetSchedulesUrl = App.ApiRootUri + "conferences/schedules?userName={0}&format=json";
		private readonly string GetScheduleUrl = App.ApiRootUri + "conferences/{1}/schedule?userName={0}&conferenceSlug={1}&format=json";
		private readonly string AddToScheduleUrl = App.ApiRootUri + "conferences/{1}/schedule?userName={0}&conferenceSlug={1}&sessionSlug=&format=json";
		private readonly string AddSessionToScheduleUrl = App.ApiRootUri + "conferences/{1}/schedule?userName={0}&conferenceSlug={1}&sessionSlug={2}&format=json";
		private readonly string RemoveFromScheduleUrl = App.ApiRootUri + "conferences/{1}/schedule?userName={0}&conferenceSlug={1}&sessionSlug=&format=json";
		private readonly string RemoveSessionFromScheduleUrl = App.ApiRootUri + "conferences/{1}/schedule?userName={0}&conferenceSlug={1}&sessionSlug={2}&format=json";

		private readonly IMvxFileStore _fileStore;
		private readonly ILocalConferencesRepository _localConferencesRepository;
		private readonly Action<IEnumerable<ConferencesListViewDto>> _getSchedulesSuccess;
		private readonly Action<ScheduleDto> _getScheduleSuccess;

		private readonly Action<Exception> _getSchedulesError;
		private readonly Action<Exception> _getScheduleError;

		private readonly Action<ScheduleDto> _addToScheduleSuccess;
		private readonly Action<Exception> _addToScheduleError;
		private readonly Action<ScheduleDto> _removeFromScheduleSuccess;
		private readonly Action<Exception> _removeFromScheduleError;

		private readonly string _userName;
		private readonly bool _isRefreshing;
		private readonly ICacheService _cache;

		private readonly ISQLiteConnection _connection;

		//private readonly string _conferenceSlug;

		private ScheduleService(IMvxFileStore fileStore, ILocalConferencesRepository localConferencesRepository, 
			string userName, bool isRefreshing, ICacheService cache, ISQLiteConnection connection,
			Action<IEnumerable<ConferencesListViewDto>> success, Action<Exception> error)
		{
			_fileStore = fileStore;
			_localConferencesRepository = localConferencesRepository;
			_getSchedulesSuccess = success;
			_getSchedulesError = error;
			_userName = userName;
			_isRefreshing = isRefreshing;
			_cache = cache;
			_connection = connection;
		}

		private ScheduleService(IMvxFileStore fileStore, ILocalConferencesRepository localConferencesRepository,
			string userName, bool isRefreshing, ICacheService cache, ISQLiteConnection connection,
			Action<ScheduleDto> success, Action<Exception> error)
		{
			_fileStore = fileStore;
			_localConferencesRepository = localConferencesRepository;

			_addToScheduleSuccess = success;
			_removeFromScheduleSuccess = success;
			_getScheduleSuccess = success;
			//_getSchedulesSuccess = success;

			_addToScheduleError = error;
			_removeFromScheduleError = error;
			_getScheduleError = error;
			_getSchedulesError = error;

			_userName = userName;
			_isRefreshing = isRefreshing;
			_cache = cache;
			_connection = connection;
		}



		public static void AddToScheduleAsync(IMvxFileStore fileStore, ILocalConferencesRepository localScheduleRepository, string userName, string conferenceSlug, bool isRefreshing, ICacheService cache, ISQLiteConnection connection, Action<ScheduleDto> success, Action<Exception> error)
		{
			MvxAsyncDispatcher.BeginAsync(() => DoAsyncAddToScheduleSchedule(fileStore, localScheduleRepository, userName, conferenceSlug, isRefreshing, cache, connection, success, error));
		}

		public static void AddSessionToScheduleAsync(IMvxFileStore fileStore, ILocalConferencesRepository localScheduleRepository, string userName, string conferenceSlug, string sessionSlug, bool isRefreshing, ICacheService cache, ISQLiteConnection connection, Action<ScheduleDto> success, Action<Exception> error)
		{
			MvxAsyncDispatcher.BeginAsync(() => DoAsyncAddSessionToScheduleSchedule(fileStore, localScheduleRepository, userName, conferenceSlug, sessionSlug, isRefreshing, cache, connection, success, error));
		}

		public static void RemoveFromScheduleAsync(IMvxFileStore fileStore, ILocalConferencesRepository localScheduleRepository, string userName, string conferenceSlug, bool isRefreshing, ICacheService cache, ISQLiteConnection connection, Action<ScheduleDto> success, Action<Exception> error)
		{
			MvxAsyncDispatcher.BeginAsync(() => DoAsyncRemoveFromSchedule(fileStore, localScheduleRepository, userName, conferenceSlug, isRefreshing, cache,connection, success, error));
		}

		public static void RemoveSessionFromScheduleAsync(IMvxFileStore fileStore, ILocalConferencesRepository localScheduleRepository, string userName, string conferenceSlug, string sessionSlug, bool isRefreshing, ICacheService cache, ISQLiteConnection connection, Action<ScheduleDto> success, Action<Exception> error)
		{
			MvxAsyncDispatcher.BeginAsync(() => DoAsyncRemoveSessionFromSchedule(fileStore, localScheduleRepository, userName, conferenceSlug, sessionSlug, isRefreshing, cache, connection, success, error));
		}

		public static void GetScheduleAsync(IMvxFileStore fileStore, ILocalConferencesRepository localScheduleRepository, string userName, string conferenceSlug, bool isRefreshing, ICacheService cache, ISQLiteConnection connection, Action<ScheduleDto> success, Action<Exception> error)
		{
			MvxAsyncDispatcher.BeginAsync(() => DoAsyncGetSchedule(fileStore, localScheduleRepository, userName, conferenceSlug, isRefreshing, cache, connection, success, error));
		}



		private static void DoAsyncGetSchedule(IMvxFileStore fileStore, ILocalConferencesRepository localScheduleRepository, string userName, string conferenceSlug, bool isRefreshing, ICacheService cache, ISQLiteConnection connection, Action<ScheduleDto> success, Action<Exception> error)
		{
			var search = new ScheduleService(fileStore, localScheduleRepository, userName, isRefreshing, cache, connection, success, error);
			search.GetSchedule(conferenceSlug);
		}

		private static void DoAsyncAddToScheduleSchedule(IMvxFileStore fileStore, ILocalConferencesRepository localScheduleRepository, string userName, string conferenceSlug, bool isRefreshing, ICacheService cache, ISQLiteConnection connection, Action<ScheduleDto> success, Action<Exception> error)
		{
			var search = new ScheduleService(fileStore, localScheduleRepository, userName, isRefreshing, cache, connection, success, error);
			search.StartAddToSchedule(conferenceSlug);
		}

		private static void DoAsyncAddSessionToScheduleSchedule(IMvxFileStore fileStore, ILocalConferencesRepository localScheduleRepository, string userName, string conferenceSlug, string sessionSlug, bool isRefreshing, ICacheService cache, ISQLiteConnection connection, Action<ScheduleDto> success, Action<Exception> error)
		{
			var search = new ScheduleService(fileStore, localScheduleRepository, userName, isRefreshing, cache, connection, success, error);
			search.StartAddSessionToSchedule(conferenceSlug, sessionSlug);
		}

		private static void DoAsyncRemoveFromSchedule(IMvxFileStore fileStore, ILocalConferencesRepository localScheduleRepository, string userName, string conferenceSlug, bool isRefreshing, ICacheService cache, ISQLiteConnection connection, Action<ScheduleDto> success, Action<Exception> error)
		{
			var search = new ScheduleService(fileStore, localScheduleRepository, userName, isRefreshing, cache, connection, success, error);
			search.StartRemoveFromSchedule(conferenceSlug);
		}

		private static void DoAsyncRemoveSessionFromSchedule(IMvxFileStore fileStore, ILocalConferencesRepository localScheduleRepository, string userName, string conferenceSlug, string sessionSlug, bool isRefreshing, ICacheService cache, ISQLiteConnection connection, Action<ScheduleDto> success, Action<Exception> error)
		{
			var search = new ScheduleService(fileStore, localScheduleRepository, userName, isRefreshing, cache, connection, success, error);
			search.StartRemoveSessionFromSchedule(conferenceSlug, sessionSlug);
		}





		private void GetSchedule(string conferenceSlug)
		{
			try
			{
				var conference = _localConferencesRepository.Get(conferenceSlug);

				if (conference != null && conference.IsAddedToSchedule && !_isRefreshing)
				{
					var dto = new ScheduleDto() { conferenceSlug = conference.Slug, sessions = conference.Sessions(_connection).Select(x => new FullSessionDto(x)).ToList(), url = "", userSlug = _userName };
					_getScheduleSuccess(dto);
				}
				else
				{
					// perform the search
					GetScheduleFromWeb(conferenceSlug);
				}
			}
			catch (Exception exception)
			{
				_getScheduleError(exception);
			}
		}

		private void GetScheduleFromWeb(string conferenceSlug)
		{
			string uri = string.Format(GetScheduleUrl, _userName, conferenceSlug);
			var request = (HttpWebRequest)WebRequest.Create(new Uri(uri));
			request.Accept = "application/json";

			request.BeginGetResponse(ReadGetScheduleCallback, request);
		}

		private void StartAddToSchedule(string conferenceSlug)
		{
			try
			{
				// perform the search
				string uri = string.Format(AddToScheduleUrl, _userName, conferenceSlug);
				var request = (HttpWebRequest)WebRequest.Create(new Uri(uri));
				request.Accept = "application/json";

				request.Method = "POST";
				request.BeginGetResponse(ReadAddToScheduleCallback, request);
			}
			catch (Exception exception)
			{
				_getSchedulesError(exception);
			}
		}

		private void StartAddSessionToSchedule(string conferenceSlug, string sessionSlug)
		{
			try
			{
				string uri = string.Format(AddSessionToScheduleUrl, _userName, conferenceSlug, sessionSlug);
				var request = (HttpWebRequest)WebRequest.Create(new Uri(uri));
				request.Accept = "application/json";

				request.Method = "POST";
				request.BeginGetResponse(ReadAddToScheduleCallback, request);
			}
			catch (Exception exception)
			{
				_getSchedulesError(exception);
			}
		}

		private void StartRemoveFromSchedule(string conferenceSlug)
		{
			try
			{
				// perform the search
				string uri = string.Format(RemoveFromScheduleUrl, _userName, conferenceSlug);
				var request = (HttpWebRequest)WebRequest.Create(new Uri(uri));
				request.Accept = "application/json";

				request.Method = "DELETE";
				request.BeginGetResponse(ReadRemoveFromScheduleCallback, request);
			}
			catch (Exception exception)
			{
				_getSchedulesError(exception);
			}
		}

		private void StartRemoveSessionFromSchedule(string conferenceSlug, string sessionSlug)
		{
			try
			{
				// perform the search
				string uri = string.Format(RemoveSessionFromScheduleUrl, _userName, conferenceSlug, sessionSlug);
				var request = (HttpWebRequest)WebRequest.Create(new Uri(uri));
				request.Accept = "application/json";

				request.Method = "DELETE";
				request.BeginGetResponse(ReadRemoveFromScheduleCallback, request);
			}
			catch (Exception exception)
			{
				_getSchedulesError(exception);
			}
		}

		private void ReadGetSchedulesCallback(IAsyncResult asynchronousResult)
		{
			try
			{
				var request = (HttpWebRequest)asynchronousResult.AsyncState;
				var response = (HttpWebResponse)request.EndGetResponse(asynchronousResult);
				using (var streamReader1 = new StreamReader(response.GetResponseStream()))
				{
					string resultString = streamReader1.ReadToEnd();
					HandleGetSchedulesResponse(resultString);
				}
			}
			catch (Exception exception)
			{
				_getSchedulesError(exception);
			}
		}

		private void ReadGetScheduleCallback(IAsyncResult asynchronousResult)
		{
			try
			{
				var request = (HttpWebRequest)asynchronousResult.AsyncState;
				var response = (HttpWebResponse)request.EndGetResponse(asynchronousResult);
				using (var streamReader1 = new StreamReader(response.GetResponseStream()))
				{
					string resultString = streamReader1.ReadToEnd();
					HandleGetScheduleResponse(resultString);
				}
			}
			catch (Exception exception)
			{
				_getScheduleError(exception);
			}
		}

		private void ReadAddToScheduleCallback(IAsyncResult asynchronousResult)
		{
			try
			{
				var request = (HttpWebRequest)asynchronousResult.AsyncState;
				var response = (HttpWebResponse)request.EndGetResponse(asynchronousResult);
				using (var streamReader1 = new StreamReader(response.GetResponseStream()))
				{
					string resultString = streamReader1.ReadToEnd();
					HandleAddToScheduleResponse(resultString);
				}
			}
			catch (Exception exception)
			{
				_getSchedulesError(exception);
			}
		}

		private void ReadRemoveFromScheduleCallback(IAsyncResult asynchronousResult)
		{
			try
			{
				var request = (HttpWebRequest)asynchronousResult.AsyncState;
				var response = (HttpWebResponse)request.EndGetResponse(asynchronousResult);
				if (response.StatusCode != HttpStatusCode.OK)
				{
					_getSchedulesError(new Exception("Could not remove conference from schedule"));
				}
				else
				{
					using (var streamReader1 = new StreamReader(response.GetResponseStream()))
					{
						string resultString = streamReader1.ReadToEnd();
						HandleRemoveFromScheduleResponse(resultString);
					}
				}

			}
			catch (Exception exception)
			{
				_getSchedulesError(exception);
			}
		}

		private async void HandleGetSchedulesResponse(string response)
		{
			var scheduledConferences = JsonConvert.DeserializeObject<List<FullConferenceDto>>(response).OrderBy(x => x.start).ToList();
			
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
				var dtos = conferences.Select(c => new ConferencesListViewDto(c, _fileStore)).ToList();
				_getSchedulesSuccess(dtos);
			}
		}

		private void HandleGetScheduleResponse(string response)
		{
			try
			{
				var schedule = JsonConvert.DeserializeObject<ScheduleDto>(response);
				var conference = _localConferencesRepository.Get(schedule.conferenceSlug);
				if (conference != null)
				{
					conference.IsAddedToSchedule = true;
					_localConferencesRepository.Save(conference);
				}

				_getScheduleSuccess(schedule);
			}
			catch (Exception ex)
			{
				_getScheduleError(ex);
			}

		}

		private void HandleAddToScheduleResponse(string response)
		{
			var scheduleDto = JsonConvert.DeserializeObject<ScheduleDto>(response);
			var conference = _localConferencesRepository.Get(scheduleDto.conferenceSlug);
			if (conference != null)
			{
				conference.IsAddedToSchedule = true;
				_localConferencesRepository.Save(conference);
			}

			_addToScheduleSuccess(scheduleDto);
		}

		private void HandleRemoveFromScheduleResponse(string response)
		{
			var scheduleDto = JsonConvert.DeserializeObject<ScheduleDto>(response);
			var conference = _localConferencesRepository.Get(scheduleDto.conferenceSlug);
			if (conference != null)
			{
				conference.IsAddedToSchedule = false;
				_localConferencesRepository.Save(conference);
			}

			_removeFromScheduleSuccess(scheduleDto);
		}


	}

}
