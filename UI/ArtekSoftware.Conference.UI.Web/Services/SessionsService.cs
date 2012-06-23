using System.Collections.Generic;
using System.Linq;
using System.Net;
using AutoMapper;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using ServiceStack.Common.Web;
using ServiceStack.ServiceInterface;

namespace ArtekSoftware.Conference.UI.Web
{
  public class SessionsService : RestServiceBase<SessionsRequest>
  {
    public override object OnGet(SessionsRequest request)
    {
      var server = MongoServer.Create("mongodb://admin:goldie12@flame.mongohq.com:27100/app4727263?safe=true");
      var database = server.GetDatabase("app4727263");

      if (request.conferenceSlug == default(string))
      {
        throw new HttpError() { StatusCode = HttpStatusCode.BadRequest };
      }

      if (request.sessionSlug == default(string))
      {
        var conference = database.GetCollection<Conference>("conferences").AsQueryable().SingleOrDefault(c => c.slug == request.conferenceSlug);
        if (conference == null)
        {
          throw new HttpError() { StatusCode = HttpStatusCode.NotFound };
        }

        var sessions = Mapper.Map<List<Session>, List<SessionsDto>>(conference.sessions);
        return sessions.ToList();
      }
      else
      {
        var conference = database.GetCollection<Conference>("conferences").AsQueryable()
          //.Where(s => s.slug == request.sessionSlug)
              .SingleOrDefault(c => c.slug == request.conferenceSlug);

        if (conference == null)
        {
          throw new HttpError(HttpStatusCode.NotFound, "Conference not found.");
        }

        var session = conference.sessions.FirstOrDefault(s => s.slug == request.sessionSlug);

        var dto = Mapper.Map<Session, SessionDto>(session);

        return dto;
      }
    }
  }
}