using System;
using System.Linq;
using System.Net;
using AutoMapper;

using ServiceStack.CacheAccess;
using ServiceStack.Common.Web;
using TekConf.Common.Entities;
using TekConf.RemoteData.Dtos.v1;
using TekConf.UI.Api.Services.Requests.v1;
using TekConf.UI.Api.UrlResolvers.v1;
using ServiceStack.ServiceHost;

namespace TekConf.UI.Api.Services.v1
{
	public class SessionSpeakerService : MongoServiceBase
	{
		private readonly IEntityConfiguration _configuration;

		private readonly IRepository<ConferenceEntity> _conferenceRepository;

		public ICacheClient CacheClient { get; set; }

		public SessionSpeakerService(IEntityConfiguration configuration, IRepository<ConferenceEntity> conferenceRepository)
		{
			_configuration = configuration;
			_conferenceRepository = conferenceRepository;
		}

		public object Get(SessionSpeaker request)
		{
			if (request.conferenceSlug == default(string))
			{
				throw new HttpError() { StatusCode = HttpStatusCode.BadRequest };
			}

			if (request.sessionSlug == default(string))
			{
				throw new HttpError() { StatusCode = HttpStatusCode.BadRequest };
			}
			var conference = _conferenceRepository
					.AsQueryable()
				//.Where(c => c.isLive)
					.FirstOrDefault(c => c.slug.ToLower() == request.conferenceSlug.ToLower());

			if (conference.IsNull())
			{
				throw new HttpError() { StatusCode = HttpStatusCode.NotFound };
			}

			var session = conference.sessions.FirstOrDefault(s => s.slug.ToLower() == request.sessionSlug.ToLower());

			if (session.IsNull())
			{
				throw new HttpError() { StatusCode = HttpStatusCode.NotFound };
			}

			return GetSingleSpeaker(request, session);
		}

		private object GetSingleSpeaker(SessionSpeaker request, SessionEntity session)
		{
			var cacheKey = "GetSingleSpeaker-" + request.conferenceSlug + "-" + request.sessionSlug + "-" + request.speakerSlug;
			var expireInTimespan = new TimeSpan(0, 0, _configuration.cacheTimeout);
			return base.RequestContext.ToOptimizedResultUsingCache(this.CacheClient, cacheKey, expireInTimespan, () =>
																			{
																				var speaker = session.speakers.FirstOrDefault(s => s.slug.ToLower() == request.speakerSlug.ToLower());

																				var speakerDto = Mapper.Map<SpeakerEntity, SpeakerDto>(speaker);
																				var resolver = new SpeakerUrlResolver(request.conferenceSlug, request.sessionSlug, speakerDto.url);
																				speakerDto.url = resolver.ResolveUrl();
																				return speakerDto;
																			});


		}


	}
}