using System;
using TekConf.Api.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;

namespace TekConf.Api.Features.Conference
{
    public class Index
    {
        public class Query : IAsyncRequest<Result>
        {
            public string Slug { get; set; }
        }

        public class Result
        {
            public List<Conference> Conferences { get; set; }

            public class Conference
            {
                public string Url { get; set; }
                public string Name { get; set; }
                public string ShortDescription { get; set; }
                public string ImageUrl { get; set; }
                public DateTime? Start { get; set; }
                public DateTime? End { get; set; }

            }
        }

        public class Handler : IAsyncRequestHandler<Query, Result>
        {
            private readonly TekConfContext _db;
            private readonly MapperConfiguration _config;

            public Handler(TekConfContext db, MapperConfiguration config)
            {
                _db = db;
                _config = config;
            }

            public async Task<Result> Handle(Query message)
            {
                var query = _db
                    .ConferenceInstances.AsQueryable();

                if (!string.IsNullOrWhiteSpace(message.Slug))
                {
                    query = query
                        .Where(x => x.Slug == message.Slug);
                }

                var conferences = await query
                    .OrderBy(x => x.Name)
                    .ProjectToListAsync<Result.Conference>(_config);

                return new Result
                {
                    Conferences = conferences,
                };
            }
        }
    }
}