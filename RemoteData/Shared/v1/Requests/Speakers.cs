using System.Collections.Generic;
using ServiceStack.ServiceHost;
using TekConf.RemoteData.Dtos.v1;

namespace TekConf.UI.Api.Services.Requests.v1
{
    [Route("/v1/conferences/{conferenceSlug}/speakers", "GET")]
    public class Speakers : IReturn<List<FullSpeakerDto>>
    {
        public string conferenceSlug { get; set; }
    }

    [Route("/v1/conferences/{conferenceSlug}/speakers/{speakerSlug}", "GET")]
    public class Speaker : IReturn<FullSpeakerDto>
    {
        public string conferenceSlug { get; set; }
        public string speakerSlug { get; set; }
    }

    [Route("/v1/conferences/{conferenceSlug}/{sessionSlug}/speakers/{slug}", "POST")]
    [Route("/v1/conferences/{conferenceSlug}/{sessionSlug}/speakers/{slug}", "PUT")]
    public class CreateSpeaker : IReturn<FullSpeakerDto>
    {
        public string conferenceSlug { get; set; }
        public string sessionSlug { get; set; }

        public string slug { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string description { get; set; }
        public string blogUrl { get; set; }
        public string twitterName { get; set; }
        public string facebookUrl { get; set; }
        public string linkedInUrl { get; set; }
        public string emailAddress { get; set; }
        public string phoneNumber { get; set; }
        public bool isFeatured { get; set; }
        public string profileImageUrl { get; set; }
        public string company { get; set; }

        public string googlePlusUrl { get; set; }
        public string vimeoUrl { get; set; }
        public string youtubeUrl { get; set; }
        public string githubUrl { get; set; }
        public string coderWallUrl { get; set; }
        public string stackoverflowUrl { get; set; }
        public string bitbucketUrl { get; set; }
        public string codeplexUrl { get; set; }
    }
}