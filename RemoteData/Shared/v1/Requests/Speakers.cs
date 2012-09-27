using System.Collections.Generic;
using ServiceStack.ServiceHost;
using TekConf.RemoteData.Dtos.v1;

namespace TekConf.UI.Api.Services.Requests.v1
{
    [Route("/v1/conferences/{conferenceSlug}/speakers", "GET")]
    public class Speakers : IReturn<List<FullSpeakerDto>>
    {
        public string conferenceSlug { get; set; }
    }

    [Route("/v1/conferences/{conferenceSlug}/speakers/{speakerSlug}", "POST")]
    [Route("/v1/conferences/{conferenceSlug}/speakers/{speakerSlug}", "PUT")]
    [Route("/v1/conferences/{conferenceSlug}/speakers/{speakerSlug}", "GET")]
    public class Speaker : IReturn<FullSpeakerDto>
    {
        public string conferenceSlug { get; set; }
        public string speakerSlug { get; set; }
    }
}