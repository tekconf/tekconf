using System;
using System.Collections.Generic;
using ServiceStack.ServiceHost;
using TekConf.RemoteData.Dtos.v1;

namespace TekConf.UI.Api.Services.Requests.v1
{
	[ServiceStack.ServiceHost.Api("Conferences Service Description")]
	[Route("/v1/conferences/count", "GET", Summary = "Gets the total count of conferences returned.")]
	public class ConferencesCount : IReturn<int>
	{
		[ApiMember(Name = "showPastConferences", Description = "Set to true to show conferences which occurred in the past.", ParameterType = "query", DataType = "boolean", IsRequired = false)]
		public bool? showPastConferences { get; set; }
		[ApiMember(Name = "searchTerm", Description = "Search conference name, description, city, country, session title, session description, speaker name, or speaker twitter handle.", ParameterType = "query", DataType = "string", IsRequired = false)]
		public string searchTerm { get; set; }
	}

	[Route("/v1/conferences/search", "GET")]
	public class Search : IReturn<List<SearchResultDto>>
	{
		[ApiMember(Name = "showPastConferences", Description = "Set to true to show conferences which occurred in the past.", ParameterType = "query", DataType = "boolean", IsRequired = false)]
		public bool? showPastConferences { get; set; }
		
		[ApiMember(Name = "searchTerm", Description = "Search conference name, description, city, country, session title, session description, speaker name, or speaker twitter handle.", ParameterType = "query", DataType = "string", IsRequired = false)]
		public string searchTerm { get; set; }

		[ApiMember(Name = "longitude", Description = "Search for conferences near a lat/long pair.", ParameterType = "query", DataType = "double", IsRequired = false)]
		public double? longitude { get; set; }

		[ApiMember(Name = "latitude", Description = "Search for conferences near a lat/long pair.", ParameterType = "query", DataType = "double", IsRequired = false)]
		public double? latitude { get; set; }

		[ApiMember(Name = "distance", Description = "Search for conferences in a distance (miles) radius near a lat/long pair or city/state/country.", ParameterType = "query", DataType = "double", IsRequired = false)]
		public double? distance { get; set; }

		[ApiMember(Name = "city", Description = "Search for conferences near a city/state/country.", ParameterType = "query", DataType = "string", IsRequired = false)]
		public string city { get; set; }

		[ApiMember(Name = "state", Description = "Search for conferences near a city/state/country.", ParameterType = "query", DataType = "string", IsRequired = false)]
		public string state { get; set; }

		[ApiMember(Name = "country", Description = "Search for conferences near a city/state/country. Defaults distance to 100 miles.", ParameterType = "query", DataType = "string", IsRequired = false)]
		public string country { get; set; }
	}

	[Route("/v1/conferences", "GET")]
	public class Conferences : IReturn<List<ConferencesDto>>
	{
		[ApiMember(Name = "search", Description = "Search conference name, description, city, country, session title, session description, speaker name, or speaker twitter handle.", ParameterType = "query", DataType = "string", IsRequired = false)]
		public string search { get; set; }
		
		[ApiMember(Name = "sortBy", Description = "Search conference name, description, city, country, session title, session description, speaker name, or speaker twitter handle.", ParameterType = "query", DataType = "string", IsRequired = false)]
		public string sortBy { get; set; }
		
		[ApiMember(Name = "showPastConferences", Description = "Sort results by start date, name, call for speakers opening date, call for speakers closing date, registration opens date, or date added.", ParameterType = "query", DataType = "boolean", IsRequired = false)]
		public bool? showPastConferences { get; set; }
		
		[ApiMember(Name = "showOnlyWithOpenCalls", Description = "Set to true to show conferences which have an open call for speakers.", ParameterType = "query", DataType = "boolean", IsRequired = false)]
		public bool? showOnlyWithOpenCalls { get; set; }
		
		[ApiMember(Name = "showOnlyOnSale", Description = "Set to true to show conferences which are currently on sale.", ParameterType = "query", DataType = "boolean", IsRequired = false)]
		public bool? showOnlyOnSale { get; set; }
		
		[ApiMember(Name = "showOnlyFeatured", Description = "Set to true to show conferences which are featured.", ParameterType = "query", DataType = "boolean", IsRequired = false)]
		public bool showOnlyFeatured { get; set; }

		[ApiMember(Name = "longitude", Description = "Search for conferences near a lat/long pair.", ParameterType = "query", DataType = "double", IsRequired = false)]
		public double? longitude { get; set; }

		[ApiMember(Name = "latitude", Description = "Search for conferences near a lat/long pair.", ParameterType = "query", DataType = "double", IsRequired = false)]
		public double? latitude { get; set; }

		[ApiMember(Name = "distance", Description = "Search for conferences in a distance (miles) radius near a lat/long pair or city/state/country.", ParameterType = "query", DataType = "double", IsRequired = false)]
		public double? distance { get; set; }

		[ApiMember(Name = "city", Description = "Search for conferences near a city/state/country.", ParameterType = "query", DataType = "string", IsRequired = false)]
		public string city { get; set; }

		[ApiMember(Name = "state", Description = "Search for conferences near a city/state/country.", ParameterType = "query", DataType = "string", IsRequired = false)]
		public string state { get; set; }

		[ApiMember(Name = "country", Description = "Search for conferences near a city/state/country. Defaults distance to 100 miles.", ParameterType = "query", DataType = "string", IsRequired = false)]
		public string country { get; set; }
	}

	[Route("/v1/conferences/{conferenceSlug}", "GET")]
	public class Conference : IReturn<FullConferenceDto>
	{
		[ApiMember(Name = "conferenceSlug", Description = "The unique slug to identify the conference.", ParameterType = "query", DataType = "string", IsRequired = true)]
		public string conferenceSlug { get; set; }
		[ApiMember(Name = "userName", Description = "The requesting user's login name. This is used to determine which sessions are on their schedule.", ParameterType = "query", DataType = "string", IsRequired = false)]
		public string userName { get; set; }
	}

	[Route("/v1/conferences/{slug}", "POST")]
	[Route("/v1/conferences/{slug}", "PUT")]
	public class CreateConference : IReturn<FullConferenceDto>
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
		public Address address { get; set; }
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

		public List<string> tags { get; set; }
		public List<string> subjects { get; set; }

	}
}