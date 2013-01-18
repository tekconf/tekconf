namespace TekConf.RemoteData.Dtos.v1
{
	public class FullSpeakerDto : SpeakerDto
	{
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