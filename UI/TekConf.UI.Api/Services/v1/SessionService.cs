using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using AutoMapper;
using FluentMongo.Linq;
using ServiceStack.CacheAccess;
using ServiceStack.Common.Web;
using TekConf.Common.Entities;
using TekConf.RemoteData.Dtos.v1;
using TekConf.UI.Api.Services.Requests.v1;
using TekConf.UI.Api.UrlResolvers.v1;
using ServiceStack.ServiceHost;
using TinyMessenger;

namespace TekConf.UI.Api.Services.v1
{
	public class SessionService : MongoServiceBase
	{
		public ICacheClient CacheClient { get; set; }
		private ITinyMessengerHub _hub;
		private readonly IConfiguration _configuration;

		private readonly IRepository<ConferenceEntity> _conferenceRepository;

		static HttpError ConferenceNotFound = HttpError.NotFound("Conference not found") as HttpError;
		static HashSet<string> NonExistingConferences = new HashSet<string>();

		static HttpError SessionNotFound = HttpError.NotFound("Session not found") as HttpError;
		static HashSet<string> NonExistingSessions = new HashSet<string>();

		public SessionService(ITinyMessengerHub hub, IConfiguration configuration, IRepository<ConferenceEntity> conferenceRepository)
		{
			_hub = hub;
			_configuration = configuration;
			_conferenceRepository = conferenceRepository;
		}

		public object Get(Session request)
		{
			if (request.conferenceSlug == default(string))
			{
				throw new HttpError() { StatusCode = HttpStatusCode.BadRequest };
			}

			return GetSingleSession(request);
		}

		public object Post(AddSession request)
		{
			var entity = Mapper.Map<SessionEntity>(request);

			var conference = _conferenceRepository
					.AsQueryable()
				//.Where(c => c.isLive)
					.FirstOrDefault(x => x.slug.ToLower() == request.conferenceSlug.ToLower());

			if (conference.IsNull())
			{
				return new HttpError() { StatusCode = HttpStatusCode.BadRequest };
			}

			entity.TrimAllProperties();
			conference.AddSession(entity);
			conference.Save();
			this.CacheClient.FlushAll();

			var sessionDto = Mapper.Map<SessionEntity, SessionDto>(entity);
			sessionDto.conferenceSlug = request.conferenceSlug;
			sessionDto.conferenceName = conference.name;
			return sessionDto;
		}

		public object Put(AddSession request)
		{
			var conference = _conferenceRepository.AsQueryable().FirstOrDefault(c => c.slug.ToLower() == request.conferenceSlug.ToLower());

			var session = conference.sessions.FirstOrDefault(s => s.slug.ToLower() == request.slug.ToLower());
			session.TrimAllProperties();
			Mapper.Map<AddSession, SessionEntity>(request, session);

			conference.Save();
			this.CacheClient.FlushAll();

			var sessionDto = Mapper.Map<SessionEntity, SessionDto>(session);
			sessionDto.conferenceSlug = request.conferenceSlug;
			sessionDto.conferenceName = conference.name;
			return sessionDto;
		}

		private object GetSingleSession(Session request)
		{
			var cacheKey = "GetSingleSession-" + request.conferenceSlug + "-" + request.sessionSlug;

			lock (NonExistingConferences)
			{
				if (NonExistingConferences.Contains(request.conferenceSlug))
				{
					throw ConferenceNotFound;
				}
			}

			lock (NonExistingSessions)
			{
				if (NonExistingSessions.Contains(request.conferenceSlug + "-" + request.sessionSlug))
				{
					throw SessionNotFound;
				}
			}
			var expireInTimespan = new TimeSpan(0, 0, _configuration.cacheTimeout);
			return base.RequestContext.ToOptimizedResultUsingCache(this.CacheClient, cacheKey, expireInTimespan, () =>
							{

								var conference = _conferenceRepository
										.AsQueryable()
									//.Where(s => s.slug.ToLower() == request.sessionSlug.ToLower())
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


								if (conference.sessions.IsNull())
								{
									lock (NonExistingSessions)
									{
										NonExistingSessions.Add(request.conferenceSlug + "-" + request.sessionSlug);
									}
									throw SessionNotFound;
								}

								var session = conference.sessions.FirstOrDefault(s => s.slug.ToLower() == request.sessionSlug.ToLower());

								if (session.IsNotNull())
								{
									var sessionDto = Mapper.Map<SessionEntity, SessionDto>(session);
									var sessionUrlResolver = new SessionUrlResolver(request.conferenceSlug, sessionDto.slug);
									var sessionSpeakersUrlResolver = new SessionSpeakersUrlResolver(request.conferenceSlug, sessionDto.slug);
									var sessionLinksUrlResolver = new SessionLinksUrlResolver(request.conferenceSlug, sessionDto.slug);
									var sessionSubjectsUrlResolver = new SessionSubjectsUrlResolver(request.conferenceSlug, sessionDto.slug);
									var sessionTagsUrlResolver = new SessionTagsUrlResolver(request.conferenceSlug, sessionDto.slug);
									var sessionPrerequisitesUrlResolver = new SessionPrerequisitesUrlResolver(request.conferenceSlug, sessionDto.slug);

									sessionDto.conferenceName = conference.name;
									sessionDto.conferenceSlug = request.conferenceSlug;
									sessionDto.url = sessionUrlResolver.ResolveUrl();
									sessionDto.speakersUrl = sessionSpeakersUrlResolver.ResolveUrl();
									sessionDto.linksUrl = sessionLinksUrlResolver.ResolveUrl();
									sessionDto.subjectsUrl = sessionSubjectsUrlResolver.ResolveUrl();
									sessionDto.tagsUrl = sessionTagsUrlResolver.ResolveUrl();
									sessionDto.prerequisitesUrl = sessionPrerequisitesUrlResolver.ResolveUrl();

									return sessionDto;
								}
								else
								{
									lock (NonExistingSessions)
									{
										NonExistingSessions.Add(request.conferenceSlug + "-" + request.sessionSlug);
									}
									throw SessionNotFound;
								}
							});

		}

	}
}