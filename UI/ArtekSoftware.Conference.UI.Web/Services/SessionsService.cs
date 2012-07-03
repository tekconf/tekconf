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

      Mapper.CreateMap<SessionEntity, SessionsDto>()
        .ForMember(dest => dest.Url, opt => opt.ResolveUsing<Bootstrapper.SessionsUrlResolver>().ConstructedBy(() => new Bootstrapper.SessionsUrlResolver(request.conferenceSlug)))
        ;

      if (request.sessionSlug == default(string))
      {
        var conference = this.Database.GetCollection<ConferenceEntity>("conferences").AsQueryable().SingleOrDefault(c => c.slug == request.conferenceSlug);
        if (conference == null)
        {
          throw new HttpError() { StatusCode = HttpStatusCode.NotFound };
        }

        var sessions = Mapper.Map<List<SessionEntity>, List<SessionsDto>>(conference.sessions);
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

        Mapper.CreateMap<SessionEntity, SessionDto>()
          .ForMember(dest => dest.url, opt => opt.ResolveUsing<Bootstrapper.SessionsUrlResolver>().ConstructedBy(() => new Bootstrapper.SessionsUrlResolver(request.conferenceSlug)))
          .ForMember(dest => dest.speakersUrl, opt => opt.ResolveUsing<Bootstrapper.SessionsSpeakersUrlResolver>().ConstructedBy(() => new Bootstrapper.SessionsSpeakersUrlResolver(request.conferenceSlug)))
          ;

        var dto = Mapper.Map<SessionEntity, SessionDto>(session);

        return dto;
      }
    }
  }
}