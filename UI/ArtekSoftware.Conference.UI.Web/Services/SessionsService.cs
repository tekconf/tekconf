using System.Collections.Generic;
using System.Linq;
using System.Net;
using ArtekSoftware.Conference.RemoteData.Dtos;
using AutoMapper;
using MongoDB.Driver.Linq;
using ServiceStack.Common.Web;

namespace ArtekSoftware.Conference.UI.Web
{
  public class SessionsService : MongoRestServiceBase<SessionsRequest>
  {
    public override object OnGet(SessionsRequest request)
    {
      if (request.conferenceSlug == default(string))
      {
        throw new HttpError() { StatusCode = HttpStatusCode.BadRequest };
      }

      if (request.sessionSlug == default(string))
      {
        var conference = this.Database.GetCollection<ConferenceEntity>("conferences").AsQueryable().SingleOrDefault(c => c.slug == request.conferenceSlug);
        if (conference == null)
        {
          throw new HttpError() { StatusCode = HttpStatusCode.NotFound };
        }

        var sessions = Mapper.Map<List<SessionEntities>, List<SessionsDto>>(conference.sessions);
        return sessions.ToList();
      }
      else
      {
        var conference = this.Database.GetCollection<ConferenceEntity>("conferences").AsQueryable()
          //.Where(s => s.slug == request.sessionSlug)
              .SingleOrDefault(c => c.slug == request.conferenceSlug);

        if (conference == null)
        {
          throw new HttpError(HttpStatusCode.NotFound, "Conference not found.");
        }

        var session = conference.sessions.FirstOrDefault(s => s.slug == request.sessionSlug);

        var dto = Mapper.Map<SessionEntities, SessionDto>(session);

        return dto;
      }
    }
  }
}