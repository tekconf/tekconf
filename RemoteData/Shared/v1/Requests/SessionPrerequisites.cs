using System.Collections.Generic;
using ServiceStack;
using ServiceStack.ServiceHost;
using TekConf.RemoteData.Dtos.v1;

namespace TekConf.UI.Api.Services.Requests.v1
{
	[Route("/v1/conferences/{conferenceSlug}/sessions/{sessionSlug}/prerequisites", "GET")]
	public class SessionPrerequisites : IReturn<List<string>>
	{
		[ApiMember(Name = "conferenceSlug", Description = "XXXX", ParameterType = "query", DataType = "string", IsRequired = false)]
		public string conferenceSlug { get; set; }
		[ApiMember(Name = "sessionSlug", Description = "XXXX", ParameterType = "query", DataType = "string", IsRequired = false)]
		public string sessionSlug { get; set; }
	}
}
