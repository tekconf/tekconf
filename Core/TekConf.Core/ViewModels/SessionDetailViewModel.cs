using System;
using Cirrious.MvvmCross.ViewModels;
using TekConf.Core.Interfaces;
using TekConf.Core.Services;
using TekConf.RemoteData.Dtos.v1;

namespace TekConf.Core.ViewModels
{
	public class SessionDetailViewModel : MvxViewModel
	{
		private readonly IRemoteDataService _remoteDataService;
		private readonly IAnalytics _analytics;

		public SessionDetailViewModel(IRemoteDataService remoteDataService, IAnalytics analytics)
		{
			_remoteDataService = remoteDataService;
			_analytics = analytics;
		}
		public void Init(Navigation navigation)
		{
			StartGetSession(navigation);
		}

		private void StartGetSession(Navigation navigation)
		{
			if (IsGettingSession)
				return;

			IsGettingSession = true;
			this.ConferenceSlug = navigation.ConferenceSlug;
			_analytics.SendView("SessionDetail-" + navigation.ConferenceSlug + "-" + navigation.SessionSlug);
			_remoteDataService.GetSession(conferenceSlug: navigation.ConferenceSlug, sessionSlug: navigation.SessionSlug, success: GetSessionSuccess, error: GetConferenceError);
		}

		private void GetConferenceError(Exception exception)
		{
			// for now we just hide the error...
			IsGettingSession = false;
		}

		private void GetSessionSuccess(FullSessionDto session)
		{
			InvokeOnMainThread(() => DisplaySession(session));
		}

		private void DisplaySession(FullSessionDto session)
		{
			IsGettingSession = false;
			Session = session;
		}

		private bool _isGettingSession;
		public bool IsGettingSession
		{
			get { return _isGettingSession; }
			set { _isGettingSession = value; RaisePropertyChanged(() => IsGettingSession); }
		}

		private FullSessionDto _session;
		public FullSessionDto Session
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

		private string _pageTitle;
		public string PageTitle
		{
			get
			{
				return _pageTitle;
			}
			set
			{
				_pageTitle = "TEKCONF - " + value.ToUpper();
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
