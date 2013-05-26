using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Cirrious.CrossCore.Core;
using Cirrious.MvvmCross.Plugins.File;
using Cirrious.MvvmCross.Plugins.Network.Reachability;
using Newtonsoft.Json;
using TekConf.Core.Repositories;
using TekConf.Core.Services;
using TekConf.RemoteData.Dtos.v1;

namespace TekConf.Core.Models
{
	public class ConferenceService
	{
		private readonly IMvxFileStore _fileStore;
		private readonly IMvxReachability _reachability;
		private readonly bool _isRefreshing;
		private readonly ICacheService _cache;
		private readonly IAuthentication _authentication;
		private readonly ILocalScheduleRepository _localScheduleRepository;
		private readonly ILocalConferencesRepository _localConferencesRepository;
		private readonly Action<ConferenceDetailViewDto> _success;
		private readonly Action<Exception> _error;
		private string _slug;

		private ConferenceService(IMvxFileStore fileStore, IMvxReachability reachability, bool isRefreshing, ICacheService cache, IAuthentication authentication, ILocalScheduleRepository localScheduleRepository, ILocalConferencesRepository localConferencesRepository, Action<ConferenceDetailViewDto> success, Action<Exception> error)
		{
			_fileStore = fileStore;
			_reachability = reachability;
			_isRefreshing = isRefreshing;
			_cache = cache;
			_authentication = authentication;
			_localScheduleRepository = localScheduleRepository;
			_localConferencesRepository = localConferencesRepository;
			_success = success;
			_error = error;
		}

		public static void GetConferenceDetailAsync(IMvxFileStore fileStore, ILocalScheduleRepository localScheduleRepository, ILocalConferencesRepository localConferencesRepository, IMvxReachability reachability, string slug, bool isRefreshing, ICacheService cache, IAuthentication authentication, Action<ConferenceDetailViewDto> success, Action<Exception> error)
		{
			MvxAsyncDispatcher.BeginAsync(() =>
				DoAsyncGetConferenceDetail(fileStore, localScheduleRepository, localConferencesRepository, reachability, slug, isRefreshing, cache, authentication, success, error)
				);
		}

		private static void DoAsyncGetConferenceDetail(IMvxFileStore fileStore, ILocalScheduleRepository localScheduleRepository, ILocalConferencesRepository localConferencesRepository, IMvxReachability reachability, string slug, bool isRefreshing, ICacheService cache, IAuthentication authentication, Action<ConferenceDetailViewDto> success, Action<Exception> error)
		{
			var search = new ConferenceService(fileStore, reachability, isRefreshing, cache, authentication, localScheduleRepository, localConferencesRepository, success, error);
			search.StartGetConferenceDetail(slug);
		}

		private ConferenceDetailViewDto _conference;
		private void GetConferenceDetailSuccess(ConferenceDetailViewDto conference)
		{
			if (_authentication.IsAuthenticated)
			{
				_conference = conference;
				ScheduleService.GetScheduleAsync(_fileStore, _localScheduleRepository, _authentication.UserName, conference.slug, false, _cache, GetScheduleSuccess, GetScheduleError);
			}
			else
			{
				_conference = conference;
				_success(conference);
			}
		}

		private void GetScheduleError(Exception exception)
		{
			_success(_conference);
		}

		private void GetScheduleSuccess(ScheduleDto schedule)
		{
			if (schedule != null && !string.IsNullOrWhiteSpace(schedule.conferenceSlug))
			{
				_conference.isAddedToSchedule = true;
			}

			_success(_conference);
		}

		private void StartGetConferenceDetail(string slug)
		{
			_slug = slug;

			try
			{
				var path = _slug + ".json";

				var conferenceDetail = _localConferencesRepository.GetConferenceDetail(slug);
				if (conferenceDetail != null && !_isRefreshing)
				{
					GetConferenceDetailSuccess(conferenceDetail);
				}
				else
				{
					GetRemoteConference(slug);
				}
			}
			catch (Exception exception)
			{
				_error(exception);
			}
		}

		private void GetRemoteConference(string slug)
		{
			var uri = string.Format("http://api.tekconf.com/v1/conferences/{0}?format=json", slug);
			var request = (HttpWebRequest)WebRequest.Create(new Uri(uri));
			request.Accept = "application/json";
			request.BeginGetResponse(ReadGetConferenceCallback, request);
		}

		private void ReadGetConferenceCallback(IAsyncResult asynchronousResult)
		{
			try
			{
				var request = (HttpWebRequest)asynchronousResult.AsyncState;
				var response = (HttpWebResponse)request.EndGetResponse(asynchronousResult);
				using (var streamReader1 = new StreamReader(response.GetResponseStream()))
				{
					string resultString = streamReader1.ReadToEnd();
					HandleGetConferenceResponse(resultString);
				}
			}
			catch (Exception exception)
			{
				_error(exception);
			}
		}

		private void HandleGetConferenceResponse(string response)
		{
			var conference = JsonConvert.DeserializeObject<FullConferenceDto>(response);

			_localConferencesRepository.SaveConference(conference);

			var conferenceDetail = _localConferencesRepository.GetConferenceDetail(conference.slug);


			GetConferenceDetailSuccess(conferenceDetail);
		}

	}
}