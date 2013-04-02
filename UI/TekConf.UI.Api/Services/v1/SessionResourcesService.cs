using System;
using System.Linq;
using System.Net;
using TekConf.Common.Entities;
using TekConf.UI.Api.Services.Requests.v1;
using FluentMongo.Linq;
using ServiceStack.CacheAccess;
using ServiceStack.Common.Web;
using ServiceStack.ServiceHost;

namespace TekConf.UI.Api.Services.v1
{
	public class SessionResourcesService : MongoServiceBase
	{
		private readonly IConfiguration _configuration;

		private readonly IRepository<ConferenceEntity> _conferenceRepository;

		public ICacheClient CacheClient { get; set; }

		public SessionResourcesService(IConfiguration configuration, IRepository<ConferenceEntity> conferenceRepository)
		{
			_configuration = configuration;
			_conferenceRepository = conferenceRepository;
		}

		public object Get(SessionResources request)
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
									.SingleOrDefault(c => c.slug.ToLower() == request.conferenceSlug.ToLower());

			if (conference.IsNull())
			{
				throw new HttpError() { StatusCode = HttpStatusCode.NotFound };
			}


			return GetSingleSessionResources(request, conference);
		}

		private object GetSingleSessionResources(SessionResources request, ConferenceEntity conference)
		{
			var cacheKey = "GetSingleSessionResources-" + request.conferenceSlug + "-" + request.sessionSlug;
			var expireInTimespan = new TimeSpan(0, 0, _configuration.cacheTimeout);
			return base.RequestContext.ToOptimizedResultUsingCache(this.CacheClient, cacheKey, expireInTimespan, () =>
			{
				var session = conference.sessions.FirstOrDefault(s => s.slug.ToLower() == request.sessionSlug.ToLower());
				if (session.IsNull())
				{
					throw new HttpError() { StatusCode = HttpStatusCode.NotFound };
				}

				return session.resources;
			});


		}
	}
}