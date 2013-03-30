using System;
using System.Linq;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;
using TekConf.Common.Entities;

namespace TekConf.UI.Api
{
	public class GenericRepository<T> : IRepository<T> where T: class
	{
		private readonly IConfiguration _configuration;
		private string dbName = typeof(T).ToGenericTypeString();

		public GenericRepository(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public void Save(T entity)
		{
			var collection = MongoCollection();
			collection.Save(entity);
		}

		public IQueryable<T> AsQueryable()
		{
			var collection = MongoCollection();
			return collection.AsQueryable();
		}

		public void Remove(Guid id)
		{
			var collection = this.LocalDatabase.GetCollection<T>(dbName);
			collection.Remove(Query.EQ("_id", id));
		}

		private MongoCollection<T> MongoCollection()
		{
			var collection = this.LocalDatabase.GetCollection<T>(dbName);
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