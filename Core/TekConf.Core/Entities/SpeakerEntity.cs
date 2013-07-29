using Cirrious.MvvmCross.Plugins.Sqlite;
using TekConf.RemoteData.Dtos.v1;

namespace TekConf.Core.Entities
{
	public class SpeakerEntity
	{
		public SpeakerEntity()
		{
			
		}

		public SpeakerEntity(int sessionId, FullSpeakerDto speaker)
		{
			if (speaker != null)
			{
				this.SessionId = sessionId;
				this.Slug = speaker.slug;
				this.BitbucketUrl = speaker.bitbucketUrl;
				this.CodeplexUrl = speaker.codeplexUrl;
				this.CoderWallUrl = speaker.coderWallUrl;
				this.GithubUrl = speaker.githubUrl;
				this.GooglePlusUrl = speaker.googlePlusUrl;
				this.ProfileImageUrl = speaker.profileImageUrl;
				this.StackoverflowUrl = speaker.stackoverflowUrl;
				this.VimeoUrl = speaker.vimeoUrl;
				this.YoutubeUrl = speaker.youtubeUrl;
				this.BlogUrl = speaker.blogUrl;
				this.Description = speaker.description;
				this.EmailAddress = speaker.emailAddress;
				this.FacebookUrl = speaker.facebookUrl;
				this.FirstName = speaker.firstName;
				this.LastName = speaker.lastName;
				this.TwitterName = speaker.twitterName;
				this.LinkedInUrl = speaker.linkedInUrl;
				this.PhoneNumber = speaker.phoneNumber;
				this.Url = speaker.url;
				this.FullName = speaker.fullName;
			}
		}

		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }
		public string Slug { get; set; }
		public int SessionId { get; set; }

		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Description { get; set; }
		public string BlogUrl { get; set; }
		public string TwitterName { get; set; }
		public string FacebookUrl { get; set; }
		public string LinkedInUrl { get; set; }
		public string EmailAddress { get; set; }
		public string PhoneNumber { get; set; }
		public string Url { get; set; }
		public string FullName { get; set; }


		public string ProfileImageUrl { get; set; }
		public string GooglePlusUrl { get; set; }
		public string VimeoUrl { get; set; }
		public string YoutubeUrl { get; set; }
		public string GithubUrl { get; set; }
		public string CoderWallUrl { get; set; }
		public string StackoverflowUrl { get; set; }
		public string BitbucketUrl { get; set; }
		public string CodeplexUrl { get; set; }
	}
}