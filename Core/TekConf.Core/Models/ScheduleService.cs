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
	public class ScheduleService
	{
		public static void GetScheduleAsync(string userName, Action<IEnumerable<FullConferenceDto>> success, Action<Exception> error)
		{
			MvxAsyncDispatcher.BeginAsync(() => DoAsyncGetScheduleSearch(userName, success, error));
		}

		public static void AddToScheduleAsync(string userName, string conferenceSlug, Action<ScheduleDto> success, Action<Exception> error)
		{
			MvxAsyncDispatcher.BeginAsync(() => DoAsyncAddToScheduleSchedule(userName, conferenceSlug, success, error));
		}

		private static void DoAsyncGetScheduleSearch(string userName, Action<IEnumerable<FullConferenceDto>> success, Action<Exception> error)
		{
			var search = new ScheduleService(userName, success, error);
			search.StartSearch();
		}

		private static void DoAsyncAddToScheduleSchedule(string userName, string conferenceSlug, Action<ScheduleDto> success, Action<Exception> error)
		{
			var search = new ScheduleService(userName, conferenceSlug, success, error);
			search.StartAddToSchedule();
		}

		//private const string ConferencesUrl = "http://api.tekconf.com/v1/conferences?search=mobidev&format=json";
		private const string GetScheduleUrl = "http://api.tekconf.com/v1/conferences/schedules?userName={0}&format=json";
		private const string AddToScheduleUrl = "http://api.tekconf.com/v1/conferences/{1}/schedule?userName={0}&conferenceSlug={1}&sessionSlug=&format=json";

		private readonly Action<IEnumerable<FullConferenceDto>> _getScheduleSuccess;
		private readonly Action<Exception> _getScheduleError;
		private readonly Action<ScheduleDto> _addToScheduleSuccess;
		private readonly Action<Exception> _addToScheduleError;
		private string _userName;
		private readonly string _conferenceSlug;

		private ScheduleService(string userName, Action<IEnumerable<FullConferenceDto>> success, Action<Exception> error)
		{
			_getScheduleSuccess = success;
			_getScheduleError = error;
			_userName = userName;
		}

		private ScheduleService(string userName, string conferenceSlug, Action<ScheduleDto> success, Action<Exception> error)
		{
			_addToScheduleSuccess = success;
			_addToScheduleError = error;
			_userName = userName;
			_conferenceSlug = conferenceSlug;
		}

		private void StartSearch()
		{
			try
			{
				// perform the search
				string uri = string.Format(GetScheduleUrl, _userName);
				var request = WebRequest.Create(new Uri(uri));
				request.BeginGetResponse(ReadGetScheduleCallback, request);
			}
			catch (Exception exception)
			{
				_getScheduleError(exception);
			}
		}

		private void StartAddToSchedule()
		{
			try
			{
				// perform the search
				string uri = string.Format(AddToScheduleUrl, _userName, _conferenceSlug);
				var request = WebRequest.Create(new Uri(uri));
				request.Method = "POST";
				request.BeginGetResponse(ReadAddToScheduleCallback, request);
			}
			catch (Exception exception)
			{
				_getScheduleError(exception);
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
				_getScheduleError(exception);
			}
		}

		private void HandleGetScheduleResponse(string response)
		{
			var conferences = JsonConvert.DeserializeObject<List<FullConferenceDto>>(response);
			_getScheduleSuccess(conferences.OrderBy(x => x.start).ToList());
		}

		private void HandleAddToScheduleResponse(string response)
		{
			var scheduleDto = JsonConvert.DeserializeObject<ScheduleDto>(response);
			_addToScheduleSuccess(scheduleDto);
		}
	}

}
