using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using ServiceStack.CacheAccess;
using ServiceStack.ServiceHost;
using TekConf.RemoteData.Dtos.v1;
using TekConf.RemoteData.Shared.v1.Requests;

namespace TekConf.UI.Api.Services.v1
{
	using TekConf.Common.Entities;

	public class SessionAddedService : MongoServiceBase
	{
		private readonly IRepository<SessionAddedMessage> _repository;
		private readonly IEntityConfiguration _configuration;
		public ICacheClient CacheClient { get; set; }

		public SessionAddedService(IRepository<SessionAddedMessage> repository, IEntityConfiguration configuration)
		{
			_repository = repository;
			_configuration = configuration;
		}

		public object Get(SessionAdded request)
		{
			var cacheKey = "GetSessionAdded";
			var expireInTimespan = new TimeSpan(0, 0, _configuration.cacheTimeout);

			return base.RequestContext.ToOptimizedResultUsingCache(this.CacheClient, cacheKey, expireInTimespan, () =>
				{
					var events = _repository
						.AsQueryable()
						.ToList();

					var eventDtos = Mapper.Map<List<SessionAddedMessage>>(events);

					return eventDtos;
				});
		}
	}
}