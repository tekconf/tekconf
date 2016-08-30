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
                .ForMember(d => d.Url, opt => opt.MapFrom(c => Statics.CurrentUrl + "/" + c.Slug));

            CreateMap<ConferenceInstance, Details.Conference>()
                .ForMember(dest => dest.Url, opt => opt.MapFrom(c => $"{Statics.CurrentUrl}/{c.Slug}"))
                .ForMember(dest => dest.Address, opt => opt.ResolveUsing<DetailAddressResolver>())
                .ForMember(dest => dest.Social, opt => opt.ResolveUsing<DetailSocialResolver>());

            CreateMap<Data.Models.Session, Details.Session>()
                .ForMember(dest => dest.Url, opt => opt.MapFrom(c => $"{Statics.CurrentUrl}/{c.ConferenceInstance.Slug}/sessions/{c.Slug}"));

            CreateMap<Data.Models.Speaker, Details.Speaker>()
                .ForMember(dest => dest.Url, opt => opt.MapFrom(c => $"{Statics.CurrentUrl}/{c.Sessions.First().ConferenceInstance.Slug}/speakers/{c.Slug}"));

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