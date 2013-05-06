using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Cirrious.CrossCore.Core;
using Cirrious.MvvmCross.Plugins.File;
using TekConf.Core.Models;
using TekConf.RemoteData.Dtos.v1;

namespace TekConf.Core.Services
{

	public class RemoteDataService : IRemoteDataService
	{
		private readonly IMvxFileStore _fileStore;

		public RemoteDataService(IMvxFileStore fileStore)
		{
			_fileStore = fileStore;
		}

		public void GetConferences(
			bool isRefreshing = false,
			string userName = null,
			string sortBy = "end",
			bool? showPastConferences = null,
			bool? showOnlyOpenCalls = null,
			bool? showOnlyOnSale = null,
			string search = null,
			string city = null,
			string state = null,
			string country = null,
			double? latitude = null,
			double? longitude = null,
			double? distance = null,
			Action<IEnumerable<FullConferenceDto>> success = null,
			Action<Exception> error = null)
		{
			ConferencesService.GetConferencesAsync(_fileStore, isRefreshing, success, error);
		}

		public void GetConference(string slug, bool isRefreshing, Action<FullConferenceDto> success = null, Action<Exception> error = null)
		{
			ConferenceService.GetConferenceAsync(_fileStore, slug, isRefreshing, success, error);
		}

		public void GetSchedule(string userName, string conferenceSlug, bool isRefreshing, Action<ScheduleDto> success = null, Action<Exception> error = null)
		{
			ScheduleService.GetScheduleAsync(_fileStore, userName, conferenceSlug, isRefreshing, success, error);
		}

		public void GetSchedules(string userName, bool isRefreshing, Action<IEnumerable<FullConferenceDto>> success = null, Action<Exception> error = null)
		{
			ScheduleService.GetSchedulesAsync(_fileStore, userName, isRefreshing, success, error);
		}

		public void AddToSchedule(string userName, string conferenceSlug, Action<ScheduleDto> success = null, Action<Exception> error = null)
		{
			ScheduleService.AddToScheduleAsync(_fileStore, userName, conferenceSlug, false, success, error);
		}

		public void GetSession(string conferenceSlug, string sessionSlug, bool isRefreshing, Action<FullSessionDto> success, Action<Exception> error)
		{
			SessionService.GetSessionAsync(_fileStore, conferenceSlug, sessionSlug, isRefreshing, success, error);
		}
	}
}