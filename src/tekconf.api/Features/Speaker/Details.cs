using System;
using Humanizer;
using TekConf.Api.Data;

namespace TekConf.Api.Features.Speaker
{
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using FluentValidation;
    using MediatR;

    public class Details
    {
        public class Query : IAsyncRequest<Speaker>
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

        public class Speaker
        {
            public string Slug { get; set; }
            public string Name { get; set; }

        }

        public class Handler : IAsyncRequestHandler<Query, Speaker>
        {
            private readonly TekConfContext _db;
            private readonly MapperConfiguration _config;

            public Handler(TekConfContext db, MapperConfiguration config)
            {
                _db = db;
                _config = config;
            }

            public async Task<Speaker> Handle(Query message)
            {
                return await _db
                        .Conferences
                        .Where(x => x.Slug == message.Slug)
                        .ProjectToSingleOrDefaultAsync<Speaker>(_config);
            }
        }
    }
}