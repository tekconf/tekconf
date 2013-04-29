using System;
using System.IO;
using System.Net;
using Cirrious.CrossCore.Core;
using Newtonsoft.Json;
using TekConf.RemoteData.Dtos.v1;

namespace TekConf.Core.Models
{
	public class SessionService
	{
		private readonly Action<FullSessionDto> _success;
		private readonly Action<Exception> _error;

		private SessionService(Action<FullSessionDto> success, Action<Exception> error)
		{
			_success = success;
			_error = error;
		}

		public static void GetSessionAsync(string conferenceSlug, string sessionSlug, Action<FullSessionDto> getSessionSuccess, Action<Exception> getSessionError)
		{
			MvxAsyncDispatcher.BeginAsync(() => DoAsyncGetSession(conferenceSlug, sessionSlug, getSessionSuccess, getSessionError));
		}

		private static void DoAsyncGetSession(string conferenceSlug, string sessionSlug, Action<FullSessionDto> getSessionSuccess, Action<Exception> getSessionError)
		{
			var search = new SessionService(getSessionSuccess, getSessionError);
			search.StartGetSession(conferenceSlug, sessionSlug);
		}

		private void StartGetSession(string conferenceSlug, string sessionSlug)
		{
			try
			{
				var uri = string.Format("http://api.tekconf.com/v1/conferences/{0}/sessions/{1}?format=json", conferenceSlug, sessionSlug);
				var request = WebRequest.Create(new Uri(uri));
				request.BeginGetResponse(ReadGetSessionCallback, request);
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
			var conferences = JsonConvert.DeserializeObject<FullSessionDto>(response);
			_success(conferences);
		}


	}
}