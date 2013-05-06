using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Cirrious.CrossCore.Core;
using Cirrious.MvvmCross.Plugins.File;
using Newtonsoft.Json;
using TekConf.RemoteData.Dtos.v1;

namespace TekConf.Core.Models
{
	public class ScheduleService
	{

		private const string GetSchedulesUrl = "http://api.tekconf.com/v1/conferences/schedules?userName={0}&format=json";
		private const string GetScheduleUrl = "http://api.tekconf.com/v1/conferences/{1}/schedule?userName={0}&conferenceSlug={1}&format=json";
		private const string AddToScheduleUrl = "http://api.tekconf.com/v1/conferences/{1}/schedule?userName={0}&conferenceSlug={1}&sessionSlug=&format=json";

		private readonly IMvxFileStore _fileStore;
		private readonly Action<IEnumerable<FullConferenceDto>> _getSchedulesSuccess;
		private readonly Action<ScheduleDto> _getScheduleSuccess;

		private readonly Action<Exception> _getSchedulesError;
		private readonly Action<Exception> _getScheduleError;

		private readonly Action<ScheduleDto> _addToScheduleSuccess;
		private readonly Action<Exception> _addToScheduleError;
		private readonly string _userName;
		private readonly bool _isRefreshing;
		private readonly string _conferenceSlug;

		private ScheduleService(IMvxFileStore fileStore, string userName, bool isRefreshing, Action<IEnumerable<FullConferenceDto>> success, Action<Exception> error)
		{
			_fileStore = fileStore;
			_getSchedulesSuccess = success;
			_getSchedulesError = error;
			_userName = userName;
			_isRefreshing = isRefreshing;
		}

		private ScheduleService(IMvxFileStore fileStore, string userName, string conferenceSlug, bool isRefreshing, Action<ScheduleDto> success, Action<Exception> error)
		{
			_fileStore = fileStore;
			_addToScheduleSuccess = success;
			_getScheduleSuccess = success;

			_addToScheduleError = error;
			_getScheduleError = error;

			_userName = userName;
			_isRefreshing = isRefreshing;
			_conferenceSlug = conferenceSlug;
		}

		public static void GetSchedulesAsync(IMvxFileStore fileStore, string userName, bool isRefreshing, Action<IEnumerable<FullConferenceDto>> success, Action<Exception> error)
		{
			MvxAsyncDispatcher.BeginAsync(() => DoAsyncGetSchedules(fileStore, userName, isRefreshing, success, error));
		}

		public static void AddToScheduleAsync(IMvxFileStore fileStore, string userName, string conferenceSlug, bool isRefreshing, Action<ScheduleDto> success, Action<Exception> error)
		{
			MvxAsyncDispatcher.BeginAsync(() => DoAsyncAddToScheduleSchedule(fileStore, userName, conferenceSlug, isRefreshing, success, error));
		}

		public static void GetScheduleAsync(IMvxFileStore fileStore, string userName, string conferenceSlug, bool isRefreshing, Action<ScheduleDto> success, Action<Exception> error)
		{
			MvxAsyncDispatcher.BeginAsync(() => DoAsyncGetSchedule(fileStore, userName, conferenceSlug, isRefreshing, success, error));
		}

		private static void DoAsyncGetSchedules(IMvxFileStore fileStore, string userName, bool isRefreshing, Action<IEnumerable<FullConferenceDto>> success, Action<Exception> error)
		{
			var search = new ScheduleService(fileStore, userName, isRefreshing, success, error);
			search.GetSchedules();
		}

		private static void DoAsyncGetSchedule(IMvxFileStore fileStore, string userName, string conferenceSlug, bool isRefreshing, Action<ScheduleDto> success, Action<Exception> error)
		{
			var search = new ScheduleService(fileStore, userName, conferenceSlug, isRefreshing, success, error);
			search.GetSchedule();
		}

		private static void DoAsyncAddToScheduleSchedule(IMvxFileStore fileStore, string userName, string conferenceSlug, bool isRefreshing, Action<ScheduleDto> success, Action<Exception> error)
		{

			var search = new ScheduleService(fileStore, userName, conferenceSlug, isRefreshing, success, error);
			search.StartAddToSchedule();
		}

		private void GetSchedules()
		{
			try
			{
				const string path = "schedules.json";

				if (_fileStore.Exists(path) && !_isRefreshing)
				{
					string response;
					if (_fileStore.TryReadTextFile(path, out response))
					{
						var conferences = JsonConvert.DeserializeObject<List<FullConferenceDto>>(response);
						_getSchedulesSuccess(conferences.OrderBy(x => x.start).ToList());
					}
				}
				else
				{
					// perform the search
					string uri = string.Format(GetSchedulesUrl, _userName);
					var request = (HttpWebRequest)WebRequest.Create(new Uri(uri));
					request.Accept = "application/json";

					request.BeginGetResponse(ReadGetSchedulesCallback, request);
				}
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
				string path = _conferenceSlug + "-schedule.json";

				if (_fileStore.Exists(path) && !_isRefreshing)
				{
					string response;
					if (_fileStore.TryReadTextFile(path, out response))
					{
						var schedule = JsonConvert.DeserializeObject<ScheduleDto>(response);
						schedule.sessions = schedule.sessions.OrderBy(x => x.start).ToList();
						_getScheduleSuccess(schedule);
					}
				}
				else
				{
					// perform the search
					string uri = string.Format(GetScheduleUrl, _userName, _conferenceSlug);
					var request = (HttpWebRequest)WebRequest.Create(new Uri(uri));
					request.Accept = "application/json";

					request.BeginGetResponse(ReadGetScheduleCallback, request);
				}
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
			const string path = "schedules.json";
			if (_fileStore.Exists(path))
			{
				_fileStore.DeleteFile(path);
			}
			if (!_fileStore.Exists(path))
			{
				_fileStore.WriteFile(path, response);
			}
			var conferences = JsonConvert.DeserializeObject<List<FullConferenceDto>>(response);
			_getSchedulesSuccess(conferences.OrderBy(x => x.start).ToList());
		}

		private void HandleGetScheduleResponse(string response)
		{
			var path = _conferenceSlug + "-schedule.json";
			if (_fileStore.Exists(path))
			{
				_fileStore.DeleteFile(path);
			}
			if (!_fileStore.Exists(path))
			{
				_fileStore.WriteFile(path, response);
			}
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
