using System.Linq;
using AutoMapper;
using TekConf.Api.Data.Models;

namespace TekConf.Api.Features.Conference
{
    public class MappingProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<ConferenceInstance, Index.Result.Conference>()
                .ForMember(d => d.Url, opt => opt.MapFrom(c => Statics.CurrentUrl + "/ " + c.Slug));

            CreateMap<ConferenceInstance, Details.Conference>()
                .ForMember(dest => dest.Url, opt => opt.ResolveUsing<DetailConferenceUrlResolver>())
                .ForMember(dest => dest.Address, opt => opt.ResolveUsing<DetailAddressResolver>())
                .ForMember(dest => dest.Social, opt => opt.ResolveUsing<DetailSocialResolver>());

            CreateMap<Data.Models.Session, Details.Session>()
                .ForMember(dest => dest.Url, opt => opt.ResolveUsing<DetailSessionUrlResolver>());

            CreateMap<Data.Models.Speaker, Details.Speaker>()
                .ForMember(dest => dest.Url, opt => opt.ResolveUsing<DetailSpeakerUrlResolver>());

        }

        public class DetailConferenceUrlResolver : IValueResolver<ConferenceInstance, Details.Conference, string>
        {
            public string Resolve(ConferenceInstance source, Details.Conference destination, string destMember, ResolutionContext context)
            {
                return $"{Statics.CurrentUrl}/{source.Slug}";
            }
        }

        public class DetailSessionUrlResolver : IValueResolver<Data.Models.Session, Details.Session, string>
        {
            public string Resolve(Data.Models.Session source, Details.Session destination, string destMember, ResolutionContext context)
            {
                return $"{Statics.CurrentUrl}/{source.ConferenceInstance.Slug}/sessions/{source.Slug}";
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

        public class DetailSocialResolver : IValueResolver<ConferenceInstance, Details.Conference, Details.Social>
        {
            public Details.Social Resolve(ConferenceInstance source, Details.Conference destination, Details.Social destMember, ResolutionContext context)
            {
                return new Details.Social();
            }
        }

        public class DetailAddressResolver : IValueResolver<ConferenceInstance, Details.Conference, Details.Address>
        {
            public Details.Address Resolve(ConferenceInstance source, Details.Conference destination, Details.Address member, ResolutionContext context)
            {
                return new Details.Address
                {
                    AddressType = source.AddressType,
                    AddressTypeId = source.AddressTypeId,
                    BuildingName = source.BuildingName,
                    City = source.City,
                    Country = source.Country,
                    GoverningDistrict = source.GoverningDistrict,
                    Latitude = source.Latitude,
                    LocalMunicipality = source.LocalMunicipality,
                    LocationName = source.LocationName,
                    Longitude = source.Longitude,
                    PostalArea = source.PostalArea,
                    State = source.State,
                    StreetDirection = source.StreetDirection,
                    StreetName = source.StreetName,
                    StreetNumber = source.StreetNumber,
                    StreetNumberSuffix = source.StreetNumberSuffix,
                    StreetType = source.StreetType
                };
            }
        }
    }
}