using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TekConf.Common.Entities.Repositories
{
	using FluentMongo.Linq;

	using MongoDB.Driver;
	using MongoDB.Driver.Builders;

	using TekConf.UI.Api;

	public class UserRepository : IRepository<UserEntity>
		{
				private readonly IConfiguration _configuration;

				public UserRepository(IConfiguration configuration)
				{
						_configuration = configuration;
				}

				public void Save(UserEntity entity)
				{
						var collection = MongoCollection();
						collection.Save(entity);
				}

				public IQueryable<UserEntity> AsQueryable()
				{
						var collection = MongoCollection();
						return collection.AsQueryable();
				}

				public void Remove(Guid id)
				{
						var collection = this.LocalDatabase.GetCollection<UserEntity>("users");
						collection.Remove(Query.EQ("_id", id));
				}

				private MongoCollection<UserEntity> MongoCollection()
				{
						var collection = this.LocalDatabase.GetCollection<UserEntity>("users");
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
