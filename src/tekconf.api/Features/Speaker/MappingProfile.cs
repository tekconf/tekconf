using System.Linq;
using AutoMapper;

namespace TekConf.Api.Features.Speaker
{
    public class MappingProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<Data.Models.Speaker, Index.Result.Speaker>()
                .ForMember(dest => dest.Url, opt => opt.MapFrom(c => $"{Statics.CurrentUrl}/{c.Sessions.First().ConferenceInstance.Slug}/speakers/{c.Slug}"));

            CreateMap<Data.Models.Speaker, Details.Speaker>()
                .ForMember(dest => dest.Url, opt => opt.MapFrom(c => $"{Statics.CurrentUrl}/{c.Sessions.First().ConferenceInstance.Slug}/speakers/{c.Slug}")); 
        }
    }
}