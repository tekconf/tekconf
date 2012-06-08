using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace ArtekSoftware.Conference.UI.Web
{
    public class Conference
    {
        [BsonId(IdGenerator = typeof(CombGuidGenerator))]
        public Guid _id { get; set; }
        public string description { get; set; }
        public string facebookUrl { get; set; }
        public string slug { get; set; }
        public string homepageUrl { get; set; }
        public string lanyrdUrl { get; set; }
        public string location { get; set; }
        public string meetupUrl { get; set; }
        public string name { get; set; }
        public object start { get; set; }
        public object end { get; set; }
        public string twitterHashTag { get; set; }
        public string twitterName { get; set; }
        public List<Session> sessions { get; set; }

    }
}