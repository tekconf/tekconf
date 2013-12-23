using System.Collections.Generic;
using ServiceStack;
using ServiceStack.ServiceHost;

namespace TekConf.UI.Api.Services.Requests.v1
{

	[Route("/v1/conferences/{conferenceSlug}/sessions/{sessionSlug}/links", "GET")]
	public class SessionLinks : IReturn<List<string>>
	{
		[ApiMember(Name = "conferenceSlug", Description = "XXXX", ParameterType = "query", DataType = "string", IsRequired = false)]
		public string conferenceSlug { get; set; }
		[ApiMember(Name = "sessionSlug", Description = "XXXX", ParameterType = "query", DataType = "string", IsRequired = false)]
		public string sessionSlug { get; set; }
	}
}
