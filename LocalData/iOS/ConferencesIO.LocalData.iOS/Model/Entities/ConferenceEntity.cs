using System;
using System.Collections.Generic;
using Catnap;

namespace ConferencesIO.LocalData.iOS
{

	public class ConferenceEntity : EntityInt
	{
		public ConferenceEntity ()
		{
			//this.Sessions = new List<SessionEntity>();
			//this.Speakers = new List<SpeakerEntity>();
		}
		public static string TableName = "ConferenceEntity";
		public static string CreateTableSql = @"CREATE TABLE " + TableName + " (Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, " +
				"description VARCHAR, " +
				"facebookUrl VARCHAR, " + 
				"slug VARCHAR, " + 
				"homepageUrl VARCHAR, " + 
				"lanyrdUrl VARCHAR, " + 
				"location VARCHAR, " + 
				"meetupUrl VARCHAR, " + 
				"name VARCHAR, " + 
				//"start VARCHAR, " + 
				//"end VARCHAR, " + 
				"twitterHashTag VARCHAR, " + 
				"twitterName VARCHAR " + 
				")";


		public string description { get; set; }
		public string facebookUrl { get; set; }
		public string slug { get; set; }
		public string homepageUrl { get; set; }
		public string lanyrdUrl { get; set; }
		public string location { get; set; }
		public string meetupUrl { get; set; }
		public string name { get; set; }
		//public DateTime start { get; set; }
		//public DateTime end { get; set; }
		public string twitterHashTag { get; set; }
		public string twitterName { get; set; }

		//public IEnumerable<SessionEntity> Sessions {get;set;}
		//public IEnumerable<SpeakerEntity> Speakers {get; set;}

	}


//	public class Forum : EntityInt
//	{
//		private IEnumerable<Post> posts = new List<Post>();
//		
//		public string Name { get; set; }
//		public TimeSpan? TimeOfDayLastUpdated { get; set; }
//		public IEnumerable<Post> Posts
//		{
//			get { return posts; }
//			set { posts = value; }
//		}
//	}
}
