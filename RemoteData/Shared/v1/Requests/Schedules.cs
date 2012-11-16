using System.Collections.Generic;
using ServiceStack.ServiceHost;
using TekConf.RemoteData.Dtos.v1;

namespace TekConf.UI.Api.Services.Requests.v1
{
    [Route("/v1/conferences/schedules", "GET")]
    public class Schedules : IReturn<List<SchedulesDto>>
    {
        public string authenticationMethod { get; set; }
        public string authenticationToken { get; set; }
    }
}