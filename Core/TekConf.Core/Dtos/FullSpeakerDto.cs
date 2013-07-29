using TekConf.Core.Entities;

namespace TekConf.RemoteData.Dtos.v1
{
	public class FullSpeakerDto : SpeakerDto
	{
		public FullSpeakerDto(SpeakerEntity speaker)
		{
			if (speaker != null)
			{
				this.slug = speaker.Slug;
				this.blogUrl = speaker.BlogUrl;
				this.description = speaker.Description;
				this.emailAddress = speaker.EmailAddress;
				this.facebookUrl = speaker.FacebookUrl;
				this.firstName = speaker.FirstName;
				this.lastName = speaker.LastName;
				this.twitterName = speaker.TwitterName;
				this.linkedInUrl = speaker.LinkedInUrl;
				this.phoneNumber = speaker.PhoneNumber;
				this.url = speaker.Url;
				this.fullName = speaker.FullName;

				this.bitbucketUrl = speaker.BitbucketUrl;
				this.codeplexUrl = speaker.CodeplexUrl;
				this.coderWallUrl = speaker.CoderWallUrl;
				this.githubUrl = speaker.GithubUrl;
				this.googlePlusUrl = speaker.GooglePlusUrl;
				this.profileImageUrl = speaker.ProfileImageUrl;
				this.stackoverflowUrl = speaker.StackoverflowUrl;
				this.vimeoUrl = speaker.VimeoUrl;
				this.youtubeUrl = speaker.YoutubeUrl;
			}
		}
		public string profileImageUrl { get; set; }
		public string googlePlusUrl { get; set; }
		public string vimeoUrl { get; set; }
		public string youtubeUrl { get; set; }
		public string githubUrl { get; set; }
		public string coderWallUrl { get; set; }
		public string stackoverflowUrl { get; set; }
		public string bitbucketUrl { get; set; }
		public string codeplexUrl { get; set; }
	}
}