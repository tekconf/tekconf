using System.Linq;
using System.Net;
using AutoMapper;
using ConferencesIO.RemoteData.Dtos.v1;
using ConferencesIO.UI.Api.Services;
using ConferencesIO.UI.Api.Services.Requests.v1;
using ConferencesIO.UI.Api.UrlResolvers.v1;
using FluentMongo.Linq;
using ServiceStack.CacheAccess;
using ServiceStack.Common.Web;
using ServiceStack.ServiceHost;

namespace ConferencesIO.UI.Api.v1
{
  public class ScheduleService : MongoRestServiceBase<ScheduleRequest>
  {
    public ICacheClient CacheClient { get; set; }

    public override object OnGet(ScheduleRequest request)
    {
      if (request.conferenceSlug == default(string) || request.userSlug == default(string))
      {
        return new HttpError() { StatusCode = HttpStatusCode.BadRequest };
      }

      return GetSingleSchedule(request);
    }

    private object GetSingleSchedule(ScheduleRequest request)
    {
      var cacheKey = "GetSingleSchedule-" + request.conferenceSlug + "-" + request.userSlug;
      return base.RequestContext.ToOptimizedResultUsingCache(this.CacheClient, cacheKey, () =>
          {
            var schedule = this.Database.GetCollection<ScheduleEntity>("schedules")
              .AsQueryable()
              .Where(s => s.ConferenceSlug == request.conferenceSlug)
              .SingleOrDefault(s => s.UserSlug == request.userSlug);

            var scheduleDto = Mapper.Map<ScheduleEntity, ScheduleDto>(schedule);
            var resolver = new ScheduleUrlResolver(request.conferenceSlug, request.userSlug);
            scheduleDto.url = resolver.ResolveUrl();
            return scheduleDto;
          });
    }
  }
}