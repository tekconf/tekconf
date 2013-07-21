using System;
using System.Collections.Generic;
using ServiceStack.ServiceHost;
using TekConf.RemoteData.Dtos.v1;

#if (MONOTOUCH)
namespace ServiceStack.ServiceHost
{
	public class Api : Attribute
	{
public Api (string description)
	{

	}
	}

	public class ApiMember : Attribute
	{
public string Name {get;set;}
public string Description {get;set;}
public string ParameterType {get;set;}
public string DataType {get;set;}
public bool IsRequired {get;set;}
	}

}
#endif

namespace TekConf.UI.Api.Services.Requests.v1
{
	[ServiceStack.ServiceHost.Api("Conferences Service Description")]
	[Route("/v1/conferences/count", "GET", Summary = "Gets the total count of conferences returned.")]
	public class ConferencesCount : IReturn<int>
	{
		[ApiMember(Name = "showPastConferences", Description = "Set to true to show conferences which occurred in the past.", ParameterType = "query", DataType = "bool", IsRequired = false)]
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
	public class Conferences : IReturn<List<FullConferenceDto>>
	{
		[ApiMember(Name = "userName", Description = "The requesting user's login name. This is used to determine which conferences are on their schedule.", ParameterType = "query", DataType = "string", IsRequired = false)]
		public string userName { get; set; }

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
		public CreateConference()
		{
			if (this.defaultTalkLength == default(int))
				defaultTalkLength = 60;
		}

		[ApiMember(Name = "slug", Description = "XXXX", ParameterType = "query", DataType = "string", IsRequired = false)]
		public string slug { get; set; }

		[ApiMember(Name = "name", Description = "XXXX", ParameterType = "query", DataType = "string", IsRequired = false)]
		public string name { get; set; }
		
		[ApiMember(Name = "start", Description = "XXXX", ParameterType = "query", DataType = "DateTime", IsRequired = false)]
		public DateTime start { get; set; }
		
		[ApiMember(Name = "end", Description = "XXXX", ParameterType = "query", DataType = "DateTime", IsRequired = false)]
		public DateTime end { get; set; }
		
		[ApiMember(Name = "callForSpeakersOpens", Description = "XXXX", ParameterType = "query", DataType = "DateTime", IsRequired = false)]
		public DateTime callForSpeakersOpens { get; set; }
		
		[ApiMember(Name = "callForSpeakersCloses", Description = "XXXX", ParameterType = "query", DataType = "DateTime", IsRequired = false)]
		public DateTime callForSpeakersCloses { get; set; }
		
		[ApiMember(Name = "registrationOpens", Description = "XXXX", ParameterType = "query", DataType = "DateTime", IsRequired = false)]
		public DateTime registrationOpens { get; set; }
		
		[ApiMember(Name = "registrationCloses", Description = "XXXX", ParameterType = "query", DataType = "DateTime", IsRequired = false)]
		public DateTime registrationCloses { get; set; }
		
		[ApiMember(Name = "description", Description = "XXXX", ParameterType = "query", DataType = "string", IsRequired = false)]
		public string description { get; set; }
		
		[ApiMember(Name = "location", Description = "XXXX", ParameterType = "query", DataType = "string", IsRequired = false)]
		public string location { get; set; }

		[ApiMember(Name = "isOnline", Description = "XXXX", ParameterType = "query", DataType = "bool", IsRequired = false)]
		public bool isOnline { get; set; }

		[ApiMember(Name = "address", Description = "XXXX", ParameterType = "query", DataType = "Address", IsRequired = false)]
		public Address address { get; set; }
		
		[ApiMember(Name = "tagline", Description = "XXXX", ParameterType = "query", DataType = "string", IsRequired = false)]
		public string tagline { get; set; }
		
		[ApiMember(Name = "imageUrl", Description = "XXXX", ParameterType = "query", DataType = "string", IsRequired = false)]
		public string imageUrl { get; set; }
		
		[ApiMember(Name = "isLive", Description = "XXXX", ParameterType = "query", DataType = "bool", IsRequired = false)]
		public bool isLive { get; set; }
		
		[ApiMember(Name = "latitude", Description = "XXXX", ParameterType = "query", DataType = "double", IsRequired = false)]
		public double latitude { get; set; }
		
		[ApiMember(Name = "longitude", Description = "XXXX", ParameterType = "query", DataType = "double", IsRequired = false)]
		public double longitude { get; set; }

		[ApiMember(Name = "defaultTalkLength", Description = "XXXX", ParameterType = "query", DataType = "int", IsRequired = false)]
		public int defaultTalkLength { get; set; }

		[ApiMember(Name = "facebookUrl", Description = "XXXX", ParameterType = "query", DataType = "string", IsRequired = false)]
		public string facebookUrl { get; set; }
		
		[ApiMember(Name = "homepageUrl", Description = "XXXX", ParameterType = "query", DataType = "string", IsRequired = false)]
		public string homepageUrl { get; set; }
		
		[ApiMember(Name = "lanyrdUrl", Description = "XXXX", ParameterType = "query", DataType = "string", IsRequired = false)]
		public string lanyrdUrl { get; set; }
		
		[ApiMember(Name = "meetupUrl", Description = "XXXX", ParameterType = "query", DataType = "string", IsRequired = false)]
		public string meetupUrl { get; set; }
		
		[ApiMember(Name = "googlePlusUrl", Description = "XXXX", ParameterType = "query", DataType = "string", IsRequired = false)]
		public string googlePlusUrl { get; set; }
		
		[ApiMember(Name = "vimeoUrl", Description = "XXXX", ParameterType = "query", DataType = "string", IsRequired = false)]
		public string vimeoUrl { get; set; }
		
		[ApiMember(Name = "youtubeUrl", Description = "XXXX", ParameterType = "query", DataType = "string", IsRequired = false)]
		public string youtubeUrl { get; set; }
		
		[ApiMember(Name = "githubUrl", Description = "XXXX", ParameterType = "query", DataType = "string", IsRequired = false)]
		public string githubUrl { get; set; }
		
		[ApiMember(Name = "linkedInUrl", Description = "XXXX", ParameterType = "query", DataType = "string", IsRequired = false)]
		public string linkedInUrl { get; set; }
		
		[ApiMember(Name = "twitterHashTag", Description = "XXXX", ParameterType = "query", DataType = "string", IsRequired = false)]
		public string twitterHashTag { get; set; }
		
		[ApiMember(Name = "twitterName", Description = "XXXX", ParameterType = "query", DataType = "string", IsRequired = false)]
		public string twitterName { get; set; }

		[ApiMember(Name = "tags", Description = "XXXX", ParameterType = "query", DataType = "string", IsRequired = false)]
		public List<string> tags { get; set; }
		
		[ApiMember(Name = "subjects", Description = "XXXX", ParameterType = "query", DataType = "string", IsRequired = false)]
		public List<string> subjects { get; set; }

		[ApiMember(Name = "rooms", Description = "XXXX", ParameterType = "query", DataType = "string", IsRequired = false)]
		public List<string> rooms { get; set; }

		[ApiMember(Name = "sessionTypes", Description = "XXXX", ParameterType = "query", DataType = "string", IsRequired = false)]
		public List<string> sessionTypes { get; set; }

	}
}
