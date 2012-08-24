using System;
using System.Collections.Generic;
using Catnap;

namespace ConferencesIO.LocalData.iOS
{
	public class SessionSpeakerEntity : EntityInt
	{
		public static string TableName = "SessionSpeaker";
		public static string CreateTableSql = @"CREATE TABLE " + TableName + " (Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, " +
			"SessionId INT NOT NULL, " +
				"SpeakerId INT NOT NULL " + 
				")";

		public int SessionId { get; set;}
		public int SpeakerId { get; set;}
	}

}
