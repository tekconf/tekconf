using System;
using System.Linq;
using System.Net;
using TekConf.UI.Api.Services.Requests.v1;
using FluentMongo.Linq;
using ServiceStack.CacheAccess;
using ServiceStack.Common.Web;
using ServiceStack.ServiceHost;

namespace TekConf.UI.Api.Services.v1
{
  public class SessionPrerequisitesService : MongoServiceBase
  {
    public ICacheClient CacheClient { get; set; }

    public object Get(SessionPrerequisites request)
    {
      if (request.conferenceSlug == default(string))
      {
        throw new HttpError() { StatusCode = HttpStatusCode.BadRequest };
      }

      if (request.sessionSlug == default(string))
      {
        throw new HttpError() { StatusCode = HttpStatusCode.BadRequest };
      }

      return GetSingleSessionPrerequisites(request);
    }

    private object GetSingleSessionPrerequisites(SessionPrerequisites request)
    {
      var cacheKey = "GetSingleSessionPrerequisites-" + request.conferenceSlug + "-" + request.sessionSlug;
      var expireInTimespan = new TimeSpan(0, 0, 20);
      return base.RequestContext.ToOptimizedResultUsingCache(this.CacheClient, cacheKey, expireInTimespan,  () =>
      {
        var conference = this.RemoteDatabase.GetCollection<ConferenceEntity>("conferences")
          .AsQueryable()
          .SingleOrDefault(c => c.slug == request.conferenceSlug);

        if (conference == null)
        {
          throw new HttpError() { StatusCode = HttpStatusCode.NotFound };
        }


        var session = conference.sessions.FirstOrDefault(s => s.slug == request.sessionSlug);
        if (session == null)
        {
          throw new HttpError() { StatusCode = HttpStatusCode.NotFound };
        }

        return session.prerequisites;

      });
    }
  }
}