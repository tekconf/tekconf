using System;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace ArtekSoftware.Conference.UI.Web
{
  public class SpeakerEntity
  {
    [BsonId(IdGenerator = typeof(CombGuidGenerator))]
    public Guid _id { get; set; }
    public string slug { get; set; }
    public string firstName { get; set; }
    public string lastName { get; set; }
    public string conferenceSlug { get; set; } //TODO
    public string sessionSlug { get; set; } //TODO
  }
}