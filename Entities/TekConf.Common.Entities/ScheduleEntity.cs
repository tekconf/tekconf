using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace TekConf.UI.Api
{
  public class ScheduleEntity
  {
    [BsonId(IdGenerator = typeof (CombGuidGenerator))]
    public Guid _id { get; set; }

    public string ConferenceSlug { get; set; }
    public string UserSlug { get; set; }
    public string AuthenticationToken { get; set; }
    public string AuthenticationMethod { get; set; }
    public List<string> SessionSlugs { get; set; } 
  }
}