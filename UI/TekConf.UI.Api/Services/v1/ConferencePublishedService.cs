using System;
using System.Linq;
using ServiceStack.CacheAccess;
using ServiceStack.ServiceHost;
using TekConf.RemoteData.Shared.v1.Requests;

namespace TekConf.UI.Api.Services.v1
{
	public class ConferencePublishedService : MongoServiceBase
	{
		private readonly IRepository<ConferencePublishedMessage> _repository;
		public ICacheClient CacheClient { get; set; }

		public ConferencePublishedService(IRepository<ConferencePublishedMessage> repository)
		{
			_repository = repository;
		}

		public object Get(ConferencePublished request)
		{
			var cacheKey = "GetConferencePublished";
			var expireInTimespan = new TimeSpan(0, 0, 120);

			return base.RequestContext.ToOptimizedResultUsingCache(this.CacheClient, cacheKey, expireInTimespan, () =>
				{
					var events = _repository
						.AsQueryable()
						.ToList();

					return events;
				});
		}
	}
}