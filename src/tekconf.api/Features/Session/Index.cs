using System.Data.Entity;
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
            public string Conference { get; set; }
        }

        public class Result
        {
            public List<Session> Sessions { get; set; }

            public class Session
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
                var sessions = await _db
                    .Sessions
                    .Include(x => x.Speakers)
                    .Include(x => x.ConferenceInstance)
                    .Where(x => x.ConferenceInstance.Slug == message.Conference)
                    .ToListAsync();

                var mapper = _config.CreateMapper();
                var dtos = mapper.Map<List<Result.Session>>(sessions);

                return new Result
                {
                    Sessions = dtos,
                };
            }
        }
    }
}