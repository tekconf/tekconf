using System;
using System.Linq;

using MongoDB.Driver.Linq;

namespace TekConf.Common.Entities.Repositories
{
	using MongoDB.Driver;
	using MongoDB.Driver.Builders;

	public class UserRepository : IRepository<UserEntity>
		{
				private readonly IEntityConfiguration _entityConfiguration;

				public UserRepository(IEntityConfiguration entityConfiguration)
				{
						this._entityConfiguration = entityConfiguration;
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
