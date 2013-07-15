using System.Collections.Generic;
using TekConf.RemoteData.Dtos.v1;

namespace TekConf.Core.Repositories
{
	public interface ILocalScheduleRepository
	{
		FullSessionDto NextScheduledSession { get; }
		void SaveSchedules(IList<FullConferenceDto> scheduledConferences);
		IList<ConferencesListViewDto> GetConferencesList();
		void SaveSchedule(ScheduleDto schedule);
		ScheduleDto GetSchedule(string conferenceSlug);
		ConferencesListViewDto NextScheduledConference { get; }

		void RemoveFromSchedule(string p);
	}
}