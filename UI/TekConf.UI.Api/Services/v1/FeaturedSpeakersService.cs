using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using TekConf.RemoteData.Dtos.v1;
using TekConf.UI.Api.Services.Requests.v1;

using ServiceStack.CacheAccess;
using ServiceStack.ServiceHost;

namespace TekConf.UI.Api.Services.v1
{
	using TekConf.Common.Entities;

	public class FeaturedSpeakersService : MongoServiceBase
	{
		private readonly IEntityConfiguration _configuration;
		private readonly IConferenceRepository _conferenceRepository;

		public ICacheClient CacheClient { get; set; }

		public FeaturedSpeakersService(IEntityConfiguration configuration, IConferenceRepository conferenceRepository)
		{
			_configuration = configuration;
			_conferenceRepository = conferenceRepository;
		}

		public object Get(FeaturedSpeakers request)
		{
			return GetFeaturedSpeakers(request);
		}

		private object GetFeaturedSpeakers(FeaturedSpeakers request)
		{
			var cacheKey = "GetFeaturedSpeakers";
			var expireInTimespan = new TimeSpan(0, 0, _configuration.cacheTimeout);
			return base.RequestContext.ToOptimizedResultUsingCache(this.CacheClient, cacheKey, expireInTimespan, () =>
					{
						var featuredSpeakers = _conferenceRepository.GetFeaturedSpeakers();

						var speakersDto = Mapper.Map<List<FullSpeakerDto>>(featuredSpeakers);
						return speakersDto.ToList();
					});

		}
	}
}