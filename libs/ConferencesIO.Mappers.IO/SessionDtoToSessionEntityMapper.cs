using System;
using ConferencesIO.LocalData.iOS;
using ConferencesIO.RemoteData.Dtos.v1;
using System.Collections.Generic;

namespace ConferencesIO.Mappers.IO
{
	public class SessionDtoToSessionEntityMapper
	{
		public IEnumerable<SessionEntity> MapAll(string conferenceSlug, IEnumerable<FullSessionDto> dtos)
		{
			var entities = new List<SessionEntity>();
			foreach (var dto in dtos)
			{
				var entity = Map (conferenceSlug, dto);
				entities.Add(entity);
			}

			return entities;
		}

		public SessionEntity Map (string conferenceSlug, FullSessionDto dto)
		{
			var entity = new SessionEntity()
			{
				Id = conferenceSlug + "/" + dto.slug,
				description = dto.description,
				difficulty = dto.difficulty,
				end = dto.end,
				//links = dto.links,
				//prerequisites = dto.prerequisites,
				//resources = dto.resources,
				room = dto.room,
				sessionType = dto.sessionType,
				slug = dto.slug,
				start = dto.start,
				//subjects = dto.subjects,
				//tags = dto.tags,
				title = dto.title,
				twitterHashTag = dto.twitterHashTag,
			};

			return entity;
		}
	}
}

