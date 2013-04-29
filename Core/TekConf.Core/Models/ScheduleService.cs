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
		public static void GetSchedulesAsync(string userName, Action<IEnumerable<FullConferenceDto>> success, Action<Exception> error)
		{
			MvxAsyncDispatcher.BeginAsync(() => DoAsyncGetSchedules(userName, success, error));
		}

		public static void AddToScheduleAsync(string userName, string conferenceSlug, Action<ScheduleDto> success, Action<Exception> error)
		{
			MvxAsyncDispatcher.BeginAsync(() => DoAsyncAddToScheduleSchedule(userName, conferenceSlug, success, error));
		}

		public static void GetScheduleAsync(string userName, string conferenceSlug, Action<ScheduleDto> success, Action<Exception> error)
		{
			MvxAsyncDispatcher.BeginAsync(() => DoAsyncGetSchedule(userName, conferenceSlug, success, error));
		}

		private static void DoAsyncGetSchedules(string userName, Action<IEnumerable<FullConferenceDto>> success, Action<Exception> error)
		{
			var search = new ScheduleService(userName, success, error);
			search.GetSchedules();
		}

		private static void DoAsyncGetSchedule(string userName, string conferenceSlug, Action<ScheduleDto> success, Action<Exception> error)
		{
			var search = new ScheduleService(userName, conferenceSlug, success, error);
			search.GetSchedule();
		}

		private static void DoAsyncAddToScheduleSchedule(string userName, string conferenceSlug, Action<ScheduleDto> success, Action<Exception> error)
		{
			var search = new ScheduleService(userName, conferenceSlug, success, error);
			search.StartAddToSchedule();
		}

		private const string GetSchedulesUrl = "http://api.tekconf.com/v1/conferences/schedules?userName={0}&format=json";
		private const string GetScheduleUrl = "http://api.tekconf.com/v1/conferences/{1}/schedule?userName={0}&conferenceSlug={1}&format=json";
		private const string AddToScheduleUrl = "http://api.tekconf.com/v1/conferences/{1}/schedule?userName={0}&conferenceSlug={1}&sessionSlug=&format=json";
		
		private readonly Action<IEnumerable<FullConferenceDto>> _getSchedulesSuccess;
		private readonly Action<ScheduleDto> _getScheduleSuccess;

		private readonly Action<Exception> _getSchedulesError;
		private readonly Action<Exception> _getScheduleError;

		private readonly Action<ScheduleDto> _addToScheduleSuccess;
		private readonly Action<Exception> _addToScheduleError;
		private readonly string _userName;
		private readonly string _conferenceSlug;

		private ScheduleService(string userName, Action<IEnumerable<FullConferenceDto>> success, Action<Exception> error)
		{
			_getSchedulesSuccess = success;
			_getSchedulesError = error;
			_userName = userName;
		}

		//private ScheduleService(string userName, string conferenceSlug, Action<ScheduleDto> success, Action<Exception> error)
		//{
		//	_getScheduleSuccess = success;
		//	_getScheduleError = error;
		//	_userName = userName;
		//	_conferenceSlug = conferenceSlug;
		//}

		private ScheduleService(string userName, string conferenceSlug, Action<ScheduleDto> success, Action<Exception> error)
		{
			_addToScheduleSuccess = success;
			_getScheduleSuccess = success;

			_addToScheduleError = error;
			_getScheduleError = error;

			_userName = userName;
			_conferenceSlug = conferenceSlug;
		}

		private void GetSchedules()
		{
			try
			{
				// perform the search
				string uri = string.Format(GetSchedulesUrl, _userName);
				var request = WebRequest.Create(new Uri(uri));
				request.BeginGetResponse(ReadGetSchedulesCallback, request);
			}
			catch (Exception exception)
			{
				_getSchedulesError(exception);
			}
		}

		private void GetSchedule()
		{
			try
			{
				// perform the search
				string uri = string.Format(GetScheduleUrl, _userName, _conferenceSlug);
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

		private void HandleGetSchedulesResponse(string response)
		{
			var conferences = JsonConvert.DeserializeObject<List<FullConferenceDto>>(response);
			_getSchedulesSuccess(conferences.OrderBy(x => x.start).ToList());
		}

		private void HandleGetScheduleResponse(string response)
		{
			var schedule = JsonConvert.DeserializeObject<ScheduleDto>(response);
			schedule.sessions = schedule.sessions.OrderBy(x => x.start).ToList();
			_getScheduleSuccess(schedule);
		}

		private void HandleAddToScheduleResponse(string response)
		{
			var scheduleDto = JsonConvert.DeserializeObject<ScheduleDto>(response);
			_addToScheduleSuccess(scheduleDto);
		}


	}

}
