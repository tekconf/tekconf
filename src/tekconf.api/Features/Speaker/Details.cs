using TekConf.Api.Data;
using System.Data.Entity;

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
            public string Conference { get; set; }
            public string Speaker { get; set; }
        }

        public class Validator : AbstractValidator<Query>
        {
            public Validator()
            {
                RuleFor(m => m.Speaker).NotNull();
            }
        }

        public class Speaker
        {
            public string Url { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Bio { get; set; }
            public string Title { get; set; }

            public string Company { get; set; }
            public string BlogUrl { get; set; }
            public string EmailAddress { get; set; }
            public string PhoneNumber { get; set; }
            public string ProfileImageUrl { get; set; }

            #region Social
            public string TwitterName { get; set; }
            public string FacebookUrl { get; set; }
            public string LinkedInUrl { get; set; }
            public string GooglePlusUrl { get; set; }
            public string VimeoUrl { get; set; }
            public string YoutubeUrl { get; set; }
            public string GithubUrl { get; set; }
            public string CoderWallUrl { get; set; }
            public string StackoverflowUrl { get; set; }
            public string BitbucketUrl { get; set; }
            public string CodeplexUrl { get; set; }
            public string InstagramUrl { get; set; }
            public string SnapchatUrl { get; set; }
            #endregion

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
                var speaker = await _db
                        .Speakers
                        .Include(s => s.Sessions)
                        .Where(x => x.Slug == message.Speaker)
                        .Where(x => x.Sessions.Any(s => s.ConferenceInstance.Slug == message.Conference))
                        .SingleOrDefaultAsync();

                var mapper = _config.CreateMapper();
                var dto = mapper.Map<Speaker>(speaker);

                return dto;
            }
        }
    }
}