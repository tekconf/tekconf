using System.Collections.Generic;
using ServiceStack.ServiceHost;
using TekConf.RemoteData.Dtos.v1;

namespace TekConf.UI.Api.Services.Requests.v1
{
    [Route("/v1/conferences/{conferenceSlug}/sessions/{sessionSlug}/speakers", "GET")]
    public class SessionSpeakers : IReturn<List<SpeakersDto>>
    {
        public string conferenceSlug { get; set; }
        public string sessionSlug { get; set; }
    }

    [Route("/v1/conferences/{conferenceSlug}/sessions/{sessionSlug}/speakers/{speakerSlug}", "GET")]
    [Route("/v1/conferences/{conferenceSlug}/sessions/{sessionSlug}/speakers/{speakerSlug}", "POST")]
    [Route("/v1/conferences/{conferenceSlug}/sessions/{sessionSlug}/speakers/{speakerSlug}", "PUT")]
    public class SessionSpeaker : IReturn<SpeakerDto>
    {
        public string conferenceSlug { get; set; }
        public string sessionSlug { get; set; }
        public string speakerSlug { get; set; }
    }

}