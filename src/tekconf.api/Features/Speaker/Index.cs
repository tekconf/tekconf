using System;
using Humanizer;
using TekConf.Api.Data;

namespace TekConf.Api.Features.Speaker
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
            public List<Speaker> Speakers { get; set; }

            public class Speaker
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
                var speakers = await _db
                    .Conferences
                    .Where(x => x.Slug == message.Slug)
                    .OrderBy(x => x.Name)
                    .ProjectToListAsync<Result.Speaker>(_config);

                return new Result
                {
                    Speakers = speakers,
                };
            }
        }


    }
}