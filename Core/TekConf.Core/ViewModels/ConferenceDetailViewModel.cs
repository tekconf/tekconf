using System;
using System.Collections.Generic;
using System.Linq;
using Cirrious.MvvmCross.ViewModels;
using TekConf.Core.Services;
using TekConf.RemoteData.Dtos.v1;

namespace TekConf.Core.ViewModels
{
	public class ConferenceDetailViewModel : MvxViewModel
	{
		private readonly IRemoteDataService _remoteDataService;

		public ConferenceDetailViewModel(IRemoteDataService remoteDataService)
		{
			_remoteDataService = remoteDataService;
		}

		public void Init(string slug)
		{
			StartSearch(slug);
		}

		private void StartSearch(string slug)
		{
			if (IsSearching)
				return;

			IsSearching = true;
			_remoteDataService.GetConference(slug: slug, success: Success, error: Error);
		}

		private void Error(Exception exception)
		{
			// for now we just hide the error...
			IsSearching = false;
		}

		private void Success(FullConferenceDto conference)
		{
			InvokeOnMainThread(() => DisplayConference(conference));
		}

		private void DisplayConference(FullConferenceDto conference)
		{
			IsSearching = false;
			Conference = conference;
		}

		private bool _isSearching;
		public bool IsSearching
		{
			get { return _isSearching; }
			set { _isSearching = value; RaisePropertyChanged("IsSearching"); }
		}

		private FullConferenceDto _conference;
		public FullConferenceDto Conference
		{
			get
			{
				return _conference;
			}
			set
			{
				_conference = value;
				RaisePropertyChanged(() => Conference);
			}
		}
	}
}