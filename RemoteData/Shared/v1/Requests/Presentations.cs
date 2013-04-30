using System.Collections.Generic;
using ServiceStack.ServiceHost;
using TekConf.RemoteData.Dtos.v1;

namespace TekConf.UI.Api.Services.Requests.v1
{
	[Route("/v1/{speakerSlug}/presentations", "GET")]
	public class Presentations : IReturn<List<PresentationDto>>
	{
		[ApiMember(Name = "speakerSlug", Description = "XXXX", ParameterType = "query", DataType = "string", IsRequired = false)]
		public string speakerSlug { get; set; }
	}
}