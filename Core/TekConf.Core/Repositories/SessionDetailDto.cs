using System.Collections.Generic;
using System.Linq;
using TekConf.RemoteData.Dtos.v1;

namespace TekConf.Core.Repositories
{
	public class SpeakerDetailViewDto
	{
		public SpeakerDetailViewDto(FullSpeakerDto fullSpeaker)
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

	public class SessionDetailDto
	{
		public SessionDetailDto(FullSessionDto fullSession)
		{
			if (fullSession != null)
			{
				slug = fullSession.slug;
				title = fullSession.title;
				startDescription = fullSession.startDescription;
				room = fullSession.room;
				description = fullSession.description;
				isAddedToSchedule = fullSession.isAddedToSchedule.HasValue && fullSession.isAddedToSchedule.Value;
				speakers = fullSession.speakers.Select(x => new SpeakerDetailViewDto(x)).ToList();
			}
		}
		public string slug { get; set; }
		public string title { get; set; }
		public string startDescription { get; set; }
		public string room { get; set; }
		public string description { get; set; }
		public IEnumerable<SpeakerDetailViewDto> speakers { get; set; }

		public bool isAddedToSchedule { get; set; }
	}
}