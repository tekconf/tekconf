using System;
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

        private object GetAllConferences()
        {
            var cacheKey = "GetAllConferences";

            //var collection = this.RemoteDatabase.GetCollection<ConferenceEntity>("conferences");
            //var thatConf = collection.AsQueryable().Where(c => c.slug == "ThatConference-2012").FirstOrDefault();
            //var nextConf = Mapper.Map<ConferenceEntity>(thatConf);
            //nextConf._id = Guid.NewGuid();
            //nextConf.start = thatConf.start.AddYears(1);
            //nextConf.end = thatConf.end.AddYears(1);
            //nextConf.slug = "ThatConference-2013";
            //collection.Save(nextConf);

            return base.RequestContext.ToOptimizedResultUsingCache(this.CacheClient, cacheKey, () =>
            {
                var conferencesDtos = this.RemoteDatabase.GetCollection<ConferenceEntity>("conferences")
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

                var resolver = new ConferencesUrlResolver();
                foreach (var conferencesDto in conferencesDtos)
                {
                    conferencesDto.url = resolver.ResolveUrl(conferencesDto.slug);
                }

                return conferencesDtos.ToList();
            });
        }

        private object GetSingleConference(ConferencesRequest request)
        {
            var cacheKey = "GetSingleConference-" + request.conferenceSlug;
            return base.RequestContext.ToOptimizedResultUsingCache(this.CacheClient, cacheKey, () =>
            {
                var conference = this.RemoteDatabase.GetCollection<ConferenceEntity>("conferences")
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
                var conference = this.RemoteDatabase.GetCollection<ConferenceEntity>("conferences")
                .AsQueryable()
                .SingleOrDefault(c => c.slug == request.conferenceSlug);

                if (conference == null)
                {
                    throw new HttpError(HttpStatusCode.NotFound, "Conference not found.");
                }

                var conferenceDto = Mapper.Map<ConferenceEntity, FullConferenceDto>(conference);

                return conferenceDto;
            });
        }


    }

    public class SessionResult
    {
        public DateKey DateKey { get; set; }
        public SessionEntity Session { get; set; }
    }
    public class DateKey
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
    }
}