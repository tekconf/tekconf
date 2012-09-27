using System;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
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

        private object GetFullSingleConference(Conference request)
        {
            var cacheKey = "GetFullSingleConference-" + request.conferenceSlug;
            var expireInTimespan = new TimeSpan(0, 0, 20);
            return base.RequestContext.ToOptimizedResultUsingCache(this.CacheClient, cacheKey, expireInTimespan, () =>
            {
                var collection = this.RemoteDatabase.GetCollection<ConferenceEntity>("app4727263");
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
            return GetAllConferences();
        }

        private object GetAllConferences()
        {
            var cacheKey = "GetAllConferences";
            var collection = this.RemoteDatabase.GetCollection<ConferenceEntity>("app4727263");

            ////TODO : Fix slugs
            //try
            //{
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


            ////TODO : Take Conference to the next year
            //var thatConf = collection.AsQueryable().FirstOrDefault(c => c.slug == "ThatConference-2012");
            //if (thatConf != null)
            //{
            //    var nextConf = Mapper.Map<ConferenceEntity>(thatConf);
            //    if (nextConf != null)
            //    {
            //        nextConf._id = Guid.NewGuid();
            //        nextConf.start = thatConf.start.AddYears(1);
            //        nextConf.end = thatConf.end.AddYears(1);
            //        nextConf.slug = "ThatConference-2013";
            //        collection.Save(nextConf);
            //    }
            //}

            var expireInTimespan = new TimeSpan(0, 0, 20);

            return base.RequestContext.ToOptimizedResultUsingCache(this.CacheClient, cacheKey, expireInTimespan, () =>
            {
                var conferencesDtos = this.RemoteDatabase.GetCollection<ConferenceEntity>("app4727263")
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