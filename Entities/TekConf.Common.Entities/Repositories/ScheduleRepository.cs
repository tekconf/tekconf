using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver.Linq;

namespace TekConf.Common.Entities.Repositories
{
    using MongoDB.Driver;
    using MongoDB.Driver.Builders;
    using TekConf.UI.Api;

    public interface IScheduleRepository : IRepository<ScheduleEntity>
    {
        IEnumerable<ScheduleEntity> GetSchedules(string userName);        
    }

    public class ScheduleRepository : IScheduleRepository
    {
        private readonly IConfiguration _configuration;

        public IEnumerable<ScheduleEntity> GetSchedules(string userName)
        {
            var schedules = this
                                .AsQueryable()
                                .Where(s => s.UserName.ToLower() == userName.ToLower())
                                .ToList();

            return schedules;
        }

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
