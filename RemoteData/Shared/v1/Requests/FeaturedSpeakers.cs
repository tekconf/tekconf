using System.Collections.Generic;
using ServiceStack.ServiceHost;
using TekConf.RemoteData.Dtos.v1;

namespace TekConf.UI.Api.Services.Requests.v1
{
    [Route("/v1/speakers/featured", "GET")]
    public class FeaturedSpeakers : IReturn<List<FullSpeakerDto>>
    {
        
    }
}