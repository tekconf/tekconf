using System;
using System.Linq;

using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;

namespace TekConf.Common.Entities
{
	public class ConferenceCreatedRepository : IRepository<ConferenceCreatedMessage>
	{
		private readonly IEntityConfiguration _entityConfiguration;

		public ConferenceCreatedRepository(IEntityConfiguration entityConfiguration)
		{
			this._entityConfiguration = entityConfiguration;
		}

		public void Save(ConferenceCreatedMessage entity)
		{
			var collection = MongoCollection();
			collection.Save(entity);
		}

		public IQueryable<ConferenceCreatedMessage> AsQueryable()
		{
			var collection = MongoCollection();
			return collection.AsQueryable();
		}

		public void Remove(Guid id)
		{
			var collection = this.LocalDatabase.GetCollection<ConferenceCreatedMessage>("conferenceCreatedEvents");
			collection.Remove(Query.EQ("_id", id));
		}

		private MongoCollection<ConferenceCreatedMessage> MongoCollection()
		{
			var collection = this.LocalDatabase.GetCollection<ConferenceCreatedMessage>("conferenceCreatedEvents");
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