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

		[ApiMember(Name = "Title", Description = "XXXX", ParameterType = "query", DataType = "string", IsRequired = false)]
		public virtual string Title { get; set; }

		[ApiMember(Name = "Description", Description = "XXXX", ParameterType = "query", DataType = "string", IsRequired = false)]
		public virtual string Description { get; set; }

		[ApiMember(Name = "Tags", Description = "XXXX", ParameterType = "query", DataType = "string", IsRequired = false)]
		public virtual List<string> Tags { get; set; }

		[ApiMember(Name = "DownloadPaths", Description = "XXXX", ParameterType = "query", DataType = "string", IsRequired = false)]
		public virtual List<string> DownloadPaths { get; set; }

		[ApiMember(Name = "Videos", Description = "XXXX", ParameterType = "query", DataType = "string", IsRequired = false)]
		public virtual List<string> Videos { get; set; }

		[ApiMember(Name = "Subjects", Description = "XXXX", ParameterType = "query", DataType = "string", IsRequired = false)]
		public virtual List<string> Subjects { get; set; }

		[ApiMember(Name = "Difficulty", Description = "XXXX", ParameterType = "query", DataType = "string", IsRequired = false)]
		public virtual string Difficulty { get; set; }

		[ApiMember(Name = "Length", Description = "XXXX", ParameterType = "query", DataType = "int", IsRequired = false)]
		public virtual int Length { get; set; }

		[ApiMember(Name = "imageUrl", Description = "XXXX", ParameterType = "query", DataType = "string", IsRequired = false)]
		public virtual string imageUrl { get; set; }


		[ApiMember(Name = "SpeakerSlug", Description = "XXXX", ParameterType = "query", DataType = "string", IsRequired = false)]
		public virtual string SpeakerSlug { get; set; }

		[ApiMember(Name = "UserName", Description = "XXXX", ParameterType = "query", DataType = "string", IsRequired = false)]
		public virtual string UserName { get; set; }

	}
}