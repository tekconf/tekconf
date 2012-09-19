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
    public class FeaturedSpeakersService : MongoRestServiceBase<FeaturedSpeakersRequest>
    {
        public ICacheClient CacheClient { get; set; }
        static HttpError SpeakerNotFound = HttpError.NotFound("Speaker not found") as HttpError;
        static HashSet<string> NonExistingSpeakers = new HashSet<string>();

        public override object OnGet(FeaturedSpeakersRequest request)
        {
            return GetAllSpeakers(request);
        }


        private object GetAllSpeakers(FeaturedSpeakersRequest request)
        {
            var cacheKey = "GetFeaturedSpeakers";
            return base.RequestContext.ToOptimizedResultUsingCache(this.CacheClient, cacheKey, () =>
            {
                var collection = this.RemoteDatabase.GetCollection<ConferenceEntity>("conferences");
                var featuredSpeakers = collection.AsQueryable().Select(c => new SpeakersDto() { });

                return featuredSpeakers.ToList();
            });


        }
    }
}