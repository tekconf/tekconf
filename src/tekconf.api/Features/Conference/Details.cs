using System;
using System.Collections.Generic;
using System.Data.Entity;
using TekConf.Api.Data;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace TekConf.Api.Features.Conference
{
    public class Details
    {
        public class Query : IAsyncRequest<Conference>
        {
            public string Slug { get; set; }
        }

        public class Validator : AbstractValidator<Query>
        {
            public Validator()
            {
                RuleFor(m => m.Slug).NotNull();
            }
        }

        public class Conference
        {
            public string Url { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }

            public string ShortDescription { get; set; }

            public string Tagline { get; set; }

            public string ImageUrl { get; set; }

            #region Dates
            public DateTime? Start { get; set; }
            public DateTime? End { get; set; }
            public DateTime? CallForSpeakersOpens { get; set; }
            public DateTime? CallForSpeakersCloses { get; set; }
            public DateTime? RegistrationOpens { get; set; }
            public DateTime? RegistrationCloses { get; set; }
            #endregion

            public int? DefaultTalkLength { get; set; }
            public int? NumberOfSessions { get; set; }

            public bool IsOnline { get; set; }
            public bool IsLive { get; set; }

            public List<Session> Sessions { get; set; }
            public Social Social { get; set; }

            public Address Address { get; set; } = new Address();

        }

        public class Session
        {
            public string Url { get; set; }
            public string Title { get; set; }
            public List<Speaker> Speakers { get; set; }


        }
        public class Speaker
        {
            public string Url { get; set; }

            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string ProfileImageUrl { get; set; }

        }
        public class Address
        {
            public string LocationName { get; set; }
            public string BuildingName { get; set; }
            public int? StreetNumber { get; set; }
            public string StreetNumberSuffix { get; set; }
            public string StreetName { get; set; }
            public string StreetType { get; set; }
            public string StreetDirection { get; set; }
            public string AddressType { get; set; }
            public string AddressTypeId { get; set; }
            public string LocalMunicipality { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public string GoverningDistrict { get; set; }
            public string PostalArea { get; set; }
            public string Country { get; set; }
            public double? Longitude { get; set; }
            public double? Latitude { get; set; }
        }

        public class Social
        {

            public string FacebookUrl { get; set; }
            public string HomepageUrl { get; set; }
            public string LanyrdUrl { get; set; }
            public string MeetupUrl { get; set; }
            public string GooglePlusUrl { get; set; }
            public string VimeoUrl { get; set; }
            public string YoutubeUrl { get; set; }
            public string GithubUrl { get; set; }
            public string LinkedInUrl { get; set; }
            public string TwitterHashTag { get; set; }
            public string TwitterName { get; set; }
            public string InstagramUrl { get; set; }
            public string SnapChatUrl { get; set; }
        }

        public class Handler : IAsyncRequestHandler<Query, Conference>
        {
            private readonly TekConfContext _db;
            private readonly MapperConfiguration _config;

            public Handler(TekConfContext db, MapperConfiguration config)
            {
                _db = db;
                _config = config;
            }

            public async Task<Conference> Handle(Query message)
            {
                var conferenceInstance = await _db
                        .ConferenceInstances
                        .Include(x => x.Conference)
                        .Include(x => x.Sessions)
                        .Include(x => x.Sessions.Select(s => s.Speakers))
                        .Where(x => x.Slug == message.Slug)
                        .SingleOrDefaultAsync();

                var mapper = _config.CreateMapper();
                var conference = mapper.Map<Conference>(conferenceInstance);
                return conference;
            }
        }
    }
}