using System.Linq;
using System.Net;
using ArtekSoftware.Conference.UI.Web.Services.Requests;
using MongoDB.Driver.Linq;
using ServiceStack.Common.Web;

namespace ArtekSoftware.Conference.UI.Web
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