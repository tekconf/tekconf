using System;
using System.Linq;
using FluentMongo.Linq;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace TekConf.UI.Api
{
	public class ConferenceRepository : IRepository<ConferenceEntity>
	{
		private readonly IConfiguration _configuration;

		public ConferenceRepository(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public void Save(ConferenceEntity entity)
		{
			var collection = MongoCollection();
			collection.Save(entity);
		}

		public IQueryable<ConferenceEntity> AsQueryable()
		{
			var collection = MongoCollection();
			return collection.AsQueryable();
		}

		public void Remove(Guid id)
		{
			var collection = this.LocalDatabase.GetCollection<ConferenceEntity>("conferences");
			collection.Remove(Query.EQ("_id", id));
		}

		private MongoCollection<ConferenceEntity> MongoCollection()
		{
			var collection = this.LocalDatabase.GetCollection<ConferenceEntity>("conferences");
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