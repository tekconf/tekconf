using System.Collections.Generic;
using ServiceStack.ServiceHost;
using TekConf.RemoteData.Dtos.v1;

namespace TekConf.UI.Api.Services.Requests.v1
{

    [Route("/v1/conferences/{conferenceSlug}/sessions", "GET")]
    public class Sessions : IReturn<List<SessionsDto>>
    {
        public string conferenceSlug { get; set; }
    }

    [Route("/v1/conferences/{conferenceSlug}/sessions/{sessionSlug}", "GET")]
    [Route("/v1/conferences/{conferenceSlug}/sessions/{sessionSlug}", "POST")]
    [Route("/v1/conferences/{conferenceSlug}/sessions/{sessionSlug}", "PUT")]
    public class Session : IReturn<SessionDto>
    {
        public string conferenceSlug { get; set; }
        public string sessionSlug { get; set; }
    }
}