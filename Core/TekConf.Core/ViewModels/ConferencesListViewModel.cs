using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Cirrious.MvvmCross.Plugins.Messenger;
using Cirrious.MvvmCross.ViewModels;
using TekConf.Core.Interfaces;
using TekConf.Core.Services;
using TekConf.RemoteData.Dtos.v1;

namespace TekConf.Core.ViewModels
{
	public class ConferencesListViewModel : MvxViewModel
	{
		private readonly IRemoteDataService _remoteDataService;
		private readonly IAnalytics _analytics;
		private readonly IAuthentication _authentication;
		private readonly ICacheService _cache;
		private readonly IMvxMessenger _messenger;
		private MvxSubscriptionToken _token;

		public ConferencesListViewModel(IRemoteDataService remoteDataService, IAnalytics analytics, IAuthentication authentication, ICacheService cache, IMvxMessenger messenger)
		{
			_remoteDataService = remoteDataService;
			_analytics = analytics;
			_authentication = authentication;
			_cache = cache;
			_messenger = messenger;
			_token = _messenger.Subscribe<AuthenticationMessage>(OnAuthenticateMessage);
		}

		private void OnAuthenticateMessage(AuthenticationMessage message)
		{
			if (message != null && !string.IsNullOrWhiteSpace(message.UserName))
			{
				_authentication.UserName = message.UserName;
				IsAuthenticated = true;
				StartGetFavorites(isRefreshing: true);
			}
		}

		public void Init(string searchTerm)
		{
			StartGetAll(searchTerm);
			StartGetFavorites();
		}

		public void Refresh()
		{
			StartGetAll(isRefreshing: true);
			StartGetFavorites(isRefreshing: true);
		}

		private void StartGetAll(string searchTerm = "", bool isRefreshing = false)
		{
			if (IsLoadingConferences)
				return;

			IsLoadingConferences = true;
			_analytics.SendView("ConferencesList");

			_remoteDataService.GetConferences(search: searchTerm, isRefreshing: isRefreshing, success: GetAllSuccess, error: GetAllError);
		}

		private void StartGetFavorites(bool isRefreshing = false)
		{
			if (IsLoadingFavorites)
				return;

			if (_authentication.IsAuthenticated)
			{
				IsLoadingFavorites = true;

				var userName = _authentication.UserName;
				_analytics.SendView("ConferencesListSchedule-" + userName);
				_remoteDataService.GetSchedules(userName, isRefreshing, GetFavoritesSuccess, GetFavoritesError);
			}
		}

		private void GetAllError(Exception exception)
		{
			// for now we just hide the error...
			_messenger.Publish(new ExceptionMessage(this, exception));

			IsLoadingConferences = false;
		}

		private void GetFavoritesError(Exception exception)
		{
			// for now we just hide the error...
			_messenger.Publish(new ExceptionMessage(this, exception));

			IsLoadingFavorites = false;
		}

		private void GetAllSuccess(IEnumerable<FullConferenceDto> enumerable)
		{
			InvokeOnMainThread(() => DisplayAllConferences(enumerable));
		}

		private void GetFavoritesSuccess(IEnumerable<FullConferenceDto> conferences)
		{
			var conferencesList = conferences.ToList();

			UpdateConferenceFavorites();
			InvokeOnMainThread(() => DisplayFavoritesConferences(conferencesList));
		}

		private void UpdateConferenceFavorites()
		{
			//var json = _cache.Get<string, string>("conferences.json");
			//string json = null;
			//List<FullConferenceDto> cachedConferences = null;
			//if (!string.IsNullOrWhiteSpace(json))
			//{
			//	cachedConferences = JsonConvert.DeserializeObject<List<FullConferenceDto>>(json);
			//	foreach (var conference in conferences)
			//	{
			//		//TODO : Set isAddedToSchedule
			//	}
			//}
		}

		private void DisplayAllConferences(IEnumerable<FullConferenceDto> enumerable)
		{
			Conferences = enumerable.ToList();
		}

		private void DisplayFavoritesConferences(IEnumerable<FullConferenceDto> enumerable)
		{
			Favorites = enumerable.ToList();
		}

		private bool _isLoadingConferences;
		public bool IsLoadingConferences
		{
			get
			{
				return _isLoadingConferences;
			}
			set
			{
				_isLoadingConferences = value;
				IsAuthenticated = _authentication.IsAuthenticated;
				RaisePropertyChanged(() => IsLoadingConferences);
			}
		}

		private bool _isLoadingFavorites;
		public bool IsLoadingFavorites
		{
			get { return _isLoadingFavorites; }
			set { _isLoadingFavorites = value; RaisePropertyChanged(() => IsLoadingFavorites); }
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

		private List<FullConferenceDto> _conferences;
		public List<FullConferenceDto> Conferences
		{
			get
			{
				return _conferences;
			}
			set
			{
				_conferences = value;
				RaisePropertyChanged(() => Conferences);
				IsLoadingConferences = false;
			}
		}

		private FullConferenceDto _selectedFavorite;
		public FullConferenceDto SelectedFavorite
		{
			get
			{
				return _selectedFavorite;
			}
			set
			{
				_selectedFavorite = value;
				RaisePropertyChanged(() => SelectedFavorite);
			}
		}

		private List<FullConferenceDto> _favorites;
		public List<FullConferenceDto> Favorites
		{
			get
			{
				return _favorites;
			}
			set
			{
				_favorites = value;
				_cache.Remove("schedules");
				_cache.Add("schedules", value, new TimeSpan(0, 0, 15));
				if (_favorites != null)
					SelectedFavorite = _favorites.FirstOrDefault(x => x.start >= DateTime.Now);

				RaisePropertyChanged(() => Favorites);
				IsLoadingFavorites = false;
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
				return new MvxCommand(() => ShowViewModel<ConferencesSearchViewModel>());
			}
		}

		public ICommand ShowDetailCommand
		{
			get
			{
				return new MvxCommand<string>(slug => ShowViewModel<ConferenceDetailViewModel>(new {slug }));
			}
		}
	}
}
