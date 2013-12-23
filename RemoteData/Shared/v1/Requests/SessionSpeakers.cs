using System.Collections.Generic;
using ServiceStack;
using ServiceStack.ServiceHost;
using TekConf.RemoteData.Dtos.v1;

namespace TekConf.UI.Api.Services.Requests.v1
{
	[Route("/v1/conferences/{conferenceSlug}/sessions/{sessionSlug}/speakers", "GET")]
	public class SessionSpeakers : IReturn<List<SpeakersDto>>
	{
		[ApiMember(Name = "conferenceSlug", Description = "XXXX", ParameterType = "query", DataType = "string", IsRequired = false)]
		public string conferenceSlug { get; set; }
		[ApiMember(Name = "sessionSlug", Description = "XXXX", ParameterType = "query", DataType = "string", IsRequired = false)]
		public string sessionSlug { get; set; }
	}

	[Route("/v1/conferences/{conferenceSlug}/sessions/{sessionSlug}/speakers/{speakerSlug}", "GET")]
	[Route("/v1/conferences/{conferenceSlug}/sessions/{sessionSlug}/speakers/{speakerSlug}", "POST")]
	[Route("/v1/conferences/{conferenceSlug}/sessions/{sessionSlug}/speakers/{speakerSlug}", "PUT")]
	public class SessionSpeaker : IReturn<SpeakerDto>
	{
		[ApiMember(Name = "conferenceSlug", Description = "XXXX", ParameterType = "query", DataType = "string", IsRequired = false)]
		public string conferenceSlug { get; set; }
		[ApiMember(Name = "sessionSlug", Description = "XXXX", ParameterType = "query", DataType = "string", IsRequired = false)]
		public string sessionSlug { get; set; }
		[ApiMember(Name = "speakerSlug", Description = "XXXX", ParameterType = "query", DataType = "string", IsRequired = false)]
		public string speakerSlug { get; set; }
	}

}
