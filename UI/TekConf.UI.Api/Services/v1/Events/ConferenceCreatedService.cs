using System;
using System.Collections.Generic;
using System.Linq;
using ServiceStack.CacheAccess;
using ServiceStack;
using ServiceStack.ServiceHost;
using TekConf.RemoteData.Dtos.v1;
using TekConf.RemoteData.Shared.v1.Requests;

namespace TekConf.UI.Api.Services.v1
{
	using AutoMapper;

	using TekConf.Common.Entities;

	public class ConferenceCreatedService : MongoServiceBase
	{
		private readonly IRepository<ConferenceCreatedMessage> _repository;
		private readonly IEntityConfiguration _entityConfiguration;
		public ICacheClient CacheClient { get; set; }

		public ConferenceCreatedService(IRepository<ConferenceCreatedMessage> repository, IEntityConfiguration entityConfiguration)
		{
			_repository = repository;
			this._entityConfiguration = entityConfiguration;
		}

		public object Get(ConferenceCreated request)
		{
			const string cacheKey = "GetConferenceCreated";
			var expireInTimespan = new TimeSpan(0, 0, this._entityConfiguration.cacheTimeout);

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