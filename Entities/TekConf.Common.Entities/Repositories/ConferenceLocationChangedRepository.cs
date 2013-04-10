using System;
using System.Linq;

using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;

namespace TekConf.Common.Entities
{
	public class ConferenceLocationChangedRepository : IRepository<ConferenceLocationChangedMessage>
	{
		private readonly IEntityConfiguration _entityConfiguration;

		public ConferenceLocationChangedRepository(IEntityConfiguration entityConfiguration)
		{
			this._entityConfiguration = entityConfiguration;
		}

		public void Save(ConferenceLocationChangedMessage entity)
		{
			var collection = MongoCollection();
			collection.Save(entity);
		}

		public IQueryable<ConferenceLocationChangedMessage> AsQueryable()
		{
			var collection = MongoCollection();
			return collection.AsQueryable();
		}

		public void Remove(Guid id)
		{
			var collection = this.LocalDatabase.GetCollection<ConferenceLocationChangedMessage>("conferenceLocationChangedEvents");
			collection.Remove(Query.EQ("_id", id));
		}

		private MongoCollection<ConferenceLocationChangedMessage> MongoCollection()
		{
			var collection = this.LocalDatabase.GetCollection<ConferenceLocationChangedMessage>("conferenceLocationChangedEvents");
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