using System.Linq;
using AutoMapper;

namespace TekConf.Api.Features.Session
{
    public class MappingProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<Data.Models.Session, Index.Result.Session>()
                .ForMember(dest => dest.Url, opt => opt.MapFrom(c => $"{Statics.CurrentUrl}/{c.ConferenceInstance.Slug}/sessions/{c.Slug}"));

            CreateMap<Data.Models.Session, Details.Session>()
                .ForMember(dest => dest.Url, opt => opt.MapFrom(c => $"{Statics.CurrentUrl}/{c.ConferenceInstance.Slug}/sessions/{c.Slug}"));

            CreateMap<Data.Models.Speaker, Details.Speaker>()
                .ForMember(dest => dest.Url, opt => opt.MapFrom(c => $"{Statics.CurrentUrl}/{c.Sessions.First().ConferenceInstance.Slug}/speakers/{c.Slug}"));
        }
    }
}