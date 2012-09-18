using AutoMapper;
using TekConf.RemoteData.Dtos.v1;

namespace TekConf.UI.Api
{
    public class Bootstrapper
    {
        public void BootstrapAutomapper()
        {
            Mapper.CreateMap<ConferenceEntity, ConferencesDto>()
                .ForMember(dest => dest.url, opt => opt.Ignore());

            Mapper.CreateMap<ConferenceEntity, ConferenceDto>()
                .ForMember(dest => dest.url, opt => opt.Ignore())
                .ForMember(dest => dest.sessionsUrl, opt => opt.Ignore())
                .ForMember(dest => dest.speakersUrl, opt => opt.Ignore());

            Mapper.CreateMap<ConferenceEntity, FullConferenceDto>();

            Mapper.CreateMap<SessionEntity, SessionsDto>()
                .ForMember(dest => dest.url, opt => opt.Ignore());

            Mapper.CreateMap<SessionEntity, SessionDto>()
                .ForMember(dest => dest.url, opt => opt.Ignore())
                .ForMember(dest => dest.speakersUrl, opt => opt.Ignore());

            Mapper.CreateMap<SessionEntity, FullSessionDto>();

            Mapper.CreateMap<SpeakerEntity, SpeakersDto>()
                .ForMember(dest => dest.url, opt => opt.Ignore());

            Mapper.CreateMap<SpeakerEntity, SpeakerDto>();

            Mapper.CreateMap<SpeakerEntity, FullSpeakerDto>();

            Mapper.CreateMap<ScheduleEntity, ScheduleDto>();
        }

    }
}