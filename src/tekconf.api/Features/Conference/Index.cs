using System;
using System.Data.Entity.Validation;
using Humanizer;
using TekConf.Api.Data;
using TekConf.Api.Data.Models;

namespace TekConf.Api.Features.Conference
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
            public List<Conference> Conferences { get; set; }

            public class Conference
            {
                public string DetailUrl { get; set; }
                public string Slug { get; set; }
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
                //await Insert();

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

            private async Task Insert()
            {
                var robGibbens = new User()
                {
                    Bio = "He rocks so much that he has to be 20 chars",
                    FirstName = "Rob",
                    LastName = "Gibbens",
                    Slug = "rob-gibbens",

                };
                var xamarinEvolve = new Data.Models.Conference()
                {
                    CreatedAt = DateTime.Now,
                    Description = "Xamarin Evolve - Mobile apps",
                    Name = "Xamarin Evolve",
                    Owner = robGibbens,
                    Slug = "xamarin-evolve"
                };

                robGibbens.OwnedConferences.Add(xamarinEvolve);

                var xamarinEvolve2017 = new ConferenceInstance()
                {
                    Conference = xamarinEvolve,
                    Name = "Xamarin Evolve 2017",
                    Slug = "2017",
                    Description = "Mobile apps are the best thing ever",
                    IsOnline = false,
                    IsLive = true
                };

                xamarinEvolve.Instances.Add(xamarinEvolve2017);

                try
                {
                    _db.Conferences.Add(xamarinEvolve);
                    await _db.SaveChangesAsync();

                }
                catch (DbEntityValidationException ex)
                {

                    var me = ex.Message;
                    throw;
                }
            }
        }


    }
}