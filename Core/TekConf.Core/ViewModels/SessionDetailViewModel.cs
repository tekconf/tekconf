using System;
using System.Collections.Generic;
using System.Windows.Input;
using Cirrious.MvvmCross.Plugins.Messenger;
using Cirrious.MvvmCross.ViewModels;
using TekConf.Core.Interfaces;
using TekConf.Core.Messages;
using TekConf.Core.Repositories;
using TekConf.Core.Services;
using TekConf.RemoteData.Dtos.v1;

namespace TekConf.Core.ViewModels
{
	using System.Linq;

	public class SessionDetailViewModel : MvxViewModel
	{
		private readonly IRemoteDataService _remoteDataService;
		private readonly IAnalytics _analytics;
		private readonly ILocalConferencesRepository _localConferencesRepository;
		private readonly IMvxMessenger _messenger;
		private readonly IAuthentication _authentication;

		public SessionDetailViewModel(IRemoteDataService remoteDataService, IAnalytics analytics, ILocalConferencesRepository localConferencesRepository, IMvxMessenger messenger, IAuthentication authentication)
		{
			_remoteDataService = remoteDataService;
			_analytics = analytics;
			_localConferencesRepository = localConferencesRepository;
			_messenger = messenger;
			_authentication = authentication;
			AddFavoriteCommand = new ActionCommand(AddSessionToFavorites);

		}

		public void Init(Navigation navigation)
		{
			StartGetSession(navigation);
		}

		public ICommand AddFavoriteCommand { get; set; }
		public void Refresh(Navigation navigation)
		{
			StartGetSession(navigation, true);
		}

		private void StartGetSession(Navigation navigation, bool isRefreshing = false)
		{
			if (IsLoading)
				return;

			IsLoading = true;
			ConferenceSlug = navigation.ConferenceSlug;
			_analytics.SendView("SessionDetail-" + navigation.ConferenceSlug + "-" + navigation.SessionSlug);
			if (!isRefreshing)
			{
				var sessionEntity = _localConferencesRepository.Get(ConferenceSlug, navigation.SessionSlug);
				if (sessionEntity != null)
				{
					var sessionDetailDto = new SessionDetailDto(sessionEntity);
					GetSessionSuccess(sessionDetailDto);
				}
				else
				{
					_remoteDataService.GetSession(navigation.ConferenceSlug, navigation.SessionSlug, isRefreshing, GetSessionSuccess, GetConferenceError);					
				}
			}
			else
			{
				_remoteDataService.GetSession(navigation.ConferenceSlug, navigation.SessionSlug, isRefreshing, GetSessionSuccess, GetConferenceError);
			}
		}

		private void GetConferenceError(Exception exception)
		{
			// for now we just hide the error...
			_messenger.Publish(new SessionDetailExceptionMessage(this, exception));

			IsLoading = false;
		}

		private void GetSessionSuccess(SessionDetailDto session)
		{
			InvokeOnMainThread(() => DisplaySession(session));
		}

		private void DisplaySession(SessionDetailDto session)
		{
			IsLoading = false;
			Session = session;
		}

		private async void AddSessionToFavorites()
		{
			if (_authentication.IsAuthenticated)
			{
				var addSuccess = new Action<ScheduleDto>(dto =>
				{
					Session.isAddedToSchedule = true;
					RaisePropertyChanged(() => Session);
					_messenger.Publish(new RefreshSessionFavoriteIconMessage(this));
				});

				var addError = new Action<Exception>(ex =>
				{
					Session.isAddedToSchedule = false;
					RaisePropertyChanged(() => Session);
					_messenger.Publish(new RefreshSessionFavoriteIconMessage(this));
				});

				var removeSuccess = new Action<ScheduleDto>(dto =>
				{
					Session.isAddedToSchedule = false;
					RaisePropertyChanged(() => Session);
					_messenger.Publish(new RefreshSessionFavoriteIconMessage(this));

				});

				var removeError = new Action<Exception>(ex =>
				{
					Session.isAddedToSchedule = true;
					RaisePropertyChanged(() => Session);
					_messenger.Publish(new RefreshSessionFavoriteIconMessage(this));

				});

				var session = _localConferencesRepository.Get(_conferenceSlug, _session.slug);
				if (session.IsAddedToSchedule == true)
				{
					removeSuccess(null);
					session.IsAddedToSchedule = false;
					_localConferencesRepository.Save(_conferenceSlug, session);
					var favorites = await _localConferencesRepository.ListFavoriteSessionsAsync(_conferenceSlug);
					if (favorites != null && favorites.Any())
					{
						var dtos = favorites.Select(s => new FullSessionDto(s)).ToList();
						var schedule = new ScheduleDto() { conferenceSlug = _conferenceSlug, sessions = dtos, url = "", userSlug = _authentication.UserName };

						_messenger.Publish(new FavoriteSessionAddedMessage(this, schedule));
						_messenger.Publish(new RefreshSessionFavoriteIconMessage(this));
					}
					var scheduleDto = await _remoteDataService.RemoveSessionFromScheduleAsync(_authentication.UserName, ConferenceSlug, Session.slug);
				}
				else
				{
					session.IsAddedToSchedule = true;
					_localConferencesRepository.Save(_conferenceSlug, session);

					var favorites = await _localConferencesRepository.ListFavoriteSessionsAsync(_conferenceSlug);
					if (favorites != null && favorites.Any())
					{
						var dtos = favorites.Select(s => new FullSessionDto(s)).ToList();
						var schedule = new ScheduleDto() { conferenceSlug = _conferenceSlug, sessions = dtos, url = "", userSlug = _authentication.UserName };
						_messenger.Publish(new FavoriteSessionAddedMessage(this, schedule));
						_messenger.Publish(new RefreshSessionFavoriteIconMessage(this));
					}

					addSuccess(null); //TODO : addSuccess(schedule);
					_remoteDataService.AddSessionToSchedule(_authentication.UserName, ConferenceSlug, Session.slug, addSuccess, addError);

				}

			}
		}

		private async void RefreshFavorites()
		{
			var userName = _authentication.UserName;

			var remoteConferences = await _remoteDataService.GetFavoritesAsync(userName, isRefreshing:true);
			if (remoteConferences != null)
			{
				GetFavoritesSuccess(remoteConferences);
			}
		}


		private void GetFavoritesError(Exception exception)
		{

		}

		private void GetFavoritesSuccess(IEnumerable<ConferencesListViewDto> conferences)
		{
		}

		private bool _isLoading;
		public bool IsLoading
		{
			get { return _isLoading; }
			set { _isLoading = value; RaisePropertyChanged(() => IsLoading); }
		}

		private SessionDetailDto _session;
		public SessionDetailDto Session
		{
			get
			{
				return _session;
			}
			set
			{
				_session = value;
				PageTitle = _session.title;
				RaisePropertyChanged(() => Session);
				_messenger.Publish(new RefreshSessionFavoriteIconMessage(this));

			}
		}

		private string _conferenceSlug;
		public string ConferenceSlug
		{
			get
			{
				return _conferenceSlug;
			}
			set
			{
				_conferenceSlug = value;
				RaisePropertyChanged(() => ConferenceSlug);
			}
		}

		public ICommand ShowSettingsCommand
		{
			get
			{
				return new MvxCommand(() => ShowViewModel<SettingsViewModel>());
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
				//_pageTitle = "TEKCONF";
				RaisePropertyChanged(() => PageTitle);
			}
		}

		public class Navigation
		{
			public string ConferenceSlug { get; set; }
			public string SessionSlug { get; set; }

		}

	}
}
