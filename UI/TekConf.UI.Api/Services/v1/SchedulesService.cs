using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using FluentMongo.Linq;
using ServiceStack.CacheAccess;
using TekConf.RemoteData.Dtos.v1;
using TekConf.UI.Api.Services;
using TekConf.UI.Api.Services.Requests.v1;
using TekConf.UI.Api.Services.v1;

namespace TekConf.UI.Api.v1
{
	public class SchedulesService : MongoServiceBase
	{
		public ICacheClient CacheClient { get; set; }

		public object Get(Schedules request)
		{
			List<ScheduleEntity> schedules = null;
			List<FullConferenceDto> conferences = new List<FullConferenceDto>();
			
			try
			{
				schedules = this.RemoteDatabase.GetCollection<ScheduleEntity>("schedules")
				                .AsQueryable()
				                .Where(s => s.UserName.ToLower() == request.userName.ToLower())
				                .ToList();

				foreach (var schedule in schedules)
				{
					var conference = this.RemoteDatabase.GetCollection<ConferenceEntity>("conferences")
					                     .AsQueryable()
					                     .SingleOrDefault(c => c.slug == schedule.ConferenceSlug);
					var conferenceDto = Mapper.Map<ConferenceEntity, FullConferenceDto>(conference);
					conferences.Add(conferenceDto);
				}
			}
			catch (Exception ex)
			{
				throw;
			}
			
			//var schedulesDto = Mapper.Map<List<ScheduleEntity>, List<SchedulesDto>>(schedules);

			return conferences;
		}
	}
}