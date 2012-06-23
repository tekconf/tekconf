using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace ArtekSoftware.Conference.UI.Web
{
  public class SessionEntities
  {
    [BsonId(IdGenerator = typeof(CombGuidGenerator))]
    public Guid _id { get; set; }
    public string slug { get; set; }
    public string conferenceSlug { get; set; }
    public string title { get; set; }
    public object start { get; set; }
    public object end { get; set; }
    public string room { get; set; }
    public string difficulty { get; set; }
    public string description { get; set; }
    public string twitterHashTag { get; set; }
    public string sessionType { get; set; }
    public List<string> links { get; set; }
    public List<string> tags { get; set; }
    public List<string> subjects { get; set; }
    public List<SpeakerEntity> speakers { get; set; }
    public List<string> resources { get; set; }
    public List<string> prerequisites { get; set; } 
  }
}