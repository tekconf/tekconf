using ServiceStack.ServiceHost;
using TekConf.RemoteData.Dtos.v1;

namespace TekConf.UI.Api.Services.Requests.v1
{
    [Route("/v1/conferences/{conferenceSlug}/schedule/{userSlug}", "GET")]
    public class Schedule : IReturn<ScheduleDto>
    {
        public string conferenceSlug { get; set; }
        public string userSlug { get; set; }
    }
}