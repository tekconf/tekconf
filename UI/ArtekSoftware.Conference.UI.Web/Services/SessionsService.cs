using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using ArtekSoftware.Conference.RemoteData.Dtos;
using ArtekSoftware.Conference.UI.Web.Services.Requests;
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

        var sessionsDtos = Mapper.Map<List<SessionEntity>, List<SessionsDto>>(conference.sessions);
        foreach (var sessionsDto in sessionsDtos)
        {
          var resolver = new SessionsUrlResolver(request.conferenceSlug, sessionsDto.slug);

          sessionsDto.url = resolver.ResolveCore(sessionsDto);
        }
        return sessionsDtos.ToList();
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

        if (session != null)
        {
          var sessionDto = Mapper.Map<SessionEntity, SessionDto>(session);
          var resolver = new SessionUrlResolver(request.conferenceSlug, sessionDto.slug);
          sessionDto.url = resolver.ResolveCore(sessionDto);
          return sessionDto;
        }
        else
        {
          return new HttpError() { StatusCode = HttpStatusCode.NotFound };
        }
      }
    }

  }
}