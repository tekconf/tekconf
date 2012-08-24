using System;
using System.Collections.Generic;
using Catnap;

namespace ConferencesIO.LocalData.iOS
{
	public class SpeakerEntity : EntityInt
	{
		public static string TableName = "SpeakerEntity";
		public static string CreateTableSql = @"CREATE TABLE " + TableName + " (Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, " +
			"slug VARCHAR, " +
				"firstName VARCHAR, " + 
				"lastName VARCHAR, " + 
				"description VARCHAR, " + 
				"blogUrl VARCHAR, " + 
				"twitterName VARCHAR, " + 
				"facebookUrl VARCHAR, " + 
				"linkedInUrl VARCHAR, " + 
				"emailAddress VARCHAR, " + 
				"phoneNumber VARCHAR, " + 
				"conferenceEntityId INT NOT NULL" +
				")";

		//public string Id {get;set;}
		public string slug { get; set; }
		public string firstName { get; set; }
		public string lastName { get; set; }
		public string description { get; set; }
		public string blogUrl { get; set; }
		public string twitterName { get; set; }
		public string facebookUrl { get; set; }
		public string linkedInUrl { get; set; }
		public string emailAddress { get; set; }
		public string phoneNumber { get; set; }
	}

}
