using System;
using System.Linq;
using AutoMapper;
using Humanizer;
using TekConf.Api.Data.Models;

namespace TekConf.Api.Features.Speaker
{
    public class MappingProfile : Profile
    {
        protected override void Configure()
        {
            //CreateMap<Data.Models.Speaker, Index.Result.Speaker>();

            CreateMap<Data.Models.Speaker, Details.Speaker>()
                .ForMember(dest => dest.Url, opt => opt.ResolveUsing<DetailSpeakerUrlResolver>());
            ;
        }
    }
    public class DetailSpeakerUrlResolver : IValueResolver<Data.Models.Speaker, Details.Speaker, string>
    {
        public string Resolve(Data.Models.Speaker source, Details.Speaker destination, string destMember, ResolutionContext context)
        {
            var conferenceSlug = source.Sessions?.FirstOrDefault()?.ConferenceInstance.Slug;

            return $"{Statics.CurrentUrl}/{conferenceSlug}/speakers/{source.Slug}";
        }
    }
}