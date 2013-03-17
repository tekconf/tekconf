using System.Collections.Generic;
using ServiceStack.ServiceHost;

namespace TekConf.UI.Api.Services.Requests.v1
{

    [Route("/v1/conferences/{conferenceSlug}/sessions/{sessionSlug}/links", "GET")]
    public class SessionLinks : IReturn<List<string>>
  {
    public string conferenceSlug { get; set; }
    public string sessionSlug { get; set; }
  }
}