using System;
using System.Linq;
using TekConf.RemoteData.Dtos.v1;

namespace TekConf.Core.Repositories
{
	public class ConferenceDetailViewDto
	{
		public ConferenceDetailViewDto()
		{
			
		}
		public ConferenceDetailViewDto(FullConferenceDto fullConference)
		{
			if (fullConference != null)
			{
				name = fullConference.name;
				DateRange = fullConference.DateRange;
				slug = fullConference.slug;
				FormattedAddress = fullConference.FormattedAddress;
				imageUrl = fullConference.imageUrl;
				description = fullConference.description;

				facebookUrl = fullConference.facebookUrl;
				homepageUrl = fullConference.homepageUrl;
				lanyrdUrl = fullConference.lanyrdUrl;
				meetupUrl = fullConference.meetupUrl;
				googlePlusUrl = fullConference.googlePlusUrl;
				vimeoUrl = fullConference.vimeoUrl;
				youtubeUrl = fullConference.youtubeUrl;
				githubUrl = fullConference.githubUrl;
				linkedInUrl = fullConference.linkedInUrl;
				twitterHashTag = fullConference.twitterHashTag;
				twitterName = fullConference.twitterName;
				hasSessions = fullConference.sessions.Any();
				isAddedToSchedule = fullConference.isAddedToSchedule;
			}
		}

		public string name { get; set; }
		public string description { get; set; }
		public string DateRange { get; set; }
		public string slug { get; set; }
		public string FormattedAddress { get; set; }
		public string imageUrl { get; set; }
		public DateTime start { get; set; }
		public bool? isAddedToSchedule { get; set; }
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
		public bool hasSessions { get; set; }
	}
}