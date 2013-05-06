using System;
using System.IO;
using System.Net;
using Cirrious.CrossCore.Core;
using Cirrious.MvvmCross.Plugins.File;
using Newtonsoft.Json;
using TekConf.RemoteData.Dtos.v1;

namespace TekConf.Core.Models
{
	public class SessionService
	{
		private readonly bool _isRefreshing;
		private readonly IMvxFileStore _fileStore;
		private readonly Action<FullSessionDto> _success;
		private readonly Action<Exception> _error;
		private string _conferenceSlug;
		private string _sessionSlug;

		private SessionService(bool isRefreshing, IMvxFileStore fileStore, Action<FullSessionDto> success, Action<Exception> error)
		{
			_isRefreshing = isRefreshing;
			_fileStore = fileStore;
			_success = success;
			_error = error;
		}

		public static void GetSessionAsync(IMvxFileStore fileStore, string conferenceSlug, string sessionSlug, bool isRefreshing, Action<FullSessionDto> getSessionSuccess, Action<Exception> getSessionError)
		{
			MvxAsyncDispatcher.BeginAsync(() => DoAsyncGetSession(fileStore, conferenceSlug, sessionSlug, isRefreshing, getSessionSuccess, getSessionError));
		}

		private static void DoAsyncGetSession(IMvxFileStore fileStore, string conferenceSlug, string sessionSlug, bool isRefreshing, Action<FullSessionDto> getSessionSuccess, Action<Exception> getSessionError)
		{
			var search = new SessionService(isRefreshing, fileStore, getSessionSuccess, getSessionError);
			search.StartGetSession(conferenceSlug, sessionSlug);
		}

		private void StartGetSession(string conferenceSlug, string sessionSlug)
		{
			try
			{
				_conferenceSlug = conferenceSlug;
				_sessionSlug = sessionSlug;

				var path = conferenceSlug + "-" + sessionSlug + ".json";

				if (_fileStore.Exists(path) && !_isRefreshing)
				{
					string response;
					if (_fileStore.TryReadTextFile(path, out response))
					{
						var session = JsonConvert.DeserializeObject<FullSessionDto>(response);
						_success(session);
					}
				}
				else
				{
					var uri = string.Format("http://api.tekconf.com/v1/conferences/{0}/sessions/{1}?format=json", conferenceSlug,
						sessionSlug);
					var request = (HttpWebRequest) WebRequest.Create(new Uri(uri));
					request.Accept = "application/json";

					request.BeginGetResponse(ReadGetSessionCallback, request);
				}
			}
			catch (Exception exception)
			{
				_error(exception);
			}
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
			var path = _conferenceSlug + "-" + _sessionSlug + ".json";

			if (_fileStore.Exists(path))
			{
				_fileStore.DeleteFile(path);
			}
			if (!_fileStore.Exists(path))
			{
				_fileStore.WriteFile(path, response);
			}

			var session = JsonConvert.DeserializeObject<FullSessionDto>(response);
			_success(session);
		}


	}
}