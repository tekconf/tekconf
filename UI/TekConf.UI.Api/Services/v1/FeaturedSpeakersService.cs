using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using TekConf.RemoteData.Dtos.v1;
using TekConf.UI.Api.Services.Requests.v1;
using FluentMongo.Linq;
using ServiceStack.CacheAccess;
using ServiceStack.ServiceHost;

namespace TekConf.UI.Api.Services.v1
{
	public class FeaturedSpeakersService : MongoServiceBase
	{
		private readonly IConfiguration _configuration;

		private readonly IRepository<ConferenceEntity> _conferenceRepository;

		public ICacheClient CacheClient { get; set; }

		public FeaturedSpeakersService(IConfiguration configuration, IRepository<ConferenceEntity> conferenceRepository)
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
					var featuredSpeakers = _conferenceRepository
																.AsQueryable()
																.Where(c => c.isLive)
																.ToList()
																.Where(c => c.sessions != null)
																.SelectMany(c => c.sessions)
																.Where(s => s.speakers != null)
																.SelectMany(s => s.speakers)
																.Where(s => s.isFeatured)
																.Where(s => !string.IsNullOrWhiteSpace(s.description))
																.Distinct()
																.Take(3);

				var speakersDto = Mapper.Map<List<FullSpeakerDto>>(featuredSpeakers);
				return speakersDto.ToList();
			});

		}
	}
}