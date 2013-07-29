using System;
using System.Collections.Generic;
using TekConf.Core.Repositories;
using TekConf.RemoteData.Dtos.v1;
using System.Threading.Tasks;
using Cirrious.MvvmCross.Plugins.Sqlite;

namespace TekConf.Core.Services
{
	public interface IRemoteDataService
	{
		Task<IEnumerable<ConferencesListViewDto>> GetConferencesAsync();
		Task<IEnumerable<ConferencesListViewDto>> GetFavoritesAsync(string userName, bool isRefreshing);
		Task<ConferenceDetailViewDto> GetConferenceDetailAsync(string slug, bool isRefreshing);
		Task<ScheduleDto> RemoveSessionFromScheduleAsync(string userName, string conferenceSlug, string sessionSlug);
		Task<ScheduleDto> RemoveFromScheduleAsync(string userName, string conferenceSlug);
		Task<ScheduleDto> GetScheduleAsync(string userName, string conferenceSlug, bool isRefreshing, ISQLiteConnection connection);
		Task<string> GetIsOauthUserRegistered(string providerId);
		Task<string> CreateOauthUser(string userId, string userName);
		Task<MobileLoginResultDto> LoginWithTekConf(string userName, string password);
		Task<SessionDetailDto> GetSessionAsync(string conferenceSlug, string sessionSlug);
		Task<ConferenceSessionsListViewDto> GetConferenceSessionsList(string slug);

		void AddToSchedule(string userName, string conferenceSlug, Action<ScheduleDto> success = null, Action<Exception> error = null);
		void AddSessionToSchedule(string userName, string conferenceSlug, string sessionSlug, Action<ScheduleDto> success = null, Action<Exception> error = null);

	}
}