namespace TekConf.Core.Repositories
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using TekConf.Core.Entities;
	using TekConf.RemoteData.Dtos.v1;

	public class ConferenceSessionListDto
	{
		private string _speakerNames;

		public ConferenceSessionListDto(SessionEntity entity)
		{
			this.title = entity.Title;
			startDescription = entity.StartDescription();
			//TODO speakerNames = entity.SpeakerNames;
			speakerNames = "";
			this.start = entity.Start;
			//tags = entity.Tags;
			tags = new List<string>();
			this.room = entity.Room;
			this.slug = entity.Slug;
		}

		public ConferenceSessionListDto(FullSessionDto fullSession)
		{
			this.title = fullSession.title;
			this.startDescription = fullSession.startDescription;
			this.speakerNames = fullSession.speakerNames;
			this.start = fullSession.start;
			this.tags = fullSession.tags;
			this.room = fullSession.room;
			this.slug = fullSession.slug;
		}

		public string room { get; set; }

		public string tagNames
		{
			get
			{
				if (tags == null || !tags.Any()) return "None";
				return tags.Aggregate(",", (current, l) => current + (l + ","));
			}
		}

		public List<string> tags { get; set; }
		public string title { get; set; }
		public string startDescription { get; set; }

		public string speakerNames
		{
			get
			{
				if (string.IsNullOrWhiteSpace(_speakerNames)) return "None";
				return _speakerNames;
			}
			set
			{
				_speakerNames = value;
			}
		}

		public DateTime start { get; set; }
		public string slug { get; set; }
	}
}