using System.Collections.Generic;
using ServiceStack.ServiceHost;
using TekConf.RemoteData.Dtos.v1;

namespace TekConf.UI.Api.Services.Requests.v1
{

  [Route("/v1/conferences/{conferenceSlug}/sessions/{sessionSlug}/prerequisites", "GET")]
  public class SessionPrerequisites : IReturn<List<string>>
  {
      public string conferenceSlug { get; set; }
      public string sessionSlug { get; set; }
  }

}