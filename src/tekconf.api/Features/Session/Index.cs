using System;
using Humanizer;
using TekConf.Api.Data;

namespace TekConf.Api.Features.Session
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using MediatR;

    public class Index
    {
        public class Query : IAsyncRequest<Result>
        {
            public string Slug { get; set; }
        }

        public class Result
        {
            public List<Session> Sessions { get; set; }

            public class Session
            {
                public string Slug { get; set; }
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
                var sessions = await _db
                    .Conferences
                    .Where(x => x.Slug == message.Slug)
                    .OrderBy(x => x.Name)
                    .ProjectToListAsync<Result.Session>(_config);

                return new Result
                {
                    Sessions = sessions,
                };
            }
        }


    }
}