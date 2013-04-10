using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using AutoMapper;
using TekConf.Common.Entities;
using TekConf.RemoteData.Dtos.v1;
using TekConf.RemoteData.v1;
using TekConf.UI.Api.Services.Requests.v1;
using TekConf.UI.Api.UrlResolvers.v1;

using ServiceStack.CacheAccess;
using ServiceStack.Common.Web;
using ServiceStack.ServiceHost;

namespace TekConf.UI.Api.Services.v1
{
	public class SessionsService : MongoServiceBase
	{
		private readonly IEntityConfiguration _configuration;

		private readonly IRepository<ConferenceEntity> _conferenceRepository;

		public ICacheClient CacheClient { get; set; }
		static HttpError ConferenceNotFound = HttpError.NotFound("Conference not found") as HttpError;
		static HashSet<string> NonExistingConferences = new HashSet<string>();

		static HttpError SessionNotFound = HttpError.NotFound("Session not found") as HttpError;
		static HashSet<string> NonExistingSessions = new HashSet<string>();

		public SessionsService(IEntityConfiguration configuration, IRepository<ConferenceEntity> conferenceRepository)
		{
			_configuration = configuration;
			_conferenceRepository = conferenceRepository;
		}

		public object Get(Sessions request)
		{
			if (request.conferenceSlug == default(string))
			{
				throw new HttpError() { StatusCode = HttpStatusCode.BadRequest };
			}

			return GetAllSessions(request);
		}

		private object GetAllSessions(Sessions request)
		{
			var cacheKey = "GetAllSessions-" + request.conferenceSlug;
			lock (NonExistingConferences)
			{
				if (NonExistingConferences.Contains(request.conferenceSlug))
				{
					throw ConferenceNotFound;
				}
			}
			var expireInTimespan = new TimeSpan(0, 0, _configuration.cacheTimeout);
			return base.RequestContext.ToOptimizedResultUsingCache(this.CacheClient, cacheKey, expireInTimespan, () =>
			{
				var conference = _conferenceRepository
				.AsQueryable()
					//.Where(c => c.isLive)
				.SingleOrDefault(c => c.slug.ToLower() == request.conferenceSlug.ToLower());

				if (conference.IsNull())
				{
					lock (NonExistingConferences)
					{
						NonExistingConferences.Add(request.conferenceSlug);
					}
					throw ConferenceNotFound;
				}

				var sessionsDtos = Mapper.Map<IEnumerable<SessionEntity>, List<SessionsDto>>(conference.sessions);
				foreach (var sessionsDto in sessionsDtos)
				{
					var sessionsUrlResolver = new SessionsUrlResolver(request.conferenceSlug);
					var sessionsSpeakersUrlResolver = new SessionsSpeakersUrlResolver(request.conferenceSlug);
					var sessionsLinksUrlResolver = new SessionsLinksUrlResolver(request.conferenceSlug);
					var sessionsSubjectsUrlResolver = new SessionsSubjectsUrlResolver(request.conferenceSlug);
					var sessionsTagsUrlResolver = new SessionsTagsUrlResolver(request.conferenceSlug);
					var sessionsPrerequisitesUrlResolver = new SessionsPrerequisitesUrlResolver(request.conferenceSlug);

					sessionsDto.url = sessionsUrlResolver.ResolveUrl(sessionsDto.slug);
					sessionsDto.speakersUrl = sessionsSpeakersUrlResolver.ResolveUrl(sessionsDto.slug);
					sessionsDto.linksUrl = sessionsLinksUrlResolver.ResolveUrl(sessionsDto.slug);
					sessionsDto.subjectsUrl = sessionsSubjectsUrlResolver.ResolveUrl(sessionsDto.slug);
					sessionsDto.tagsUrl = sessionsTagsUrlResolver.ResolveUrl(sessionsDto.slug);
					sessionsDto.prerequisitesUrl = sessionsPrerequisitesUrlResolver.ResolveUrl(sessionsDto.slug);

				}
				return sessionsDtos.ToList();
			});


		}
	}
}