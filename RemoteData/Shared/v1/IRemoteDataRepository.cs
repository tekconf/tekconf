using System;
using System.Collections.Generic;
using TekConf.RemoteData.Dtos.v1;
using TekConf.UI.Api.Services.Requests.v1;

namespace TekConf.RemoteData.v1
{
	public interface IRemoteDataRepository
	{
		void GetSchedule(string conferenceSlug, string userName, Action<ScheduleDto> callback);
		void GetSchedules(string userName, Action<List<FullConferenceDto>> callback);
		void GetPresentations(string userName, Action<List<PresentationDto>> callback);
		void GetPresentation(string slug, string userName, Action<PresentationDto> callback);
		void AddSessionToSchedule(string conferenceSlug, string sessionSlug, string userName, Action<ScheduleDto> callback);
		void RemoveSessionFromSchedule(string conferenceSlug, string sessionSlug, string userName, Action<ScheduleDto> callback);

		void GetConferences(Action<IList<FullConferenceDto>> callback, string userName = null, string sortBy = "end", 
			bool? showPastConferences = false, bool? showOnlyOpenCalls = false, bool? showOnlyOnSale = false, 
			string search = null, string city = null, string state = null, string country = null, double? latitude = null, 
			double? longitude = null, double? distance = null);

		void GetConferencesCount(Action<int> callback, bool? showPastConferences = false, string search = null);
		void GetFeaturedConferences(Action<IList<FullConferenceDto>> callback);
		void GetConferencesWithOpenCalls(Action<IList<FullConferenceDto>> callback);
		void GetConference(string slug, Action<FullConferenceDto> callback);
		void GetFullConference(string slug, string userName, Action<FullConferenceDto> callback);
		void GetFeaturedSpeakers(Action<List<FullSpeakerDto>> callback);
		void GetSessionSpeakers(string conferenceSlug, string sessionSlug, Action<IList<SpeakersDto>> callback);
		void GetSpeakers(string conferenceSlug, Action<IList<FullSpeakerDto>> callback);
		void GetSpeaker(string conferenceSlug, string speakerSlug, Action<FullSpeakerDto> callback);
		void GetSessions(string conferenceSlug, Action<IList<SessionsDto>> callback);
		void GetSession(string conferenceSlug, string slug, Action<SessionDto> callback);
		void CreatePresentation(CreatePresentation presentation, string userName, string password, Action<PresentationDto> callback);
		void CreatePresentationHistory(CreatePresentationHistory history, string userName, string password, Action<PresentationDto> callback);
		void CreateConference(CreateConference conference, string userName, string password, Action<FullConferenceDto> callback);
		void EditConference(CreateConference conference, string userName, string password, Action<FullConferenceDto> callback);
		void GetUser(string userName, Action<UserDto> callback);
		void AddSessionToConference(AddSession session, string userName, string password, Action<SessionDto> callback);
		void EditSessionInConference(AddSession session, string userName, string password, Action<SessionDto> callback);
		void AddSpeakerToSession(CreateSpeaker speaker, string userName, string password, Action<FullSpeakerDto> callback);
		void EditSpeaker(CreateSpeaker speaker, string userName, string password, Action<FullSpeakerDto> callback);

		void GetSubscription(string emailAddress, Action<SubscriptionDto> callback);
		void AddSubscription(string emailAddress, Action<SubscriptionDto> callback);
	}
}