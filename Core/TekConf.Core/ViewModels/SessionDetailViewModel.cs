using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cirrious.MvvmCross.ViewModels;
using TekConf.Core.Services;
using TekConf.RemoteData.Dtos.v1;

namespace TekConf.Core.ViewModels
{
	public class SessionDetailViewModel : MvxViewModel
	{		
		public class Navigation
		{
			public string ConferenceSlug { get; set; }
			public string SessionSlug { get; set; }

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

		private readonly IRemoteDataService _remoteDataService;

		public SessionDetailViewModel(IRemoteDataService remoteDataService)
		{
			_remoteDataService = remoteDataService;
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
	}
}
