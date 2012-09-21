using System;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace TekConf.UI.Api
{
    public class SpeakerEntity
    {
        [BsonId(IdGenerator = typeof(CombGuidGenerator))]
        public Guid _id { get; set; }
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
    }
}