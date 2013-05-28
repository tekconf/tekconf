using System;
using System.Windows.Input;
using Cirrious.MvvmCross.Plugins.Messenger;
using Cirrious.MvvmCross.ViewModels;
using TekConf.Core.Interfaces;
using TekConf.Core.Repositories;
using TekConf.Core.Services;
using TekConf.RemoteData.Dtos.v1;

namespace TekConf.Core.ViewModels
{
	public class SessionDetailViewModel : MvxViewModel
	{
		private readonly IRemoteDataService _remoteDataService;
		private readonly IAnalytics _analytics;
		private readonly IMvxMessenger _messenger;

		public SessionDetailViewModel(IRemoteDataService remoteDataService, IAnalytics analytics, IMvxMessenger messenger)
		{
			_remoteDataService = remoteDataService;
			_analytics = analytics;
			_messenger = messenger;
		}

		public void Init(Navigation navigation)
		{
			StartGetSession(navigation);
		}

		public void Refresh(Navigation navigation)
		{
			StartGetSession(navigation, true);
		}

		private void StartGetSession(Navigation navigation, bool isRefreshing = false)
		{
			if (IsLoading)
				return;

			IsLoading = true;
			ConferenceSlug = navigation.ConferenceSlug;
			_analytics.SendView("SessionDetail-" + navigation.ConferenceSlug + "-" + navigation.SessionSlug);
			_remoteDataService.GetSession(navigation.ConferenceSlug, navigation.SessionSlug, isRefreshing, GetSessionSuccess, GetConferenceError);
		}

		private void GetConferenceError(Exception exception)
		{
			// for now we just hide the error...
			_messenger.Publish(new ExceptionMessage(this, exception));

			IsLoading = false;
		}

		private void GetSessionSuccess(SessionDetailDto session)
		{
			InvokeOnMainThread(() => DisplaySession(session));
		}

		private void DisplaySession(SessionDetailDto session)
		{
			IsLoading = false;
			Session = session;
		}

		private bool _isLoading;
		public bool IsLoading
		{
			get { return _isLoading; }
			set { _isLoading = value; RaisePropertyChanged(() => IsLoading); }
		}

		private SessionDetailDto _session;
		public SessionDetailDto Session
		{
			get
			{
				return _session;
			}
			set
			{
				_session = value;
				PageTitle = _session.title;
				RaisePropertyChanged(() => Session);

			}
		}

		private string _conferenceSlug;
		public string ConferenceSlug
		{
			get
			{
				return _conferenceSlug;
			}
			set
			{
				_conferenceSlug = value;
				RaisePropertyChanged(() => ConferenceSlug);
			}
		}

		public ICommand ShowSettingsCommand
		{
			get
			{
				return new MvxCommand(() => ShowViewModel<SettingsViewModel>());
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
				//_pageTitle = "TEKCONF";
				RaisePropertyChanged(() => PageTitle);
			}
		}

		public class Navigation
		{
			public string ConferenceSlug { get; set; }
			public string SessionSlug { get; set; }

		}

	}
}
