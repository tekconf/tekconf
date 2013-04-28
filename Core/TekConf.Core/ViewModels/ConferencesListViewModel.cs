using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Cirrious.MvvmCross.ViewModels;
using TekConf.Core.Services;
using TekConf.RemoteData.Dtos.v1;

namespace TekConf.Core.ViewModels
{
	public class ConferencesListViewModel : MvxViewModel
	{
		private readonly IRemoteDataService _remoteDataService;

		public ConferencesListViewModel(IRemoteDataService remoteDataService)
		{
			_remoteDataService = remoteDataService;
		}

		public void Init(string searchTerm)
		{
			StartGetAll(searchTerm);
			StartGetFavorites();
		}

		private void StartGetAll(string searchTerm)
		{
			if (IsSearchingForAll)
				return;

			IsSearchingForAll = true;
			_remoteDataService.GetConferences(success: GetAllSuccess, error: GetAllError);
		}

		private void StartGetFavorites()
		{
			if (IsSearchingForFavorites)
				return;

			IsSearchingForFavorites = true;
			string userName = "robgibbens"; //TODO
			_remoteDataService.GetSchedule(userName, success: GetFavoritesSuccess, error: GetFavoritesError);
		}

		private void GetAllError(Exception exception)
		{
			// for now we just hide the error...
			IsSearchingForAll = false;
		}

		private void GetFavoritesError(Exception exception)
		{
			// for now we just hide the error...
			IsSearchingForFavorites = false;
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
			IsSearchingForAll = false;
			Conferences = enumerable.ToList();
		}

		private void DisplayFavoritesConferences(IEnumerable<FullConferenceDto> enumerable)
		{
			IsSearchingForFavorites = false;
			Favorites = enumerable.ToList();
		}

		private bool _isSearchingForAll;
		public bool IsSearchingForAll
		{
			get { return _isSearchingForAll; }
			set { _isSearchingForAll = value; RaisePropertyChanged("IsSearchingForAll"); }
		}

		private bool _isSearchingForFavorites;
		public bool IsSearchingForFavorites
		{
			get { return _isSearchingForFavorites; }
			set { _isSearchingForFavorites = value; RaisePropertyChanged("IsSearchingForFavorites"); }
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
				RaisePropertyChanged(() => Favorites);
			}
		}

		public ICommand ShowDetailCommand
		{
			get { return new MvxCommand<string>((slug) => ShowViewModel<ConferenceDetailViewModel>(new { slug = slug })); 	}
		}
	}
}
