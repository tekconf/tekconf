using System.Data.Entity;
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
            public string Conference { get; set; }
        }

        public class Result
        {
            public List<Speaker> Speakers { get; set; }

            public class Speaker
            {
                public string Url { get; set; }
                public string FirstName { get; set; }
                public string LastName { get; set; }
                public string Title { get; set; }
                public string Company { get; set; }
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
                    .Speakers
                    .Include(x => x.Sessions)
                    .Include(s => s.Sessions.Select(c => c.ConferenceInstance))
                    .Where(x => x.Sessions.Any(c => c.ConferenceInstance.Slug == message.Conference))
                    .OrderBy(x => x.LastName)
                    .ToListAsync();

                var mapper = _config.CreateMapper();
                var dtos = mapper.Map<List<Result.Speaker>>(speakers);

                return new Result
                {
                    Speakers = dtos,
                };
            }
        }


    }
}