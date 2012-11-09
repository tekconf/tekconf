using System;
using System.Collections.Generic;
using ServiceStack.ServiceHost;
using TekConf.RemoteData.Dtos.v1;

namespace TekConf.UI.Api.Services.Requests.v1
{

    [Route("/v1/conferences/{conferenceSlug}/sessions", "GET")]
    public class Sessions : IReturn<List<SessionsDto>>
    {
        public string conferenceSlug { get; set; }
    }

    [Route("/v1/conferences/{conferenceSlug}/sessions/{sessionSlug}", "GET")]
    public class Session : IReturn<SessionDto>
    {
        public string conferenceSlug { get; set; }
        public string sessionSlug { get; set; }
    }

    [Route("/v1/conferences/{conferenceSlug}/sessions/{slug}", "POST")]
    [Route("/v1/conferences/{conferenceSlug}/sessions/{slug}", "PUT")]
    public class AddSession : IReturn<SessionDto>
    {
        public string slug { get; set; }
        public string conferenceSlug { get; set; }
        public string title { get; set; }
        public DateTime start { get; set; }
        public DateTime end { get; set; }
        public string room { get; set; }
        public string difficulty { get; set; }
        public string description { get; set; }
        public string twitterHashTag { get; set; }
        public string sessionType { get; set; }
        public List<string> links { get; set; }
        public List<string> tags { get; set; }
        public List<string> subjects { get; set; }
        public List<string> resources { get; set; }
        public List<string> prerequisites { get; set; }
    }
}