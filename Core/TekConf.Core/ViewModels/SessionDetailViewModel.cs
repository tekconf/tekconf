using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
		private readonly IMessageBox _messageBox;
		private readonly INetworkConnection _networkConnection;

		public SessionDetailViewModel(IRemoteDataService remoteDataService, IAnalytics analytics, 
			ILocalConferencesRepository localConferencesRepository, 
			IMvxMessenger messenger, IAuthentication authentication,
			IMessageBox messageBox, INetworkConnection networkConnection)
		{
			_remoteDataService = remoteDataService;
			_analytics = analytics;
			_localConferencesRepository = localConferencesRepository;
			_messenger = messenger;
			_authentication = authentication;
			_messageBox = messageBox;
			_networkConnection = networkConnection;
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

		private async void StartGetSession(Navigation navigation, bool isRefreshing = false)
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
					var speakers = await _localConferencesRepository.GetSpeakersAsync(sessionEntity.Id);
					foreach (var speaker in speakers)
					{
						var speakerDto = new SpeakerDetailViewDto(speaker);
						sessionDetailDto.AddSpeaker(speakerDto);
					}
					GetSessionSuccess(sessionDetailDto);
				}
				else
				{
					if (!_networkConnection.IsNetworkConnected())
					{
						InvokeOnMainThread(() => _messageBox.Show(_networkConnection.NetworkDownMessage));
					}
					else
					{
						var sessionDetailDto = await _remoteDataService.GetSessionAsync(navigation.ConferenceSlug, navigation.SessionSlug);
						var session = _localConferencesRepository.Get(navigation.ConferenceSlug, sessionDetailDto.slug);
						var speakers = await _localConferencesRepository.GetSpeakersAsync(session.Id);
						foreach (var speaker in speakers)
						{
							var speakerDto = new SpeakerDetailViewDto(speaker);
							sessionDetailDto.AddSpeaker(speakerDto);
						}
						GetSessionSuccess(sessionDetailDto);	
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
					var sessionDetailDto = await _remoteDataService.GetSessionAsync(navigation.ConferenceSlug, navigation.SessionSlug);
					var session = _localConferencesRepository.Get(navigation.ConferenceSlug, sessionDetailDto.slug);
					var speakers = await _localConferencesRepository.GetSpeakersAsync(session.Id);
					foreach (var speaker in speakers)
					{
						var speakerDto = new SpeakerDetailViewDto(speaker);
						sessionDetailDto.AddSpeaker(speakerDto);
					}
					GetSessionSuccess(sessionDetailDto);	
				}
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
			if (_authentication.IsAuthenticated) {
				var addSuccess = new Action<ScheduleDto> (dto =>
				{
					Session.isAddedToSchedule = true;
					RaisePropertyChanged ("Session");
					_messenger.Publish (new RefreshSessionFavoriteIconMessage (this));
				});

				var addError = new Action<Exception> (ex =>
				{
					Session.isAddedToSchedule = false;
					RaisePropertyChanged ("Session");
					_messenger.Publish (new RefreshSessionFavoriteIconMessage (this));
				});

				var removeSuccess = new Action<ScheduleDto> (dto =>
				{
					Session.isAddedToSchedule = false;
					RaisePropertyChanged ("Session");
					_messenger.Publish (new RefreshSessionFavoriteIconMessage (this));

				});

				var removeError = new Action<Exception> (ex =>
				{
					Session.isAddedToSchedule = true;
					RaisePropertyChanged ("Session");
					_messenger.Publish (new RefreshSessionFavoriteIconMessage (this));

				});

				var session = _localConferencesRepository.Get (ConferenceSlug, _session.slug);
				if (session.IsAddedToSchedule == true) {
					removeSuccess (null);
					session.IsAddedToSchedule = false;
					_localConferencesRepository.Save (ConferenceSlug, session);
					var favorites = await _localConferencesRepository.ListFavoriteSessionsAsync (ConferenceSlug);
					if (favorites != null && favorites.Any ()) {
						var dtos = favorites.Select (s => new FullSessionDto (s)).ToList ();
						var schedule = new ScheduleDto () {
							conferenceSlug = ConferenceSlug,
							sessions = dtos,
							url = "",
							userSlug = _authentication.UserName
						};

						_messenger.Publish (new FavoriteSessionAddedMessage (this, schedule));
						_messenger.Publish (new RefreshSessionFavoriteIconMessage (this));
					}
					if (!_networkConnection.IsNetworkConnected ()) {
						InvokeOnMainThread (() => _messageBox.Show (_networkConnection.NetworkDownMessage));
					} else {
						var scheduleDto = await _remoteDataService.RemoveSessionFromScheduleAsync (_authentication.UserName, ConferenceSlug, Session.slug);
					}
				} else {
					session.IsAddedToSchedule = true;
					_localConferencesRepository.Save (ConferenceSlug, session);

					var favorites = await _localConferencesRepository.ListFavoriteSessionsAsync (ConferenceSlug);
					if (favorites != null && favorites.Any ()) {
						var dtos = favorites.Select (s => new FullSessionDto (s)).ToList ();
						var schedule = new ScheduleDto () {
							conferenceSlug = ConferenceSlug,
							sessions = dtos,
							url = "",
							userSlug = _authentication.UserName
						};
						_messenger.Publish (new FavoriteSessionAddedMessage (this, schedule));
						_messenger.Publish (new RefreshSessionFavoriteIconMessage (this));
					}

					addSuccess (null);

					if (!_networkConnection.IsNetworkConnected ()) {
						InvokeOnMainThread (() => _messageBox.Show (_networkConnection.NetworkDownMessage));
					} else {
						_remoteDataService.AddSessionToSchedule (_authentication.UserName, ConferenceSlug, Session.slug, addSuccess, addError);
					}

				}

			} else {
				_messageBox.Show ("You must be logged in to favorite a session");
			}
		}

		public bool IsLoading { get; set; }

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
				if (_session != null)
				{
					PageTitle = _session.title;
				}

				RaisePropertyChanged("Session");
				_messenger.Publish(new RefreshSessionFavoriteIconMessage(this));
			}
		}

		public string ConferenceSlug { get; set; }

		public ICommand ShowSettingsCommand
		{
			get
			{
				return new MvxCommand(() => ShowViewModel<SettingsViewModel>());
			}
		}

		public string PageTitle { get; set; }

		public class Navigation
		{
			public string ConferenceSlug { get; set; }
			public string SessionSlug { get; set; }
		}

	}
}
