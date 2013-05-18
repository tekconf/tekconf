using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Cirrious.MvvmCross.ViewModels;
using TekConf.Core.Interfaces;
using TekConf.Core.Services;
using TekConf.RemoteData.Dtos.v1;

namespace TekConf.Core.ViewModels
{
	public class ConferenceDetailViewModel : MvxViewModel
	{
		private readonly IRemoteDataService _remoteDataService;
		private readonly IAnalytics _analytics;
		private readonly IAuthentication _authentication;

		public ConferenceDetailViewModel(IRemoteDataService remoteDataService, IAnalytics analytics, IAuthentication authentication)
		{
			_remoteDataService = remoteDataService;
			_analytics = analytics;
			_authentication = authentication;
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
			_remoteDataService.GetConference(slug: slug, isRefreshing:isRefreshing,success: Success, error: Error);
		}

		private void Error(Exception exception)
		{
			// for now we just hide the error...
			IsLoading = false;
		}

		private void Success(FullConferenceDto conference)
		{
			InvokeOnMainThread(() => DisplayConference(conference));
		}

		private void DisplayConference(FullConferenceDto conference)
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
				if (Conference == null || Conference.sessions == null || !Conference.sessions.Any())
					return false;
				else
					return true;
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
				PageTitle = value.name;
				RaisePropertyChanged(() => Conference);
			}
		}

		public ICommand ShowSessionsCommand
		{
			get
			{
				return new MvxCommand<string>((slug) => ShowViewModel<ConferenceSessionsViewModel>(new { slug = slug }));
			}
		}

		public ICommand ShowSettingsCommand
		{
			get
			{
				return new MvxCommand(() => ShowViewModel<SettingsViewModel>());
			}
		}


		public ICommand AddFavoriteCommand { get; private set; }


		private void AddConferenceToFavorites()
		{
			var addSuccess =  new Action<ScheduleDto>(dto => {});
			var addError = new Action<Exception>(ex => { });
			_remoteDataService.AddToSchedule("robgibbens", this.Conference.slug, success:addSuccess, error:addError);
		}

		public ICommand ShowSessionDetailCommand
		{
			get
			{
				return new MvxCommand<SessionDetailViewModel.Navigation>((navigation) =>
					ShowViewModel<SessionDetailViewModel>(navigation)
					);
			}
		}

	}
}