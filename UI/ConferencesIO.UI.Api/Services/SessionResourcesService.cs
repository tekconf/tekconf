using System.Linq;
using System.Net;
using ConferencesIO.UI.Api.Services.Requests;
using FluentMongo.Linq;
using ServiceStack.Common.Web;

namespace ConferencesIO.UI.Api
{
  public class SessionResourcesService : MongoRestServiceBase<SessionResourcesRequest>
  {
    public override object OnGet(SessionResourcesRequest request)
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

        return session.resources;
      }
    }
  }
}