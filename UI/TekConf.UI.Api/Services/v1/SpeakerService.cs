using System;
using System.Collections.Generic;
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

namespace TekConf.UI.Api.Services.v1
{
	public class SpeakerService : MongoServiceBase
	{
		private readonly IConfiguration _configuration;

		private readonly IRepository<ConferenceEntity> _conferenceRepository;

		public ICacheClient CacheClient { get; set; }
		static HttpError ConferenceNotFound = HttpError.NotFound("Conference not found") as HttpError;
		static HashSet<string> NonExistingConferences = new HashSet<string>();

		static HttpError SpeakerNotFound = HttpError.NotFound("Speaker not found") as HttpError;
		static HashSet<string> NonExistingSpeakers = new HashSet<string>();

		public SpeakerService(IConfiguration configuration, IRepository<ConferenceEntity> conferenceRepository)
		{
			_configuration = configuration;
			_conferenceRepository = conferenceRepository;
		}

		public object Get(Speaker request)
		{
			if (request.conferenceSlug == default(string))
			{
				throw new HttpError() { StatusCode = HttpStatusCode.BadRequest };
			}

			return GetSingleSpeaker(request);

		}

		private object GetSingleSpeaker(Speaker request)
		{
			var cacheKey = "GetSingleSpeaker-" + request.conferenceSlug + "-" + request.speakerSlug;

			lock (NonExistingConferences)
			{
				if (NonExistingConferences.Contains(request.conferenceSlug))
				{
					throw ConferenceNotFound;
				}
			}

			lock (NonExistingSpeakers)
			{
				if (NonExistingSpeakers.Contains(request.conferenceSlug + "-" + request.speakerSlug))
				{
					throw SpeakerNotFound;
				}
			}

			var expireInTimespan = new TimeSpan(0, 0, _configuration.cacheTimeout);
			return base.RequestContext.ToOptimizedResultUsingCache(this.CacheClient, cacheKey, expireInTimespan, () =>
					{
						var conference = _conferenceRepository
								 .AsQueryable()
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

						var speakersList = new List<SpeakerEntity>();

						//TODO : Linq this
						foreach (var session in conference.sessions)
						{
							if (session.speakers != null)
							{
								foreach (var speakerEntity in session.speakers)
								{
									if (!speakersList.Any(s => s.slug.ToLower() == speakerEntity.slug.ToLower()))
									{
										speakersList.Add(speakerEntity);
									}
								}
							}
						}
						var speakers = speakersList.ToList();

						var speaker = speakers.FirstOrDefault(s => s.slug.ToLower() == request.speakerSlug.ToLower());

						if (speaker == null)
						{
							lock (NonExistingSpeakers)
							{
								NonExistingSpeakers.Add(request.conferenceSlug + "-" + request.speakerSlug);
							}
							throw SpeakerNotFound;
						}

						var speakerDto = Mapper.Map<SpeakerEntity, FullSpeakerDto>(speaker);
						var resolver = new ConferencesSpeakerResolver(request.conferenceSlug, speakerDto.slug);
						speakerDto.url = resolver.ResolveUrl();

						return speakerDto;
					});

		}

		public object Post(CreatePresentation presentation)
		{
			var dto = new PresentationDto()
				{
					Title = presentation.Title,
					Slug = presentation.Title.GenerateSlug(),
					Tags = presentation.Tags,
					Description = presentation.Description
				};

			return dto;
		}

		public object Put(CreatePresentation presentation)
		{
			var x = presentation.Title;

			return null;
		}

	}
}