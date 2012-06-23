using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
        var conference = this.Database.GetCollection<Conference>("conferences").AsQueryable().SingleOrDefault(c => c.slug == request.conferenceSlug);
        if (conference == null)
        {
          throw new HttpError() { StatusCode = HttpStatusCode.NotFound };
        }

        var sessions = Mapper.Map<List<Session>, List<SessionsDto>>(conference.sessions);
        return sessions.ToList();
      }
      else
      {
        var conference = this.Database.GetCollection<Conference>("conferences").AsQueryable()
          //.Where(s => s.slug == request.sessionSlug)
              .SingleOrDefault(c => c.slug == request.conferenceSlug);

        if (conference == null)
        {
          throw new HttpError(HttpStatusCode.NotFound, "Conference not found.");
        }

        var session = conference.sessions.FirstOrDefault(s => s.slug == request.sessionSlug);
        SessionDto dto = null;

        try
        {
          dto = Mapper.Map<Session, SessionDto>(session);
        }
        catch (Exception ee)
        {
          var x = ee.Message;
          throw;
        }


        return dto;
      }
    }
  }
}