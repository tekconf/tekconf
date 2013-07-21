using System;
using System.Collections.Generic;
using Cirrious.CrossCore.Core;
using Cirrious.MvvmCross.Plugins.File;
using Cirrious.MvvmCross.Plugins.Network.Reachability;
using Cirrious.MvvmCross.Plugins.Sqlite;
using TekConf.Core.Repositories;
using TekConf.Core.Services;

namespace TekConf.Core.Models
{
	using TekConf.Core.Entities;

	public class ConferenceSessionsService
	{
		private readonly IMvxFileStore _fileStore;
		private readonly IMvxReachability _reachability;
		private readonly bool _isRefreshing;
		private readonly ICacheService _cache;
		private readonly IAuthentication _authentication;
		private readonly ILocalConferencesRepository _localConferencesRepository;

		private readonly ISQLiteConnection _connection;

		private readonly Action<ConferenceSessionsListViewDto> _success;
		private readonly Action<Exception> _error;
		private string _slug;

		private ConferenceSessionsService(IMvxFileStore fileStore, IMvxReachability reachability, bool isRefreshing, ICacheService cache, IAuthentication authentication, 
			ILocalConferencesRepository localConferencesRepository, ISQLiteConnection connection, Action<ConferenceSessionsListViewDto> success, Action<Exception> error)
		{
			_fileStore = fileStore;
			_reachability = reachability;
			_isRefreshing = isRefreshing;
			_cache = cache;
			_authentication = authentication;
			_localConferencesRepository = localConferencesRepository;
			this._connection = connection;
			_success = success;
			_error = error;
		}


		public static void GetConferenceSessionsAsync(IMvxFileStore fileStore, ILocalConferencesRepository localConferencesRepository, IMvxReachability reachability, 
			string slug, bool isRefreshing, ICacheService cache, IAuthentication authentication, ISQLiteConnection connection, 
			Action<ConferenceSessionsListViewDto> success, Action<Exception> error)
		{
			MvxAsyncDispatcher.BeginAsync(() =>
				DoAsyncGetConferenceSessions(fileStore, localConferencesRepository, reachability, slug, isRefreshing, cache, connection, authentication, success, error)
				);
		}

		private static void DoAsyncGetConferenceSessions(IMvxFileStore fileStore, ILocalConferencesRepository localConferencesRepository, IMvxReachability reachability, 
			string slug, bool isRefreshing,ICacheService cache, ISQLiteConnection connection, IAuthentication authentication, 
			Action<ConferenceSessionsListViewDto> success, Action<Exception> error)
		{
			var conference = localConferencesRepository.Get(slug);

			IEnumerable<SessionEntity> sessions = null;
				 sessions = conference.Sessions(connection);
			var conferenceSessionListView = new ConferenceSessionsListViewDto(sessions)
			{
				name = conference.Name,
				slug = conference.Slug
			};
			
			success(conferenceSessionListView);
		}

	}
}