using AutoMapper;

namespace TekConf.Api.Features.Tag
{
    public class MappingProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<Data.Models.Tag, Index.Result.Tag>()
                .ForMember(d => d.Url, opt => opt.MapFrom(tag => Statics.CurrentUrl + "/" + tag.ConferenceInstance.Slug + "/tags/" + tag.Slug));

            CreateMap<Data.Models.Tag, Details.Tag>()
                .ForMember(dest => dest.Url, opt => opt.MapFrom(c => Statics.CurrentUrl + "/" + c.ConferenceInstance.Slug + "/tags/" + c.Slug))
                .ForMember(dest => dest.ConferenceUrl, opt => opt.MapFrom(tag => Statics.CurrentUrl + "/" + tag.ConferenceInstance.Slug));

            CreateMap<Data.Models.Session, Details.Session>()
                .ForMember(dest => dest.Url, opt => opt.MapFrom(session => Statics.CurrentUrl + "/" + session.ConferenceInstance.Slug + "/sessions/" + session.Slug));
        }
    }
}