using System;
using System.Collections.Generic;
using AutoMapper;
using TekConf.RemoteData.Dtos.v1;
using TekConf.UI.Api.Services.Requests.v1;

namespace TekConf.UI.Api
{
    public class Bootstrapper
    {
        public void BootstrapAutomapper()
        {
            Mapper.AddFormatter<TrimmingFormatter>();

            Mapper.CreateMap<ConferenceEntity, ConferencesDto>()
                .ForMember(dest => dest.url, opt => opt.Ignore());

            Mapper.CreateMap<ConferenceEntity, ConferenceEntity>();
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

            Mapper.CreateMap<AddSession, SessionEntity>()
                .ForMember(s => s._id, opt => opt.UseValue(Guid.NewGuid()))
                .ForMember(s => s.speakers, opt => opt.UseValue(new List<SpeakerEntity>()))
                ;

            Mapper.CreateMap<User, UserEntity>()
                .ForMember(u => u._id, opt => opt.UseValue(Guid.NewGuid()));

            Mapper.CreateMap<CreateConference, ConferenceEntity>()
                //.ForMember(c => c._id, opt => opt.UseValue(Guid.NewGuid()))
                .ForMember(c => c.sessions, opt => opt.UseValue(new List<SessionEntity>()))
                ;

            Mapper.CreateMap<CreateSpeaker, SpeakerEntity>()
                .ForMember(c => c._id, opt => opt.UseValue(Guid.NewGuid()))
                ;

            Mapper.CreateMap<SpeakerEntity, SpeakerDto>();

            Mapper.CreateMap<SpeakerEntity, FullSpeakerDto>();

            Mapper.CreateMap<ScheduleEntity, ScheduleDto>();

            Mapper.CreateMap<AddressEntity, Address>();
            Mapper.CreateMap<Address, AddressEntity>();
            Mapper.CreateMap<AddressEntity, AddressDto>();
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
            if (context.SourceValue == null)
            {
                return null;
            }

            if (!(context.SourceValue is T))
            {
                return context.SourceValue == null ? String.Empty : context.SourceValue.ToString();
            }

            return FormatValueCore((T)context.SourceValue);
        }

        protected abstract string FormatValueCore(T value);
    }
}