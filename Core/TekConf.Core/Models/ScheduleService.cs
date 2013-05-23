using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Cirrious.CrossCore.Core;
using Cirrious.MvvmCross.Plugins.File;
using Newtonsoft.Json;
using TekConf.Core.Services;
using TekConf.Core.ViewModels;
using TekConf.RemoteData.Dtos.v1;

namespace TekConf.Core.Models
{
	public class ScheduleService
	{

		private const string GetSchedulesUrl = "http://api.tekconf.com/v1/conferences/schedules?userName={0}&format=json";
		private const string GetScheduleUrl = "http://api.tekconf.com/v1/conferences/{1}/schedule?userName={0}&conferenceSlug={1}&format=json";
		private const string AddToScheduleUrl = "http://api.tekconf.com/v1/conferences/{1}/schedule?userName={0}&conferenceSlug={1}&sessionSlug=&format=json";
		private const string RemoveFromScheduleUrl = "http://api.tekconf.com/v1/conferences/{1}/schedule?userName={0}&conferenceSlug={1}&sessionSlug=&format=json";

		private readonly IMvxFileStore _fileStore;
		private readonly Action<IEnumerable<FullConferenceDto>> _getSchedulesSuccess;
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
		private readonly string _conferenceSlug;

		private ScheduleService(IMvxFileStore fileStore, string userName, bool isRefreshing, ICacheService cache, Action<IEnumerable<FullConferenceDto>> success, Action<Exception> error)
		{
			_fileStore = fileStore;
			_getSchedulesSuccess = success;
			_getSchedulesError = error;
			_userName = userName;
			_isRefreshing = isRefreshing;
			_cache = cache;
		}

		private ScheduleService(IMvxFileStore fileStore, string userName, string conferenceSlug, bool isRefreshing, ICacheService cache, Action<ScheduleDto> success, Action<Exception> error)
		{
			_fileStore = fileStore;

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
			_conferenceSlug = conferenceSlug;
		}

		public static void GetSchedulesAsync(IMvxFileStore fileStore, string userName, bool isRefreshing, ICacheService cache, Action<IEnumerable<FullConferenceDto>> success, Action<Exception> error)
		{
			MvxAsyncDispatcher.BeginAsync(() => DoAsyncGetSchedules(fileStore, userName, isRefreshing, cache, success, error));
		}

		public static void AddToScheduleAsync(IMvxFileStore fileStore, string userName, string conferenceSlug, bool isRefreshing, ICacheService cache, Action<ScheduleDto> success, Action<Exception> error)
		{
			MvxAsyncDispatcher.BeginAsync(() => DoAsyncAddToScheduleSchedule(fileStore, userName, conferenceSlug, isRefreshing, cache, success, error));
		}

		public static void RemoveFromScheduleAsync(IMvxFileStore fileStore, string userName, string conferenceSlug, bool isRefreshing, ICacheService cache, Action<ScheduleDto> success, Action<Exception> error)
		{
			MvxAsyncDispatcher.BeginAsync(() => DoAsyncRemoveFromScheduleSchedule(fileStore, userName, conferenceSlug, isRefreshing, cache, success, error));
		}

		public static void GetScheduleAsync(IMvxFileStore fileStore, string userName, string conferenceSlug, bool isRefreshing, ICacheService cache, Action<ScheduleDto> success, Action<Exception> error)
		{
			MvxAsyncDispatcher.BeginAsync(() => DoAsyncGetSchedule(fileStore, userName, conferenceSlug, isRefreshing, cache, success, error));
		}

		private static void DoAsyncGetSchedules(IMvxFileStore fileStore, string userName, bool isRefreshing, ICacheService cache, Action<IEnumerable<FullConferenceDto>> success, Action<Exception> error)
		{
			var search = new ScheduleService(fileStore, userName, isRefreshing, cache, success, error);
			search.GetSchedules();
		}

		private static void DoAsyncGetSchedule(IMvxFileStore fileStore, string userName, string conferenceSlug, bool isRefreshing, ICacheService cache, Action<ScheduleDto> success, Action<Exception> error)
		{
			var search = new ScheduleService(fileStore, userName, conferenceSlug, isRefreshing, cache, success, error);
			search.GetSchedule();
		}

		private static void DoAsyncAddToScheduleSchedule(IMvxFileStore fileStore, string userName, string conferenceSlug, bool isRefreshing, ICacheService cache, Action<ScheduleDto> success, Action<Exception> error)
		{
			var search = new ScheduleService(fileStore, userName, conferenceSlug, isRefreshing, cache, success, error);
			search.StartAddToSchedule();
		}

		private static void DoAsyncRemoveFromScheduleSchedule(IMvxFileStore fileStore, string userName, string conferenceSlug, bool isRefreshing, ICacheService cache, Action<ScheduleDto> success, Action<Exception> error)
		{
			var search = new ScheduleService(fileStore, userName, conferenceSlug, isRefreshing, cache, success, error);
			search.StartRemoveFromSchedule();
		}

		private void GetSchedules()
		{
			try
			{
				const string path = "schedules.json";

				//var cachedSchedules = _cache.Get<string, List<FullConferenceDto>>("schedules");
				List<FullConferenceDto> cachedSchedules = null;
				if (cachedSchedules != null && !_isRefreshing)
				{
					_getSchedulesSuccess(cachedSchedules);
				}
				else if (_fileStore.Exists(path) && !_isRefreshing)
				{
					string response;
					if (_fileStore.TryReadTextFile(path, out response))
					{
						var conferences = JsonConvert.DeserializeObject<List<FullConferenceDto>>(response).OrderBy(x => x.start).ToList();
						_cache.Add("schedules", conferences, new TimeSpan(0, 0, 15));
			
						_getSchedulesSuccess(conferences);
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

				//var cachedSchedule = _cache.Get<string, ScheduleDto>(path);
				ScheduleDto cachedSchedule = null;
				if (cachedSchedule != null && !_isRefreshing)
				{
					_getScheduleSuccess(cachedSchedule);
				}
				else if (_fileStore.Exists(path) && !_isRefreshing)
				{
					string response;
					if (_fileStore.TryReadTextFile(path, out response))
					{
						if (!response.IsNullOrWhiteSpace())
							GetScheduleFromFileSystem(response);
						else
							GetScheduleFromWeb();
					}
				}
				else
				{
					// perform the search
					GetScheduleFromWeb();
				}
			}
			catch (Exception exception)
			{
				_getScheduleError(exception);
			}
		}

		private void GetScheduleFromWeb()
		{
			string uri = string.Format(GetScheduleUrl, _userName, _conferenceSlug);
			var request = (HttpWebRequest) WebRequest.Create(new Uri(uri));
			request.Accept = "application/json";

			request.BeginGetResponse(ReadGetScheduleCallback, request);
		}

		private void GetScheduleFromFileSystem(string response)
		{
			var schedule = JsonConvert.DeserializeObject<ScheduleDto>(response);
			schedule.sessions = schedule.sessions.OrderBy(x => x.start).ToList();
			_cache.Add("schedule", schedule, new TimeSpan(0, 0, 15));
			_getScheduleSuccess(schedule);
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

		private void StartRemoveFromSchedule()
		{
			try
			{
				// perform the search
				string uri = string.Format(RemoveFromScheduleUrl, _userName, _conferenceSlug);
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

		private void HandleGetSchedulesResponse(string response)
		{
			const string schedulesLastUpdatedPath = "scheduleLastUpdated.json";
			if (_fileStore.Exists(schedulesLastUpdatedPath))
			{
				_fileStore.DeleteFile(schedulesLastUpdatedPath);
			}

			if (!_fileStore.Exists(schedulesLastUpdatedPath))
			{
				var schedulesLastUpdated = new DataLastUpdated { LastUpdated = DateTime.Now };
				var json = JsonConvert.SerializeObject(schedulesLastUpdated);
				_fileStore.WriteFile(schedulesLastUpdatedPath, json);
			}

			const string schedulesPath = "schedules.json";
			if (_fileStore.Exists(schedulesPath))
			{
				_fileStore.DeleteFile(schedulesPath);
			}
			if (!_fileStore.Exists(schedulesPath))
			{
				_fileStore.WriteFile(schedulesPath, response);
			}
			
			var conferences = JsonConvert.DeserializeObject<List<FullConferenceDto>>(response).OrderBy(x => x.start).ToList();
			_cache.Add("schedules", conferences, new TimeSpan(0, 0, 15));
			_getSchedulesSuccess(conferences);
		}

		private void HandleGetScheduleResponse(string response)
		{
			try
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
				
				if (schedule.IsNull())
					schedule = new ScheduleDto();

				schedule.sessions = schedule.sessions.OrderBy(x => x.start).ToList();
				_cache.Add(path, schedule, new TimeSpan(0, 0, 15));

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
			_addToScheduleSuccess(scheduleDto);
		}

		private void HandleRemoveFromScheduleResponse(string response)
		{
			var scheduleDto = JsonConvert.DeserializeObject<ScheduleDto>(response);
			_removeFromScheduleSuccess(scheduleDto);
		}


	}

}
