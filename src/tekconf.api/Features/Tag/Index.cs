using System;
using TekConf.Api.Data;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;

namespace TekConf.Api.Features.Tag
{
    public class Index
    {
        public class Query : IAsyncRequest<Result>
        {
            public string Conference { get; set; }
        }

        public class Result
        {
            public List<Tag> Tags { get; set; }

            public class Tag
            {
                public string Url { get; set; }
                public string Name { get; set; }
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
                var tags = await _db
                    .Tags
                    .Include(x => x.Sessions)
                    .Include(x => x.ConferenceInstance)
                    .Where(x => x.ConferenceInstance.Slug == message.Conference)
                    .OrderBy(x => x.Name)
                    .ToListAsync();

                var mapper = _config.CreateMapper();
                var dtos = mapper.Map<List<Result.Tag>>(tags);

                return new Result
                {
                    Tags = dtos,
                };
            }
        }
    }
}