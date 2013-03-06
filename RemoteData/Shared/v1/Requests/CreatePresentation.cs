using System.Collections.Generic;
using ServiceStack.ServiceHost;
using TekConf.RemoteData.Dtos.v1;

namespace TekConf.UI.Api.Services.Requests.v1
{
	[Route("/v1/{speakerSlug}/presentations/{slug}", "POST")]
	[Route("/v1/{speakerSlug}/presentations/{slug}", "PUT")]
	public class CreatePresentation : IReturn<PresentationDto>
	{
		public virtual string Slug { get; set; }
		public virtual string SpeakerSlug { get; set; }
		public virtual string UserName { get; set; }
		public virtual string Title { get; set; }
		public virtual string Description { get; set; }
		public virtual List<string> Tags { get; set; }
	}
}