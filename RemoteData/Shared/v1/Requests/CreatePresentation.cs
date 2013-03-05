using System.Collections.Generic;
using ServiceStack.ServiceHost;
using TekConf.RemoteData.Dtos.v1;

namespace TekConf.UI.Api.Services.Requests.v1
{
	[Route("/v1/{speakerSlug}/presentations/{slug}", "POST")]
	[Route("/v1/{speakerSlug}/presentations/{slug}", "PUT")]
	public class CreatePresentation : IReturn<PresentationDto>
	{
		public string Slug { get; set; }
		public string SpeakerSlug { get; set; }
		public string UserName { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public List<string> Tags { get; set; }
	}
}