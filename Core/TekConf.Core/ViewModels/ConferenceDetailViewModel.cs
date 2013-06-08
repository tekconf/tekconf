using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Cirrious.MvvmCross.Plugins.Messenger;
using Cirrious.MvvmCross.ViewModels;
using TekConf.Core.Interfaces;
using TekConf.Core.Models;
using TekConf.Core.Repositories;
using TekConf.Core.Services;
using TekConf.RemoteData.Dtos.v1;

namespace TekConf.Core.ViewModels
{
	public class ConferenceDetailViewModel : MvxViewModel
	{
		private readonly IRemoteDataService _remoteDataService;
		private readonly IAnalytics _analytics;
		private readonly IAuthentication _authentication;
		private readonly IMvxMessenger _messenger;

		public ConferenceDetailViewModel(IRemoteDataService remoteDataService, IAnalytics analytics, IAuthentication authentication, IMvxMessenger messenger)
		{
			_remoteDataService = remoteDataService;
			_analytics = analytics;
			_authentication = authentication;
			_messenger = messenger;
			AddFavoriteCommand = new ActionCommand(AddConferenceToFavorites);
		}

		public void Init(string slug)
		{
			StartSearch(slug);
		}

		public void Refresh(string slug)
		{
			StartSearch(slug, true);
		}

		private void StartSearch(string slug, bool isRefreshing = false)
		{
			if (IsLoading)
				return;

			IsLoading = true;
			_analytics.SendView("ConferenceDetail-" + slug);
			_remoteDataService.GetConferenceDetail(slug, isRefreshing, Success, Error);
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
		}

		private bool _isLoading;
		public bool IsLoading
		{
			get { return _isLoading; }
			set { _isLoading = value; RaisePropertyChanged(() => IsLoading); }
		}

		public bool HasSessions
		{
			get
			{
				return Conference != null && Conference.hasSessions;
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
					_messenger.Publish(new FavoriteRefreshMessage(this));
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
						_connectItems.Add(new ConnectItem { Name ="facebookUrl", Value = Conference.facebookUrl});

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
				return new MvxCommand<string>(slug => ShowViewModel<ConferenceSessionsViewModel>(new {slug }));
			}
		}

		public ICommand ShowSettingsCommand
		{
			get
			{
				return new MvxCommand(() => ShowViewModel<SettingsViewModel>());
			}
		}


		public ICommand AddFavoriteCommand { get; set; }

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
					_messenger.Publish(new FavoriteAddedMessage(this, dto));
				});

				var addError = new Action<Exception>(ex =>
				{
					Conference.isAddedToSchedule = false;
					RaisePropertyChanged(() => Conference);
					RefreshFavorites();
				});

				var removeSuccess = new Action<ScheduleDto>(dto =>
				{
					Conference.isAddedToSchedule = false;
					RaisePropertyChanged(() => Conference);
					RefreshFavorites();
				});
				var removeError = new Action<Exception>(ex =>
				{
					Conference.isAddedToSchedule = true;
					RaisePropertyChanged(() => Conference);
					RefreshFavorites();
				});

				if (Conference.isAddedToSchedule == true)
				{
					removeSuccess(null);
					_remoteDataService.RemoveFromSchedule(_authentication.UserName, Conference.slug, removeSuccess, removeError);					
				}
				else
				{
					addSuccess(null);
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