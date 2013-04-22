using System;
using System.Collections.Generic;
using TekConf.RemoteData.Dtos.v1;
using TekConf.UI.Api.Services.Requests.v1;

namespace TekConf.RemoteData.v1
{
	using System.Runtime.CompilerServices;
	using System.Threading.Tasks;

	public interface IRemoteDataRepository
	{
		// START ASYNC AWAIT
		Task<List<SessionsDto>> GetSessionsAsync(string conferenceSlug);
		Task<IList<FullConferenceDto>> GetConferencesAsync(
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
			double? distance = null);
		Task<ScheduleDto> GetSchedule(string conferenceSlug, string userName);

		Task<List<FullConferenceDto>> GetSchedules(string userName);

		Task<List<PresentationDto>> GetPresentations(string userName);

		Task<PresentationDto> GetPresentation(string slug, string userName);

		Task<SessionDto> GetSession(string conferenceSlug, string slug);

		Task<SubscriptionDto> GetSubscription(string emailAddress);

		Task<UserDto> GetUser(string userName);

		Task<int> GetConferencesCount(bool? showPastConferences = false, string search = null);

		Task<IList<FullConferenceDto>> GetFeaturedConferences();

		Task<IList<FullConferenceDto>> GetConferencesWithOpenCalls();

		Task<FullConferenceDto> GetConference(string slug);

		Task<FullConferenceDto> GetFullConference(string slug, string userName);

		Task<List<FullSpeakerDto>> GetFeaturedSpeakers();

		Task<IList<SpeakersDto>> GetSessionSpeakers(string conferenceSlug, string sessionSlug);

		Task<IList<FullSpeakerDto>> GetSpeakers(string conferenceSlug);

		Task<FullSpeakerDto> GetSpeaker(string conferenceSlug, string speakerSlug);

		Task<SubscriptionDto> AddSubscription(string emailAddress);

		Task<FullConferenceDto> EditConference(CreateConference conference, string userName, string password);

		Task<ScheduleDto> AddSessionToSchedule(string conferenceSlug, string sessionSlug, string userName);

		Task<ScheduleDto> RemoveSessionFromSchedule(string conferenceSlug, string sessionSlug, string userName);

		Task<PresentationDto> CreatePresentation(CreatePresentation presentation, string userName, string password);

		Task<PresentationDto> CreatePresentationHistory(CreatePresentationHistory history, string userName, string password);

		Task<FullConferenceDto> CreateConference(CreateConference conference, string userName, string password);

		Task<FullSpeakerDto> AddSpeakerToSession(CreateSpeaker speaker, string userName, string password);

		Task<SessionDto> AddSessionToConference(AddSession session, string userName, string password);

		Task<SessionDto> EditSessionInConference(AddSession session, string userName, string password);

		Task<FullSpeakerDto> EditSpeaker(CreateSpeaker speaker, string userName, string password);

		//START OLD STYLE



	}
}