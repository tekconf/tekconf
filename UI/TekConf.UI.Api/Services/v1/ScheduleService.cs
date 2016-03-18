using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using AutoMapper;
using TekConf.Common.Entities;
using TekConf.Common.Entities.Messages;
using TekConf.RemoteData.Dtos.v1;
using TekConf.UI.Api.Services;
using TekConf.UI.Api.Services.Requests.v1;
using ServiceStack.CacheAccess;
using ServiceStack.Common.Web;
using TinyMessenger;

namespace TekConf.UI.Api.v1
{
	public class ScheduleService : MongoServiceBase
	{
		private readonly ITinyMessengerHub _hub;
		private readonly IRepository<ScheduleEntity> _scheduleRepository;

		private readonly IRepository<ConferenceEntity> _conferenceRepository;

		public ICacheClient CacheClient { get; set; }

		public ScheduleService(ITinyMessengerHub hub, IRepository<ScheduleEntity> scheduleRepository, IRepository<ConferenceEntity> conferenceRepository)
		{
			_hub = hub;
			_scheduleRepository = scheduleRepository;
			_conferenceRepository = conferenceRepository;
		}

		public object Get(Schedule request)
		{
			if (request.conferenceSlug == default(string))
			{
				return new HttpError() { StatusCode = HttpStatusCode.BadRequest };
			}

			return GetSingleSchedule(request);
		}

		public object Post(AddSessionToSchedule request)
		{
			ScheduleEntity schedule = _scheduleRepository.AsQueryable()
																	.Where(x => x.UserName == request.userName)
																	.FirstOrDefault(s => s.ConferenceSlug.ToLower() == request.conferenceSlug.ToLower());

			if (schedule.IsNull())
			{
				schedule = new ScheduleEntity()
											 {
												 _id = Guid.NewGuid(),
												 ConferenceSlug = request.conferenceSlug,
												 UserName = request.userName,
												 SessionSlugs = new List<string>(),
											 };
				_hub.Publish(new ScheduleCreatedMessage() { UserName = request.userName, ConferenceSlug = request.conferenceSlug });
			}

			var conference =
					_conferenceRepository.AsQueryable()
					.FirstOrDefault(c => c.slug == request.conferenceSlug);

			if (!conference.IsNull())
			{
				if (!string.IsNullOrWhiteSpace(request.sessionSlug) && !schedule.SessionSlugs.Any(s => s == request.sessionSlug))
				{
					_hub.Publish(new SessionAddedToScheduleMessage() { UserName = request.userName, ConferenceSlug = request.conferenceSlug, SessionSlug = request.sessionSlug });

					schedule.SessionSlugs.Add(request.sessionSlug);
				}
			}

			_scheduleRepository.Save(schedule);

			this.CacheClient.FlushAll();

			var scheduleDto = Mapper.Map<ScheduleDto>(schedule);

			return scheduleDto;
		}

		public object Delete(RemoveSessionFromSchedule request)
		{
			if (string.IsNullOrWhiteSpace(request.userName))
				return new HttpError(HttpStatusCode.NotFound, "", "UserName is required");
			if (string.IsNullOrWhiteSpace(request.conferenceSlug))
				return new HttpError(HttpStatusCode.NotFound, "", "ConferenceSlug is required");

			var schedule = _scheduleRepository.AsQueryable()
												.Where(x => x.UserName == request.userName)
												.FirstOrDefault(s => s.ConferenceSlug.ToLower() == request.conferenceSlug.ToLower());

			if (schedule.IsNull())
				return new HttpError(HttpStatusCode.NotFound, "", "Could not find conference schedule. ConferenceSlug:" + request.conferenceSlug + " UserName:" + request.userName);

			if (string.IsNullOrWhiteSpace(request.sessionSlug))
			{
				_scheduleRepository.Remove(schedule._id);
			}
			else
			{
				if (schedule.SessionSlugs.IsNull() || !schedule.SessionSlugs.Any(s => s == request.sessionSlug))
					return new HttpError(HttpStatusCode.NotFound, "Could not find session schedule. ConferenceSlug:" + request.conferenceSlug + " SessionSlug:" + request.sessionSlug + " UserName:"  + request.userName);

				schedule.SessionSlugs.Remove(request.sessionSlug);
				_scheduleRepository.Save(schedule);

			}

			this.CacheClient.FlushAll();

			var scheduleDto = Mapper.Map<ScheduleDto>(schedule);

			return scheduleDto;
		}

		private object GetSingleSchedule(Schedule request)
		{
			return GetSchedule(request);
		}

		private ScheduleDto GetSchedule(Schedule request)
		{
			var schedule = _scheduleRepository
												 .AsQueryable()
												 .Where(s => s.ConferenceSlug.ToLower() == request.conferenceSlug.ToLower())
												 .FirstOrDefault(s => s.UserName.ToLower() == request.userName.ToLower());

			var scheduleDto = Mapper.Map<ScheduleEntity, ScheduleDto>(schedule);
			foreach (var sessionDto in scheduleDto.sessions)
			{
				sessionDto.isAddedToSchedule = true;
			}
			return scheduleDto;
		}
	}
}