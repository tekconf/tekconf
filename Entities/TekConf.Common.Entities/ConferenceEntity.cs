using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace TekConf.UI.Api
{
    public class ConferenceEntity
    {
        [BsonId(IdGenerator = typeof(CombGuidGenerator))]
        public Guid _id { get; set; }

        public string name { get; set; }
        public string slug { get; set; }
        public DateTime start { get; set; }
        public DateTime end { get; set; }
        public DateTime callForSpeakersOpens { get; set; }
        public DateTime callForSpeakersCloses { get; set; }
        public DateTime registrationOpens { get; set; }
        public DateTime registrationCloses { get; set; }

        public string location { get; set; }
        public AddressEntity address { get; set; }
        public string description { get; set; }

        public string imageUrl { get; set; }
        public string tagLine { get; set; }
        public List<SessionEntity> sessions { get; set; }

        public string facebookUrl { get; set; }
        public string homepageUrl { get; set; }
        public string lanyrdUrl { get; set; }
        public string twitterHashTag { get; set; }
        public string twitterName { get; set; }
        public string meetupUrl { get; set; }
        public string googlePlusUrl { get; set; }
        public string vimeoUrl { get; set; }
        public string youtubeUrl { get; set; }
        public string githubUrl { get; set; }
        public string linkedInUrl { get; set; }
    }
}