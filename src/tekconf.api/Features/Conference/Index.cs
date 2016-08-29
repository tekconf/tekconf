using System;
using TekConf.Api.Data;
using TekConf.Api.Data.Models;
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
                var evolve = await _db.ConferenceInstances.FindAsync(1);
                var james = new User()
                {
                    Slug = "james-montemagno",
                    Bio = "Xamarin Developer Evangelist, James Montemagno, has been a .NET developer for over a decade while working in a wide range of industries including game development, printer software, and web services. Prior to joining Xamarin, James was a professional mobile developer with 4+ years of experience on the Xamarin platform, having published several apps on iOS, Android, and Windows.",
                    FirstName = "James",
                    LastName = "Montemagno",
                };
                var laval = new User()
                {
                    FirstName = "Jérémie",
                    LastName = "Laval",
                    Slug = "jeremie-laval",
                    Bio = "Xamarin hacker Jérémie Laval has been in the Mono scene for as long as we can remember, working on the various open-source parts of the project. These days this French guy spends his time improving the developer experience for Xamarin.iOS and Xamarin.Android. Très cool, huh?",
                };

                _db.Users.Add(james);

                var jamesSpeaker = new Data.Models.Speaker()
                {
                    Slug = "james-montemagno",
                    Bio =
                        "Xamarin Developer Evangelist, James Montemagno, has been a .NET developer for over a decade while working in a wide range of industries including game development, printer software, and web services. Prior to joining Xamarin, James was a professional mobile developer with 4+ years of experience on the Xamarin platform, having published several apps on iOS, Android, and Windows.",
                    Title = "Developer Evangelist & Engineer",
                    Company = "Xamarin",
                    GithubUrl = "https://github.com/jamesmontemagno",
                    TwitterName = "JamesMontemagno",
                    BlogUrl = "http://motzcod.es/",
                    FacebookUrl = "https://www.facebook.com/motzcodes",
                    EmailAddress = "james.montemagno@microsoft.com",
                    FirstName = "James",
                    LastName = "Montemagno",
                    ProfileImageUrl = "https://avatars3.githubusercontent.com/u/1676321?v=3&s=460",
                    User = james
                };
                _db.Speakers.Add(jamesSpeaker);

                _db.Users.Add(laval);
                var lavalSpeaker = new Data.Models.Speaker
                {
                    FirstName = "Jérémie",
                    LastName = "Laval",
                    Slug = "jeremie-laval",
                    Company = "Microsoft",
                    Title = "Android Designer",
                    Bio =
                        "Xamarin hacker Jérémie Laval has been in the Mono scene for as long as we can remember, working on the various open-source parts of the project. These days this French guy spends his time improving the developer experience for Xamarin.iOS and Xamarin.Android. Très cool, huh?",
                    ProfileImageUrl = "https://cdn2.xamarin.com/evolve-2016/speakers/jeremie-laval@2x.jpg",
                    GithubUrl = "https://github.com/garuma",
                    BlogUrl = "http://neteril.org",
                    User = laval
                };
                _db.Speakers.Add(lavalSpeaker);


                var materialPresentation = new Presentation()
                {
                    Slug = "beautiful-apps-with-material-design",
                    Description = "Building beautiful Android apps doesn\'t have to be hard. Since its original induction, Material Design (MD) has taken the Android world by storm, generating rapid adoption throughout the dev community. A lot has changed from the original material design specification, with the updates to the Android Support v7 AppCompat library and intro of the Support Design library, MD themes, controls, and features now available on all devices running Android. This session will show you how to transform your app into a stunning work of Material art, and even how to utilize MD in your Xamarin.Forms apps.",
                    Title = "Everyone Can Create Beautiful Apps with Material Design",
                    Owners = new List<User>()
                                        {
                                            james,
                                            laval
                                        }
                };

                _db.Presentations.Add(materialPresentation);

                var materialDesign = new Data.Models.Session
                {
                    Title = "Everyone Can Create Beautiful Apps with Material Design",
                    ConferenceInstance = evolve,
                    Description = "Building beautiful Android apps doesn\'t have to be hard. Since its original induction, Material Design (MD) has taken the Android world by storm, generating rapid adoption throughout the dev community. A lot has changed from the original material design specification, with the updates to the Android Support v7 AppCompat library and intro of the Support Design library, MD themes, controls, and features now available on all devices running Android. This session will show you how to transform your app into a stunning work of Material art, and even how to utilize MD in your Xamarin.Forms apps.",
                    Slug = "beautiful-apps-with-material-design",
                    Presentation = materialPresentation,
                    Speakers = new List<Data.Models.Speaker>()
                    {
                        jamesSpeaker,
                        lavalSpeaker
                    }
                };

                _db.Sessions.Add(materialDesign);
                await _db.SaveChangesAsync();
                //var robGibbens = new User()
                //{
                //    Bio = "He rocks so much that he has to be 20 chars",
                //    FirstName = "Rob",
                //    LastName = "Gibbens",
                //    Slug = "rob-gibbens",

                //};
                //var xamarinEvolve = new Data.Models.Conference()
                //{
                //    CreatedAt = DateTime.Now,
                //    Description = "Xamarin Evolve - Mobile apps",
                //    Name = "Xamarin Evolve",
                //    Owner = robGibbens,
                //    Slug = "xamarin-evolve"
                //};

                //robGibbens.OwnedConferences.Add(xamarinEvolve);

                //var xamarinEvolve2017 = new ConferenceInstance()
                //{
                //    Conference = xamarinEvolve,
                //    Name = "Xamarin Evolve 2017",
                //    Slug = "2017",
                //    Description = "Mobile apps are the best thing ever",
                //    IsOnline = false,
                //    IsLive = true
                //};

                //xamarinEvolve.Instances.Add(xamarinEvolve2017);

                //try
                //{
                //    _db.Conferences.Add(xamarinEvolve);
                //    await _db.SaveChangesAsync();

                //}
                //catch (DbEntityValidationException ex)
                //{

                //    var me = ex.Message;
                //    throw;
                //}
            }
        }


    }
}