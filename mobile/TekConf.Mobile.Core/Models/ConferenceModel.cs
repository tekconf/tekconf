using System;
namespace TekConf.Mobile.Core
{
	public class ConferenceModel
	{
		public bool IsAddedToSchedule { get; set; }
		public string Slug { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }

		public DateTime? StartDate { get; set; }

		public DateTime? EndDate { get; set; }

		public bool IsLive { get; set; }

		public DateTime DatePublished { get; set; }

		public int? DefaultSessionLength { get; set; }

		public DateTime? CallForSpeakersOpen { get; set; }

		public DateTime? CallForSpeakersCloses { get; set; }
		public DateTime? RegistrationOpens { get; set; }

		public DateTime? RegistrationCloses { get; set; }

		public DateTime DateAdded { get; set; }

		public DateTime LastUpdated { get; set; }

		public bool IsOnlineConference { get; set; }

		public string ImageUrl { get; set; }
		public string ImageSquareUrl { get; set; }
		public string TagLine { get; set; }
		public string FacebookUrl { get; set; }
		public string HomepageUrl { get; set; }
		public string LanyrdUrl { get; set; }

		public string TwitterHashtag { get; set; }
		public string TwitterName { get; set; }
		public string MeetupUrl { get; set; }
		public string GooglePlusUrl { get; set; }
		public string VimeoUrl { get; set; }
		public string YouTubeUrl { get; set; }
		public string GithubUrl { get; set; }
		public string LinkedInUrl { get; set; }
		public string HighlightColor { get; set; }
		//public Address Address { get; set; } = new Address();
		//public List<Session> Sessions { get; set; }
	}
}

