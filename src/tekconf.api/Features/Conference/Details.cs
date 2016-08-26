using System;
using Humanizer;
using TekConf.Api.Data;

namespace TekConf.Api.Features.Conference
{
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using FluentValidation;
    using MediatR;

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
            public string Slug { get; set; }
            public string Name { get; set; }

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
                return await _db
                        .ConferenceInstances
                        .Where(x => x.Slug == message.Slug)
                        .ProjectToSingleOrDefaultAsync<Conference>(_config);
            }
        }
    }
}