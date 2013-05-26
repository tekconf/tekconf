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
	public class ConferenceSessionsService
	{
		private readonly IMvxFileStore _fileStore;
		private readonly IMvxReachability _reachability;
		private readonly bool _isRefreshing;
		private readonly ICacheService _cache;
		private readonly IAuthentication _authentication;
		private readonly ILocalScheduleRepository _localScheduleRepository;
		private readonly ILocalConferencesRepository _localConferencesRepository;
		private readonly Action<ConferenceSessionsListViewDto> _success;
		private readonly Action<Exception> _error;
		private string _slug;

		private ConferenceSessionsService(IMvxFileStore fileStore, IMvxReachability reachability, bool isRefreshing, ICacheService cache, IAuthentication authentication, 
			ILocalScheduleRepository localScheduleRepository, ILocalConferencesRepository localConferencesRepository,
			Action<ConferenceSessionsListViewDto> success, Action<Exception> error)
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


		public static void GetConferenceSessionsAsync(IMvxFileStore fileStore, ILocalScheduleRepository localScheduleRepository, 
			ILocalConferencesRepository localConferencesRepository, IMvxReachability reachability, string slug, bool isRefreshing,
			ICacheService cache, IAuthentication authentication, Action<ConferenceSessionsListViewDto> success, Action<Exception> error)
		{
			MvxAsyncDispatcher.BeginAsync(() =>
				DoAsyncGetConferenceSessions(fileStore, localScheduleRepository, localConferencesRepository, reachability, slug, isRefreshing, cache, authentication, success, error)
				);
		}

		private static void DoAsyncGetConferenceSessions(IMvxFileStore fileStore, ILocalScheduleRepository localScheduleRepository, 
																											ILocalConferencesRepository localConferencesRepository, IMvxReachability reachability, string slug, bool isRefreshing,
																											ICacheService cache, IAuthentication authentication, Action<ConferenceSessionsListViewDto> success, Action<Exception> error)
		{
			var conference = GetFullConference(fileStore, slug);
			
			var sessions = conference.sessions;
			var conferenceSessionListView = new ConferenceSessionsListViewDto(sessions)
			{
				name = conference.name,
				slug = conference.slug
			};
			
			success(conferenceSessionListView);
		}

		private static FullConferenceDto GetFullConference(IMvxFileStore fileStore, string slug)
		{
			var conferenceJsonPath = slug + ".json";

			if (fileStore.Exists(conferenceJsonPath))
			{
				string json;
				if (fileStore.TryReadTextFile(conferenceJsonPath, out json))
				{
					var fullConference = JsonConvert.DeserializeObject<FullConferenceDto>(json);
					return fullConference;
				}
			}

			return new FullConferenceDto();
		}
	}
}