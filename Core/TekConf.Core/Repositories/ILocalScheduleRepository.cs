//using System.Collections.Generic;
//using TekConf.RemoteData.Dtos.v1;

//namespace TekConf.Core.Repositories
//{
//	using TekConf.Core.Entities;

//	public interface ILocalScheduleRepository
//	{
//		FullSessionDto NextScheduledSession { get; }
//		void SaveSchedules(IList<FullConferenceDto> scheduledConferences);
//		IList<ConferenceEntity> GetConferencesList();
//		void SaveSchedule(ScheduleDto schedule);
//		ScheduleDto GetSchedule(string conferenceSlug);
//		ConferencesListViewDto NextScheduledConference { get; }

//		void RemoveFromSchedule(string p);
//	}
//}