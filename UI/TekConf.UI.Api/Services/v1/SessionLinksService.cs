using System;
using System.Linq;
using System.Net;
using TekConf.Common.Entities;
using TekConf.UI.Api.Services.Requests.v1;

using ServiceStack.CacheAccess;
using ServiceStack.Common.Web;
using ServiceStack.ServiceHost;

namespace TekConf.UI.Api.Services.v1
{
	public class SessionLinksService : MongoServiceBase
	{
		private readonly IRepository<ConferenceEntity> _conferenceRepository;

		private readonly IEntityConfiguration _entityConfiguration;
		public ICacheClient CacheClient { get; set; }

		public SessionLinksService(IRepository<ConferenceEntity> conferenceRepository)
		{
			_conferenceRepository = conferenceRepository;
		}

		public SessionLinksService(IEntityConfiguration entityConfiguration)
		{
			this._entityConfiguration = entityConfiguration;
		}

		public object Get(SessionLinks request)
		{
			if (request.conferenceSlug == default(string))
			{
				throw new HttpError() { StatusCode = HttpStatusCode.BadRequest };
			}

			if (request.sessionSlug == default(string))
			{
				throw new HttpError() { StatusCode = HttpStatusCode.BadRequest };
			}

			return GetSingleSessionLinks(request);
		}

		private object GetSingleSessionLinks(SessionLinks request)
		{
			var cacheKey = "GetSingleSessionLinks-" + request.conferenceSlug + "-" + request.sessionSlug;
			var expireInTimespan = new TimeSpan(0, 0, this._entityConfiguration.cacheTimeout);
			return base.RequestContext.ToOptimizedResultUsingCache(this.CacheClient, cacheKey, expireInTimespan, () =>
			{
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

				return session.links;
			});

		}
	}
}