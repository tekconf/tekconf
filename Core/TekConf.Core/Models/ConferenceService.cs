using System;
using System.IO;
using System.Net;
using Cirrious.CrossCore.Core;
using Newtonsoft.Json;
using TekConf.RemoteData.Dtos.v1;

namespace TekConf.Core.Models
{
	public class ConferenceService
	{
		private readonly Action<FullConferenceDto> _success;
		private readonly Action<Exception> _error;

		private ConferenceService(Action<FullConferenceDto> success, Action<Exception> error)
		{
			_success = success;
			_error = error;
		}

		public static void GetConferenceAsync(string slug, Action<FullConferenceDto> success, Action<Exception> error)
		{
			MvxAsyncDispatcher.BeginAsync(() => DoAsyncSearch(slug, success, error));
		}

		private static void DoAsyncSearch(string slug, Action<FullConferenceDto> success, Action<Exception> error)
		{
			var search = new ConferenceService(success, error);
			search.StartSearch(slug);
		}

		private void StartSearch(string slug)
		{
			try
			{
				var uri = string.Format("http://api.tekconf.com/v1/conferences/{0}?format=json", slug);
				var request = WebRequest.Create(new Uri(uri));
				request.BeginGetResponse(ReadCallback, request);
			}
			catch (Exception exception)
			{
				_error(exception);
			}
		}

		private void ReadCallback(IAsyncResult asynchronousResult)
		{
			try
			{
				var request = (HttpWebRequest)asynchronousResult.AsyncState;
				var response = (HttpWebResponse)request.EndGetResponse(asynchronousResult);
				using (var streamReader1 = new StreamReader(response.GetResponseStream()))
				{
					string resultString = streamReader1.ReadToEnd();
					HandleResponse(resultString);
				}
			}
			catch (Exception exception)
			{
				_error(exception);
			}
		}

		private void HandleResponse(string response)
		{
			var conferences = JsonConvert.DeserializeObject<FullConferenceDto>(response);
			_success(conferences);
		}

	}
}