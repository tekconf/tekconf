using System;
using System.Linq;

using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;

namespace TekConf.Common.Entities
{
	public class SpeakerAddedRepository : IRepository<SpeakerAddedMessage>
	{
		private readonly IEntityConfiguration _entityConfiguration;

		public SpeakerAddedRepository(IEntityConfiguration entityConfiguration)
		{
			this._entityConfiguration = entityConfiguration;
		}

		public void Save(SpeakerAddedMessage entity)
		{
			var collection = MongoCollection();
			collection.Save(entity);
		}

		public IQueryable<SpeakerAddedMessage> AsQueryable()
		{
			var collection = MongoCollection();
			return collection.AsQueryable();
		}

		public void Remove(Guid id)
		{
			var collection = this.LocalDatabase.GetCollection<SpeakerAddedMessage>("speakerAddedEvents");
			collection.Remove(Query.EQ("_id", id));
		}

		private MongoCollection<SpeakerAddedMessage> MongoCollection()
		{
			var collection = this.LocalDatabase.GetCollection<SpeakerAddedMessage>("speakerAddedEvents");
			return collection;
		}

		private MongoServer _localServer;
		private MongoDatabase _localDatabase;
		private MongoDatabase LocalDatabase
		{
			get
			{
				if (_localServer == null)
				{
					var mongoServer = this._entityConfiguration.MongoServer;
					_localServer = MongoServer.Create(mongoServer);
				}

				if (_localDatabase == null)
				{
					_localDatabase = _localServer.GetDatabase("tekconf");

				}
				return _localDatabase;
			}
		}
	}
}