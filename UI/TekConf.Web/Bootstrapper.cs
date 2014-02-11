using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using AutoMapper;
using Funq;
using MongoDB.Bson.Serialization;
using TekConf.RemoteData.Dtos.v1;
using TekConf.UI.Api;
using TekConf.UI.Api.Services.Requests.v1;
using TinyMessenger;
using TekConf.Common.Entities;

namespace TekConf.Web
{
	public class Bootstrapper
	{
		private readonly Container _container;

		public Bootstrapper(Container container)
		{
			_container = container;
		}

		public void BootstrapMongoDb()
		{
			var hub = _container.Resolve<ITinyMessengerHub>();

			var repository = new ConferenceRepository(new EntityConfiguration());

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
					.ForMember(s => s.speakers, opt => opt.Ignore())
					;

			Mapper.CreateMap<SpeakerEntity, SpeakersDto>()
				.ForMember(dest => dest.url, opt => opt.Ignore());

			Mapper.CreateMap<SpeakerEntity, FullSpeakerDto>();
			Mapper.CreateMap<AddressEntity, AddressDto>();
			Mapper.CreateMap<ConferenceEntity, FullConferenceDto>()
											.ForMember(dest => dest.imageUrl, opt => opt.ResolveUsing<ImageResolver>())
                                            .ForMember(dest => dest.imageUrlSquare, opt => opt.ResolveUsing<ImageResolverSquare>())
											.ForMember(dest => dest.numberOfSessions, opt => opt.ResolveUsing<SessionsCounterResolver>());

			Mapper.CreateMap<CreateConference, ConferenceEntity>()
							.ForMember(c => c._id, opt => opt.Ignore())
							.ForMember(dest => dest.start, opt => opt.ResolveUsing<ConferenceStartTimeZoneResolver>())
							.ForMember(dest => dest.end, opt => opt.ResolveUsing<ConferenceEndTimeZoneResolver>())
							.ForMember(c => c.position, opt => opt.ResolveUsing<PositionResolver>())
							.ConstructUsing((Func<ResolutionContext, ConferenceEntity>)(r => new ConferenceEntity(_container.Resolve<ITinyMessengerHub>(), _container.Resolve<IConferenceRepository>())));


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
				//TimeZoneInfo est = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
				//return TimeZoneInfo.ConvertTimeFromUtc(source.start, est);
				return source.start;
			}
		}

		public class EndTimeZoneResolver : ValueResolver<AddSession, DateTime>
		{
			protected override DateTime ResolveCore(AddSession source)
			{
				//TimeZoneInfo est = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
				//return TimeZoneInfo.ConvertTimeFromUtc(source.end, est);
				return source.end;
			}
		}

		public class ConferenceStartTimeZoneResolver : ValueResolver<CreateConference, DateTime?>
		{
			protected override DateTime? ResolveCore(CreateConference source)
			{
				//TimeZoneInfo est = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
				//return TimeZoneInfo.ConvertTimeFromUtc(source.start, est);
				return source.start;
			}
		}

		public class ConferenceEndTimeZoneResolver : ValueResolver<CreateConference, DateTime?>
		{
			protected override DateTime? ResolveCore(CreateConference source)
			{
				//TimeZoneInfo est = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
				//return TimeZoneInfo.ConvertTimeFromUtc(source.end, est);
				return source.end;
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

        public class ImageResolverSquare : ValueResolver<ConferenceEntity, string>
        {
            protected override string ResolveCore(ConferenceEntity source)
            {
                var webUrl = ConfigurationManager.AppSettings["webUrl"];

                if (string.IsNullOrWhiteSpace(source.imageUrlSquare))
                {
                    return webUrl + "/img/conferences/DefaultConferenceSquare.png";
                }
                else if (!source.imageUrlSquare.StartsWith("http"))
                {
                    return webUrl + source.imageUrlSquare;
                }
                else
                {
                    return source.imageUrlSquare;
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