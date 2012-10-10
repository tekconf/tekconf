using System;
using System.Collections.Generic;

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
        
        public List<FullSessionDto> sessions { get; set; }

    }
}