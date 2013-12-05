using System;
using System.Collections.Generic;
using System.Linq;


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
        public DateTime lastUpdated { get; set; }
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
		public List<string> subjects { get; set; }
		public List<string> tags { get; set; }

		public List<FullSessionDto> sessions { get; set; }

		public int numberOfSessions { get; set; }
		public bool? isAddedToSchedule { get; set; }
		public bool? isOnline { get; set; }

		public string DateRange
		{
			get
			{

				string range;
				if (start == default(DateTime) || end == default(DateTime))
				{
					range = "No Date Set";
				}
				else if (start.Month == end.Month && start.Year == end.Year)
				{
					// They begin and end in the same month
					if (start.Date == end.Date)
					{
						range = start.ToString("MMMM") + " " + start.Day + ", " + start.Year;
					}
					else
						range = start.ToString("MMMM") + " " + start.Day + " - " + end.Day + ", " + start.Year;
				}
				else
				{
					// They begin and end in different months
					if (start.Year == end.Year)
					{
						range = start.ToString("MMMM") + " " + start.Day + " - " + end.ToString("MMMM") + " " + end.Day + ", " + start.Year;
					}
					else
					{
						range = start.ToString("MMMM") + " " + start.Day + ", " + start.Year + " - " + end.ToString("MMMM") + " " + end.Day + ", " + end.Year;
					}

				}

				return range;
			}
		}

		public bool IsOnSale()
		{
			bool isOnSale = registrationOpens <= DateTime.Now && registrationCloses >= DateTime.Now;

			return isOnSale;
		}


		public bool IsOpenCallForSpeakers()
		{
			var isOpenCall = callForSpeakersOpens <= DateTime.Now && callForSpeakersCloses >= DateTime.Now;

			return isOpenCall;
		}

		public string FormattedAddress
		{
			get
			{
				string formattedAddress;
				if (isOnline == true)
				{
					formattedAddress = "online";
				}
				else if (address == null)
				{
					formattedAddress = "No location set";
				}
				else if (!string.IsNullOrWhiteSpace(address.City) && !string.IsNullOrWhiteSpace(address.State))
				{
					formattedAddress = address.City + ", " + address.State;
				}
				else if (!string.IsNullOrWhiteSpace(address.City) && !string.IsNullOrWhiteSpace(address.Country))
				{
					formattedAddress = address.City + ", " + address.Country;
				}
				else if (!string.IsNullOrWhiteSpace(address.City))
				{
					formattedAddress = address.City;
				}
				else
				{
					formattedAddress = "No location set";
				}

				return formattedAddress;
			}
		}

		public string CalculateConferenceDates(FullConferenceDto conference)
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
						//@this.start.ToString("MMMM")<text> </text>@this.start.Day<text> - </text>@this.end.Day<text>, </text>@this.start.Year
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