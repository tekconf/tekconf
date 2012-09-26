using System;
using System.Linq;
using TekConf.UI.Api.Services.Requests.v1;
using FluentMongo.Linq;
using ServiceStack.CacheAccess;
using ServiceStack.ServiceHost;

namespace TekConf.UI.Api.Services.v1
{
    public class FeaturedSpeakersService : MongoRestServiceBase<FeaturedSpeakersRequest>
    {
        public ICacheClient CacheClient { get; set; }

        public override object OnGet(FeaturedSpeakersRequest request)
        {
            return GetAllSpeakers(request);
        }

        private object GetAllSpeakers(FeaturedSpeakersRequest request)
        {
            var cacheKey = "GetFeaturedSpeakers";
            var expireInTimespan = new TimeSpan(0, 0, 20);
            return base.RequestContext.ToOptimizedResultUsingCache(this.CacheClient, cacheKey, expireInTimespan, () =>
            {
                var collection = this.RemoteDatabase.GetCollection<ConferenceEntity>("app4727263");

                var featuredSpeakers = collection
                                        .AsQueryable()
                                        .ToList()
                                        .Where(c => c.sessions != null)
                                        .SelectMany(c => c.sessions)
                                        .Where(s => s.speakers != null)
                                        .SelectMany(s => s.speakers)
                                        .Where(s => s.isFeatured)
                                        .Where(s => !string.IsNullOrWhiteSpace(s.description))
                                        .Distinct()
                                        .Take(3);

                return featuredSpeakers.ToList();
            });

        }
    }
}