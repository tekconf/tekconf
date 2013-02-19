using AutoMapper;
using TekConf.RemoteData.Dtos.v1;
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
        }

        public class LatitudeResolver : ValueResolver<FullConferenceDto, double>
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

        public class LongitudeResolver : ValueResolver<FullConferenceDto, double>
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
    }
}