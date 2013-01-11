using System;
using System.Linq;
using ServiceStack.CacheAccess;
using ServiceStack.ServiceHost;
using TekConf.RemoteData.Shared.v1.Requests;

namespace TekConf.UI.Api.Services.v1
{
	public class ConferenceStartDateChangedService : MongoServiceBase
	{
		private readonly IRepository<ConferenceStartDateChangedMessage> _repository;
		public ICacheClient CacheClient { get; set; }

		public ConferenceStartDateChangedService(IRepository<ConferenceStartDateChangedMessage> repository)
		{
			_repository = repository;
		}

		public object Get(ConferenceStartDateChanged request)
		{
			var cacheKey = "GetConferenceStartDateChanged";
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