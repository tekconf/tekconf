using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Cirrious.MvvmCross.ViewModels;
using TekConf.Core.Services;
using TekConf.RemoteData.Dtos.v1;

namespace TekConf.Core.ViewModels
{
	public class ConferenceSessionsViewModel : MvxViewModel
	{
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
				RaisePropertyChanged(() => PageTitle);
			}
		}

		private readonly IRemoteDataService _remoteDataService;

		public ConferenceSessionsViewModel(IRemoteDataService remoteDataService)
		{
			_remoteDataService = remoteDataService;
		}

		public void Init(string slug)
		{
			StartGetConference(slug);
			string userName = "robgibbens"; //TODO
			StartGetSchedule(userName, slug);
		}

		private void StartGetConference(string slug)
		{
			if (IsGettingConferences)
				return;

			IsGettingConferences = true;
			_remoteDataService.GetConference(slug: slug, success: GetConferenceSuccess, error: GetConferenceError);
		}

		private void GetConferenceError(Exception exception)
		{
			// for now we just hide the error...
			IsGettingConferences = false;
		}

		private void GetConferenceSuccess(FullConferenceDto conference)
		{
			InvokeOnMainThread(() => DisplayConference(conference));
		}

		private void DisplayConference(FullConferenceDto conference)
		{
			IsGettingConferences = false;
			Conference = conference;
		}

		private bool _isGettingConferences;
		public bool IsGettingConferences
		{
			get { return _isGettingConferences; }
			set { _isGettingConferences = value; RaisePropertyChanged("IsGettingConferences"); }
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

			}
		}



		private void StartGetSchedule(string userName, string slug)
		{
			if (IsGettingSchedule)
				return;

			IsGettingSchedule = true;
			_remoteDataService.GetSchedule(userName: userName, conferenceSlug: slug, success: GetScheduleSuccess, error: GetScheduleError);
		}
		private void GetScheduleError(Exception exception)
		{
			// for now we just hide the error...
			IsGettingSchedule = false;
		}
		private void GetScheduleSuccess(ScheduleDto conference)
		{
			InvokeOnMainThread(() => DisplaySchedule(conference));
		}
		private void DisplaySchedule(ScheduleDto conference)
		{
			IsGettingSchedule = false;
			Schedule = conference;
		}
		private bool _isGettingSchedule;
		public bool IsGettingSchedule
		{
			get { return _isGettingSchedule; }
			set { _isGettingSchedule = value; RaisePropertyChanged("IsGettingSchedule"); }
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