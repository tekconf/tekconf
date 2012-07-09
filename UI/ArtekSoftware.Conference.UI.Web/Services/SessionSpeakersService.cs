using System.Collections.Generic;
using System.Linq;
using System.Net;
using ArtekSoftware.Conference.RemoteData.Dtos;
using ArtekSoftware.Conference.UI.Web.Services.Requests;
using AutoMapper;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using ServiceStack.Common.Web;
using ServiceStack.ServiceInterface;

namespace ArtekSoftware.Conference.UI.Web
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