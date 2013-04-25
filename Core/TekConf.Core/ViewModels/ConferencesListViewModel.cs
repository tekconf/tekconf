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
			StartSearch(searchTerm);
		}

		private void StartSearch(string searchTerm)
		{
			if (IsSearching)
				return;

			IsSearching = true;
			_remoteDataService.GetConferences(success: Success, error: Error);
		}

		private void Error(Exception exception)
		{
			// for now we just hide the error...
			IsSearching = false;
		}

		private void Success(IEnumerable<FullConferenceDto> enumerable)
		{
			InvokeOnMainThread(() => DisplayConferences(enumerable));
		}

		private void DisplayConferences(IEnumerable<FullConferenceDto> enumerable)
		{
			IsSearching = false;
			Conferences = enumerable.ToList();
		}

		private bool _isSearching;
		public bool IsSearching
		{
			get { return _isSearching; }
			set { _isSearching = value; RaisePropertyChanged("IsSearching"); }
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

		public ICommand ShowDetailCommand
		{
			get { return new MvxCommand<string>((slug) => ShowViewModel<ConferenceDetailViewModel>(new { slug = slug })); 	}
		}
	}
}
