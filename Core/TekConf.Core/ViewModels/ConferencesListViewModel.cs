using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Input;
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

		public ConferencesListViewModel(IRemoteDataService remoteDataService, IAnalytics analytics)
		{
			_remoteDataService = remoteDataService;
			_analytics = analytics;
		}

		public void Init(string searchTerm)
		{
			StartGetAll();
			StartGetFavorites();
		}

		public void Refresh()
		{
			StartGetAll(isRefreshing:true);
			StartGetFavorites(isRefreshing: true);
		}

		private void StartGetAll(bool isRefreshing = false)
		{
			if (IsLoadingConferences)
				return;

			IsLoadingConferences = true;
			_analytics.SendView("ConferencesList");
			_remoteDataService.GetConferences(isRefreshing: isRefreshing, success: GetAllSuccess, error: GetAllError);
		}

		private void StartGetFavorites(bool isRefreshing = false)
		{
			if (IsLoadingFavorites)
				return;

			IsLoadingFavorites = true;
			const string userName = "robgibbens"; //TODO
			_analytics.SendView("ConferencesListSchedule-" + userName);
			_remoteDataService.GetSchedules(userName, isRefreshing: isRefreshing, success: GetFavoritesSuccess, error: GetFavoritesError);
		}

		private void GetAllError(Exception exception)
		{
			// for now we just hide the error...
			IsLoadingConferences = false;
		}

		private void GetFavoritesError(Exception exception)
		{
			// for now we just hide the error...
			IsLoadingFavorites = false;
		}

		private void GetAllSuccess(IEnumerable<FullConferenceDto> enumerable)
		{
			InvokeOnMainThread(() => DisplayAllConferences(enumerable));
		}

		private void GetFavoritesSuccess(IEnumerable<FullConferenceDto> enumerable)
		{
			InvokeOnMainThread(() => DisplayFavoritesConferences(enumerable));
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
				RaisePropertyChanged(() => IsLoadingConferences);
			}
		}

		private bool _isLoadingFavorites;
		public bool IsLoadingFavorites
		{
			get { return _isLoadingFavorites; }
			set { _isLoadingFavorites = value; RaisePropertyChanged(() => IsLoadingFavorites); }
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

		public ICommand ShowDetailCommand
		{
			get { return new MvxCommand<string>((slug) => ShowViewModel<ConferenceDetailViewModel>(new { slug = slug })); 	}
		}
	}
}
