using System;
using System.Linq;
using FluentMongo.Linq;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace TekConf.UI.Api
{
	public class ConferenceSavedRepository : IRepository<ConferenceSavedMessage>
	{
		private readonly IConfiguration _configuration;

		public ConferenceSavedRepository(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public void Save(ConferenceSavedMessage entity)
		{
			var collection = MongoCollection();
			collection.Save(entity);
		}

		public IQueryable<ConferenceSavedMessage> AsQueryable()
		{
			var collection = MongoCollection();
			return collection.AsQueryable();
		}

		public void Remove(Guid id)
		{
			var collection = this.LocalDatabase.GetCollection<ConferenceSavedMessage>("conferenceSavedEvents");
			collection.Remove(Query.EQ("_id", id));
		}

		private MongoCollection<ConferenceSavedMessage> MongoCollection()
		{
			var collection = this.LocalDatabase.GetCollection<ConferenceSavedMessage>("conferenceSavedEvents");
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