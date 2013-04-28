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

		public List<FullSessionDto> SessionsByTime
		{
			get { return sessions == null ? new List<FullSessionDto>() : sessions.OrderBy(x => x.start).ToList(); }
		}
		public List<FullSessionDto> sessions { get; set; }

		public int numberOfSessions { get; set; }
		public bool? isAddedToSchedule { get; set; }
		public bool? isOnline { get; set; }

		public string DateRange
		{
			get
			{

				var range = "";
				if (this.start == default(DateTime) || this.end == default(DateTime))
				{
					range = "No Date Set";
				}
				else if (this.start.Month == this.end.Month && this.start.Year == this.end.Year)
				{
					// They begin and end in the same month
					if (this.start.Date == this.end.Date)
					{
						range = this.start.ToString("MMMM") + " " + this.start.Day + ", " + this.start.Year;
					}
					else
					{
						range = this.start.ToString("MMMM") + " " + this.start.Day + " - " + this.end.Day + ", " + this.start.Year;
					}
				}
				else
				{
					// They begin and end in different months
					if (this.start.Year == this.end.Year)
					{
						range = this.start.ToString("MMMM") + " " + this.start.Day + " - " + this.end.ToString("MMMM") + " " + this.end.Day + ", " + this.start.Year;
					}
					else
					{
						range = this.start.ToString("MMMM") + " " + this.start.Day + ", " + this.start.Year + " - " + this.end.ToString("MMMM") + " " + this.end.Day + ", " + this.end.Year;
					}

				}

				return range;
			}
		}

		public bool IsOnSale()
		{
			bool isOnSale = this.registrationOpens <= DateTime.Now && this.registrationCloses >= DateTime.Now;

			return isOnSale;
		}


		public bool IsOpenCallForSpeakers()
		{
			bool isOpenCall = this.callForSpeakersOpens <= DateTime.Now && this.callForSpeakersCloses >= DateTime.Now;

			return isOpenCall;
		}

		public string FormattedAddress
		{
			get
			{
				string formattedAddress = "";
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