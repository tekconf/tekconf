using System;
using System.Collections.Generic;
using SQLite;

namespace ConferencesIO.LocalData.iOS
{

	public class SessionEntity
	{
		public string Id { get; set;}

		public string slug { get; set; }
		public string title { get; set; }
		public DateTime start { get; set; }
		public DateTime end { get; set; }
		public string room { get; set; }
		public string difficulty { get; set; }
		public string description { get; set; }
		public string twitterHashTag { get; set; }
		public string sessionType { get; set; }
		//public List<string> links { get; set; }
		//public List<string> tags { get; set; }
		//public List<string> subjects { get; set; }
		//public List<string> resources { get; set; }
		//public List<string> prerequisites { get; set; }
		//public List<FullSpeakerDto> speakers { get; set; }
	}

	public class LinkEntity
	{
		public string Id {get;set;}
		public string ParentId {get;set;}
		public string Link {get;set;}
	}

	public class ConferenceEntity
	{
		public string description { get; set; }
		public string facebookUrl { get; set; }
		public string slug { get; set; }
		public string homepageUrl { get; set; }
		public string lanyrdUrl { get; set; }
		public string location { get; set; }
		public string meetupUrl { get; set; }
		public string name { get; set; }
		public DateTime start { get; set; }
		public DateTime end { get; set; }
		public string twitterHashTag { get; set; }
		public string twitterName { get; set; }
		//public List<SessionEntity> sessions { get; set; }
	}
}

