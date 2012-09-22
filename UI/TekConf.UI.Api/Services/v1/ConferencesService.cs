using System;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
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

            //try
            //{
            //    var collection = this.RemoteDatabase.GetCollection<ConferenceEntity>("conferences");
            //    foreach (var conference in collection.AsQueryable())
            //    {
            //        conference.slug = (conference.name + "-" + conference.start.Year).GenerateSlug();
            //        foreach (var session in conference.sessions)
            //        {
            //            session.slug = session.title.GenerateSlug();
            //            if (session.speakers != null)
            //            {
            //                foreach (var speaker in session.speakers)
            //                {
            //                    speaker.slug = speaker.fullName.GenerateSlug();
            //                }
            //            }
            //        }
            //        collection.Save(conference);

            //    }
            //}
            //catch (Exception ex)
            //{

            //    var sdsds = ex.Message;
            //}


            //var thatConf = collection.AsQueryable().Where(c => c.slug == "ThatConference-2012").FirstOrDefault();
            //var nextConf = Mapper.Map<ConferenceEntity>(thatConf);
            //nextConf._id = Guid.NewGuid();
            //nextConf.start = thatConf.start.AddYears(1);
            //nextConf.end = thatConf.end.AddYears(1);
            //nextConf.slug = "ThatConference-2013";
            //collection.Save(nextConf);
            var expireInTimespan = new TimeSpan(0, 0, 20);
            return base.RequestContext.ToOptimizedResultUsingCache(this.CacheClient, cacheKey, expireInTimespan, () =>
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
        public static string GenerateSlug(this string phrase)
        {
            string slug = phrase.RemoveAccent().ToLower();
            // invalid chars           
            slug = Regex.Replace(slug, @"[^a-z0-9\s-]", "");
            // convert multiple spaces into one space   
            slug = Regex.Replace(slug, @"\s+", " ").Trim();
            // cut and trim 
            slug = slug.Substring(0, slug.Length <= 45 ? slug.Length : 45).Trim();
            slug = Regex.Replace(slug, @"\s", "-"); // hyphens   
            return slug;
        }

        public static string RemoveAccent(this string txt)
        {
            byte[] bytes = System.Text.Encoding.GetEncoding("Cyrillic").GetBytes(txt);
            return System.Text.Encoding.ASCII.GetString(bytes);
        }
    }
}