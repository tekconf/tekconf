using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using AutoMapper;
using MongoDB.Bson.Serialization;
using TekConf.RemoteData.Dtos.v1;
using TekConf.UI.Api;
using TekConf.UI.Api.Services.Requests.v1;
using TinyMessenger;
using Configuration = TekConf.UI.Api.Configuration;

namespace TekConf.UI.Web
{
	public class Bootstrapper
	{
		public void BootstrapMongoDb()
		{
			ITinyMessengerHub hub = new TinyMessengerHub();

			var repository = new ConferenceRepository(new Configuration());

			BsonClassMap.RegisterClassMap<ConferenceEntity>()
					.SetCreator(() => new ConferenceEntity(hub, repository));
		}

		public void BootstrapAutomapper()
		{
			Mapper.CreateMap<FullConferenceDto, CreateConference>()
					.ForMember(dest => dest.latitude, opt => opt.ResolveUsing<LatitudeResolver>())
					.ForMember(dest => dest.longitude, opt => opt.ResolveUsing<LongitudeResolver>())
					;
			Mapper.CreateMap<AddressDto, Address>();

			Mapper.CreateMap<FullSessionDto, AddSession>();

			Mapper.CreateMap<FullSpeakerDto, CreateSpeaker>();

			Mapper.CreateMap<SessionEntity, SessionDto>()
				.ForMember(dest => dest.url, opt => opt.Ignore())
				.ForMember(dest => dest.speakersUrl, opt => opt.Ignore());

			Mapper.CreateMap<SessionEntity, SessionEntity>()
	.ForMember(s => s._id, opt => opt.Ignore())
	;

			Mapper.CreateMap<SpeakerEntity, FullSpeakerDto>();
			Mapper.CreateMap<AddressEntity, AddressDto>();
			Mapper.CreateMap<ConferenceEntity, FullConferenceDto>()
											.ForMember(dest => dest.imageUrl, opt => opt.ResolveUsing<ImageResolver>())
											.ForMember(dest => dest.numberOfSessions, opt => opt.ResolveUsing<SessionsCounterResolver>());

			Mapper.CreateMap<AddSession, SessionEntity>()
				.ForMember(s => s._id, opt => opt.UseValue(Guid.NewGuid()))
				.ForMember(dest => dest.start, opt => opt.ResolveUsing<StartTimeZoneResolver>())
				.ForMember(dest => dest.end, opt => opt.ResolveUsing<EndTimeZoneResolver>())
				.ForMember(s => s.speakers, opt => opt.UseValue(new List<SpeakerEntity>()))
				;

			Mapper.CreateMap<SessionEntity, FullSessionDto>();

		}

		public class SessionsCounterResolver : ValueResolver<ConferenceEntity, int>
		{
			protected override int ResolveCore(ConferenceEntity source)
			{
				return source.sessions.Count();
			}
		}

		public class StartTimeZoneResolver : ValueResolver<AddSession, DateTime>
		{
			protected override DateTime ResolveCore(AddSession source)
			{
				TimeZoneInfo est = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
				return TimeZoneInfo.ConvertTimeFromUtc(source.start, est);
			}
		}

		public class EndTimeZoneResolver : ValueResolver<AddSession, DateTime>
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

		public class LatitudeResolver : ValueResolver<FullConferenceDto, double>
		{
			protected override double ResolveCore(FullConferenceDto source)
			{
				if (source != null && source.position != null && source.position.Length == 2)
				{
					return source.position[1];
				}
				else
				{
					return 0.0;
				}
			}
		}

		public class LongitudeResolver : ValueResolver<FullConferenceDto, double>
		{
			protected override double ResolveCore(FullConferenceDto source)
			{
				if (source != null && source.position != null && source.position.Length == 2)
				{
					return source.position[0];
				}
				else
				{
					return 0.0;
				}
			}
		}
	}
}