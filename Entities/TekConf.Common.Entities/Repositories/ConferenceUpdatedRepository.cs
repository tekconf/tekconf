using System;
using System.Linq;
using FluentMongo.Linq;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace TekConf.UI.Api
{
	public class ConferenceUpdatedRepository : IRepository<ConferenceUpdatedMessage>
	{
		private readonly IConfiguration _configuration;

		public ConferenceUpdatedRepository(IConfiguration configuration)
		{
			_configuration = configuration;
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
					var mongoServer = _configuration.MongoServer;
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