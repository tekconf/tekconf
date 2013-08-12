using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Cirrious.MvvmCross.Plugins.File;
using Cirrious.MvvmCross.Plugins.Messenger;
using Cirrious.MvvmCross.ViewModels;
using TekConf.Core.Interfaces;
using TekConf.Core.Messages;
using TekConf.Core.Repositories;
using TekConf.Core.Services;
using TekConf.RemoteData.Dtos.v1;
using System.Threading.Tasks;

namespace TekConf.Core.ViewModels
{
	public class ConferencesListViewModel : MvxViewModel
	{
		private readonly IRemoteDataService _remoteDataService;
		private readonly ILocalConferencesRepository _localConferencesRepository;
		private readonly IAnalytics _analytics;
		private readonly IAuthentication _authentication;
		private readonly IMvxFileStore _fileStore;
		private readonly IMvxMessenger _messenger;
		private readonly INetworkConnection _networkConnection;
		private readonly IMessageBox _messageBox;
		private MvxSubscriptionToken _authenticationMessageToken;
		private MvxSubscriptionToken _favoritesUpdatedMessageToken;

		public ConferencesListViewModel(IRemoteDataService remoteDataService,
																		ILocalConferencesRepository localConferencesRepository,
																		IAnalytics analytics,
																		IAuthentication authentication,
																		IMvxFileStore fileStore,
																		IMvxMessenger messenger,
																		INetworkConnection networkConnection,
																		IMessageBox messageBox)
		{
			_remoteDataService = remoteDataService;
			_localConferencesRepository = localConferencesRepository;
			_analytics = analytics;
			_authentication = authentication;
			_fileStore = fileStore;
			_messenger = messenger;
			_networkConnection = networkConnection;
			_messageBox = messageBox;
			_authenticationMessageToken = _messenger.Subscribe<AuthenticationMessage>(OnAuthenticateMessage);
			_favoritesUpdatedMessageToken = _messenger.Subscribe<FavoriteConferencesUpdatedMessage>(OnFavoritesUpdatedMessage);
		}

		public async void Init(Parameters parameters)
		{
			var allConferences = await StartGetAll(isRefreshing: parameters.IsRefreshing);
			var favorites = await StartGetFavorites(isRefreshing: parameters.IsRefreshing);

			InvokeOnMainThread(() =>
				{
					DisplayAllConferences(allConferences);
					DisplayFavoritesConferences(favorites);
				}
			);

		}

		public async void Refresh()
		{

			var allConferences = await StartGetAll(isRefreshing: true);
			var favorites = await StartGetFavorites(isRefreshing: true);

			InvokeOnMainThread(() =>
			{
				DisplayAllConferences(allConferences);
				DisplayFavoritesConferences(favorites);
			}
				);
		}

		private async Task<IList<ConferencesListViewDto>> StartGetAll(string searchTerm = "", bool isRefreshing = false)
		{
			IEnumerable<ConferencesListViewDto> conferences = new List<ConferencesListViewDto>();
			if (IsLoadingConferences)
				return new List<ConferencesListViewDto>();

			IsLoadingConferences = true;
			_analytics.SendView("ConferencesList");

			if (!isRefreshing)
			{
				var localConferences = await _localConferencesRepository.ListAsync();
				if (localConferences.Any())
				{
					conferences = localConferences.Select(conference => new ConferencesListViewDto(conference, this._fileStore)).ToList();
				}
				else
				{
					if (!_networkConnection.IsNetworkConnected())
					{
						InvokeOnMainThread(() => _messageBox.Show(_networkConnection.NetworkDownMessage));
					}
					else
					{
						var remoteConferences = await _remoteDataService.GetConferencesAsync();
						if (remoteConferences != null)
						{
							conferences = remoteConferences;
						}
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
					var remoteConferences = await _remoteDataService.GetConferencesAsync();
					if (remoteConferences != null)
					{
						conferences = remoteConferences;
					}
				}
			}

			return conferences.ToList();
		}

		private async Task<IList<ConferencesListViewDto>> StartGetFavorites(bool isRefreshing = false)
		{
			IEnumerable<ConferencesListViewDto> conferences = new List<ConferencesListViewDto>();

			if (IsLoadingFavorites)
				return new List<ConferencesListViewDto>();

			if (_authentication.IsAuthenticated)
			{
				IsLoadingFavorites = true;
				var userName = _authentication.UserName;
				_analytics.SendView("ConferencesListSchedule-" + userName);

				if (!isRefreshing)
				{
					var localConferences = await _localConferencesRepository.ListFavoritesAsync();

					if (localConferences != null && localConferences.Any())
					{
						conferences = localConferences.Select(conference => new ConferencesListViewDto(conference, this._fileStore)).ToList();
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
						var remoteConferences = await _remoteDataService.GetFavoritesAsync(userName, isRefreshing: true);
						if (remoteConferences != null)
						{
							conferences = remoteConferences;
						}
					}
				}
			}

			return conferences.ToList();
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

		public bool IsLoadingFavorites { get; set; }
		public bool IsAuthenticated { get; set; }
		public List<ConferencesListViewDto> Conferences { get; set; }
		public FullConferenceDto SelectedFavorite { get; set; }
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

		public ICommand ShowSettingsCommand { get { return new MvxCommand(() => ShowViewModel<SettingsViewModel>()); } }
		public ICommand ShowSearchCommand { get { return new MvxCommand(() => ShowViewModel<ConferencesSearchViewModel>()); } }

		public ICommand ShowDetailCommand
		{
			get
			{
				//return new MvxCommand(() => ShowViewModel<ConferenceDetailViewModel>());
				
				return new MvxCommand<string>(slug => ShowViewModel<ConferenceDetailViewModel>(new {slug}));
			}
		}

		private void OnFavoritesUpdatedMessage(FavoriteConferencesUpdatedMessage message)
		{
			DisplayFavoritesConferences(message.Conferences);
		}

		private async void OnAuthenticateMessage(AuthenticationMessage message)
		{
			if (message != null && !string.IsNullOrWhiteSpace(message.UserName))
			{
				_authentication.UserName = message.UserName;
				IsAuthenticated = true;
				ShowViewModel<ConferencesListViewModel>(new ConferencesListViewModel.Parameters() { IsRefreshing = true } );
				//var favorites = await StartGetFavorites(true);
				//InvokeOnMainThread(
				//	() => 
				//		DisplayFavoritesConferences(favorites)
				//);
			}
		}

		public class Parameters
		{
			public bool IsRefreshing { get; set; }
		}
	}

}
