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
	using Cirrious.MvvmCross.Plugins.File;

	public class ConferenceDetailViewModel : MvxViewModel
	{
		private readonly IRemoteDataService _remoteDataService;
		private readonly ILocalConferencesRepository _localConferencesRepository;
		private readonly IAnalytics _analytics;
		private readonly IAuthentication _authentication;
		private readonly IMvxMessenger _messenger;
		private readonly IMvxFileStore _fileStore;

		public ConferenceDetailViewModel(IRemoteDataService remoteDataService, ILocalConferencesRepository localConferencesRepository, 
			IAnalytics analytics, IAuthentication authentication, IMvxMessenger messenger, IMvxFileStore fileStore)
		{
			_remoteDataService = remoteDataService;
			_localConferencesRepository = localConferencesRepository;
			_analytics = analytics;
			_authentication = authentication;
			_messenger = messenger;
			this._fileStore = fileStore;
			//AddFavoriteCommand = new ActionCommand(AddConferenceToFavorites);
		}

		public void Init(string slug)
		{
			StartGetConference(slug);
		}

		public void Refresh(string slug)
		{
			StartGetConference(slug, true);
		}

		private void StartGetConference(string slug, bool isRefreshing = false)
		{
			if (IsLoading)
				return;

			IsLoading = true;
			_analytics.SendView("ConferenceDetail-" + slug);

			if (!isRefreshing)
			{
				var conference = _localConferencesRepository.Get(slug);
				if (conference != null)
				{
					var conferenceDetailView = new ConferenceDetailViewDto(conference);
					this.Success(conferenceDetailView);
					return;
				}
			}

			try
			{
				_remoteDataService.GetConferenceDetail(slug, isRefreshing, Success, Error);
			}
			catch (Exception ex)
			{
				Error(ex);
			}
		}

		private void Error(Exception exception)
		{
			// for now we just hide the error...
			_messenger.Publish(new ConferenceDetailExceptionMessage(this, exception));
			IsLoading = false;
		}

		private void Success(ConferenceDetailViewDto conference)
		{
			InvokeOnMainThread(() => DisplayConference(conference));
		}

		private void DisplayConference(ConferenceDetailViewDto conference)
		{
			IsAuthenticated = _authentication.IsAuthenticated;
			IsLoading = false;
			Conference = conference;
			_messenger.Publish(new FavoriteRefreshMessage(this));
		}

		public bool IsLoading { get; set; }
		

		public bool HasSessions
		{
			get
			{
				return Conference != null && Conference.hasSessions;
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

		private void RefreshFavorites()
		{
			var userName = _authentication.UserName;
			_remoteDataService.GetSchedules(userName, true, GetFavoritesSuccess, GetFavoritesError);
		}

		private void GetFavoritesError(Exception exception)
		{

		}

		private void GetFavoritesSuccess(IEnumerable<ConferencesListViewDto> conferences)
		{
		}


		private void AddConferenceToFavorites()
		{
			if (_authentication.IsAuthenticated)
			{
				var addSuccess = new Action<ScheduleDto>(dto =>
				{
					Conference.isAddedToSchedule = true;
					RaisePropertyChanged(() => Conference);
					//_messenger.Publish(new FavoriteAddedMessage(this, dto));
					//_messenger.Publish(new FavoriteRefreshMessage(this));

				});

				var addError = new Action<Exception>(ex =>
				{
					Conference.isAddedToSchedule = false;
					RaisePropertyChanged(() => Conference);
					//_messenger.Publish(new FavoriteRefreshMessage(this));
					//RefreshFavorites();
				});

				var removeSuccess = new Action<ScheduleDto>(dto =>
				{
					Conference.isAddedToSchedule = false;
					RaisePropertyChanged(() => Conference);
					//_messenger.Publish(new FavoriteRefreshMessage(this));
					//RefreshFavorites();
				});

				var removeError = new Action<Exception>(ex =>
				{
					Conference.isAddedToSchedule = true;
					RaisePropertyChanged(() => Conference);
					//_messenger.Publish(new FavoriteRefreshMessage(this));
					//RefreshFavorites();
				});

				var conference = _localConferencesRepository.Get(Conference.slug);

				if (Conference.isAddedToSchedule == true)
				{
					removeSuccess(null);
					
					conference.IsAddedToSchedule = false;
					_localConferencesRepository.Save(conference);

					var conferences = _localConferencesRepository.GetFavorites();

					if (conferences != null && conferences.Any())
					{
						var dtos = conferences.Select(c => new ConferencesListViewDto(c, _fileStore)).ToList();
						_messenger.Publish(new FavoriteConferencesUpdatedMessage(this, dtos));
					}

					_remoteDataService.RemoveFromSchedule(_authentication.UserName, Conference.slug, removeSuccess, removeError);
				}
				else
				{
					var schedule = new ScheduleDto() { conferenceSlug = Conference.slug, sessions = new List<FullSessionDto>(), url = "", userSlug = _authentication.UserName };
					conference.IsAddedToSchedule = true;
					_localConferencesRepository.Save(conference);
					var conferences = _localConferencesRepository.GetFavorites();
					if (conferences != null && conferences.Any())
					{
						var dtos = conferences.Select(c => new ConferencesListViewDto(c, _fileStore)).ToList();
						_messenger.Publish(new FavoriteConferencesUpdatedMessage(this, dtos));
					}

					addSuccess(schedule);
					_remoteDataService.AddToSchedule(_authentication.UserName, Conference.slug, addSuccess, addError);
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