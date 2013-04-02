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
	public class ConferenceCreatedService : MongoServiceBase
	{
		private readonly IRepository<ConferenceCreatedMessage> _repository;
		private readonly IConfiguration _configuration;
		public ICacheClient CacheClient { get; set; }

		public ConferenceCreatedService(IRepository<ConferenceCreatedMessage> repository, IConfiguration configuration)
		{
			_repository = repository;
			_configuration = configuration;
		}

		public object Get(ConferenceCreated request)
		{
			var cacheKey = "GetConferenceCreated";
			var expireInTimespan = new TimeSpan(0, 0, _configuration.cacheTimeout);

			return base.RequestContext.ToOptimizedResultUsingCache(this.CacheClient, cacheKey, expireInTimespan, () =>
				{
					var events = _repository
						.AsQueryable()
						.ToList();

					var eventDtos = Mapper.Map<List<ConferenceCreatedDto>>(events);
					return eventDtos;
				});
		}
	}
}