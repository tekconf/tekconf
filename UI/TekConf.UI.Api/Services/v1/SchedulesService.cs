using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using FluentMongo.Linq;
using ServiceStack.CacheAccess;
using TekConf.RemoteData.Dtos.v1;
using TekConf.UI.Api.Services;
using TekConf.UI.Api.Services.Requests.v1;

namespace TekConf.UI.Api.v1
{
    public class SchedulesService : MongoServiceBase
    {
        public ICacheClient CacheClient { get; set; }
        public object Get(Schedules request)
        {
            var schedules = this.RemoteDatabase.GetCollection<ScheduleEntity>("schedules")
                               .AsQueryable()
                               .Where(s => s.AuthenticationMethod.ToLower() == request.authenticationMethod.ToLower())
                               .Where(s => s.AuthenticationToken.ToLower() == request.authenticationToken.ToLower())
                               .ToList();

            var schedulesDto = Mapper.Map<List<ScheduleEntity>, List<SchedulesDto>>(schedules);

            return schedulesDto;
        }
    }
}