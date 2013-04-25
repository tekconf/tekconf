using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Cirrious.CrossCore.Core;
using Newtonsoft.Json;
using TekConf.RemoteData.Dtos.v1;

namespace TekConf.Core.Models
{
	public class ConferencesService
	{
		public static void GetConferencesAsync(Action<IEnumerable<FullConferenceDto>> success, Action<Exception> error)
		{
			MvxAsyncDispatcher.BeginAsync(() => DoAsyncSearch(success, error));
		}

		private static void DoAsyncSearch(Action<IEnumerable<FullConferenceDto>> success, Action<Exception> error)
		{
			var search = new ConferencesService(success, error);
			search.StartSearch();
		}

		private const string ConferencesUrl = "http://api.tekconf.com/v1/conferences?format=json";

		private readonly Action<IEnumerable<FullConferenceDto>> _success;
		private readonly Action<Exception> _error;

		private ConferencesService(Action<IEnumerable<FullConferenceDto>> success, Action<Exception> error)
		{
			_success = success;
			_error = error;
		}

		private void StartSearch()
		{
			try
			{
				// perform the search
				const string uri = ConferencesUrl;
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
			var conferences = JsonConvert.DeserializeObject<List<FullConferenceDto>>(response);
			_success(conferences);
		}

	}

}
