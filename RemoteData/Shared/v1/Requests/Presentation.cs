using ServiceStack.ServiceHost;
using TekConf.RemoteData.Dtos.v1;

namespace TekConf.UI.Api.Services.Requests.v1
{
	[Route("/v1/{speakerSlug}/presentations/{slug}", "GET")]
	public class Presentation : IReturn<PresentationDto>
	{
		[ApiMember(Name = "speakerSlug", Description = "XXXX", ParameterType = "query", DataType = "string", IsRequired = false)]
		public string speakerSlug { get; set; }

		[ApiMember(Name = "slug", Description = "XXXX", ParameterType = "query", DataType = "string", IsRequired = false)]
		public string slug { get; set; }
	}
}