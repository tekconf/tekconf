using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Plugins.Sqlite;
using TekConf.RemoteData.Dtos.v1;

namespace TekConf.Core.Entities
{
	public class ConferenceEntity
	{
		public ConferenceEntity()
		{

		}

		public ConferenceEntity(FullConferenceDto conference)
		{
			Slug = conference.slug;
			CallForSpeakersCloses = conference.callForSpeakersCloses;
			CallForSpeakersOpens = conference.callForSpeakersOpens;
			DefaultTalkLength = conference.defaultTalkLength;
			Description = conference.description;
			End = conference.end;
			FacebookUrl = conference.facebookUrl;
			GithubUrl = conference.githubUrl;
			GooglePlusUrl = conference.googlePlusUrl;
			HomepageUrl = conference.homepageUrl;
			ImageUrl = conference.imageUrl;
			IsAddedToSchedule = conference.isAddedToSchedule.HasValue && conference.isAddedToSchedule.Value;
			IsLive = conference.isLive;
			IsOnline = conference.isOnline;
			LanyrdUrl = conference.lanyrdUrl;
			LinkedInUrl = conference.linkedInUrl;
			Location = conference.location;
			MeetupUrl = conference.meetupUrl;
			Name = conference.name;
			NumberOfSessions = conference.numberOfSessions;
			RegistrationCloses = conference.registrationCloses;
			RegistrationOpens = conference.registrationOpens;
			Start = conference.start;
			Tagline = conference.tagline;
			TwitterHashTag = conference.twitterHashTag;
			TwitterName = conference.twitterName;
			VimeoUrl = conference.vimeoUrl;
			YoutubeUrl = conference.youtubeUrl;
			
			if (conference.address != null)
			{
				StreetDirection = conference.address.StreetDirection;
				StreetName = conference.address.StreetName;
				StreetNumber = conference.address.StreetNumber;
				BuildingName = conference.address.BuildingName;
				StreetNumberSuffix = conference.address.StreetNumberSuffix;
				StreetType = conference.address.StreetType;
				AddressType = conference.address.AddressType;
				AddressTypeId = conference.address.AddressTypeId;
				LocalMunicipality = conference.address.LocalMunicipality;
				City = conference.address.City;
				State = conference.address.State;
				GoverningDistrict = conference.address.GoverningDistrict;
				PostalArea = conference.address.PostalArea;
				Country = conference.address.Country;
			}

			if (conference.position != null && conference.position.Length == 2)
			{
				Longitude = conference.position[0];
				Latitude = conference.position[1];
			}
		}

		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }
		public string Slug { get; set; }
		public string Name { get; set; }
		public DateTime Start { get; set; }
		public DateTime End { get; set; }
		public DateTime CallForSpeakersOpens { get; set; }
		public DateTime CallForSpeakersCloses { get; set; }
		public DateTime RegistrationOpens { get; set; }
		public DateTime RegistrationCloses { get; set; }
		public string Description { get; set; }
		public string Location { get; set; }
		public string Tagline { get; set; }
		public string ImageUrl { get; set; }
		public bool IsLive { get; set; }
		public string FacebookUrl { get; set; }
		public string HomepageUrl { get; set; }
		public string LanyrdUrl { get; set; }
		public string MeetupUrl { get; set; }
		public string GooglePlusUrl { get; set; }
		public string VimeoUrl { get; set; }
		public string YoutubeUrl { get; set; }
		public string GithubUrl { get; set; }
		public string LinkedInUrl { get; set; }
		public string TwitterHashTag { get; set; }
		public string TwitterName { get; set; }
		public int DefaultTalkLength { get; set; }
		public int NumberOfSessions { get; set; }
		public bool IsAddedToSchedule { get; set; }
		public bool? IsOnline { get; set; }

		public int StreetNumber { get; set; }
		public string BuildingName { get; set; }
		public string StreetNumberSuffix { get; set; }
		public string StreetName { get; set; }
		public string StreetType { get; set; }
		public string StreetDirection { get; set; }
		public string AddressType { get; set; }
		public string AddressTypeId { get; set; }
		public string LocalMunicipality { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public string GoverningDistrict { get; set; }
		public string PostalArea { get; set; }
		public string Country { get; set; }
		public double Longitude { get; set; }
		public double Latitude { get; set; }

		public IEnumerable<SessionEntity> Sessions(ISQLiteConnection connection)
		{
			var sessions =  connection.Table<SessionEntity>().Where(x => x.ConferenceId == this.Id).ToList();
			return sessions.AsEnumerable();
		}

		//public double[] Position { get; set; }
		//public List<string> Rooms { get; set; }
		//public List<string> SessionTypes { get; set; }
		//public List<string> Subjects { get; set; }
		//public List<string> Tags { get; set; }

		public string DateRange()
		{

			string range;
			if (Start == default(DateTime) || End == default(DateTime))
			{
				range = "No Date Set";
			}
			else if (Start.Month == End.Month && Start.Year == End.Year)
			{
				// They begin and end in the same month
				if (Start.Date == End.Date)
				{
					range = Start.ToString("MMMM") + " " + Start.Day + ", " + Start.Year;
				}
				else
					range = Start.ToString("MMMM") + " " + Start.Day + " - " + End.Day + ", " + Start.Year;
			}
			else
			{
				// They begin and end in different months
				if (Start.Year == End.Year)
				{
					range = Start.ToString("MMMM") + " " + Start.Day + " - " + End.ToString("MMMM") + " " + End.Day + ", " + Start.Year;
				}
				else
				{
					range = Start.ToString("MMMM") + " " + Start.Day + ", " + Start.Year + " - " + End.ToString("MMMM") + " " + End.Day + ", " + End.Year;
				}

			}

			return range;
		}

		public string FormattedCity()
		{
			string formattedAddress;
			if (IsOnline == true)
			{
				formattedAddress = "online";
			}
			else if (!string.IsNullOrWhiteSpace(City) && !string.IsNullOrWhiteSpace(State))
			{
				formattedAddress = City + ", " + State;
			}
			else if (!string.IsNullOrWhiteSpace(City) && !string.IsNullOrWhiteSpace(Country))
			{
				formattedAddress = City + ", " + Country;
			}
			else if (!string.IsNullOrWhiteSpace(City))
			{
				formattedAddress = City;
			}
			else
			{
				formattedAddress = "No location set";
			}

			return formattedAddress;
		}

		public string FormattedAddress()
		{
			string formattedAddress;
			if (IsOnline == true)
			{
				formattedAddress = "online";
			}
			else if (StreetNumber != default(int) && !string.IsNullOrWhiteSpace(StreetName) && !string.IsNullOrWhiteSpace(City) &&
			         !string.IsNullOrWhiteSpace(State))
			{
				formattedAddress = StreetNumber + " " + StreetName + "\n" + City + ", " + State + " " + PostalArea;
			}
			else if (!string.IsNullOrWhiteSpace(City) && !string.IsNullOrWhiteSpace(State))
			{
				formattedAddress = City + ", " + State;
			}
			else if (!string.IsNullOrWhiteSpace(City) && !string.IsNullOrWhiteSpace(Country))
			{
				formattedAddress = City + ", " + Country;
			}
			else if (!string.IsNullOrWhiteSpace(City))
			{
				formattedAddress = City;
			}
			else
			{
				formattedAddress = "No location set";
			}

			return formattedAddress;
		}

		public override bool Equals(object conference)
		{
			return Equals(conference as ConferenceEntity);
		}

		public bool Equals(ConferenceEntity conference)
		{
			return conference != null && this.Slug == conference.Slug;
		}

		public override int GetHashCode()
		{
			return this.Slug.GetHashCode();
		}
	}
}
