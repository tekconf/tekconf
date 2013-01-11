using System;
using System.Linq;
using FluentMongo.Linq;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace TekConf.UI.Api
{
	public class ConferenceLocationChangedRepository : IRepository<ConferenceLocationChangedMessage>
	{
		private readonly IConfiguration _configuration;

		public ConferenceLocationChangedRepository(IConfiguration configuration)
		{
			_configuration = configuration;
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