using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Cirrious.CrossCore.Core;
using Cirrious.MvvmCross.Plugins.File;
using Newtonsoft.Json;
using TekConf.Core.Repositories;
using TekConf.Core.Services;
using TekConf.RemoteData.Dtos.v1;

namespace TekConf.Core.Models
{
	public class SessionService
	{
		private readonly bool _isRefreshing;
		private readonly IMvxFileStore _fileStore;
		private readonly ICacheService _cache;
		private readonly ILocalSessionRepository _localSessionRepository;
		private readonly Action<SessionDetailDto> _success;
		private readonly Action<Exception> _error;
		private string _conferenceSlug;
		private string _sessionSlug;

		private SessionService(bool isRefreshing, IMvxFileStore fileStore, ICacheService cache, ILocalSessionRepository localSessionRepository, Action<SessionDetailDto> success, Action<Exception> error)
		{
			_isRefreshing = isRefreshing;
			_fileStore = fileStore;
			_cache = cache;
			_localSessionRepository = localSessionRepository;
			_success = success;
			_error = error;
		}

		public static void GetSessionAsync(IMvxFileStore fileStore, string conferenceSlug, string sessionSlug, bool isRefreshing, ICacheService cache, ILocalSessionRepository localSessionRepository, Action<SessionDetailDto> getSessionSuccess, Action<Exception> getSessionError)
		{
			MvxAsyncDispatcher.BeginAsync(() => DoAsyncGetSession(fileStore, conferenceSlug, sessionSlug, isRefreshing, cache, localSessionRepository, getSessionSuccess, getSessionError));
		}

		private static void DoAsyncGetSession(IMvxFileStore fileStore, string conferenceSlug, string sessionSlug, bool isRefreshing, ICacheService cache, ILocalSessionRepository localSessionRepository, Action<SessionDetailDto> getSessionSuccess, Action<Exception> getSessionError)
		{
			var search = new SessionService(isRefreshing, fileStore, cache, localSessionRepository, getSessionSuccess, getSessionError);
			search.StartGetSession(conferenceSlug, sessionSlug);
		}

		private void StartGetSession(string conferenceSlug, string sessionSlug)
		{
			try
			{
				_conferenceSlug = conferenceSlug;
				_sessionSlug = sessionSlug;

				var session = _localSessionRepository.GetSession(conferenceSlug, sessionSlug);

				if (session != null && !_isRefreshing)
				{
					_success(session);
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
			var request = (HttpWebRequest) WebRequest.Create(new Uri(uri));
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
				_localSessionRepository.SaveSession(_conferenceSlug, fullSession);
				var session = _localSessionRepository.GetSession(_conferenceSlug, _sessionSlug);
				if (session != null)
				{
					_success(session);
				}
				else
				{
					_error(new Exception("Session not found"));
				}
			}
			else
			{
				_error(new Exception("Session not downloaded"));

			}

		}


	}

}