using System;
using System.Linq;
using System.Linq.Expressions;
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
    public class ConferenceService : MongoServiceBase
    {
        public ICacheClient CacheClient { get; set; }

        public object Get(Conference request)
        {
            var fullConferenceDto = GetFullSingleConference(request);
            return fullConferenceDto;
        }

        public object Post(CreateConference conference)
        {
            var address = Mapper.Map<AddressEntity>(conference.address);

            var entity = new ConferenceEntity()
                             {
                                 _id = Guid.NewGuid(),
                                 description = conference.description,
                                 end = conference.end,
                                 facebookUrl = conference.facebookUrl,
                                 githubUrl = conference.githubUrl,
                                 googlePlusUrl = conference.googlePlusUrl,
                                 homepageUrl = conference.homepageUrl,
                                 imageUrl = conference.imageUrl,
                                 lanyrdUrl = conference.lanyrdUrl,
                                 linkedInUrl = conference.linkedInUrl,
                                 location = conference.location,
                                 meetupUrl = conference.meetupUrl,
                                 name = conference.name,
                                 slug = conference.name.GenerateSlug(),
                                 start = conference.start,
                                 tagLine = conference.tagline,
                                 twitterHashTag = conference.twitterHashTag,
                                 twitterName = conference.twitterName,
                                 vimeoUrl = conference.vimeoUrl,
                                 youtubeUrl = conference.youtubeUrl,
                                 address = address
                             };

            var collection = this.RemoteDatabase.GetCollection<ConferenceEntity>("conferences");
            collection.Save(entity);
            var conferenceDto = Mapper.Map<ConferenceEntity, FullConferenceDto>(entity);

            return conferenceDto;
        }

        private object GetFullSingleConference(Conference request)
        {
            var cacheKey = "GetFullSingleConference-" + request.conferenceSlug;
            var expireInTimespan = new TimeSpan(0, 0, 20);
            return base.RequestContext.ToOptimizedResultUsingCache(this.CacheClient, cacheKey, expireInTimespan, () =>
            {
                var collection = this.RemoteDatabase.GetCollection<ConferenceEntity>("conferences");
                var conference = collection
                .AsQueryable()
                .SingleOrDefault(c => c.slug == request.conferenceSlug);

                if (conference == null)
                {
                    throw new HttpError(HttpStatusCode.NotFound, "Conference not found.");
                }

                ////TODO : Temp import
                //if (conference.name == "CodeMash")
                //{
                //    conference.githubUrl = "http://github.com";
                //    conference.googlePlusUrl = "http://plus.google.com";
                //    conference.lanyrdUrl = "http://lanyrd.com";
                //    conference.meetupUrl = "http://meetup.com";
                //    conference.vimeoUrl = "http://vimeo.com";
                //    conference.youtubeUrl = "http://youtube.com";
                //    collection.Save(conference);
                //}
                var conferenceDto = Mapper.Map<ConferenceEntity, FullConferenceDto>(conference);

                return conferenceDto;
            });
        }
    }

    public class ConferencesService : MongoServiceBase
    {
        public ICacheClient CacheClient { get; set; }

        public object Get(Conferences request)
        {
            return GetAllConferences(request);
        }

        private object GetAllConferences(Conferences request)
        {
            string searchCacheKey = request.search ?? string.Empty;
            string sortByCacheKey = request.sortBy ?? string.Empty;
            string showPastConferencesCacheKey = request.showPastConferences.ToString() ?? string.Empty;

            var cacheKey = "GetAllConferences-" + searchCacheKey + "-" + sortByCacheKey + "-" + showPastConferencesCacheKey;
            var expireInTimespan = new TimeSpan(0, 0, 20);

            return base.RequestContext.ToOptimizedResultUsingCache(this.CacheClient, cacheKey, expireInTimespan, () =>
            {
                var orderByFunc = GetOrderByFunc(request.sortBy);
                var search = GetSearch(request.search);
                var showPastConferences = GetShowPastConferences(request.showPastConferences);
                
                var query = this.RemoteDatabase.GetCollection<ConferenceEntity>("conferences")
                  .AsQueryable();
                
                if (search != null)
                {
                    query = query.Where(search);
                }

                if (showPastConferences != null)
                {
                    query = query.Where(showPastConferences);
                }
                  
                var conferencesDtos = query
                  .OrderBy(orderByFunc)
                  .ThenBy(c => c.start)
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
                  .ToList();

                var resolver = new ConferencesUrlResolver();
                foreach (var conferencesDto in conferencesDtos)
                {
                    conferencesDto.url = resolver.ResolveUrl(conferencesDto.slug);
                }

                return conferencesDtos.ToList();
            });
        }

        private Expression<Func<ConferenceEntity, bool>> GetShowPastConferences(bool? showPastConferences)
        {
            Expression<Func<ConferenceEntity, bool>> searchBy = null;
           
            if (showPastConferences == null || !(bool)showPastConferences)
            {
                searchBy = c => c.end > DateTime.Now;
            }

            return searchBy;            
        }
        private Expression<Func<ConferenceEntity, bool>> GetSearch(string search)
        {
            Expression<Func<ConferenceEntity, bool>> searchBy = null;

            if (!string.IsNullOrWhiteSpace(search))
            {
                searchBy = c => c.name.Contains(search)
                    || c.description.Contains(search);
            }

            return searchBy;
        }

        private Func<ConferenceEntity, object> GetOrderByFunc(string sortBy)
        {
            Func<ConferenceEntity, object> orderByFunc = null;

            if (sortBy == "startDate")
            {
                orderByFunc = c => c.start;
            }
            else if (sortBy == "name")
            {
                orderByFunc = c => c.name;
            }
            else if (sortBy == "callForSpeakersOpeningDate")
            {
                orderByFunc = c => c.callForSpeakersOpens;
            }
            else if (sortBy == "callForSpeakersClosingDate")
            {
                orderByFunc = c => c.callForSpeakersCloses;
            }
            else if (sortBy == "registrationOpens")
            {
                orderByFunc = c => c.registrationOpens;
            }
            else
            {
                orderByFunc = c => c.end;
            }

            return orderByFunc;
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

    public static class Helpers
    {
        //public static string GenerateSlug(this string phrase)
        //{
        //    string slug = phrase.RemoveAccent().ToLower();
        //    // invalid chars           
        //    slug = Regex.Replace(slug, @"[^a-z0-9\s-]", "");
        //    // convert multiple spaces into one space   
        //    slug = Regex.Replace(slug, @"\s+", " ").Trim();
        //    // cut and trim 
        //    slug = slug.Substring(0, slug.Length <= 45 ? slug.Length : 45).Trim();
        //    slug = Regex.Replace(slug, @"\s", "-"); // hyphens   
        //    return slug;
        //}

        //public static string RemoveAccent(this string txt)
        //{
        //    byte[] bytes = System.Text.Encoding.GetEncoding("Cyrillic").GetBytes(txt);
        //    return System.Text.Encoding.ASCII.GetString(bytes);
        //}
    }
}