using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace ConferencesIO.UI.Web
{
  public class ScheduleEntity
  {
    [BsonId(IdGenerator = typeof (CombGuidGenerator))]
    public Guid _id { get; set; }

    public string ConferenceSlug { get; set; }
    public string UserSlug { get; set; }
    public List<string> SessionUrls { get; set; } 
  }
}