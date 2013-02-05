using ServiceStack.ServiceHost;
using TekConf.RemoteData.Dtos.v1;

namespace TekConf.UI.Api.Services.Requests.v1
{
	[Route("/v1/conferences/{conferenceSlug}/schedule", "POST")]
	public class AddSessionToSchedule : IReturn<ScheduleDto>
	{
		public string conferenceSlug { get; set; }
		public string userName { get; set; }
		public string sessionSlug { get; set; }
	}
}