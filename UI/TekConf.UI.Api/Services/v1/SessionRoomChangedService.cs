using System;
using System.Linq;
using ServiceStack.CacheAccess;
using ServiceStack.ServiceHost;
using TekConf.RemoteData.Shared.v1.Requests;

namespace TekConf.UI.Api.Services.v1
{

	public class SessionRoomChangedService : MongoServiceBase
	{
		private readonly IRepository<SessionRoomChangedMessage> _repository;
		public ICacheClient CacheClient { get; set; }

		public SessionRoomChangedService(IRepository<SessionRoomChangedMessage> repository)
		{
			_repository = repository;
		}

		public object Get(SessionRoomChanged request)
		{
			var cacheKey = "GetSessionRoomChanged";
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