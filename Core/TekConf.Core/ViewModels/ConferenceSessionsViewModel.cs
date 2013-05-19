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
	public class ConferenceSessionsViewModel : MvxViewModel
	{

		private readonly IRemoteDataService _remoteDataService;
		private readonly IAnalytics _analytics;

		public ConferenceSessionsViewModel(IRemoteDataService remoteDataService, IAnalytics analytics)
		{
			_remoteDataService = remoteDataService;
			_analytics = analytics;
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

		public void Init(string slug)
		{
			string userName = "robgibbens"; //TODO

			StartGetConference(slug);
			StartGetSchedule(userName, slug, false);
		}

		public void Refresh(string slug)
		{
			string userName = "robgibbens"; //TODO

			StartGetConference(slug, true);
			StartGetSchedule(userName, slug, true);
		}

		private void StartGetConference(string slug, bool isRefreshing = false)
		{
			if (IsLoadingConference)
				return;

			IsLoadingConference = true;
			_analytics.SendView("ConferenceSessions-" + slug);
			_remoteDataService.GetConference(slug: slug, isRefreshing: isRefreshing, success: GetConferenceSuccess, error: GetConferenceError);
		}

		private void GetConferenceError(Exception exception)
		{
			// for now we just hide the error...
			IsLoadingConference = false;
		}

		private void GetConferenceSuccess(FullConferenceDto conference)
		{
			InvokeOnMainThread(() => DisplayConference(conference));
		}

		private void DisplayConference(FullConferenceDto conference)
		{
			IsLoadingConference = false;
			Conference = conference;
		}

		private bool _isLoadingConference;
		public bool IsLoadingConference
		{
			get { return _isLoadingConference; }
			set { _isLoadingConference = value; RaisePropertyChanged(() => IsLoadingConference); }
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
				PageTitle = _conference.name;
				RaisePropertyChanged(() => Conference);
				IsLoadingConference = false;

			}
		}

		private void StartGetSchedule(string userName, string slug, bool isRefreshing)
		{
			if (IsLoadingSchedule)
				return;

			IsLoadingSchedule = true;
			_remoteDataService.GetSchedule(userName: userName, conferenceSlug: slug, isRefreshing:isRefreshing, success: GetScheduleSuccess, error: GetScheduleError);
		}

		private void GetScheduleError(Exception exception)
		{
			// for now we just hide the error...
			IsLoadingSchedule = false;
		}

		private void GetScheduleSuccess(ScheduleDto conference)
		{
			InvokeOnMainThread(() => DisplaySchedule(conference));
		}

		private void DisplaySchedule(ScheduleDto conference)
		{
			IsLoadingSchedule = false;
			Schedule = conference;
		}

		private bool _isLoadingSchedule;
		public bool IsLoadingSchedule
		{
			get { return _isLoadingSchedule; }
			set { _isLoadingSchedule = value; RaisePropertyChanged(() => IsLoadingSchedule); }
		}

		private ScheduleDto _schedule;
		public ScheduleDto Schedule
		{
			get
			{
				return _schedule;
			}
			set
			{
				_schedule = value;
				RaisePropertyChanged(() => Schedule);
				IsLoadingSchedule = false;
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
				return new MvxCommand(() => ShowViewModel<ConferenceSearchViewModel>());
			}
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