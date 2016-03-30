using Refit;
using System.Threading.Tasks;
using System.Collections.Generic;
using Tekconf.DTO;

namespace TekConf.Mobile.Core.Services
{
	[Headers("Accept: application/json")]
	public interface ITekConfApi
	{
		[Get("/conferences")]
		[Headers("Authorization: Bearer")]
		Task<List<Conference>> GetConferences();

		[Get("/conferences/{slug}")]
		[Headers("Authorization: Bearer")]
		Task<Conference> GetConference(string slug);

		[Get("/schedules")]
		[Headers("Authorization: Bearer")]
		Task<List<Schedule>> GetSchedules();

		[Get("/schedules?conferenceSlug={slug}")]
		[Headers("Authorization: Bearer")]
		Task<Schedule> GetSchedule(string slug);

		[Post("/schedules?conferenceSlug={slug}")]
		[Headers("Authorization: Bearer")]
		Task<Schedule> AddToSchedule(string slug);

        [Delete("/schedules?conferenceSlug={slug}")]
        [Headers("Authorization: Bearer")]
        Task RemoveFromSchedule(string slug);
    }
}