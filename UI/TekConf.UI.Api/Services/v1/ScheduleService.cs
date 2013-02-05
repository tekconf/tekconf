using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using AutoMapper;
using TekConf.RemoteData.Dtos.v1;
using TekConf.UI.Api.Services;
using TekConf.UI.Api.Services.Requests.v1;
using FluentMongo.Linq;
using ServiceStack.CacheAccess;
using ServiceStack.Common.Web;

namespace TekConf.UI.Api.v1
{
	public class ScheduleService : MongoServiceBase
	{
		public ICacheClient CacheClient { get; set; }

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
			var scheduleCollection = this.RemoteDatabase.GetCollection<ScheduleEntity>("schedules");
			ScheduleEntity schedule = scheduleCollection.AsQueryable()
				.Where(x => x.UserName == request.userName)
					.SingleOrDefault(s => s.ConferenceSlug.ToLower() == request.conferenceSlug.ToLower());

			if (schedule == null)
			{
				schedule = new ScheduleEntity()
											 {
												 _id = Guid.NewGuid(),
												 ConferenceSlug = request.conferenceSlug,
												 UserName = request.userName,
												 SessionSlugs = new List<string>(),
											 };
			}

			var repository = new ConferenceRepository(new Configuration());
			var conference =
					repository.AsQueryable()
					.SingleOrDefault(c => c.slug == request.conferenceSlug);

			if (conference != null)
			{
				if (!string.IsNullOrWhiteSpace(request.sessionSlug) && !schedule.SessionSlugs.Any(s => s == request.sessionSlug))
				{
					schedule.SessionSlugs.Add(request.sessionSlug);
				}
			}

			scheduleCollection.Save(schedule);
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
			var schedule = this.RemoteDatabase.GetCollection<ScheduleEntity>("schedules")
												 .AsQueryable()
												 .Where(s => s.ConferenceSlug.ToLower() == request.conferenceSlug.ToLower())
												 .SingleOrDefault(s => s.UserName.ToLower() == request.userName.ToLower());

			var scheduleDto = Mapper.Map<ScheduleEntity, ScheduleDto>(schedule);

			return scheduleDto;
		}
	}
}