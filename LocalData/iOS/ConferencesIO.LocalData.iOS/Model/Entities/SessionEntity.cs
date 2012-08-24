using System;
using System.Collections.Generic;
using Catnap;

namespace ConferencesIO.LocalData.iOS
{

	public class SessionEntity : EntityInt
	{
		public SessionEntity ()
		{
			this.SessionSpeakers = new List<SessionSpeakerEntity>();
		}

		public static string TableName = "SessionEntity";
		public static string CreateTableSql = @"CREATE TABLE " + TableName + " (Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, " +
			"slug VARCHAR, " +
				"title VARCHAR, " + 
				"start VARCHAR, " + 
				"end VARCHAR, " + 
				"room VARCHAR, " + 
				"difficulty VARCHAR, " + 
				"description VARCHAR, " + 
				"twitterHashTag VARCHAR, " + 
				"sessionType VARCHAR, " + 
				"conferenceEntityId INT NOT NULL" +
				")";

		public string slug { get; set; }
		public string title { get; set; }
		public DateTime start { get; set; }
		public DateTime end { get; set; }
		public string room { get; set; }
		public string difficulty { get; set; }
		public string description { get; set; }
		public string twitterHashTag { get; set; }
		public string sessionType { get; set; }
		public IEnumerable<SessionSpeakerEntity> SessionSpeakers {get;set;}

		//public List<string> links { get; set; }
		//public List<string> tags { get; set; }
		//public List<string> subjects { get; set; }
		//public List<string> resources { get; set; }
		//public List<string> prerequisites { get; set; }
		//public List<FullSpeakerDto> speakers { get; set; }
	}


}

