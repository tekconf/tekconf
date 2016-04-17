using System;
using System.Collections.Generic;
using SQLite;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;

namespace TekConf.Mobile.Core
{
	[Table("Sessions")]
	public class SessionModel
	{
		[PrimaryKey, AutoIncrement, Column("_id")]
		public int Id { get; set; }

		[Unique, Indexed]
		public string Slug { get; set; }

		public string Title { get; set; }

		public DateTime? StartDate { get; set; }

		public DateTime? EndDate { get; set; }

		public string Description { get; set; }

		public string Room { get; set; }

		[ForeignKey(typeof(ConferenceModel))]     // Specify the foreign key
		public int ConferenceId { get; set; }

		[ManyToOne]      // Many to one relationship with Stock
		public ConferenceModel Conference { get; set; }
		//public List<Speaker> Speakers { get; set; }

		//public string SpeakerName()
		//{
		//	if (Speakers == null || !Speakers.Any())
		//	{
		//		return "N/A";
		//	}

		//	if (Speakers.Count() == 1)
		//	{
		//		var speaker = Speakers.First();
		//		return string.Format("{0} {1}", speaker.FirstName, speaker.LastName);
		//	}

		//	return string.Format("{0} speakers", Speakers.Count());
		//}
	}
}
