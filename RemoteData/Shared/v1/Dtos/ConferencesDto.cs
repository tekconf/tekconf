//using System;
//using TekConf.RemoteData.v1;

//namespace TekConf.RemoteData.Dtos.v1
//{
//	public class ConferencesDto
//		{
//				public string name { get; set; }
//				public DateTime start { get; set; }
//				public DateTime end { get; set; }
//				public DateTime callForSpeakersOpens { get; set; }
//				public DateTime callForSpeakersCloses { get; set; }
//				public string location { get; set; }
//				public AddressDto address { get; set; }
//				public string url { get; set; }
//				public string description { get; set; }
//				public string imageUrl { get; set; }
//				public DateTime registrationOpens { get; set; }
//				public DateTime registrationCloses { get; set; }
//				public int numberOfSessions { get; set; }
//				public string slug
//				{
//						get { return name.GenerateSlug(); }
//				}

//				public bool IsOpenCallForSpeakers()
//				{
//						bool isOpenCall = this.callForSpeakersOpens <= DateTime.Now && this.callForSpeakersCloses >= DateTime.Now;

//						return isOpenCall;
//				}

//				public bool IsOnSale()
//				{
//						bool isOnSale = this.registrationOpens <= DateTime.Now && this.registrationCloses >= DateTime.Now;

//						return isOnSale;
//				}

//				public string CalculateConferenceDates(ConferencesDto conference)
//				{
//						string conferenceDates = "No dates scheduled";
//						if (conference.start != default(DateTime) && conference.end != default(DateTime))
//						{
//								if (conference.start.Date == conference.end.Date)
//								{
//										conferenceDates = conference.start.ToString("MMMM d, yyyy");
//								}
//								else if (conference.start.Year == conference.end.Year)
//								{
//										if (conference.start.Month == conference.end.Month)
//										{
//												//@startDate.ToString("MMMM")<text> </text>@startDate.Day<text> - </text>@endDate.Day<text>, </text>@startDate.Year
//												conferenceDates = conference.start.ToString("MMMM d") + " - " + conference.end.Day + ", " + conference.end.Year;
//										}
//										else
//										{
//												conferenceDates = conference.start.ToString("MMMM d") + " - " + conference.end.ToString("MMMM d") + ", " + conference.end.Year;
//										}
//								}
//								else
//								{
//										conferenceDates = conference.start.ToString("MMMM d, yyyy") + " - " + conference.end.ToString("MMMM d, yyyy");
//								}
//						}

//						return conferenceDates;
//				}
//		}
//}