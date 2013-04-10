using System;
using System.Linq;

using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;

namespace TekConf.Common.Entities
{
	public class ConferenceStartDateChangedRepository : IRepository<ConferenceStartDateChangedMessage>
	{
		private readonly IEntityConfiguration _entityConfiguration;

		public ConferenceStartDateChangedRepository(IEntityConfiguration entityConfiguration)
		{
			this._entityConfiguration = entityConfiguration;
		}

		public void Save(ConferenceStartDateChangedMessage entity)
		{
			var collection = MongoCollection();
			collection.Save(entity);
		}

		public IQueryable<ConferenceStartDateChangedMessage> AsQueryable()
		{
			var collection = MongoCollection();
			return collection.AsQueryable();
		}

		public void Remove(Guid id)
		{
			var collection = this.LocalDatabase.GetCollection<ConferenceStartDateChangedMessage>("conferenceStartDateChangedEvents");
			collection.Remove(Query.EQ("_id", id));
		}

		private MongoCollection<ConferenceStartDateChangedMessage> MongoCollection()
		{
			var collection = this.LocalDatabase.GetCollection<ConferenceStartDateChangedMessage>("conferenceStartDateChangedEvents");
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