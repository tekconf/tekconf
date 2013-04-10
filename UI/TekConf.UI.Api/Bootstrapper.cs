using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using AutoMapper;
using MongoDB.Bson.Serialization;
using TekConf.Common.Entities;
using TekConf.Common.Entities.Messages;
using TekConf.RemoteData.Dtos.v1;
using TekConf.UI.Api.Services.Requests.v1;
using TinyMessenger;

namespace TekConf.UI.Api
{
	public class Bootstrapper
	{
		public void BootstrapMongoDb(Funq.Container container)
		{
			var hub = container.Resolve<ITinyMessengerHub>();

			var repository = new ConferenceRepository(new Configuration());

			BsonClassMap.RegisterClassMap<ConferenceEntity>()
					.SetCreator(() => new ConferenceEntity(hub, repository));
		}

		public void BootstrapAutomapper(Funq.Container container)
		{
			Mapper.AddFormatter<TrimmingFormatter>();

			Mapper.CreateMap<ConferenceEntity, ConferenceEntity>()
							.ForMember(c => c._id, opt => opt.Ignore())
							.ForMember(dest => dest.imageUrl, opt => opt.ResolveUsing<ImageResolver>())
							.ConstructUsing((Func<ResolutionContext, ConferenceEntity>)(r => new ConferenceEntity(container.Resolve<ITinyMessengerHub>(), container.Resolve<IRepository<ConferenceEntity>>())));

			Mapper.CreateMap<ConferenceEntity, FullConferenceDto>()
							.ForMember(dest => dest.imageUrl, opt => opt.ResolveUsing<ImageResolver>())
							.ForMember(dest => dest.numberOfSessions, opt => opt.ResolveUsing<SessionsCounterResolver>());

			Mapper.CreateMap<SessionEntity, SessionsDto>()
							.ForMember(dest => dest.url, opt => opt.Ignore());

			Mapper.CreateMap<SessionEntity, SessionDto>()
							.ForMember(dest => dest.url, opt => opt.Ignore())
							.ForMember(dest => dest.speakersUrl, opt => opt.Ignore());

			Mapper.CreateMap<SessionEntity, FullSessionDto>();

			Mapper.CreateMap<SpeakerEntity, SpeakersDto>()
							.ForMember(dest => dest.url, opt => opt.Ignore());

			Mapper.CreateMap<AddSession, SessionEntity>()
							.ForMember(s => s._id, opt => opt.UseValue(Guid.NewGuid()))
							.ForMember(s => s.speakers, opt => opt.UseValue(new List<SpeakerEntity>()))
							.ForMember(dest => dest.start, opt => opt.ResolveUsing<SessionStartTimeZoneResolver>())
							.ForMember(dest => dest.end, opt => opt.ResolveUsing<SessionEndTimeZoneResolver>())
							;

			Mapper.CreateMap<SessionEntity, SessionEntity>()
				.ForMember(s => s._id, opt => opt.Ignore())
				.ForMember(s => s.speakers, opt => opt.Ignore())
				;

			Mapper.CreateMap<CreatePresentation, PresentationEntity>()
						.ForMember(p => p._id, opt => opt.UseValue(Guid.NewGuid()))
						.ForMember(p => p.slug, opt => opt.ResolveUsing<PresentationSlugResolver>());

			Mapper.CreateMap<CreatePresentationHistory, HistoryEntity>();
			Mapper.CreateMap<HistoryEntity, HistoryDto>();

			Mapper.CreateMap<PresentationEntity, PresentationDto>();


			Mapper.CreateMap<User, UserEntity>()
							.ForMember(u => u._id, opt => opt.UseValue(Guid.NewGuid()));

			Mapper.CreateMap<CreateConference, ConferenceEntity>()
							.ForMember(c => c._id, opt => opt.Ignore())
							.ForMember(dest => dest.start, opt => opt.ResolveUsing<ConferenceStartTimeZoneResolver>())
							.ForMember(dest => dest.end, opt => opt.ResolveUsing<ConferenceEndTimeZoneResolver>())
							.ForMember(c => c.position, opt => opt.ResolveUsing<PositionResolver>())
							.ConstructUsing((Func<ResolutionContext, ConferenceEntity>)(r => new ConferenceEntity(container.Resolve<ITinyMessengerHub>(), container.Resolve<IRepository<ConferenceEntity>>())));


			Mapper.CreateMap<CreateSpeaker, SpeakerEntity>()
							.ForMember(c => c._id, opt => opt.UseValue(Guid.NewGuid()))
							;


			Mapper.CreateMap<SpeakerEntity, SpeakerDto>();

			Mapper.CreateMap<SpeakerEntity, FullSpeakerDto>();

			Mapper.CreateMap<ScheduleEntity, ScheduleDto>()
							.ForMember(dto => dto.conferenceSlug, opt => opt.MapFrom(entity => entity.ConferenceSlug))
							.ForMember(dto => dto.sessions, opt => opt.ResolveUsing<ScheduleSessionResolver>())
							;

			Mapper.CreateMap<AddressEntity, Address>();
			Mapper.CreateMap<Address, AddressEntity>();
			Mapper.CreateMap<AddressEntity, AddressDto>();

			Mapper.CreateMap<ConferenceCreatedMessage, ConferenceCreatedDto>();
			Mapper.CreateMap<ConferenceEndDateChangedMessage, ConferenceEndDateChangedDto>();
			Mapper.CreateMap<ConferenceLocationChangedMessage, ConferenceLocationChangedDto>();
			Mapper.CreateMap<ConferencePublishedMessage, ConferencePublishedDto>();
			Mapper.CreateMap<ConferenceStartDateChangedMessage, ConferenceStartDateChangedDto>();
			Mapper.CreateMap<ConferenceUpdatedMessage, ConferenceUpdatedDto>();
			Mapper.CreateMap<ScheduleCreatedMessage, ScheduleCreatedDto>();
			Mapper.CreateMap<SessionAddedMessage, SessionAddedDto>();
			Mapper.CreateMap<SessionAddedToScheduleMessage, SessionAddedToScheduleDto>();
			Mapper.CreateMap<SessionRemovedMessage, SessionRemovedDto>();
			Mapper.CreateMap<SessionRoomChangedMessage, SessionRoomChangedDto>();
			Mapper.CreateMap<SpeakerAddedMessage, SpeakerAddedDto>();
			Mapper.CreateMap<SpeakerRemovedMessage, SpeakerRemovedDto>();

		}

	}

	public class PositionResolver : ValueResolver<CreateConference, double[]>
	{
		protected override double[] ResolveCore(CreateConference source)
		{
			return new double[] { source.longitude, source.latitude };
		}
	}

	public class PresentationSlugResolver : ValueResolver<CreatePresentation, string>
	{
		protected override string ResolveCore(CreatePresentation source)
		{
			return source.Title.GenerateSlug();
		}
	}

	public class SessionStartTimeZoneResolver : ValueResolver<AddSession, DateTime>
	{
		protected override DateTime ResolveCore(AddSession source)
		{
			TimeZoneInfo est = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
			return TimeZoneInfo.ConvertTimeFromUtc(source.start, est);
		}
	}

	public class SessionEndTimeZoneResolver : ValueResolver<AddSession, DateTime>
	{
		protected override DateTime ResolveCore(AddSession source)
		{
			TimeZoneInfo est = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
			return TimeZoneInfo.ConvertTimeFromUtc(source.end, est);
		}
	}

	public class ConferenceStartTimeZoneResolver : ValueResolver<CreateConference, DateTime?>
	{
		protected override DateTime? ResolveCore(CreateConference source)
		{
			TimeZoneInfo est = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
			return TimeZoneInfo.ConvertTimeFromUtc(source.start, est);
		}
	}

	public class ConferenceEndTimeZoneResolver : ValueResolver<CreateConference, DateTime?>
	{
		protected override DateTime? ResolveCore(CreateConference source)
		{

			TimeZoneInfo est = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
			return TimeZoneInfo.ConvertTimeFromUtc(source.end, est);
		}
	}

	public class SessionsCounterResolver : ValueResolver<ConferenceEntity, int>
	{
		protected override int ResolveCore(ConferenceEntity source)
		{
			return source.sessions.Count();
		}
	}

	public class ConferenceResolver : ValueResolver<ScheduleEntity, string>
	{
		protected override string ResolveCore(ScheduleEntity source)
		{
			var conferenceName = string.Empty;

			var repository = new ConferenceRepository(new Configuration());
			var conference = repository.AsQueryable()
																.SingleOrDefault(c => c.slug.ToLower() == source.ConferenceSlug.ToLower());
			if (conference.IsNotNull())
			{
				conferenceName = conference.name;
			}

			return conferenceName;
		}
	}
	public class ScheduleSessionResolver : ValueResolver<ScheduleEntity, List<FullSessionDto>>
	{
		protected override List<FullSessionDto> ResolveCore(ScheduleEntity source)
		{
			var sessions = new List<FullSessionDto>();

			var repository = new ConferenceRepository(new Configuration());
			var conference = repository.AsQueryable()
															.SingleOrDefault(c => c.slug.ToLower() == source.ConferenceSlug.ToLower());

			foreach (var sessionSlug in source.SessionSlugs)
			{
				var session = conference.sessions.SingleOrDefault(c => c.slug == sessionSlug);
				var sessionDto = Mapper.Map<FullSessionDto>(session);
				sessions.Add(sessionDto);
			}

			return sessions;
		}
	}

	public class ImageResolver : ValueResolver<ConferenceEntity, string>
	{
		protected override string ResolveCore(ConferenceEntity source)
		{
			var webUrl = ConfigurationManager.AppSettings["webUrl"];

			if (string.IsNullOrWhiteSpace(source.imageUrl))
			{
				return webUrl + "/img/conferences/DefaultConference.png";
			}
			else if (!source.imageUrl.StartsWith("http"))
			{
				return webUrl + source.imageUrl;
			}
			else
			{
				return source.imageUrl;
			}
		}
	}
	public class TrimmingFormatter : BaseFormatter<string>
	{
		protected override string FormatValueCore(string value)
		{
			return value.Trim();
		}
	}

	public abstract class BaseFormatter<T> : IValueFormatter
	{
		public string FormatValue(ResolutionContext context)
		{
			if (context.SourceValue.IsNull())
			{
				return null;
			}

			if (!(context.SourceValue is T))
			{
				return context.SourceValue.IsNull() ? String.Empty : context.SourceValue.ToString();
			}

			return FormatValueCore((T)context.SourceValue);
		}

		protected abstract string FormatValueCore(T value);
	}
}