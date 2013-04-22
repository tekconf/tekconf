using System;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace TekConf.Common.Entities
{
	public class SubscriptionEntity : IEntity
	{
		[BsonId(IdGenerator = typeof(CombGuidGenerator))]
		public Guid _id { get; set; }

		public string EmailAddress { get; set; }
	}
}