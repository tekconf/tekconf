using System;
using System.Linq;
using System.Net;
using AutoMapper;

using ServiceStack.CacheAccess;
using ServiceStack.Common.Web;
using TekConf.Common.Entities;
using TekConf.RemoteData.Dtos.v1;
using TekConf.UI.Api.Services.Requests.v1;
using ServiceStack.ServiceHost;
using TinyMessenger;

namespace TekConf.UI.Api.Services.v1
{
	public class ConferenceService : MongoServiceBase
	{
		private readonly ITinyMessengerHub _hub;
		private readonly IRepository<ConferenceEntity> _conferenceRepository;
		private readonly IRepository<ScheduleEntity> _scheduleRepository;
		private readonly IEntityConfiguration _entityConfiguration;
		public ICacheClient CacheClient { get; set; }

		public ConferenceService(ITinyMessengerHub hub, IRepository<ConferenceEntity> conferenceRepository, IRepository<ScheduleEntity> scheduleRepository, IEntityConfiguration entityConfiguration)
		{
			_hub = hub;
			this._conferenceRepository = conferenceRepository;
			this._scheduleRepository = scheduleRepository;
			this._entityConfiguration = entityConfiguration;
		}

		public object Get(Conference request)
		{
			var cacheKey = "GetFullSingleConference-" + request.conferenceSlug;
			var expireInTimespan = new TimeSpan(0, 0, this._entityConfiguration.cacheTimeout);

			return base.RequestContext.ToOptimizedResultUsingCache(this.CacheClient, cacheKey, expireInTimespan, () =>
			{

				var conference = this._conferenceRepository
						.AsQueryable()
					//.Where(c => c.isLive)
						.FirstOrDefault(c => c.slug.ToLower() == request.conferenceSlug.ToLower());

				if (conference.IsNull())
				{
					//return new HttpError(HttpStatusCode.NotFound, "Conference not found.");
					return new FullConferenceDto();
				}

				FullConferenceDto conferenceDto = Mapper.Map<ConferenceEntity, FullConferenceDto>(conference);

				if (!string.IsNullOrWhiteSpace(request.userName))
				{
					var schedule = _scheduleRepository.AsQueryable()
											.Where(x => x.ConferenceSlug == request.conferenceSlug)
											.FirstOrDefault(x => x.UserName == request.userName);
					if (schedule.IsNotNull())
					{
						foreach (var sessionSlug in schedule.SessionSlugs)
						{
							var session = conferenceDto.sessions.FirstOrDefault(x => x.slug == sessionSlug);
							if (session.IsNotNull())
							{
								session.isAddedToSchedule = true;
							}
						}
					}
				}
				return conferenceDto;
			});
		}

		public object Post(CreateSpeaker speaker)
		{
			FullSpeakerDto speakerDto = null;
			try
			{
				if (string.IsNullOrWhiteSpace(speaker.profileImageUrl))
				{
					if (!string.IsNullOrWhiteSpace(speaker.emailAddress))
					{
						var profileImage = new GravatarImage();

						var profileImageUrl = profileImage.GetUrl(speaker.emailAddress, 100, "pg");

						if (profileImage.GravatarExists(profileImageUrl))
						{
							speaker.profileImageUrl = profileImageUrl;
						}
						else
						{
							speaker.profileImageUrl = "/img/speakers/default.png";
						}
					}
					else
					{
						speaker.profileImageUrl = "/img/speakers/default.png";
					}
				}

				var entity = Mapper.Map<SpeakerEntity>(speaker);

				var conference = this._conferenceRepository.AsQueryable()
													.FirstOrDefault(c => c.slug.ToLower() == speaker.conferenceSlug.ToLower());

				if (conference.IsNull())
				{
					return new HttpError() { StatusCode = HttpStatusCode.BadRequest };
				}

				var session = conference.sessions
											.FirstOrDefault(s => s.slug.ToLower() == speaker.sessionSlug.ToLower());
				
				if (session.IsNull())
				{
					return new HttpError() { StatusCode = HttpStatusCode.BadRequest };
				}

				entity.TrimAllProperties();
				conference.AddSpeakerToSession(session.slug, entity);

				conference.Save();
				this.CacheClient.FlushAll();


				speakerDto = Mapper.Map<SpeakerEntity, FullSpeakerDto>(entity);
			}
			catch (Exception ex)
			{
				var s = ex.Message;
				throw;
			}



			return speakerDto;
		}

		public object Put(CreateSpeaker request)
		{
			var conference = this._conferenceRepository.AsQueryable()
															.Where(c => c.slug.ToLower() == request.conferenceSlug.ToLower())
															.FirstOrDefault(c => c.sessions != null);


			if (conference.IsNotNull() && conference.sessions != null)
			{
				var speakers = conference.sessions
						.Where(session => session.speakers != null)
						.SelectMany(session => session.speakers)
						.Where(speaker => speaker.slug.ToLower() == request.slug.ToLower())
						.ToList();

				SpeakerEntity lastSpeakerEntity = null;
				foreach (var speakerEntity in speakers)
				{
					speakerEntity.TrimAllProperties(); ;
					Mapper.Map<CreateSpeaker, SpeakerEntity>(request, speakerEntity);
					lastSpeakerEntity = speakerEntity;
				}


				conference.Save();
				var speakerDto = Mapper.Map<SpeakerEntity, FullSpeakerDto>(lastSpeakerEntity);

				return speakerDto;
			}
			else
			{
				return new HttpError() { StatusCode = HttpStatusCode.BadRequest };
			}
		}

		public object Post(CreateConference conference)
		{
			FullConferenceDto conferenceDto = null;
			try
			{
				var conferenceEntity = Mapper.Map<ConferenceEntity>(conference);

				conferenceEntity.dateAdded = DateTime.Now; // TODO : This logic should be encapsulated
				if (conferenceEntity.isLive)
				{
					conferenceEntity.Publish();
				}

				conferenceEntity.subjects = conferenceEntity.subjects.Where(x => !string.IsNullOrWhiteSpace(x)).Distinct().ToList();
				conferenceEntity.tags = conferenceEntity.tags.Where(x => !string.IsNullOrWhiteSpace(x)).Distinct().ToList();
				conferenceEntity.rooms = conferenceEntity.rooms.Where(x => !string.IsNullOrWhiteSpace(x)).Distinct().ToList();
				conferenceEntity.sessionTypes = conferenceEntity.sessionTypes.Where(x => !string.IsNullOrWhiteSpace(x)).Distinct().ToList();

				conferenceEntity.TrimAllProperties();
				conferenceEntity.Save();
				
				this.CacheClient.FlushAll();

				conferenceDto = Mapper.Map<ConferenceEntity, FullConferenceDto>(conferenceEntity);
			}
			catch (Exception ex)
			{
				var s = ex.Message;
				throw;
			}

			return conferenceDto;
		}

		public object Put(CreateConference conference)
		{
			FullConferenceDto conferenceDto = null;
			try
			{
				var existingConference = _conferenceRepository.AsQueryable()
															.FirstOrDefault(c => c.slug.ToLower() == conference.slug.ToLower());

				bool existingConferenceIsLive = existingConference.isLive;
				Mapper.Map<CreateConference, ConferenceEntity>(conference, existingConference);

				if (!existingConferenceIsLive && existingConference.isLive)
				{
					existingConference.Publish();
				}

				existingConference.subjects = existingConference.subjects.Where(x => !string.IsNullOrWhiteSpace(x)).Distinct().ToList();
				existingConference.tags = existingConference.tags.Where(x => !string.IsNullOrWhiteSpace(x)).Distinct().ToList();
				existingConference.rooms = existingConference.rooms.Where(x => !string.IsNullOrWhiteSpace(x)).Distinct().ToList();
				existingConference.sessionTypes = existingConference.sessionTypes.Where(x => !string.IsNullOrWhiteSpace(x)).Distinct().ToList();

				existingConference.Save();

				this.CacheClient.FlushAll();

				conferenceDto = Mapper.Map<ConferenceEntity, FullConferenceDto>(existingConference);
			}
			catch (Exception ex)
			{
				var s = ex.Message;
				throw;
			}

			return conferenceDto;
		}


	}
}