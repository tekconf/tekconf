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
	using Entities;

	public class ConferenceSessionsViewModel : MvxViewModel
	{
		private readonly IRemoteDataService _remoteDataService;
		private readonly IAnalytics _analytics;
		private readonly IMvxMessenger _messenger;
		private readonly IAuthentication _authentication;
		private readonly ILocalConferencesRepository _localConferencesRepository;

		private readonly ISQLiteConnection _connection;
		private readonly IMessageBox _messageBox;
		private readonly INetworkConnection _networkConnection;
		private MvxSubscriptionToken _favoritesUpdatedMessageToken;

		public ConferenceSessionsViewModel(IRemoteDataService remoteDataService, IAnalytics analytics, IMvxMessenger messenger,
																			IAuthentication authentication, 
																			ILocalConferencesRepository localConferencesRepository,
																			ISQLiteConnection connection, IMessageBox messageBox, INetworkConnection networkConnection)
		{
			_remoteDataService = remoteDataService;
			_analytics = analytics;
			_messenger = messenger;
			_authentication = authentication;
			_localConferencesRepository = localConferencesRepository;
			_connection = connection;
			_messageBox = messageBox;
			_networkConnection = networkConnection;

			_favoritesUpdatedMessageToken = _messenger.Subscribe<FavoriteSessionAddedMessage>(OnFavoritesUpdatedMessage);
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

		public string PageTitle { get; set; }

		public bool IsAuthenticated { get; set; }

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
					shouldAddFavorites = (Schedule == null || Schedule.sessions == null || !Schedule.sessions.Any());
				}

				return shouldAddFavorites;
			}
		}

		private void OnFavoritesUpdatedMessage(FavoriteSessionAddedMessage message)
		{
			this.DisplayFavoriteSessions(message.Schedule);
		}

		private void DisplayFavoriteSessions(ScheduleDto schedule)
		{
			Schedule = schedule;
		}

		private async void StartGetConference(string slug, bool isRefreshing = false)
		{
			if (IsLoadingConference)
				return;

			IsLoadingConference = true;
			_analytics.SendView("ConferenceSessions-" + slug);
			
			if (!isRefreshing)
			{
				var conference = _localConferencesRepository.Get(slug);
				if (conference != null)
				{
					IEnumerable<SessionEntity> sessions = conference.Sessions(_connection);
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
						if (!_networkConnection.IsNetworkConnected())
						{
							InvokeOnMainThread(() => _messageBox.Show(_networkConnection.NetworkDownMessage));
						}
						else
						{
							var sessionsList = await _remoteDataService.GetConferenceSessionsList(slug);
							GetConferenceSuccess(sessionsList);
						}
					}
				}
				else
				{
					if (!_networkConnection.IsNetworkConnected())
					{
						InvokeOnMainThread(() => _messageBox.Show(_networkConnection.NetworkDownMessage));
					}
					else
					{
						var sessionsList = await _remoteDataService.GetConferenceSessionsList(slug);
						GetConferenceSuccess(sessionsList);
					}
				}
			}
			else
			{
				if (!_networkConnection.IsNetworkConnected())
				{
					InvokeOnMainThread(() => _messageBox.Show(_networkConnection.NetworkDownMessage));
				}
				else
				{
					var sessionsList = await _remoteDataService.GetConferenceSessionsList(slug);
					GetConferenceSuccess(sessionsList);
				}
			}
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
				RaisePropertyChanged("IsLoadingConference");
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
				RaisePropertyChanged("Conference");
				RaisePropertyChanged("HasSessions");

				IsLoadingConference = false;

			}
		}

		private async void StartGetSchedule(string userName, string conferenceSlug, bool isRefreshing)
		{
			if (IsLoadingSchedule)
				return;

			IsLoadingSchedule = true;
			if (!isRefreshing)
			{
				var favorites = await _localConferencesRepository.ListFavoriteSessionsAsync(conferenceSlug);
				if (favorites != null && favorites.Any())
				{
					var dtos = favorites.Select(s => new FullSessionDto(s)).ToList();
					var schedule = new ScheduleDto() { conferenceSlug = conferenceSlug, sessions = dtos, url = "", userSlug = _authentication.UserName };

					GetScheduleSuccess(schedule);
				}
				else
				{
					if (!_networkConnection.IsNetworkConnected())
					{
						InvokeOnMainThread(() => _messageBox.Show(_networkConnection.NetworkDownMessage));
					}
					else
					{
						var schedule = await _remoteDataService.GetScheduleAsync(userName, conferenceSlug, isRefreshing: false, connection: _connection);
						GetScheduleSuccess(schedule);
					}
				}
			}
			else
			{
				if (!_networkConnection.IsNetworkConnected())
				{
					InvokeOnMainThread(() => _messageBox.Show(_networkConnection.NetworkDownMessage));
				}
				else
				{
					var schedule = await _remoteDataService.GetScheduleAsync(userName, conferenceSlug, isRefreshing: true, connection: _connection);
					GetScheduleSuccess(schedule);
				}
			}
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

		public bool IsLoadingSchedule { get; set; }
		

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
				RaisePropertyChanged("HasSessions");
			}
		}

		public List<FullSessionGroup> Sessions
		{
			get
			{
				if (Schedule != null && Schedule.sessions != null)
				{
					var grouped = Schedule.sessions
						.OrderBy(x => x.start)
						.GroupBy(session => session.start.ToString("ddd, h:mm tt"))
						.Select(slot => new FullSessionGroup(
							slot.Key,
							slot.OrderBy(session => session.start).ThenBy(t => t.title)));

					var groupList = grouped.ToList();
					return groupList;
				}
				else
					return new List<FullSessionGroup>();
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
				RaisePropertyChanged("Schedule");
				RaisePropertyChanged("Sessions");
				RaisePropertyChanged("ShouldAddFavorites");
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