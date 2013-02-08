using System;
using System.Linq;
using MongoDB.Driver.Linq;

namespace TekConf.Common.Entities.Repositories
{
	

	using MongoDB.Driver;
	using MongoDB.Driver.Builders;

	using TekConf.UI.Api;

	public class ScheduleRepository : IRepository<ScheduleEntity>
		{
				private readonly IConfiguration _configuration;

				public ScheduleRepository(IConfiguration configuration)
				{
						_configuration = configuration;
				}

				public void Save(ScheduleEntity entity)
				{
						var collection = MongoCollection();
						collection.Save(entity);
				}

				public IQueryable<ScheduleEntity> AsQueryable()
				{
						var collection = MongoCollection();
						return collection.AsQueryable();
				}

				public void Remove(Guid id)
				{
						var collection = this.LocalDatabase.GetCollection<ScheduleEntity>("schedules");
						collection.Remove(Query.EQ("_id", id));
				}

				private MongoCollection<ScheduleEntity> MongoCollection()
				{
						var collection = this.LocalDatabase.GetCollection<ScheduleEntity>("schedules");
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
