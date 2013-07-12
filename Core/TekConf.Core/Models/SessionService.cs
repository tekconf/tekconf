using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Cirrious.CrossCore.Core;
using Cirrious.MvvmCross.Plugins.File;
using Cirrious.MvvmCross.Plugins.Sqlite;
using Newtonsoft.Json;
using TekConf.Core.Entities;
using TekConf.Core.Repositories;
using TekConf.Core.Services;
using TekConf.RemoteData.Dtos.v1;

namespace TekConf.Core.Models
{
	public class SessionService
	{
		private readonly bool _isRefreshing;
		private readonly IMvxFileStore _fileStore;
		private readonly ILocalConferencesRepository _localConferencesRepository;
		private readonly ISQLiteConnectionFactory _factory;
		private readonly ICacheService _cache;
		private readonly Action<SessionDetailDto> _success;
		private readonly Action<Exception> _error;
		private string _conferenceSlug;
		private string _sessionSlug;

		private SessionService(bool isRefreshing, IMvxFileStore fileStore, 
			ILocalConferencesRepository localConferencesRepository, 
			ISQLiteConnectionFactory factory, 
			ICacheService cache, 
			Action<SessionDetailDto> success, Action<Exception> error)
		{
			_isRefreshing = isRefreshing;
			_fileStore = fileStore;
			_localConferencesRepository = localConferencesRepository;
			_factory = factory;
			_cache = cache;
			_success = success;
			_error = error;
		}

		public static void GetSessionAsync(IMvxFileStore fileStore, string conferenceSlug, string sessionSlug, bool isRefreshing, ILocalConferencesRepository localConferencesRepository, ISQLiteConnectionFactory factory, ICacheService cache, Action<SessionDetailDto> getSessionSuccess, Action<Exception> getSessionError)
		{
			MvxAsyncDispatcher.BeginAsync(() => DoAsyncGetSession(fileStore, conferenceSlug, sessionSlug, isRefreshing, localConferencesRepository, factory, cache, getSessionSuccess, getSessionError));
		}

		private static void DoAsyncGetSession(IMvxFileStore fileStore, string conferenceSlug, string sessionSlug, bool isRefreshing, ILocalConferencesRepository localConferencesRepository, ISQLiteConnectionFactory factory, ICacheService cache, Action<SessionDetailDto> getSessionSuccess, Action<Exception> getSessionError)
		{
			var search = new SessionService(isRefreshing, fileStore, localConferencesRepository, factory, cache, getSessionSuccess, getSessionError);
			search.StartGetSession(conferenceSlug, sessionSlug);
		}

		private void StartGetSession(string conferenceSlug, string sessionSlug)
		{
			try
			{
				_conferenceSlug = conferenceSlug;
				_sessionSlug = sessionSlug;

				var conference = _localConferencesRepository.Get(conferenceSlug);
				var connection = _factory.Create("conferences.db");
				var session = conference.Sessions(connection).FirstOrDefault(x => x.Slug == sessionSlug);

				var sessionDetailDto = new SessionDetailDto(session);
				if (session != null && !_isRefreshing)
				{
					_success(sessionDetailDto);
				}
				else
				{
					GetSessionFromWeb(conferenceSlug, sessionSlug);
				}
			}
			catch (Exception exception)
			{
				_error(exception);
			}
		}

		private void GetSessionFromWeb(string conferenceSlug, string sessionSlug)
		{
			var uri = string.Format(App.ApiRootUri + "conferences/{0}/sessions/{1}?format=json", conferenceSlug,
				sessionSlug);
			var request = (HttpWebRequest)WebRequest.Create(new Uri(uri));
			request.Accept = "application/json";

			request.BeginGetResponse(ReadGetSessionCallback, request);
		}

		private void ReadGetSessionCallback(IAsyncResult asynchronousResult)
		{
			try
			{
				var request = (HttpWebRequest)asynchronousResult.AsyncState;
				var response = (HttpWebResponse)request.EndGetResponse(asynchronousResult);
				using (var streamReader1 = new StreamReader(response.GetResponseStream()))
				{
					string resultString = streamReader1.ReadToEnd();
					HandleGetSessionResponse(resultString);
				}
			}
			catch (Exception exception)
			{
				_error(exception);
			}
		}


		private void HandleGetSessionResponse(string response)
		{
			if (!string.IsNullOrWhiteSpace(response))
			{
				var fullSession = JsonConvert.DeserializeObject<FullSessionDto>(response);
				var conference = _localConferencesRepository.Get(_conferenceSlug);
				var sessionEntity = new SessionEntity(conference.Id, fullSession);
				var connection = _factory.Create("conferences.db");
				var existingSession = connection.Table<SessionEntity>().Where(x => x.ConferenceId == conference.Id).FirstOrDefault(x => x.Slug == fullSession.slug);
				if (existingSession != null)
				{
					connection.Delete(existingSession);
				}

				connection.Insert(sessionEntity);
				var sessionDetailDto = new SessionDetailDto(sessionEntity);
				_success(sessionDetailDto);
			}
			else
			{
				_error(new Exception("Session not downloaded"));

			}

		}


	}

}