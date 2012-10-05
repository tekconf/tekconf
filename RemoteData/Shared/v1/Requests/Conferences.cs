using System;
using System.Collections.Generic;
using ServiceStack.ServiceHost;
using TekConf.RemoteData.Dtos.v1;

namespace TekConf.UI.Api.Services.Requests.v1
{
    [Route("/v1/conferences", "GET")]
    public class Conferences : IReturn<List<FullConferenceDto>>
    {
    }


    [Route("/v1/conferences/{conferenceSlug}", "GET")]
    public class Conference : IReturn<FullConferenceDto>
    {
        public string conferenceSlug { get; set; }        
    }

    [Route("/v1/conferences/{slug}", "POST")]
    [Route("/v1/conferences/{slug}", "PUT")]
    public class CreateConference : IReturn<FullConferenceDto>
    {
        public string slug { get; set; }
        
        public string name { get; set; }
        public DateTime start { get; set; }
        public DateTime end { get; set; }
        public string description { get; set; }
        public string location { get; set; }
        public string tagline { get; set; }
        public string imageUrl { get; set; }

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

    }

}