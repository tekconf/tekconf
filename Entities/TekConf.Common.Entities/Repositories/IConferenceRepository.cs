namespace TekConf.Common.Entities
{
	using System.Collections.Generic;
	using System.Linq;

	public interface IConferenceRepository : IRepository<ConferenceEntity>
	{
		int GetConferenceCount(string searchTerm, bool? showPastConferences);
		IEnumerable<SpeakerEntity> GetFeaturedSpeakers();
		IEnumerable<ConferenceEntity> GetFeaturedConferences();
		IEnumerable<ConferenceEntity> GetNewestConferences();
		List<ConferenceEntity> GeoSearch(double latitude, double longitude, double rangeInMiles);
		IEnumerable<ConferenceEntity> GetConferences(string search, string sortBy, bool? showPastConferences, bool? showOnlyWithOpenCalls, bool? showOnlyOnSale, bool showOnlyFeatured, double? longitude, double? latitude, double? distance, string city, string state, string country);
		SessionEntity SaveSession(string conferenceSlug, string originalSessionSlug, SessionEntity session);
	}
}