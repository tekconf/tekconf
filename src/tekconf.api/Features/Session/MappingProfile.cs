using System.Linq;
using AutoMapper;

namespace TekConf.Api.Features.Session
{
    public class MappingProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<Data.Models.Session, Index.Result.Session>()
                .ForMember(dest => dest.Url, opt => opt.ResolveUsing<IndexSessionUrlResolver>());

            CreateMap<Data.Models.Session, Details.Session>()
                .ForMember(dest => dest.Url, opt => opt.ResolveUsing<DetailSessionUrlResolver>());

            CreateMap<Data.Models.Speaker, Details.Speaker>()
                .ForMember(dest => dest.Url, opt => opt.ResolveUsing<DetailSpeakerUrlResolver>());
        }
    }

    public class DetailSpeakerUrlResolver : IValueResolver<Data.Models.Speaker, Details.Speaker, string>
    {
        public string Resolve(Data.Models.Speaker source, Details.Speaker destination, string destMember, ResolutionContext context)
        {
            var conferenceSlug = source.Sessions?.FirstOrDefault()?.ConferenceInstance?.Slug;

            return $"{Statics.CurrentUrl}/{conferenceSlug}/speakers/{source.Slug}";
        }
    }

    public class IndexSessionUrlResolver : IValueResolver<Data.Models.Session, Index.Result.Session, string>
    {
        public string Resolve(Data.Models.Session source, Index.Result.Session destination, string destMember, ResolutionContext context)
        {
            return $"{Statics.CurrentUrl}/{source?.ConferenceInstance?.Slug}/sessions/{source?.Slug}";
        }
    }
    public class DetailSessionUrlResolver : IValueResolver<Data.Models.Session, Details.Session, string>
    {
        public string Resolve(Data.Models.Session source, Details.Session destination, string destMember, ResolutionContext context)
        {
            return $"{Statics.CurrentUrl}/{source?.ConferenceInstance?.Slug}/sessions/{source?.Slug}";
        }
    }
}