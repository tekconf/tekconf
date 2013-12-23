using System.Collections.Generic;
using ServiceStack;
using ServiceStack.ServiceHost;
using TekConf.RemoteData.Dtos.v1;

namespace TekConf.UI.Api.Services.Requests.v1
{
	[Route("/v1/conferences/{conferenceSlug}/speakers", "GET")]
	public class Speakers : IReturn<List<FullSpeakerDto>>
	{
		[ApiMember(Name = "conferenceSlug", Description = "XXXX", ParameterType = "query", DataType = "string", IsRequired = false)]
		public string conferenceSlug { get; set; }
	}

	[Route("/v1/conferences/{conferenceSlug}/speakers/{speakerSlug}", "GET")]
	public class Speaker : IReturn<FullSpeakerDto>
	{
		[ApiMember(Name = "conferenceSlug", Description = "XXXX", ParameterType = "query", DataType = "string", IsRequired = false)]
		public string conferenceSlug { get; set; }
		[ApiMember(Name = "speakerSlug", Description = "XXXX", ParameterType = "query", DataType = "string", IsRequired = false)]
		public string speakerSlug { get; set; }
	}

	[Route("/v1/conferences/{conferenceSlug}/{sessionSlug}/speakers/{slug}", "POST")]
	[Route("/v1/conferences/{conferenceSlug}/speakers/{slug}", "PUT")]
	public class CreateSpeaker : IReturn<FullSpeakerDto>
	{
		[ApiMember(Name = "conferenceSlug", Description = "XXXX", ParameterType = "query", DataType = "string", IsRequired = false)]
		public string conferenceSlug { get; set; }
		[ApiMember(Name = "sessionSlug", Description = "XXXX", ParameterType = "query", DataType = "string", IsRequired = false)]
		public string sessionSlug { get; set; }

		[ApiMember(Name = "slug", Description = "XXXX", ParameterType = "query", DataType = "string", IsRequired = false)]
		public string slug { get; set; }
		[ApiMember(Name = "firstName", Description = "XXXX", ParameterType = "query", DataType = "string", IsRequired = false)]
		public string firstName { get; set; }
		[ApiMember(Name = "lastName", Description = "XXXX", ParameterType = "query", DataType = "string", IsRequired = false)]
		public string lastName { get; set; }
		[ApiMember(Name = "description", Description = "XXXX", ParameterType = "query", DataType = "string", IsRequired = false)]
		public string description { get; set; }
		[ApiMember(Name = "blogUrl", Description = "XXXX", ParameterType = "query", DataType = "string", IsRequired = false)]
		public string blogUrl { get; set; }
		[ApiMember(Name = "twitterName", Description = "XXXX", ParameterType = "query", DataType = "string", IsRequired = false)]
		public string twitterName { get; set; }
		[ApiMember(Name = "facebookUrl", Description = "XXXX", ParameterType = "query", DataType = "string", IsRequired = false)]
		public string facebookUrl { get; set; }
		[ApiMember(Name = "linkedInUrl", Description = "XXXX", ParameterType = "query", DataType = "string", IsRequired = false)]
		public string linkedInUrl { get; set; }
		[ApiMember(Name = "emailAddress", Description = "XXXX", ParameterType = "query", DataType = "string", IsRequired = false)]
		public string emailAddress { get; set; }
		[ApiMember(Name = "phoneNumber", Description = "XXXX", ParameterType = "query", DataType = "string", IsRequired = false)]
		public string phoneNumber { get; set; }
		[ApiMember(Name = "isFeatured", Description = "XXXX", ParameterType = "query", DataType = "string", IsRequired = false)]
		public bool isFeatured { get; set; }
		[ApiMember(Name = "profileImageUrl", Description = "XXXX", ParameterType = "query", DataType = "string", IsRequired = false)]
		public string profileImageUrl { get; set; }
		[ApiMember(Name = "company", Description = "XXXX", ParameterType = "query", DataType = "string", IsRequired = false)]
		public string company { get; set; }

		[ApiMember(Name = "googlePlusUrl", Description = "XXXX", ParameterType = "query", DataType = "string", IsRequired = false)]
		public string googlePlusUrl { get; set; }
		[ApiMember(Name = "vimeoUrl", Description = "XXXX", ParameterType = "query", DataType = "string", IsRequired = false)]
		public string vimeoUrl { get; set; }
		[ApiMember(Name = "youtubeUrl", Description = "XXXX", ParameterType = "query", DataType = "string", IsRequired = false)]
		public string youtubeUrl { get; set; }
		[ApiMember(Name = "githubUrl", Description = "XXXX", ParameterType = "query", DataType = "string", IsRequired = false)]
		public string githubUrl { get; set; }
		[ApiMember(Name = "coderWallUrl", Description = "XXXX", ParameterType = "query", DataType = "string", IsRequired = false)]
		public string coderWallUrl { get; set; }
		[ApiMember(Name = "stackoverflowUrl", Description = "XXXX", ParameterType = "query", DataType = "string", IsRequired = false)]
		public string stackoverflowUrl { get; set; }
		[ApiMember(Name = "bitbucketUrl", Description = "XXXX", ParameterType = "query", DataType = "string", IsRequired = false)]
		public string bitbucketUrl { get; set; }
		[ApiMember(Name = "codeplexUrl", Description = "XXXX", ParameterType = "query", DataType = "string", IsRequired = false)]
		public string codeplexUrl { get; set; }
	}
}
