using ServiceStack.ServiceHost;
using TekConf.RemoteData.Dtos.v1;

namespace TekConf.UI.Api.Services.Requests.v1
{
    [Route("/v1/conferences/{conferenceSlug}/schedule", "POST")]
    public class AddSessionToSchedule : IReturn<ScheduleDto>
    {
        public string authenticationMethod { get; set; }
        public string authenticationToken { get; set; }
        public string conferenceSlug { get; set; }
        public string sessionSlug { get; set; }
    }
}