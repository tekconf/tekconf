using System;
using System.Collections.Generic;

namespace TekConf.RemoteData.Dtos.v1
{
    public class FullConferenceDto
    {
        public string slug { get; set; }

        public string name { get; set; }
        public DateTime start { get; set; }
        public DateTime end { get; set; }
        public DateTime callForSpeakersOpens { get; set; }
        public DateTime callForSpeakersCloses { get; set; }
        public DateTime registrationOpens { get; set; }
        public DateTime registrationCloses { get; set; }
        public string description { get; set; }
        public string location { get; set; }
        public AddressDto address { get; set; }
        public string tagline { get; set; }
        public string imageUrl { get; set; }
        public bool isLive { get; set; }
        public string facebookUrl { get; set; }
        public string homepageUrl { get; set; }
        public string lanyrdUrl { get; set; }
        public string meetupUrl { get; set; }
        public string googlePlusUrl { get; set; }
        public string vimeoUrl { get; set; }
        public string youtubeUrl { get; set; }
        public string githubUrl { get; set; }
        public string linkedInUrl { get; set; }
        public string twitterHashTag { get; set; }
        public string twitterName { get; set; }
        public double[] position { get; set; }
				public int defaultTalkLength { get; set; }
				public List<string> rooms { get; set; }
				public List<string> sessionTypes { get; set; } 
        public List<FullSessionDto> sessions { get; set; }

        public bool IsOnSale()
        {
            bool isOnSale = this.registrationOpens <= DateTime.Now && this.registrationCloses >= DateTime.Now;

            return isOnSale;
        }

		public string CalculateConferenceDates (FullConferenceDto conference)
		{
			string conferenceDates = "No dates scheduled";
			if (conference.start != default(DateTime) && conference.end != default(DateTime))
			{
				if (conference.start.Date == conference.end.Date)
				{
					conferenceDates = conference.start.ToString("MMMM d, yyyy");
				}
				else if (conference.start.Year == conference.end.Year)
				{
					if (conference.start.Month == conference.end.Month)
					{
						//@startDate.ToString("MMMM")<text> </text>@startDate.Day<text> - </text>@endDate.Day<text>, </text>@startDate.Year
						conferenceDates = conference.start.ToString("MMMM d") + " - " + conference.end.Day + ", " + conference.end.Year;
					}
					else
					{
						conferenceDates = conference.start.ToString("MMMM d") + " - " + conference.end.ToString("MMMM d") + ", " + conference.end.Year;
					}
				}
				else
				{
					conferenceDates = conference.start.ToString("MMMM d, yyyy") + " - " + conference.end.ToString("MMMM d, yyyy");
				}
			}
			
			return conferenceDates;
		}
    }
}