using System.Collections.Generic;
using ServiceStack.ServiceHost;
using TekConf.RemoteData.Dtos.v1;

namespace TekConf.UI.Api.Services.Requests.v1
{
	[Route("/v1/{speakerSlug}/presentations/{slug}", "POST")]
	[Route("/v1/{speakerSlug}/presentations/{slug}", "PUT")]
	public class CreatePresentation : IReturn<PresentationDto>
	{
		[ApiMember(Name = "Slug", Description = "XXXX", ParameterType = "query", DataType = "string", IsRequired = false)]
		public virtual string Slug { get; set; }
		[ApiMember(Name = "SpeakerSlug", Description = "XXXX", ParameterType = "query", DataType = "string", IsRequired = false)]
		public virtual string SpeakerSlug { get; set; }
		[ApiMember(Name = "UserName", Description = "XXXX", ParameterType = "query", DataType = "string", IsRequired = false)]
		public virtual string UserName { get; set; }
		[ApiMember(Name = "Title", Description = "XXXX", ParameterType = "query", DataType = "string", IsRequired = false)]
		public virtual string Title { get; set; }
		[ApiMember(Name = "Description", Description = "XXXX", ParameterType = "query", DataType = "string", IsRequired = false)]
		public virtual string Description { get; set; }
		[ApiMember(Name = "Tags", Description = "XXXX", ParameterType = "query", DataType = "string", IsRequired = false)]
		public virtual List<string> Tags { get; set; }
	}
}