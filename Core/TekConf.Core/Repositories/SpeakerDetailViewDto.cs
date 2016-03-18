using TekConf.Core.Entities;

namespace TekConf.Core.Repositories
{
	using TekConf.RemoteData.Dtos.v1;

	public class SpeakerDetailViewDto
	{
		public SpeakerDetailViewDto(SpeakerEntity speaker)
		{
			var fullSpeaker = new FullSpeakerDto(speaker);
			Initialize(fullSpeaker);
		}

		public SpeakerDetailViewDto(FullSpeakerDto fullSpeaker)
		{
			Initialize(fullSpeaker);
		}

		private void Initialize(FullSpeakerDto fullSpeaker)
		{
			if (fullSpeaker != null)
			{
				fullName = fullSpeaker.fullName;
				description = fullSpeaker.description;
				slug = fullSpeaker.slug;
			}
		}

		public string fullName { get; set; }
		public string description { get; set; }
		public string slug { get; set; }
	}
}