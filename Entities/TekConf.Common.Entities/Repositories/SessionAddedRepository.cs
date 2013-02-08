using System;
using System.Linq;

using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;

namespace TekConf.UI.Api
{
	public class SessionAddedRepository : IRepository<SessionAddedMessage>
	{
		private readonly IConfiguration _configuration;

		public SessionAddedRepository(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public void Save(SessionAddedMessage entity)
		{
			var collection = MongoCollection();
			collection.Save(entity);
		}

		public IQueryable<SessionAddedMessage> AsQueryable()
		{
			var collection = MongoCollection();
			return collection.AsQueryable();
		}

		public void Remove(Guid id)
		{
			var collection = this.LocalDatabase.GetCollection<SessionAddedMessage>("sessionAddedEvents");
			collection.Remove(Query.EQ("_id", id));
		}

		private MongoCollection<SessionAddedMessage> MongoCollection()
		{
			var collection = this.LocalDatabase.GetCollection<SessionAddedMessage>("sessionAddedEvents");
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