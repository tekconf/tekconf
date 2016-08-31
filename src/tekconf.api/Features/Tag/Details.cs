using System;
using System.Collections.Generic;
using System.Data.Entity;
using TekConf.Api.Data;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace TekConf.Api.Features.Tag
{
    public class Details
    {
        public class Query : IAsyncRequest<Tag>
        {
            public string Conference { get; set; }
            public string Slug { get; set; }
        }

        public class Validator : AbstractValidator<Query>
        {
            public Validator()
            {
                RuleFor(m => m.Slug).NotNull();
            }
        }

        public class Tag
        {
            public string Url { get; set; }
            public string ConferenceUrl { get; set; }
            public string Name { get; set; }
            public List<Session> Sessions { get; set; }
        }

        public class Session
        {
            public string Url { get; set; }
            public string Title { get; set; }
        }
        

        public class Handler : IAsyncRequestHandler<Query, Tag>
        {
            private readonly TekConfContext _db;
            private readonly MapperConfiguration _config;

            public Handler(TekConfContext db, MapperConfiguration config)
            {
                _db = db;
                _config = config;
            }

            public async Task<Tag> Handle(Query message)
            {
                var tag = await _db
                    .Tags
                    .Include(x => x.Sessions)
                    .Include(x => x.ConferenceInstance)
                    .Where(x => x.ConferenceInstance.Slug == message.Conference)
                    .Where(x => x.Slug == message.Slug)
                    .OrderBy(x => x.Name)
                    .FirstOrDefaultAsync();

                var mapper = _config.CreateMapper();
                var dto = mapper.Map<Tag>(tag);

                return dto;
            }
        }
    }
}