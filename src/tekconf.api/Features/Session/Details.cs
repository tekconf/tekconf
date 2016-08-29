using System.Collections.Generic;
using System.Data.Entity;
using TekConf.Api.Data;

namespace TekConf.Api.Features.Session
{
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using FluentValidation;
    using MediatR;

    public class Details
    {
        public class Query : IAsyncRequest<Session>
        {
            public string Conference { get; set; }
            public string Session { get; set; }
        }

        public class Validator : AbstractValidator<Query>
        {
            public Validator()
            {
                RuleFor(m => m.Session).NotNull();
            }
        }

        public class Session
        {
            public string Url { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public List<Speaker> Speakers {get;set;}
        }

        public class Speaker
        {
            public string Url { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string ProfileImageUrl { get; set; }
        }

        public class Handler : IAsyncRequestHandler<Query, Session>
        {
            private readonly TekConfContext _db;
            private readonly MapperConfiguration _config;

            public Handler(TekConfContext db, MapperConfiguration config)
            {
                _db = db;
                _config = config;
            }

            public async Task<Session> Handle(Query message)
            {
                var session =  await _db
                        .Sessions
                        .Include(x => x.ConferenceInstance)
                        .Where(x => x.ConferenceInstance.Slug == message.Conference)
                        .Where(x => x.Slug == message.Session)
                        .SingleOrDefaultAsync();

                var mapper = _config.CreateMapper();
                var dto = mapper.Map<Session>(session);

                return dto;
            }
        }
    }
}