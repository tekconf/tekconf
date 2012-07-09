using System.Linq;
using System.Net;
using ConferencesIO.UI.Api.Services.Requests.v1;
using FluentMongo.Linq;
using ServiceStack.CacheAccess;
using ServiceStack.Common.Web;
using ServiceStack.ServiceHost;

namespace ConferencesIO.UI.Api.Services.v1
{
  public class SessionLinksService : MongoRestServiceBase<SessionLinksRequest>
  {
    public ICacheClient CacheClient { get; set; }

    public override object OnGet(SessionLinksRequest request)
    {
      if (request.conferenceSlug == default(string))
      {
        throw new HttpError() { StatusCode = HttpStatusCode.BadRequest };
      }

      if (request.sessionSlug == default(string))
      {
        throw new HttpError() { StatusCode = HttpStatusCode.BadRequest };
      }

      return GetSingleSessionLinks(request);
    }

    private object GetSingleSessionLinks(SessionLinksRequest request)
    {
      var cacheKey = "GetSingleSessionLinks-" + request.conferenceSlug + "-" + request.sessionSlug;
      return base.RequestContext.ToOptimizedResultUsingCache(this.CacheClient, cacheKey, () =>
      {
        var conference = this.Database.GetCollection<ConferenceEntity>("conferences")
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

        return session.links;
      });

    }
  }
}