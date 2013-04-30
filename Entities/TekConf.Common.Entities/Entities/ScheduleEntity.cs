using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace TekConf.Common.Entities
{
  public class ScheduleEntity : IEntity
  {
    [BsonId(IdGenerator = typeof (CombGuidGenerator))]
    public Guid _id { get; set; }

    public string ConferenceSlug { get; set; }
		public string UserName { get; set; }
    public List<string> SessionSlugs { get; set; } 
  }
}