using System.Collections.Generic;
using ServiceStack.ServiceHost;
using TekConf.RemoteData.Dtos.v1;

namespace TekConf.UI.Api.Services.Requests.v1
{
	[Route("/v1/{speakerSlug}/presentations/{presentationSlug}/history", "POST")]
	[Route("/v1/{speakerSlug}/presentations/{presentationSlug}/history", "PUT")]
	public class CreatePresentationHistory : IReturn<PresentationDto>
	{
		[ApiMember(Name = "UserName", Description = "XXXX", ParameterType = "query", DataType = "string", IsRequired = false)]
		public virtual string UserName { get; set; }

		[ApiMember(Name = "SpeakerSlug", Description = "XXXX", ParameterType = "query", DataType = "string", IsRequired = false)]
		public virtual string SpeakerSlug { get; set; }

		[ApiMember(Name = "PresentationSlug", Description = "XXXX", ParameterType = "query", DataType = "string", IsRequired = false)]
		public virtual string PresentationSlug { get; set; }
		
		[ApiMember(Name = "ConferenceName", Description = "XXXX", ParameterType = "query", DataType = "string", IsRequired = false)]
		public virtual string ConferenceName { get; set; }
		
		[ApiMember(Name = "Notes", Description = "XXXX", ParameterType = "query", DataType = "string", IsRequired = false)]
		public virtual string Notes { get; set; }
		
		[ApiMember(Name = "Links", Description = "XXXX", ParameterType = "query", DataType = "string", IsRequired = false)]
		public virtual List<string> Links { get; set; }
	}
}