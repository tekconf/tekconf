using System.Collections.Generic;
using ServiceStack.ServiceHost;
using TekConf.RemoteData.Dtos.v1;

namespace TekConf.UI.Api.Services.Requests.v1
{
    [Route("/v1/conferences", "GET")]
    public class Conferences : IReturn<List<FullConferenceDto>>
    {
        
    }

    [Route("/v1/conferences/{conferenceSlug}", "POST")]
    [Route("/v1/conferences/{conferenceSlug}", "PUT")]
    [Route("/v1/conferences/{conferenceSlug}", "GET")]
    public class Conference : IReturn<FullConferenceDto>
    {
        public string conferenceSlug { get; set; }        
    }

}