using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using AutoMapper;
using ConferencesIO.RemoteData.Dtos.v1;
using ConferencesIO.UI.Api.Services.Requests.v1;
using ConferencesIO.UI.Api.UrlResolvers.v1;
using FluentMongo.Linq;
using MongoDB.Driver;
using ServiceStack.CacheAccess;
using ServiceStack.Common.Web;
using ServiceStack.ServiceHost;
using ServiceStack.Text;

namespace ConferencesIO.UI.Api.Services.v1
{
    public class ConferencesService : MongoRestServiceBase<ConferencesRequest>
    {
        public ICacheClient CacheClient { get; set; }

        public override object OnGet(ConferencesRequest request)
        {
            if (request.conferenceSlug == default(string))
            {
                return GetAllConferences();
            }
            else
            {
                var detail = base.RequestContext.Get<IHttpRequest>().QueryString["detail"];
                if (string.Compare(detail, "all", StringComparison.InvariantCultureIgnoreCase) == 0)
                {
                    var fullConferenceDto = GetFullSingleConference(request);
                    return fullConferenceDto;
                }
                var conferenceDto = GetSingleConference(request);
                return conferenceDto;
            }
        }

        private object GetSingleConference(ConferencesRequest request)
        {
            var cacheKey = "GetSingleConference-" + request.conferenceSlug;
            return base.RequestContext.ToOptimizedResultUsingCache(this.CacheClient, cacheKey, () =>
            {
                //This delegate will be executed if the cache doesn't have an item
                //with the provided key

                //Return here your response DTO
                //It will be cached automatically

                var conference = this.Database.GetCollection<ConferenceEntity>("conferences")
                .AsQueryable()
                .SingleOrDefault(c => c.slug == request.conferenceSlug);

                if (conference == null)
                {
                    throw new HttpError(HttpStatusCode.NotFound, "Conference not found.");
                }

                var conferenceDto = Mapper.Map<ConferenceEntity, ConferenceDto>(conference);
                var conferenceUrlResolver = new ConferenceUrlResolver(conferenceDto.slug);
                var conferenceSessionsUrlResolver = new ConferenceSessionsUrlResolver(conferenceDto.slug);
                var conferenceSpeakersUrlResolver = new ConferenceSpeakersUrlResolver(conferenceDto.slug);

                conferenceDto.url = conferenceUrlResolver.ResolveUrl();
                conferenceDto.sessionsUrl = conferenceSessionsUrlResolver.ResolveUrl();
                conferenceDto.speakersUrl = conferenceSpeakersUrlResolver.ResolveUrl();

                return conferenceDto;
            });
        }

        private object GetFullSingleConference(ConferencesRequest request)
        {
            var cacheKey = "GetFullSingleConference-" + request.conferenceSlug;
            return base.RequestContext.ToOptimizedResultUsingCache(this.CacheClient, cacheKey, () =>
            {
                //This delegate will be executed if the cache doesn't have an item
                //with the provided key

                //Return here your response DTO
                //It will be cached automatically

                var conference = this.Database.GetCollection<ConferenceEntity>("conferences")
                .AsQueryable()
                .SingleOrDefault(c => c.slug == request.conferenceSlug);

                if (conference == null)
                {
                    throw new HttpError(HttpStatusCode.NotFound, "Conference not found.");
                }

                var conferenceDto = Mapper.Map<ConferenceEntity, FullConferenceDto>(conference);
                
                //var conferenceUrlResolver = new ConferenceUrlResolver(conferenceDto.slug);
                //var conferenceSessionsUrlResolver = new ConferenceSessionsUrlResolver(conferenceDto.slug);
                //var conferenceSpeakersUrlResolver = new ConferenceSpeakersUrlResolver(conferenceDto.slug);

                //conferenceDto.url = conferenceUrlResolver.ResolveUrl();
                //conferenceDto.sessionsUrl = conferenceSessionsUrlResolver.ResolveUrl();
                //conferenceDto.speakersUrl = conferenceSpeakersUrlResolver.ResolveUrl();

                return conferenceDto;
            });
        }

        private object GetAllConferences()
        {
            var cacheKey = "GetAllConferences";
            return base.RequestContext.ToOptimizedResultUsingCache(this.CacheClient, cacheKey, () =>
            {
                //List<ConferenceEntity> conferences = null;

                var conferencesDtos = this.Database.GetCollection<ConferenceEntity>("conferences")
                  .AsQueryable()
                  .Select(c => new ConferencesDto()
                                   {
                                       name = c.name,
                                       start = c.start,
                                       end = c.end,
                                       location = c.location,
                                       //url = c.url,
                                       slug = c.slug,
                                       description = c.description,
                                       imageUrl = c.imageUrl
                                   })


                  .OrderBy(c => c.end)
                  .ThenBy(c => c.start)
                  .ToList();

                //var server = MongoServer.Create("mongodb://localhost/conferences");
                //var db = server.GetDatabase("conferences");
                //var collection = db.GetCollection<ConferenceEntity>("conferences");
                //if (!collection.AsQueryable().Any())
                //{
                //    collection.InsertBatch(conferences);
                //}
                
               // var p = conferences.Dump();

                //var conferencesDtos = Mapper.Map<List<ConferenceEntity>, List<ConferencesDto>>(conferences);
                var resolver = new ConferencesUrlResolver();
                foreach (var conferencesDto in conferencesDtos)
                {
                    conferencesDto.url = resolver.ResolveUrl(conferencesDto.slug);
                }

                return conferencesDtos.ToList();
            });
        }
    }
}