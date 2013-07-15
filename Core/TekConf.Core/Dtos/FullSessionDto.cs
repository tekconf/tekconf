using System;
using System.Collections.Generic;
using System.Linq;

namespace TekConf.RemoteData.Dtos.v1
{
	using TekConf.Core.Entities;

	public class FullSessionDto
	{
		public FullSessionDto()
		{
			
		}
		public FullSessionDto(SessionEntity entity)
		{
			this.slug = entity.Slug;
			this.description = entity.Description;
			this.difficulty = entity.Difficulty;
			this.end = entity.End;
			this.isAddedToSchedule = entity.IsAddedToSchedule;
			this.links = new List<string>(); //TODO
			this.prerequisites = new List<string>(); //TODO
			this.resources = new List<string>(); //TODO
			this.room = entity.Room;
			this.sessionType = entity.SessionType;
			this.speakers = new List<FullSpeakerDto>(); //TODO
			this.start = entity.Start;
			this.subjects = new List<string>(); //TODO
			this.tags = new List<string>(); //TODO
			this.title = entity.Title;
			this.twitterHashTag = entity.TwitterHashTag;
		}
		private string _room;
		public string slug { get; set; }
		public string title { get; set; }
		public DateTime start { get; set; }
		public DateTime end { get; set; }
		public string room
		{
			get
			{
				if (string.IsNullOrEmpty(_room))
					return "No room assigned";

				return _room;
			}
			set { _room = value; }
		}

		public string difficulty { get; set; }
		public string description { get; set; }
		public string twitterHashTag { get; set; }
		public string sessionType { get; set; }
		public List<string> links { get; set; }
		public List<string> tags { get; set; }
		public List<string> subjects { get; set; }
		public List<string> resources { get; set; }
		public List<string> prerequisites { get; set; }
		public List<FullSpeakerDto> speakers { get; set; }

		public string speakerNames
		{
			get
			{
				string names;
				if (speakers != null && speakers.Any())
				{
					names = speakers.Select(xs => xs.fullName).Aggregate((current, next) => current + ", " + next);
				}
				else
				{
					names = "No speakers assigned";
				}

				return names;
			}
		}

		public bool? isAddedToSchedule { get; set; }
		public string startDescription
		{
			get
			{
				if (start == default(DateTime))
				{
					return "Not scheduled yet";
				}
				
				return start.ToString("dddd h:mm tt");
			}
		}
	}
}