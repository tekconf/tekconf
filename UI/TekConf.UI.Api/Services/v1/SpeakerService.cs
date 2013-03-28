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
		private readonly IRepository<PresentationEntity> _presentationRepository;

		public ICacheClient CacheClient { get; set; }
		static HttpError ConferenceNotFound = HttpError.NotFound("Conference not found") as HttpError;
		static HashSet<string> NonExistingConferences = new HashSet<string>();

		static HttpError SpeakerNotFound = HttpError.NotFound("Speaker not found") as HttpError;
		static HashSet<string> NonExistingSpeakers = new HashSet<string>();

		public SpeakerService(IConfiguration configuration, IRepository<ConferenceEntity> conferenceRepository, IRepository<PresentationEntity> presentationRepository)
		{
			_configuration = configuration;
			_conferenceRepository = conferenceRepository;
			_presentationRepository = presentationRepository;
		}

		public object Get(Presentations request)
		{
			if (request.speakerSlug == default(string))
			{
				throw new HttpError() { StatusCode = HttpStatusCode.BadRequest };
			}

			var cacheKey = "GetPresentations-" + request.speakerSlug;
			var expireInTimespan = new TimeSpan(0, 0, _configuration.cacheTimeout);

			return base.RequestContext.ToOptimizedResultUsingCache(this.CacheClient, cacheKey, expireInTimespan, () =>
			{
				var presentations = _presentationRepository
					.AsQueryable()
					.Where(p => p.SpeakerSlug == request.speakerSlug)
					.ToList();

				var presentationDto = Mapper.Map<List<PresentationEntity>, List<PresentationDto>>(presentations);

				return presentationDto;
			});
		}

		public object Get(Presentation request)
		{
			if (request.slug == default(string) || request.speakerSlug == default(string))
			{
				throw new HttpError() { StatusCode = HttpStatusCode.BadRequest };
			}

			var cacheKey = "GetPresentation-" + request.speakerSlug + "-" + request.slug;
			var expireInTimespan = new TimeSpan(0, 0, _configuration.cacheTimeout);
			
			return base.RequestContext.ToOptimizedResultUsingCache(this.CacheClient, cacheKey, expireInTimespan, () =>
				{
					var presentation = _presentationRepository
						.AsQueryable()
						.Where(p => p.SpeakerSlug == request.speakerSlug)
						.SingleOrDefault(p => p.slug.ToLower() == request.slug.ToLower());

					var presentationDto = Mapper.Map<PresentationEntity, PresentationDto>(presentation);

					return presentationDto;
				});
		}

		public object Get(Speaker request)
		{
			if (request.conferenceSlug == default(string))
			{
				throw new HttpError() { StatusCode = HttpStatusCode.BadRequest };
			}

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

				if (conference.IsNull())
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
					if (session.speakers.IsNotNull())
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

				if (speaker.IsNull())
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
			var entity = Mapper.Map<CreatePresentation, PresentationEntity>(presentation);
			entity.UploadedOn = DateTime.Now;

			_presentationRepository.Save(entity);

			var dto = Mapper.Map<PresentationEntity, PresentationDto>(entity);

			return dto;
		}

		public object Put(CreatePresentation presentation)
		{
			var x = presentation.Title;

			return null;
		}

		//public object Post(CreatePresentationHistory history)
		//{
		//	var entity = Mapper.Map<CreatePresentation, PresentationEntity>(history);

		//	_presentationRepository.Save(entity);

		//	var dto = Mapper.Map<PresentationEntity, PresentationDto>(entity);

		//	return dto;
		//}

		public object Put(CreatePresentationHistory history)
		{
			var presentation = _presentationRepository.AsQueryable()
				.Where(x => x.slug == history.PresentationSlug)
				.FirstOrDefault(x => x.SpeakerSlug == history.SpeakerSlug);

			if (presentation.IsNotNull())
			{
				var historyEntity = Mapper.Map<CreatePresentationHistory, HistoryEntity>(history);
				presentation.History.Add(historyEntity);
				_presentationRepository.Save(presentation);
				var dto = Mapper.Map<PresentationEntity, PresentationDto>(presentation);
				
				return dto;
			}

			return new HttpResult(HttpStatusCode.NotFound);
		}

	}
}