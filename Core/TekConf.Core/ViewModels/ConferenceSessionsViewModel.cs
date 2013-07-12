using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Cirrious.MvvmCross.Plugins.Messenger;
using Cirrious.MvvmCross.Plugins.Sqlite;
using Cirrious.MvvmCross.ViewModels;
using TekConf.Core.Interfaces;
using TekConf.Core.Messages;
using TekConf.Core.Models;
using TekConf.Core.Repositories;
using TekConf.Core.Services;
using TekConf.RemoteData.Dtos.v1;

namespace TekConf.Core.ViewModels
{
	public class ConferenceSessionsViewModel : MvxViewModel
	{

		private readonly IRemoteDataService _remoteDataService;
		private readonly IAnalytics _analytics;
		private readonly IMvxMessenger _messenger;
		private readonly IAuthentication _authentication;
		private readonly ILocalScheduleRepository _localScheduleRepository;
		private readonly ILocalConferencesRepository _localConferencesRepository;
		private readonly ISQLiteConnectionFactory _sqLiteConnectionFactory;

		public ConferenceSessionsViewModel(IRemoteDataService remoteDataService, IAnalytics analytics, IMvxMessenger messenger,
																			IAuthentication authentication, 
																			ILocalScheduleRepository localScheduleRepository, 
																			ILocalConferencesRepository localConferencesRepository,
																			ISQLiteConnectionFactory sqLiteConnectionFactory)
		{
			_remoteDataService = remoteDataService;
			_analytics = analytics;
			_messenger = messenger;
			_authentication = authentication;
			_localScheduleRepository = localScheduleRepository;
			_localConferencesRepository = localConferencesRepository;
			_sqLiteConnectionFactory = sqLiteConnectionFactory;
		}

		public void Init(string slug)
		{
			HasSessions = true;
			StartGetConference(slug);
			if (_authentication.IsAuthenticated)
			{
				var userName = _authentication.UserName;
				StartGetSchedule(userName, slug, false);
			}
		}

		private string _pageTitle;
		public string PageTitle
		{
			get
			{
				return _pageTitle;
			}
			set
			{
				_pageTitle = value.ToUpper();
				RaisePropertyChanged(() => PageTitle);
			}
		}

		private bool _isAuthenticated;
		public bool IsAuthenticated
		{
			get
			{
				return _isAuthenticated;
			}
			set
			{
				_isAuthenticated = value;
				RaisePropertyChanged(() => IsAuthenticated);
			}
		}

		public void Refresh(string slug)
		{
			StartGetConference(slug, true);
			if (_authentication.IsAuthenticated)
			{
				var userName = _authentication.UserName;
				StartGetSchedule(userName, slug, true);
			}
		}

		public bool ShouldAddFavorites
		{
			get
			{
				bool shouldAddFavorites = false;
				if (_authentication.IsAuthenticated)
				{
					shouldAddFavorites = HasSessions && (Schedule == null || Schedule.sessions == null || !Schedule.sessions.Any());
				}

				return shouldAddFavorites;
			}
		}

		private void StartGetConference(string slug, bool isRefreshing = false)
		{
			if (IsLoadingConference)
				return;

			IsLoadingConference = true;
			_analytics.SendView("ConferenceSessions-" + slug);
			
			if (!isRefreshing)
			{
				var connection = _sqLiteConnectionFactory.Create("conferences.db");
				var conference = _localConferencesRepository.Get(slug);
				if (conference != null)
				{
					var sessions = conference.Sessions(connection);
					if (sessions != null)
					{
						var conferenceSessionsListViewDto = new ConferenceSessionsListViewDto(sessions)
						{
							name = conference.Name,
							slug = conference.Slug
						};
						GetConferenceSuccess(conferenceSessionsListViewDto);
					}
					else
					{
						_remoteDataService.GetConferenceSessionsList(slug, isRefreshing, GetConferenceSuccess, GetConferenceError);						
					}
				}
				else
				{
					_remoteDataService.GetConferenceSessionsList(slug, isRefreshing, GetConferenceSuccess, GetConferenceError);
				}
			}
			else
			{
				_remoteDataService.GetConferenceSessionsList(slug, isRefreshing, GetConferenceSuccess, GetConferenceError);				
			}
		}

		private void GetConferenceError(Exception exception)
		{
			// for now we just hide the error...
			_messenger.Publish(new ConferenceSessionsExceptionMessage(this, exception));
			IsLoadingConference = false;
		}

		private void GetConferenceSuccess(ConferenceSessionsListViewDto conference)
		{
			InvokeOnMainThread(() => DisplayConference(conference));
		}

		private void DisplayConference(ConferenceSessionsListViewDto conference)
		{
			IsLoadingConference = false;
			Conference = conference;
		}

		private bool _isLoadingConference;
		public bool IsLoadingConference
		{
			get { return _isLoadingConference; }
			set
			{
				_isLoadingConference = value;
				IsAuthenticated = _authentication.IsAuthenticated;
				RaisePropertyChanged(() => IsLoadingConference);
			}
		}

		private ConferenceSessionsListViewDto _conference;
		public ConferenceSessionsListViewDto Conference
		{
			get
			{
				return _conference;
			}
			set
			{
				_conference = value;
				PageTitle = _conference.name;
				RaisePropertyChanged(() => Conference);
				RaisePropertyChanged(() => HasSessions);

				IsLoadingConference = false;

			}
		}

		private void StartGetSchedule(string userName, string slug, bool isRefreshing)
		{
			if (IsLoadingSchedule)
				return;

			IsLoadingSchedule = true;
			_remoteDataService.GetSchedule(userName, slug, isRefreshing, GetScheduleSuccess, GetScheduleError);
		}

		private void GetScheduleError(Exception exception)
		{
			// for now we just hide the error...
			_messenger.Publish(new ConferenceSessionsExceptionMessage(this, exception));

			IsLoadingSchedule = false;
		}

		private void GetScheduleSuccess(ScheduleDto conference)
		{
			InvokeOnMainThread(() => DisplaySchedule(conference));
		}

		private void DisplaySchedule(ScheduleDto conference)
		{
			IsLoadingSchedule = false;
			Schedule = conference;
		}

		private bool _isLoadingSchedule;
		public bool IsLoadingSchedule
		{
			get { return _isLoadingSchedule; }
			set { _isLoadingSchedule = value; RaisePropertyChanged(() => IsLoadingSchedule); }
		}

		public bool HasSessions
		{
			get
			{
				if (Conference.IsNull())
					return false;

				return Conference.Sessions.Any();
			}
			set
			{
				RaisePropertyChanged(() => HasSessions);
			}
		}

		public List<FullSessionDto> Sessions
		{
			get
			{
				if (Schedule != null && Schedule.sessions != null)
					return Schedule.sessions;
				else
					return new List<FullSessionDto>();
			}
		}

		private ScheduleDto _schedule;
		public ScheduleDto Schedule
		{
			get
			{
				return _schedule;
			}
			set
			{
				_schedule = value;
				RaisePropertyChanged(() => Schedule);
				RaisePropertyChanged(() => Sessions);
				RaisePropertyChanged(() => ShouldAddFavorites);
				IsLoadingSchedule = false;
			}
		}

		public ICommand ShowSettingsCommand
		{
			get
			{
				return new MvxCommand(() => ShowViewModel<SettingsViewModel>());
			}
		}

		public ICommand ShowSearchCommand
		{
			get
			{
				return new MvxCommand(() => ShowViewModel<ConferenceSearchViewModel>());
			}
		}

		public ICommand ShowSessionDetailCommand
		{
			get
			{
				return new MvxCommand<SessionDetailViewModel.Navigation>(navigation =>
					ShowViewModel<SessionDetailViewModel>(navigation)
					);
			}
		}

	}
}