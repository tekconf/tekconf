using System;
using System.Collections.Generic;
using TekConf.Core.Repositories;
using TekConf.RemoteData.Dtos.v1;

namespace TekConf.Core.Services
{
	public interface IRemoteDataService
	{
		void GetConferences(
					bool isRefreshing = false,
					string userName = null,
					string sortBy = "end",
					bool? showPastConferences = false,
					bool? showOnlyOpenCalls = false,
					bool? showOnlyOnSale = false,
					string search = null,
					string city = null,
					string state = null,
					string country = null,
					double? latitude = null,
					double? longitude = null,
					double? distance = null,
					Action<IEnumerable<ConferencesListViewDto>> success = null,
					Action<Exception> error = null);

		void GetConferenceSessionsList(string slug, bool isRefreshing,
			Action<ConferenceSessionsListViewDto> success = null, Action<Exception> error = null);

		void GetConferenceDetail(string slug, bool isRefreshing,
			Action<ConferenceDetailViewDto> success = null,
			Action<Exception> error = null);

		void GetSchedule(string userName, string conferenceSlug, bool isRefreshing, Action<ScheduleDto> success = null, Action<Exception> error = null);
		void GetSchedules(string userName, bool isRefreshing, Action<IEnumerable<ConferencesListViewDto>> success = null, Action<Exception> error = null);

		void LoginWithTekConf(string userName, string password, Action<bool, string> success = null, Action<Exception> error = null);

		void AddToSchedule(string userName, string conferenceSlug, Action<ScheduleDto> success = null, Action<Exception> error = null);
		void RemoveFromSchedule(string userName, string conferenceSlug, Action<ScheduleDto> success = null, Action<Exception> error = null);
		void GetSession(string conferenceSlug, string sessionSlug, bool isRefreshing, Action<SessionDetailDto> success, Action<Exception> error);
		void GetIsOauthUserRegistered(string userId, Action<string> success, Action<Exception> error);
		void CreateOauthUser(string userId, string userName, Action<string> success, Action<Exception> error);

	}
}
