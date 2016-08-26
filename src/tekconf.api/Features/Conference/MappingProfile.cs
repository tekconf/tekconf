using AutoMapper;
using TekConf.Api.Data.Models;

namespace TekConf.Api.Features.Conference
{
    public class MappingProfile : Profile
    {
        protected override void Configure()
        {
            //TODO : Don't hardcode url
            CreateMap<ConferenceInstance, Index.Result.Conference>()
                .ForMember(d => d.DetailUrl, opt => opt.MapFrom(c => "http://localhost:2901/conference/details?slug=" + c.Slug));
                

            CreateMap<ConferenceInstance, Details.Conference>();
        }
    }
}