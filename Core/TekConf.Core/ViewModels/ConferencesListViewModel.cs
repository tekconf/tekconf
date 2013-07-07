using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Cirrious.MvvmCross.Plugins.Messenger;
using Cirrious.MvvmCross.ViewModels;
using PropertyChanged;
using TekConf.Core.Interfaces;
using TekConf.Core.Messages;
using TekConf.Core.Repositories;
using TekConf.Core.Services;
using TekConf.RemoteData.Dtos.v1;

namespace TekConf.Core.ViewModels
{
	//[ImplementPropertyChanged]
	public class ConferencesListViewModel : MvxViewModel
	{
		private readonly IRemoteDataService _remoteDataService;
		private readonly ILocalConferencesRepository _localConferencesRepository;
		private readonly ILocalScheduleRepository _localScheduleRepository;
		private readonly IAnalytics _analytics;
		private readonly IAuthentication _authentication;
		private readonly IMvxMessenger _messenger;
		private MvxSubscriptionToken _authenticationMessageToken;
		private MvxSubscriptionToken _favoriteAddedMessageToken;
		private MvxSubscriptionToken _favoritesUpdatedMessageToken;

		public ConferencesListViewModel(IRemoteDataService remoteDataService,
																		ILocalConferencesRepository localConferencesRepository,
																		ILocalScheduleRepository localScheduleRepository,
																		IAnalytics analytics,
																		IAuthentication authentication,
																		IMvxMessenger messenger)
		{
			_remoteDataService = remoteDataService;
			_localConferencesRepository = localConferencesRepository;
			_localScheduleRepository = localScheduleRepository;
			_analytics = analytics;
			_authentication = authentication;
			_messenger = messenger;
			_authenticationMessageToken = _messenger.Subscribe<AuthenticationMessage>(OnAuthenticateMessage);
			_favoriteAddedMessageToken = _messenger.Subscribe<FavoriteAddedMessage>(OnFavoriteAddedMessage);
			_favoritesUpdatedMessageToken = _messenger.Subscribe<FavoriteConferencesUpdatedMessage>(OnFavoritesUpdatedMessage);
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

			if (!isRefreshing)
			{
				var conferences = _localConferencesRepository.GetConferencesListView();
				if (conferences != null && conferences.Any())
				{
					this.GetAllSuccess(conferences);
					return;
				}
			}

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

				if (!isRefreshing)
				{
					var schedule = _localScheduleRepository.GetConferencesList();

					if (schedule != null && schedule.Any())
					{
						GetFavoritesSuccess(schedule);
						return;
					}
				}

				_remoteDataService.GetSchedules(userName, isRefreshing, GetFavoritesSuccess, GetFavoritesError);
			}
		}

		private void GetAllError(Exception exception)
		{
			// for now we just hide the error...
			_messenger.Publish(new ConferencesListAllExceptionMessage(this, exception));

			IsLoadingConferences = false;
		}

		private void GetFavoritesError(Exception exception)
		{
			// for now we just hide the error...
			_messenger.Publish(new ConferencesListFavoritesExceptionMessage(this, exception));

			IsLoadingFavorites = false;
		}

		private void GetAllSuccess(IEnumerable<ConferencesListViewDto> conferences)
		{
			InvokeOnMainThread(() => DisplayAllConferences(conferences));
		}

		private void GetFavoritesSuccess(IEnumerable<ConferencesListViewDto> conferences)
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

		private void DisplayAllConferences(IEnumerable<ConferencesListViewDto> conferences)
		{
			Conferences = conferences.ToList();
			IsLoadingConferences = false;
		}

		private void DisplayFavoritesConferences(IEnumerable<ConferencesListViewDto> favorites)
		{
			Favorites = favorites.OrderByDescending(x => x.start).ToList();
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

		public bool ShouldAddFavorites
		{
			get
			{
				bool shouldAddFavorites = false;
				if (_authentication.IsAuthenticated)
				{
					shouldAddFavorites = Favorites == null || !Favorites.Any();
				}

				return shouldAddFavorites;
			}
		}

		//public bool IsLoadingFavorites { get; set; }
		private bool _isLoadingFavorites;
		public bool IsLoadingFavorites
		{
			get { return _isLoadingFavorites; }
			set
			{
				_isLoadingFavorites = value;
				RaisePropertyChanged(() => IsLoadingFavorites);
			}
		}

		//public bool IsAuthenticated { get; set; }
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

		//private List<ConferencesListViewDto> _conferences;
		public List<ConferencesListViewDto> Conferences { get; set; }
		//{
		//	get
		//	{
		//		return _conferences;
		//	}
		//	set
		//	{
		//		_conferences = value;
		//		RaisePropertyChanged(() => Conferences);
		//	}
		//}

		//public FullConferenceDto SelectedFavorite { get; set; }
		//private FullConferenceDto _selectedFavorite;
		public FullConferenceDto SelectedFavorite { get; set; }
		//{
		//	get
		//	{
		//		return _selectedFavorite;
		//	}
		//	set
		//	{
		//		_selectedFavorite = value;
		//		RaisePropertyChanged(() => SelectedFavorite);
		//	}
		//}

		private List<ConferencesListViewDto> _favorites;
		public List<ConferencesListViewDto> Favorites
		{
			get
			{
				return _favorites;
			}
			set
			{
				_favorites = value;
				RaisePropertyChanged(() => Favorites);
				RaisePropertyChanged(() => ShouldAddFavorites);
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
				return new MvxCommand<string>(slug => ShowViewModel<ConferenceDetailViewModel>(new { slug }));
			}
		}

		private void OnFavoritesUpdatedMessage(FavoriteConferencesUpdatedMessage message)
		{
			DisplayFavoritesConferences(message.Conferences);
		}

		private void OnFavoriteAddedMessage(FavoriteAddedMessage message)
		{
			StartGetFavorites(true);
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
	}
}
