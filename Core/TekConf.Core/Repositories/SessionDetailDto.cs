using System.Collections.Generic;
using System.Linq;
using TekConf.Core.Entities;
using TekConf.RemoteData.Dtos.v1;

namespace TekConf.Core.Repositories
{
	public class SessionDetailDto
	{
		public SessionDetailDto(SessionEntity entity)
		{
			if (entity != null)
			{
				slug = entity.Slug;
				title = entity.Title;
				startDescription = entity.StartDescription();
				room = entity.Room;
				description = entity.Description;
				isAddedToSchedule = entity.IsAddedToSchedule;
				//TODO speakers = entity.Speakers.Select(x => new SpeakerDetailViewDto(x)).ToList();
			}
		}

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