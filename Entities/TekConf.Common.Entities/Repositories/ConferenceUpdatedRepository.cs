using System;
using System.Linq;

using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;

namespace TekConf.Common.Entities
{
	public class ConferenceUpdatedRepository : IRepository<ConferenceUpdatedMessage>
	{
		private readonly IEntityConfiguration _entityConfiguration;

		public ConferenceUpdatedRepository(IEntityConfiguration entityConfiguration)
		{
			this._entityConfiguration = entityConfiguration;
		}

		public void Save(ConferenceUpdatedMessage entity)
		{
			var collection = MongoCollection();
			collection.Save(entity);
		}

		public IQueryable<ConferenceUpdatedMessage> AsQueryable()
		{
			var collection = MongoCollection();
			return collection.AsQueryable();
		}

		public void Remove(Guid id)
		{
			var collection = this.LocalDatabase.GetCollection<ConferenceUpdatedMessage>("conferenceSavedEvents");
			collection.Remove(Query.EQ("_id", id));
		}

		private MongoCollection<ConferenceUpdatedMessage> MongoCollection()
		{
			var collection = this.LocalDatabase.GetCollection<ConferenceUpdatedMessage>("conferenceSavedEvents");
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