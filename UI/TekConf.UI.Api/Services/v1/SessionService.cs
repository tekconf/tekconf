using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using AutoMapper;
using FluentMongo.Linq;
using ServiceStack.CacheAccess;
using ServiceStack.Common.Web;
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

		static HttpError ConferenceNotFound = HttpError.NotFound("Conference not found") as HttpError;
		static HashSet<string> NonExistingConferences = new HashSet<string>();

		static HttpError SessionNotFound = HttpError.NotFound("Session not found") as HttpError;
		static HashSet<string> NonExistingSessions = new HashSet<string>();

		public SessionService(ITinyMessengerHub hub)
		{
			_hub = hub;
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

			var collection = this.RemoteDatabase.GetCollection<ConferenceEntity>("conferences");
			var conference = collection
					.AsQueryable()
				//.Where(c => c.isLive)
					.FirstOrDefault(x => x.slug.ToLower() == request.conferenceSlug.ToLower());

			if (conference == null)
			{
				return new HttpError() { StatusCode = HttpStatusCode.BadRequest };
			}

			conference.Hub = _hub;
			conference.AddSession(entity);
			conference.Save(collection);

			var sessionDto = Mapper.Map<SessionEntity, SessionDto>(entity);
			sessionDto.conferenceSlug = request.conferenceSlug;
			sessionDto.conferenceName = conference.name;
			return sessionDto;
		}

		public object Put(AddSession request)
		{
			var collection = this.RemoteDatabase.GetCollection<ConferenceEntity>("conferences");
			var conference = collection.AsQueryable().FirstOrDefault(c => c.slug.ToLower() == request.conferenceSlug.ToLower());
			conference.Hub = _hub;
			var session = conference.sessions.FirstOrDefault(s => s.slug.ToLower() == request.slug.ToLower());
			Mapper.Map<AddSession, SessionEntity>(request, session);

			conference.Save(collection);
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
			var expireInTimespan = new TimeSpan(0, 0, 120);
			return base.RequestContext.ToOptimizedResultUsingCache(this.CacheClient, cacheKey, expireInTimespan, () =>
							{
								var conference = this.RemoteDatabase.GetCollection<ConferenceEntity>("conferences")
										.AsQueryable()
									//.Where(s => s.slug.ToLower() == request.sessionSlug.ToLower())
									//.Where(c => c.isLive)
										.SingleOrDefault(c => c.slug.ToLower() == request.conferenceSlug.ToLower());

								if (conference == null)
								{
									lock (NonExistingConferences)
									{
										NonExistingConferences.Add(request.conferenceSlug);
									}
									throw ConferenceNotFound;
								}


								if (conference.sessions == null)
								{
									lock (NonExistingSessions)
									{
										NonExistingSessions.Add(request.conferenceSlug + "-" + request.sessionSlug);
									}
									throw SessionNotFound;
								}

								var session = conference.sessions.FirstOrDefault(s => s.slug.ToLower() == request.sessionSlug.ToLower());

								if (session != null)
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