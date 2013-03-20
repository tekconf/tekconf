using System.Collections.Generic;
using System.Threading.Tasks;
using TekConf.RemoteData.Dtos.v1;
using TekConf.UI.Api.Services.Requests.v1;

namespace TekConf.UI.Web.Controllers
{
	public interface IRemoteDataRepositoryAsync
	{
		Task<IList<ConferencesDto>> GetConferences(
			string sortBy = null,
			bool? showPastConferences = false,
			bool? showOnlyOpenCalls = false,
			bool? showOnlyOnSale = false,
			string search = null,
			string city = null, string state = null, string country = null,
			double? latitude = null, double? longitude = null, double? distance = null);

		Task<int> GetConferencesCount(bool? showPastConferences, string search);
		Task<ScheduleDto> GetSchedule(string conferenceSlug, string userName);
		Task<List<FullConferenceDto>> GetSchedules(string userName);
		Task<FullConferenceDto> GetFullConference(string conferenceSlug, string userName);
		Task<IList<ConferencesDto>> GetFeaturedConferences();
		Task<IList<ConferencesDto>> GetConferencesWithOpenCalls();
		Task<IList<FullSpeakerDto>> GetFeaturedSpeakers();
		Task<IList<SessionsDto>> GetSessions(string conferenceSlug);
		Task<SessionDto> GetSessionDetail(string conferenceSlug, string sessionSlug);
		Task<FullSpeakerDto> GetSpeaker(string conferenceSlug, string speakerSlug);
		Task<IList<SpeakersDto>> GetSessionSpeakers(string conferenceSlug, string sessionSlug);
		Task<PresentationDto> CreatePresentation(CreatePresentation presentation);
		Task<PresentationDto> CreatePresentationHistory(CreatePresentationHistory history);
		Task<FullConferenceDto> CreateConference(CreateConference conference);
		Task<ScheduleDto> AddSessionToSchedule(string conferenceSlug, string sessionSlug, string userName, string password);
		Task<ScheduleDto> RemoveSessionFromSchedule(string conferenceSlug, string sessionSlug, string userName, string password);
		Task<FullSpeakerDto> AddSpeakerToSession(CreateSpeaker speaker);
		Task<FullSpeakerDto> EditSpeaker(CreateSpeaker speaker);
		Task<PresentationDto> GetPresentation(string slug, string userName);
	}
}