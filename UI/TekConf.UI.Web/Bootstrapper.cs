using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using AutoMapper;
using TekConf.RemoteData.Dtos.v1;
using TekConf.UI.Api;
using TekConf.UI.Api.Services.Requests.v1;

namespace TekConf.UI.Web
{
    public class Bootstrapper
    {
        public void BootstrapAutomapper()
        {
            Mapper.CreateMap<FullConferenceDto, CreateConference>()
                .ForMember(dest => dest.latitude, opt=> opt.ResolveUsing<LatitudeResolver>())
                .ForMember(dest => dest.longitude, opt => opt.ResolveUsing<LongitudeResolver>())
                ;
            Mapper.CreateMap<AddressDto, Address>();

            Mapper.CreateMap<FullSessionDto, AddSession>();

            Mapper.CreateMap<FullSpeakerDto, CreateSpeaker>();

            Mapper.CreateMap<SpeakerEntity, FullSpeakerDto>();
            Mapper.CreateMap<AddressEntity, AddressDto>();
            Mapper.CreateMap<ConferenceEntity, FullConferenceDto>()
                            .ForMember(dest => dest.imageUrl, opt => opt.ResolveUsing<ImageResolver>())
                            .ForMember(dest => dest.numberOfSessions, opt => opt.ResolveUsing<SessionsCounterResolver>());

            Mapper.CreateMap<SessionEntity, FullSessionDto>();

        }

        public class SessionsCounterResolver : ValueResolver<ConferenceEntity, int>
        {
            protected override int ResolveCore(ConferenceEntity source)
            {
                return source.sessions.Count();
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