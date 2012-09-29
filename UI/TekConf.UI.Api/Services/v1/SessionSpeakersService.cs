using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using AutoMapper;
using TekConf.RemoteData.Dtos.v1;
using TekConf.UI.Api.Services.Requests.v1;
using TekConf.UI.Api.UrlResolvers.v1;
using FluentMongo.Linq;
using ServiceStack.CacheAccess;
using ServiceStack.Common.Web;
using ServiceStack.ServiceHost;

namespace TekConf.UI.Api.Services.v1
{
    public class SessionSpeakersService : MongoServiceBase
    {
        public ICacheClient CacheClient { get; set; }

        public object Get(SessionSpeakers request)
        {
            if (request.conferenceSlug == default(string))
            {
                throw new HttpError() { StatusCode = HttpStatusCode.BadRequest };
            }

            if (request.sessionSlug == default(string))
            {
                throw new HttpError() { StatusCode = HttpStatusCode.BadRequest };
            }

            var conference = this.RemoteDatabase.GetCollection<ConferenceEntity>("conferences")
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


            return GetAllSpeakers(request, session);

        }

        private object GetAllSpeakers(SessionSpeakers request, SessionEntity session)
        {
            var cacheKey = "GetAllSpeakers-" + request.conferenceSlug + "-" + request.sessionSlug;
            var expireInTimespan = new TimeSpan(0, 0, 20);
            return base.RequestContext.ToOptimizedResultUsingCache(this.CacheClient, cacheKey, expireInTimespan, () =>
            {
                var speakersDtos = Mapper.Map<List<SpeakerEntity>, List<SpeakersDto>>(session.speakers);
                var resolver = new SpeakersUrlResolver(request.conferenceSlug, request.sessionSlug);
                foreach (var speakersDto in speakersDtos)
                {
                    speakersDto.url = resolver.ResolveUrl(speakersDto.slug);
                }
                return speakersDtos.ToList();
            });

        }
    }

    public class SessionSpeakerService : MongoServiceBase
    {
        public ICacheClient CacheClient { get; set; }

        public object Get(SessionSpeaker request)
        {
            if (request.conferenceSlug == default(string))
            {
                throw new HttpError() { StatusCode = HttpStatusCode.BadRequest };
            }

            if (request.sessionSlug == default(string))
            {
                throw new HttpError() { StatusCode = HttpStatusCode.BadRequest };
            }

            var conference = this.RemoteDatabase.GetCollection<ConferenceEntity>("conferences")
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

            return GetSingleSpeaker(request, session);
        }

        private object GetSingleSpeaker(SessionSpeaker request, SessionEntity session)
        {
            var cacheKey = "GetSingleSpeaker-" + request.conferenceSlug + "-" + request.sessionSlug + "-" + request.speakerSlug;
            var expireInTimespan = new TimeSpan(0, 0, 20);
            return base.RequestContext.ToOptimizedResultUsingCache(this.CacheClient, cacheKey, expireInTimespan, () =>
            {
                var speaker = session.speakers.FirstOrDefault(s => s.slug == request.speakerSlug);

                var speakerDto = Mapper.Map<SpeakerEntity, SpeakerDto>(speaker);
                var resolver = new SpeakerUrlResolver(request.conferenceSlug, request.sessionSlug, speakerDto.url);
                speakerDto.url = resolver.ResolveUrl();
                return speakerDto;
            });


        }


    }


}