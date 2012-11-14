using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using AutoMapper;
using TekConf.RemoteData.Dtos.v1;
using TekConf.UI.Api.Services;
using TekConf.UI.Api.Services.Requests.v1;
using TekConf.UI.Api.UrlResolvers.v1;
using FluentMongo.Linq;
using ServiceStack.CacheAccess;
using ServiceStack.Common.Web;
using ServiceStack.ServiceHost;

namespace TekConf.UI.Api.v1
{
    public class ScheduleService : MongoServiceBase
    {
        public ICacheClient CacheClient { get; set; }

        public object Get(Schedule request)
        {
            if (request.conferenceSlug == default(string) || request.authenticationMethod == default(string) || request.authenticationToken == default(string))
            {
                return new HttpError() { StatusCode = HttpStatusCode.BadRequest };
            }

            return GetSingleSchedule(request);
        }

        public object Post(AddSessionToSchedule request)
        {
            var scheduleCollection = this.RemoteDatabase.GetCollection<ScheduleEntity>("schedules");
            ScheduleEntity schedule = scheduleCollection.AsQueryable()
                .Where(s => s.ConferenceSlug.ToLower() == request.conferenceSlug.ToLower())
                .Where(s => s.AuthenticationMethod.ToLower() == request.authenticationMethod.ToLower())
                .SingleOrDefault(s => s.AuthenticationToken.ToLower() == request.authenticationToken.ToLower());

            if (schedule == null)
            {
                schedule = new ScheduleEntity()
                               {
                                   _id = Guid.NewGuid(),
                                   AuthenticationMethod = request.authenticationMethod,
                                   AuthenticationToken = request.authenticationToken,
                                   ConferenceSlug = request.conferenceSlug,
                                   SessionSlugs = new List<string>(),
                               };
            }
            var conferenceCollection = this.RemoteDatabase.GetCollection<ConferenceEntity>("conferences");

            var conference =
                conferenceCollection.AsQueryable()
                .SingleOrDefault(c => c.slug == request.conferenceSlug);

            if (conference != null)
            {
                if (!schedule.SessionSlugs.Any(s => s == request.sessionSlug))
                {
                    //var sessionEntity = conference.sessions
                    //                              .FirstOrDefault(s => s.slug == request.sessionSlug);

                    schedule.SessionSlugs.Add(request.sessionSlug);
                }
            }

            scheduleCollection.Save(schedule);

            var scheduleDto = Mapper.Map<ScheduleDto>(schedule);

            return scheduleDto;
        }

        private object GetSingleSchedule(Schedule request)
        {

            var cacheKey = "GetSingleSchedule-" + request.conferenceSlug + "-" + request.authenticationMethod + "-" + request.authenticationToken;
            var expireInTimespan = new TimeSpan(0, 0, 120);
            return base.RequestContext.ToOptimizedResultUsingCache(this.CacheClient, cacheKey, expireInTimespan, () =>
                {
                    var schedule = this.RemoteDatabase.GetCollection<ScheduleEntity>("schedules")
                      .AsQueryable()
                      .Where(s => s.ConferenceSlug.ToLower() == request.conferenceSlug.ToLower())
                      .Where(s => s.AuthenticationMethod.ToLower() == request.authenticationMethod.ToLower())
                      .SingleOrDefault(s => s.AuthenticationToken.ToLower() == request.authenticationToken.ToLower());

                    var scheduleDto = Mapper.Map<ScheduleEntity, ScheduleDto>(schedule);
                    var resolver = new ScheduleUrlResolver(request.conferenceSlug, request.userSlug);
                    scheduleDto.url = resolver.ResolveUrl();
                    return scheduleDto;
                });
        }
    }
}