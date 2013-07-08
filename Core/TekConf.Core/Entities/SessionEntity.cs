using System;
using Cirrious.MvvmCross.Plugins.Sqlite;
using TekConf.RemoteData.Dtos.v1;

namespace TekConf.Core.Entities
{
	public class SessionEntity
	{
		public SessionEntity()
		{
			
		}
		public SessionEntity(int conferenceId, FullSessionDto session)
		{
			if (session != null)
			{
				this.Slug = session.slug;
				this.ConferenceId = conferenceId;
				this.Description = session.description;
				this.Difficulty = session.difficulty;
				this.End = session.end;
				this.IsAddedToSchedule = session.isAddedToSchedule;
				this.Room = session.room;
				this.SessionType = session.sessionType;
				this.Start = session.start;
				this.Title = session.title;
				this.TwitterHashTag = session.twitterHashTag;
			}
		}

		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }
		public string Slug { get; set; }
		public int ConferenceId { get; set; }
		public string Title { get; set; }
		public DateTime Start { get; set; }
		public DateTime End { get; set; }
		public string Room { get; set; }
		public string Difficulty { get; set; }
		public string Description { get; set; }
		public string TwitterHashTag { get; set; }
		public string SessionType { get; set; }
		public bool? IsAddedToSchedule { get; set; }
		//public List<string> Links { get; set; }
		//public List<string> Tags { get; set; }
		//public List<string> Subjects { get; set; }
		//public List<string> Resources { get; set; }
		//public List<string> Prerequisites { get; set; }
		//public List<FullSpeakerDto> Speakers { get; set; }
		//public string SpeakerNames
		//{
		//	get
		//	{
		//		string names;
		//		if (speakers != null && speakers.Any())
		//		{
		//			names = speakers.Select(xs => xs.fullName).Aggregate((current, next) => current + ", " + next);
		//		}
		//		else
		//		{
		//			names = "No speakers assigned";
		//		}

		//		return names;
		//	}
		//}
		//public string StartDescription
		//{
		//	get
		//	{
		//		if (Start == default(DateTime))
		//		{
		//			return "Not scheduled yet";
		//		}

		//		return Start.ToString("dddd h:mm tt");
		//	}
		//}
	}
}