using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Cirrious.MvvmCross.Plugins.Messenger;
using Cirrious.MvvmCross.ViewModels;
using TekConf.Core.Interfaces;
using TekConf.Core.Messages;
using TekConf.Core.Models;
using TekConf.Core.Repositories;
using TekConf.Core.Services;
using TekConf.RemoteData.Dtos.v1;

namespace TekConf.Core.ViewModels
{
	using System.Threading.Tasks;

	using Cirrious.MvvmCross.Plugins.File;
	using Cirrious.MvvmCross.Plugins.Sqlite;

	using TekConf.Core.Entities;

	public class ConferenceDetailViewModel : MvxViewModel
	{
		private readonly IRemoteDataService _remoteDataService;
		private readonly ILocalConferencesRepository _localConferencesRepository;
		private readonly IAnalytics _analytics;
		private readonly IAuthentication _authentication;
		private readonly IMvxMessenger _messenger;
		private readonly IMvxFileStore _fileStore;
		private readonly IMessageBox _messageBox;
		private readonly INetworkConnection _networkConnection;

		private readonly ISQLiteConnection _sqLiteConnection;

		public ConferenceDetailViewModel(IRemoteDataService remoteDataService, ILocalConferencesRepository localConferencesRepository, 
			IAnalytics analytics, IAuthentication authentication, IMvxMessenger messenger, IMvxFileStore fileStore, 
			IMessageBox messageBox, INetworkConnection networkConnection, ISQLiteConnection sqLiteConnection)
		{
			_remoteDataService = remoteDataService;
			_localConferencesRepository = localConferencesRepository;
			_analytics = analytics;
			_authentication = authentication;
			_messenger = messenger;
			_fileStore = fileStore;
			_messageBox = messageBox;
			_networkConnection = networkConnection;
			_sqLiteConnection = sqLiteConnection;
		}

		public async void Init(string slug)
		{
			var conference = await StartGetConference(slug, isRefreshing: false);
			Success(conference);
		}

		public async void Refresh(string slug)
		{
			var conference = await StartGetConference(slug, isRefreshing: true);
			Success(conference);
		}

		private async Task<ConferenceDetailViewDto> StartGetConference(string slug, bool isRefreshing = false)
		{
			ConferenceDetailViewDto conferenceDto = null;
			if (IsLoading)
				return null;

			IsLoading = true;
			_analytics.SendView("ConferenceDetail-" + slug);

			if (!isRefreshing)
			{
				var conference = _localConferencesRepository.Get(slug);
				if (conference != null)
				{
					conferenceDto = new ConferenceDetailViewDto(conference);
				}
				else
				{
					if (!_networkConnection.IsNetworkConnected())
					{
						InvokeOnMainThread(() => _messageBox.Show(_networkConnection.NetworkDownMessage));
					}
					else
					{
						conferenceDto = await _remoteDataService.GetConferenceDetailAsync(slug, isRefreshing: false);
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
					conferenceDto = await _remoteDataService.GetConferenceDetailAsync(slug, isRefreshing: true);
				}
			}

			return conferenceDto;
		}

		public IEnumerable<SessionEntity> Sessions;

		public List<SessionEntityGroup> SessionsByTime
		{
			get
			{
				var grouped = Sessions
												.OrderBy(x => x.Start)
												.GroupBy(session => session.Start.ToString("ddd, h:mm tt"))
												.Select(slot => new SessionEntityGroup(
																				slot.Key,
																				slot.OrderBy(session => session.Start).ThenBy(t => t.Title)));

				var groupList = grouped.ToList();
				return groupList;
			}
		}

		private void Success(ConferenceDetailViewDto conference)
		{
			var conferenceEntity = _localConferencesRepository.Get(conference.slug);
			if (conferenceEntity != null)
			{
				IEnumerable<SessionEntity> sessions = conferenceEntity.Sessions(_sqLiteConnection);
				if (sessions != null)
				{
					InvokeOnMainThread(() => Sessions = sessions);

				}
			}

			InvokeOnMainThread(() => DisplayConference(conference));
		}

		private void DisplayConference(ConferenceDetailViewDto conference)
		{
			IsAuthenticated = _authentication.IsAuthenticated;
			IsLoading = false;
			Conference = conference;
			_messenger.Publish(new RefreshConferenceFavoriteIconMessage(this));
		}

		public bool IsLoading { get; set; }
		

		public bool HasSessions
		{
			get
			{
				return Conference != null && Conference.hasSessions;
			}
		}

		public bool IsOnline
		{
			get
			{
				return Conference != null && !string.IsNullOrWhiteSpace(Conference.FormattedAddress) && Conference.FormattedAddress.ToLower() == "online";
			}
		}
		public bool IsAuthenticated { get; set; }
		public string PageTitle { get; set; }

		private ConferenceDetailViewDto _conference;
		public ConferenceDetailViewDto Conference
		{
			get
			{
				return _conference;
			}
			set
			{
				if (_conference != value)
				{
					_conference = value;
					if (value != null)
						PageTitle = value.name;

					RaisePropertyChanged(() => Conference);
					RaisePropertyChanged(() => ConnectItems);
					RaisePropertyChanged(() => HasConnectItems);
				}
			}
		}

		public bool HasConnectItems
		{
			get
			{
				if (Conference.IsNull())
					return false;

				return !Conference.facebookUrl.IsNullOrWhiteSpace()
					|| !Conference.homepageUrl.IsNullOrWhiteSpace()
					|| !Conference.lanyrdUrl.IsNullOrWhiteSpace()
					|| !Conference.meetupUrl.IsNullOrWhiteSpace()
					|| !Conference.googlePlusUrl.IsNullOrWhiteSpace()
					|| !Conference.vimeoUrl.IsNullOrWhiteSpace()
					|| !Conference.youtubeUrl.IsNullOrWhiteSpace()
					|| !Conference.githubUrl.IsNullOrWhiteSpace()
					|| !Conference.linkedInUrl.IsNullOrWhiteSpace()
					|| !Conference.twitterHashTag.IsNullOrWhiteSpace()
					|| !Conference.twitterName.IsNullOrWhiteSpace();

			}
		}

		private List<ConnectItem> _connectItems;
		public List<ConnectItem> ConnectItems
		{
			get
			{
				if (_connectItems == null)
					_connectItems = new List<ConnectItem>();

				if (Conference != null)
				{
					if (!Conference.facebookUrl.IsNullOrWhiteSpace() && _connectItems.All(x => x.Name != "facebookUrl"))
						_connectItems.Add(new ConnectItem { Name = "facebookUrl", Value = Conference.facebookUrl });

					if (!Conference.homepageUrl.IsNullOrWhiteSpace() && _connectItems.All(x => x.Name != "homepageUrl"))
						_connectItems.Add(new ConnectItem { Name = "homepageUrl", Value = Conference.homepageUrl });

					if (!Conference.lanyrdUrl.IsNullOrWhiteSpace() && _connectItems.All(x => x.Name != "lanyrdUrl"))
						_connectItems.Add(new ConnectItem { Name = "lanyrdUrl", Value = Conference.lanyrdUrl });

					if (!Conference.meetupUrl.IsNullOrWhiteSpace() && _connectItems.All(x => x.Name != "meetupUrl"))
						_connectItems.Add(new ConnectItem { Name = "meetupUrl", Value = Conference.meetupUrl });

					if (!Conference.googlePlusUrl.IsNullOrWhiteSpace() && _connectItems.All(x => x.Name != "googlePlusUrl"))
						_connectItems.Add(new ConnectItem { Name = "googlePlusUrl", Value = Conference.googlePlusUrl });

					if (!Conference.vimeoUrl.IsNullOrWhiteSpace() && _connectItems.All(x => x.Name != "vimeoUrl"))
						_connectItems.Add(new ConnectItem { Name = "vimeoUrl", Value = Conference.vimeoUrl });

					if (!Conference.youtubeUrl.IsNullOrWhiteSpace() && _connectItems.All(x => x.Name != "youtubeUrl"))
						_connectItems.Add(new ConnectItem { Name = "youtubeUrl", Value = Conference.youtubeUrl });

					if (!Conference.githubUrl.IsNullOrWhiteSpace() && _connectItems.All(x => x.Name != "githubUrl"))
						_connectItems.Add(new ConnectItem { Name = "githubUrl", Value = Conference.githubUrl });

					if (!Conference.linkedInUrl.IsNullOrWhiteSpace() && _connectItems.All(x => x.Name != "linkedInUrl"))
						_connectItems.Add(new ConnectItem { Name = "linkedInUrl", Value = Conference.linkedInUrl });

					if (!Conference.twitterHashTag.IsNullOrWhiteSpace() && _connectItems.All(x => x.Name != "twitterHashTag"))
						_connectItems.Add(new ConnectItem { Name = "twitterHashTag", Value = Conference.twitterHashTag });

					if (!Conference.twitterName.IsNullOrWhiteSpace() && _connectItems.All(x => x.Name != "twitterName"))
						_connectItems.Add(new ConnectItem { Name = "twitterName", Value = Conference.twitterName });

				}
				return _connectItems.OrderBy(x => x.Name).ToList();
			}
		}

		public ICommand ShowSessionsCommand
		{
			get
			{
				return new MvxCommand<string>(slug => ShowViewModel<ConferenceSessionsViewModel>(new { slug }));
			}
		}

		public ICommand ShowSettingsCommand
		{
			get
			{
				return new MvxCommand(() => ShowViewModel<SettingsViewModel>());
			}
		}

		public ICommand AddFavoriteCommand
		{
			get
			{
				return new ActionCommand(AddConferenceToFavorites);
			}
		}

		private void GetFavoritesError(Exception exception)
		{

		}

		private void GetFavoritesSuccess(IEnumerable<ConferencesListViewDto> conferences)
		{
		}


		private async void AddConferenceToFavorites()
		{
			if (_authentication.IsAuthenticated)
			{
				var addSuccess = new Action<ScheduleDto>(dto =>
				{
					Conference.isAddedToSchedule = true;
					RaisePropertyChanged(() => Conference);
					_messenger.Publish(new RefreshConferenceFavoriteIconMessage(this));
				});

				var addError = new Action<Exception>(ex =>
				{
					Conference.isAddedToSchedule = false;
					RaisePropertyChanged(() => Conference);
					_messenger.Publish(new RefreshConferenceFavoriteIconMessage(this));
				});

				var removeSuccess = new Action<ScheduleDto>(dto =>
				{
					Conference.isAddedToSchedule = false;
					RaisePropertyChanged(() => Conference);
					_messenger.Publish(new RefreshConferenceFavoriteIconMessage(this));
				});

				var removeError = new Action<Exception>(ex =>
				{
					Conference.isAddedToSchedule = true;
					RaisePropertyChanged(() => Conference);
					_messenger.Publish(new RefreshConferenceFavoriteIconMessage(this));
				});

				var conference = _localConferencesRepository.Get(Conference.slug);

				if (Conference.isAddedToSchedule == true)
				{
					removeSuccess(null);
					
					conference.IsAddedToSchedule = false;
					_localConferencesRepository.Save(conference);

					var conferences = await _localConferencesRepository.ListFavoritesAsync();

					if (conferences != null && conferences.Any())
					{
						var dtos = conferences.Select(c => new ConferencesListViewDto(c, _fileStore)).ToList();
						_messenger.Publish(new FavoriteConferencesUpdatedMessage(this, dtos));
						_messenger.Publish(new RefreshConferenceFavoriteIconMessage(this));
					}

					if (!_networkConnection.IsNetworkConnected())
					{
						InvokeOnMainThread(() => _messageBox.Show(_networkConnection.NetworkDownMessage));
					}
					else
					{
						var scheduleDto = await _remoteDataService.RemoveFromScheduleAsync(_authentication.UserName, Conference.slug);
					}
				}
				else
				{
					var schedule = new ScheduleDto() { conferenceSlug = Conference.slug, sessions = new List<FullSessionDto>(), url = "", userSlug = _authentication.UserName };
					conference.IsAddedToSchedule = true;
					_localConferencesRepository.Save(conference);
					var conferences = await _localConferencesRepository.ListFavoritesAsync();
					if (conferences != null && conferences.Any())
					{
						var dtos = conferences.Select(c => new ConferencesListViewDto(c, _fileStore)).ToList();
						_messenger.Publish(new FavoriteConferencesUpdatedMessage(this, dtos));
						_messenger.Publish(new RefreshConferenceFavoriteIconMessage(this));
					}

					addSuccess(schedule);

					if (!_networkConnection.IsNetworkConnected())
					{
						InvokeOnMainThread(() => _messageBox.Show(_networkConnection.NetworkDownMessage));
					}
					else
					{
						_remoteDataService.AddToSchedule(_authentication.UserName, Conference.slug, addSuccess, addError);
					}
				}
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