using System.Collections.Generic;
using System.Linq;
using System.Net;
using ConferencesIO.RemoteData.Dtos;
using ConferencesIO.RemoteData.Dtos.v1;
using ConferencesIO.UI.Api.Services.Requests.v1;
using AutoMapper;
using ConferencesIO.UI.Api.UrlResolvers.v1;
using FluentMongo.Linq;
using ServiceStack.Common.Web;

namespace ConferencesIO.UI.Api.Services.v1
{
  public class SessionSpeakersService : MongoRestServiceBase<SessionSpeakersRequest>
  {
    public override object OnGet(SessionSpeakersRequest request)
    {
      if (request.conferenceSlug == default(string))
      {
        throw new HttpError() { StatusCode = HttpStatusCode.BadRequest };
      }

      if (request.sessionSlug == default(string))
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

      var session = conference.sessions.SingleOrDefault(s => s.slug == request.sessionSlug);

      if (session == null)
      {
        throw new HttpError() { StatusCode = HttpStatusCode.NotFound };
      }

      if (request.speakerSlug == default(string))
      {
        return GetAllSpeakers(request, session);
      }
      else
      {
        return GetSingleSpeaker(request, session);
      }


    }

    private static object GetSingleSpeaker(SessionSpeakersRequest request, SessionEntity session)
    {
      var speaker = session.speakers.FirstOrDefault(s => s.slug == request.speakerSlug);

      var speakerDto = Mapper.Map<SpeakerEntity, SpeakerDto>(speaker);
      var resolver = new SpeakerUrlResolver(request.conferenceSlug, request.sessionSlug, speakerDto.url);
      speakerDto.url = resolver.ResolveUrl();
      return speakerDto;
    }

    private static object GetAllSpeakers(SessionSpeakersRequest request, SessionEntity session)
    {
      var speakersDtos = Mapper.Map<List<SpeakerEntity>, List<SpeakersDto>>(session.speakers);
      var resolver = new SpeakersUrlResolver(request.conferenceSlug, request.sessionSlug);
      foreach (var speakersDto in speakersDtos)
      {
        speakersDto.url = resolver.ResolveUrl(speakersDto.slug);
      }
      return speakersDtos.ToList();
    }
  }
}