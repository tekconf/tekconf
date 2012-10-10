using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using AutoMapper;
using TekConf.RemoteData.Dtos.v1;
using TekConf.RemoteData.v1;
using TekConf.UI.Api.Services.Requests.v1;
using TekConf.UI.Api.UrlResolvers.v1;
using FluentMongo.Linq;
using ServiceStack.CacheAccess;
using ServiceStack.Common.Web;
using ServiceStack.ServiceHost;

namespace TekConf.UI.Api.Services.v1
{

    public class SessionsService : MongoServiceBase
    {
        public ICacheClient CacheClient { get; set; }
        static HttpError ConferenceNotFound = HttpError.NotFound("Conference not found") as HttpError;
        static HashSet<string> NonExistingConferences = new HashSet<string>();

        static HttpError SessionNotFound = HttpError.NotFound("Session not found") as HttpError;
        static HashSet<string> NonExistingSessions = new HashSet<string>();

        public object Get(Sessions request)
        {
            if (request.conferenceSlug == default(string))
            {
                throw new HttpError() { StatusCode = HttpStatusCode.BadRequest };
            }

            return GetAllSessions(request);
        }

        private object GetAllSessions(Sessions request)
        {
            var cacheKey = "GetAllSessions-" + request.conferenceSlug;
            lock (NonExistingConferences)
            {
                if (NonExistingConferences.Contains(request.conferenceSlug))
                {
                    throw ConferenceNotFound;
                }
            }
            var expireInTimespan = new TimeSpan(0, 0, 20);
            return base.RequestContext.ToOptimizedResultUsingCache(this.CacheClient, cacheKey, expireInTimespan, () =>
            {
                var conference =
                this.RemoteDatabase.GetCollection<ConferenceEntity>("conferences")
                .AsQueryable()
                .Where(c => c.isLive)
                .SingleOrDefault(c => c.slug == request.conferenceSlug);

                if (conference == null)
                {
                    lock (NonExistingConferences)
                    {
                        NonExistingConferences.Add(request.conferenceSlug);
                    }
                    throw ConferenceNotFound;
                }

                var sessionsDtos = Mapper.Map<List<SessionEntity>, List<SessionsDto>>(conference.sessions);
                foreach (var sessionsDto in sessionsDtos)
                {
                    var sessionsUrlResolver = new SessionsUrlResolver(request.conferenceSlug);
                    var sessionsSpeakersUrlResolver = new SessionsSpeakersUrlResolver(request.conferenceSlug);
                    var sessionsLinksUrlResolver = new SessionsLinksUrlResolver(request.conferenceSlug);
                    var sessionsSubjectsUrlResolver = new SessionsSubjectsUrlResolver(request.conferenceSlug);
                    var sessionsTagsUrlResolver = new SessionsTagsUrlResolver(request.conferenceSlug);
                    var sessionsPrerequisitesUrlResolver = new SessionsPrerequisitesUrlResolver(request.conferenceSlug);

                    sessionsDto.url = sessionsUrlResolver.ResolveUrl(sessionsDto.slug);
                    sessionsDto.speakersUrl = sessionsSpeakersUrlResolver.ResolveUrl(sessionsDto.slug);
                    sessionsDto.linksUrl = sessionsLinksUrlResolver.ResolveUrl(sessionsDto.slug);
                    sessionsDto.subjectsUrl = sessionsSubjectsUrlResolver.ResolveUrl(sessionsDto.slug);
                    sessionsDto.tagsUrl = sessionsTagsUrlResolver.ResolveUrl(sessionsDto.slug);
                    sessionsDto.prerequisitesUrl = sessionsPrerequisitesUrlResolver.ResolveUrl(sessionsDto.slug);

                }
                return sessionsDtos.ToList();
            });


        }
    }

    public class SessionService : MongoServiceBase
    {
        public ICacheClient CacheClient { get; set; }
        static HttpError ConferenceNotFound = HttpError.NotFound("Conference not found") as HttpError;
        static HashSet<string> NonExistingConferences = new HashSet<string>();

        static HttpError SessionNotFound = HttpError.NotFound("Session not found") as HttpError;
        static HashSet<string> NonExistingSessions = new HashSet<string>();

        public object Get(Session request)
        {
            if (request.conferenceSlug == default(string))
            {
                throw new HttpError() { StatusCode = HttpStatusCode.BadRequest };
            }

            return GetSingleSession(request);
        }

        public object Post(AddSession request)
        {
            var entity = Mapper.Map<SessionEntity>(request);

            var collection = this.RemoteDatabase.GetCollection<ConferenceEntity>("conferences");
            var conference = collection
                .AsQueryable()
                .Where(c => c.isLive)
                .FirstOrDefault(x => x.slug == request.conferenceSlug);

            if (conference == null)
            {
                return new HttpError() { StatusCode = HttpStatusCode.BadRequest };
            }

            conference.sessions.Add(entity);
            collection.Save(conference);

            var conferenceDto = Mapper.Map<ConferenceEntity, FullConferenceDto>(conference);

            return conferenceDto;
        }

        private object GetSingleSession(Session request)
        {
            var cacheKey = "GetSingleSession-" + request.conferenceSlug + "-" + request.sessionSlug;

            lock (NonExistingConferences)
            {
                if (NonExistingConferences.Contains(request.conferenceSlug))
                {
                    throw ConferenceNotFound;
                }
            }

            lock (NonExistingSessions)
            {
                if (NonExistingSessions.Contains(request.conferenceSlug + "-" + request.sessionSlug))
                {
                    throw SessionNotFound;
                }
            }
            var expireInTimespan = new TimeSpan(0, 0, 20);
            return base.RequestContext.ToOptimizedResultUsingCache(this.CacheClient, cacheKey, expireInTimespan, () =>
            {
                var conference = this.RemoteDatabase.GetCollection<ConferenceEntity>("conferences")
                    .AsQueryable()
                    //.Where(s => s.slug == request.sessionSlug)
                    .Where(c => c.isLive)
                    .SingleOrDefault(c => c.slug == request.conferenceSlug);

                if (conference == null)
                {
                    lock (NonExistingConferences)
                    {
                        NonExistingConferences.Add(request.conferenceSlug);
                    }
                    throw ConferenceNotFound;
                }


                if (conference.sessions == null)
                {
                    lock (NonExistingSessions)
                    {
                        NonExistingSessions.Add(request.conferenceSlug + "-" + request.sessionSlug);
                    }
                    throw SessionNotFound;
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
                    var sessionPrerequisitesUrlResolver = new SessionPrerequisitesUrlResolver(request.conferenceSlug, sessionDto.slug);

                    sessionDto.url = sessionUrlResolver.ResolveUrl();
                    sessionDto.speakersUrl = sessionSpeakersUrlResolver.ResolveUrl();
                    sessionDto.linksUrl = sessionLinksUrlResolver.ResolveUrl();
                    sessionDto.subjectsUrl = sessionSubjectsUrlResolver.ResolveUrl();
                    sessionDto.tagsUrl = sessionTagsUrlResolver.ResolveUrl();
                    sessionDto.prerequisitesUrl = sessionPrerequisitesUrlResolver.ResolveUrl();

                    return sessionDto;
                }
                else
                {
                    lock (NonExistingSessions)
                    {
                        NonExistingSessions.Add(request.conferenceSlug + "-" + request.sessionSlug);
                    }
                    throw SessionNotFound;
                }
            });

        }

    }

}