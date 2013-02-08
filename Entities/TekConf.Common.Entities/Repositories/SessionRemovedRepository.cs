using System;
using System.Linq;

using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;

namespace TekConf.UI.Api
{
	public class SessionRemovedRepository : IRepository<SessionRemovedMessage>
	{
		private readonly IConfiguration _configuration;

		public SessionRemovedRepository(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public void Save(SessionRemovedMessage entity)
		{
			var collection = MongoCollection();
			collection.Save(entity);
		}

		public IQueryable<SessionRemovedMessage> AsQueryable()
		{
			var collection = MongoCollection();
			return collection.AsQueryable();
		}

		public void Remove(Guid id)
		{
			var collection = this.LocalDatabase.GetCollection<SessionRemovedMessage>("sessionRemovedEvents");
			collection.Remove(Query.EQ("_id", id));
		}

		private MongoCollection<SessionRemovedMessage> MongoCollection()
		{
			var collection = this.LocalDatabase.GetCollection<SessionRemovedMessage>("sessionRemovedEvents");
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