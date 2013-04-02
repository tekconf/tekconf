using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;
using TekConf.UI.Api;

namespace TekConf.Common.Entities.Repositories
{
    public interface IGeoLocationRepository : IRepository<GeoLocationEntity>
    {
        
    }

    public class GeoLocationRepository : IGeoLocationRepository
	{
		private readonly IConfiguration _configuration;

		public GeoLocationRepository(IConfiguration configuration)
		{
			_configuration = configuration;
			CreateIndexes();
		}

		public void Save(GeoLocationEntity entity)
		{
			var collection = MongoCollection();
			collection.Save(entity);
		}

		public IQueryable<GeoLocationEntity> AsQueryable()
		{
			var collection = MongoCollection();
			return collection.AsQueryable();
		}

		private void CreateIndexes()
		{
			var collection = this.LocalDatabase.GetCollection<GeoLocationEntity>("cities");
			collection.EnsureIndex(new string[] { "countrycode", "name", "latitude", "longitude" });
			collection.EnsureIndex(new string[] { "latitude", "longitude" });
			collection.EnsureIndex(new string[] { "name" });

		}
		public void Remove(Guid id)
		{
			var collection = this.LocalDatabase.GetCollection<GeoLocationEntity>("cities");
			collection.Remove(Query.EQ("_id", id));
		}

		private MongoCollection<GeoLocationEntity> MongoCollection()
		{
			var collection = this.LocalDatabase.GetCollection<GeoLocationEntity>("cities");
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
					_localDatabase = _localServer.GetDatabase("geolocation");

				}
				return _localDatabase;
			}
		}
	}
}
