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
                .ForMember(d => d.Url, opt => opt.MapFrom(conferenceInstance => Statics.CurrentUrl + "/" + conferenceInstance.Slug));

            CreateMap<ConferenceInstance, Details.Conference>()
                .ForMember(dest => dest.Url, opt => opt.MapFrom(conferenceInstance => $"{Statics.CurrentUrl}/{conferenceInstance.Slug}"))
                .ForMember(dest => dest.Address, opt => opt.ResolveUsing<DetailAddressResolver>())
                .ForMember(dest => dest.Social, opt => opt.ResolveUsing<DetailSocialResolver>());

            CreateMap<Data.Models.Session, Details.Session>()
                .ForMember(dest => dest.Url, opt => opt.MapFrom(session => $"{Statics.CurrentUrl}/{session.ConferenceInstance.Slug}/sessions/{session.Slug}"));

            CreateMap<Data.Models.Speaker, Details.Speaker>()
                .ForMember(dest => dest.Url, opt => opt.MapFrom(speaker => $"{Statics.CurrentUrl}/{speaker.Sessions.First().ConferenceInstance.Slug}/speakers/{speaker.Slug}"));

            CreateMap<Data.Models.Tag, Details.Tag>()
                .ForMember(dest => dest.Url, opt => opt.MapFrom(tag => Statics.CurrentUrl + "/" + tag.ConferenceInstance.Slug + "/tags/" + tag.Slug));

            CreateMap<Data.Models.Difficulty, Details.Difficulty>();
        }

        public class DetailSocialResolver : IValueResolver<ConferenceInstance, Details.Conference, Details.Social>
        {
            public Details.Social Resolve(ConferenceInstance source, Details.Conference destination, Details.Social destMember, ResolutionContext context)
            {
                //TODO : Implement
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