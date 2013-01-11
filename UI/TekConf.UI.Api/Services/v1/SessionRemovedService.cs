using System;
using System.Linq;
using ServiceStack.CacheAccess;
using ServiceStack.ServiceHost;
using TekConf.RemoteData.Shared.v1.Requests;

namespace TekConf.UI.Api.Services.v1
{
	public class SessionRemovedService : MongoServiceBase
	{
		private readonly IRepository<SessionRemovedMessage> _repository;
		public ICacheClient CacheClient { get; set; }

		public SessionRemovedService(IRepository<SessionRemovedMessage> repository)
		{
			_repository = repository;
		}

		public object Get(SessionRemoved request)
		{
			var cacheKey = "GetSessionRemoved";
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