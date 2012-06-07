using System;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace ArtekSoftware.Conference.UI.Web
{
    public class Speaker
    {
        [BsonId(IdGenerator = typeof(CombGuidGenerator))]
        public Guid _id { get; set; }
        public string slug { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
    }
}