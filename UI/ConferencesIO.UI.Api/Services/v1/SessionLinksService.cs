using System.Linq;
using System.Net;
using ConferencesIO.UI.Api.Services.Requests.v1;
using FluentMongo.Linq;
using ServiceStack.Common.Web;

namespace ConferencesIO.UI.Api.Services.v1
{
  public class SessionLinksService : MongoRestServiceBase<SessionLinksRequest>
  {
    public override object OnGet(SessionLinksRequest request)
    {
      if (request.conferenceSlug == default(string))
      {
        throw new HttpError() { StatusCode = HttpStatusCode.BadRequest };
      }

      var conference = this.Database.GetCollection<ConferenceEntity>("conferences")
                  .AsQueryable()
                  .SingleOrDefault(c => c.slug == request.conferenceSlug);
      if (conference == null)
      {
        throw new HttpError() { StatusCode = HttpStatusCode.NotFound };
      }

      if (request.sessionSlug == default(string))
      {
        throw new HttpError() { StatusCode = HttpStatusCode.BadRequest };
      }
      else
      {
        var session = conference.sessions.FirstOrDefault(s => s.slug == request.sessionSlug);
        if (session == null)
        {
          throw new HttpError() { StatusCode = HttpStatusCode.NotFound };
        }

        return session.links;
      }
    }
  }
}