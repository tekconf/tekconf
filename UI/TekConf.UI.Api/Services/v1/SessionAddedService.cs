using System;
using System.Linq;
using ServiceStack.CacheAccess;
using ServiceStack.ServiceHost;
using TekConf.RemoteData.Shared.v1.Requests;

namespace TekConf.UI.Api.Services.v1
{
	public class SessionAddedService : MongoServiceBase
	{
		private readonly IRepository<SessionAddedMessage> _repository;
		public ICacheClient CacheClient { get; set; }

		public SessionAddedService(IRepository<SessionAddedMessage> repository)
		{
			_repository = repository;
		}

		public object Get(SessionAdded request)
		{
			var cacheKey = "GetSessionAdded";
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