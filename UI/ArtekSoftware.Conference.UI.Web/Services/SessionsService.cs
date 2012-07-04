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
        return GetAllSessions(request);
      }
      else
      {
        return GetSingleSession(request);
      }
    }

    private object GetSingleSession(SessionsRequest request)
    {
      var conference = this.Database.GetCollection<ConferenceEntity>("conferences").AsQueryable()
        //.Where(s => s.slug == request.sessionSlug)
        .SingleOrDefault(c => c.slug == request.conferenceSlug);

      if (conference == null)
      {
        throw new HttpError(HttpStatusCode.NotFound, "Conference not found.");
      }

      if (conference.sessions == null)
      {
        return new HttpError() {StatusCode = HttpStatusCode.NotFound};
      }

      var session = conference.sessions.FirstOrDefault(s => s.slug == request.sessionSlug);

      if (session != null)
      {
        var sessionDto = Mapper.Map<SessionEntity, SessionDto>(session);
        var sessionUrlResolver = new SessionUrlResolver(request.conferenceSlug, sessionDto.slug);
        var sessionSpeakersUrlResolver = new SessionSpeakersUrlResolver(request.conferenceSlug, sessionDto.slug);
        var sessionLinksUrlResolver = new SessionLinksUrlResolver(request.conferenceSlug, sessionDto.slug);
        var sessionSubjectsUrlResolver = new SessionSubjectsUrlResolver(request.conferenceSlug, sessionDto.slug);
        var sessionTagsUrlResolver = new SessionTagsUrlResolver(request.conferenceSlug, sessionDto.slug);
        
        sessionDto.url = sessionUrlResolver.ResolveUrl();
        sessionDto.speakersUrl = sessionSpeakersUrlResolver.ResolveUrl();
        sessionDto.linksUrl = sessionLinksUrlResolver.ResolveUrl();
        sessionDto.subjectsUrl = sessionSubjectsUrlResolver.ResolveUrl();
        sessionDto.tagsUrl = sessionTagsUrlResolver.ResolveUrl();

        return sessionDto;
      }
      else
      {
        return new HttpError() {StatusCode = HttpStatusCode.NotFound};
      }
    }

    private object GetAllSessions(SessionsRequest request)
    {
      var conference =
        this.Database.GetCollection<ConferenceEntity>("conferences").AsQueryable().SingleOrDefault(
          c => c.slug == request.conferenceSlug);

      if (conference == null)
      {
        throw new HttpError() {StatusCode = HttpStatusCode.NotFound};
      }

      var sessionsDtos = Mapper.Map<List<SessionEntity>, List<SessionsDto>>(conference.sessions);
      foreach (var sessionsDto in sessionsDtos)
      {
        var sessionsUrlResolver = new SessionsUrlResolver(request.conferenceSlug);
        var sessionsSpeakersUrlResolver = new SessionsSpeakersUrlResolver(request.conferenceSlug);
        var sessionsLinksUrlResolver = new SessionsLinksUrlResolver(request.conferenceSlug);
        var sessionsSubjectsUrlResolver = new SessionsSubjectsUrlResolver(request.conferenceSlug);
        var sessionsTagsUrlResolver = new SessionsTagsUrlResolver(request.conferenceSlug);

        sessionsDto.url = sessionsUrlResolver.ResolveUrl(sessionsDto.slug);
        sessionsDto.speakersUrl = sessionsSpeakersUrlResolver.ResolveUrl(sessionsDto.slug);
        sessionsDto.linksUrl = sessionsLinksUrlResolver.ResolveUrl(sessionsDto.slug);
        sessionsDto.subjectsUrl = sessionsSubjectsUrlResolver.ResolveUrl(sessionsDto.slug);
        sessionsDto.tagsUrl = sessionsTagsUrlResolver.ResolveUrl(sessionsDto.slug);
      }
      return sessionsDtos.ToList();
    }
  }
}