using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using FluentMongo.Linq;
using ServiceStack.Text;
using TekConf.UI.Api;

namespace UberImporter.MonkeySpace
{
    public class MonkeySpaceImporter
    {
        public void Import()
        {


            var url = "http://monkeyspace.org/data/schedule.json";
            var request = new WebClient();
            var json = request.DownloadString(url);
            var root = json.FromJson<root>();
            if (root != null)
            {
                MongoDbConnection connection = new MongoDbConnection();
                var collection = connection.RemoteDatabase.GetCollection<ConferenceEntity>("conferences");
                if (!collection.AsQueryable().Any(c => c.slug == "MonkeySpace-2012"))
                {
                    var conference = new ConferenceEntity()
                    {
                        _id = Guid.NewGuid(),
                        description = @"MonkeySpace, formerly known as Monospace, is the official cross platform and open-source .NET conference. Want to learn more about developing for the iPhone, Android, Mac, and *nix platforms using .NET technologies? How about developing games or learning more about open-source projects using .NET technologies? MonkeySpace has provided an annual venue to collaborate, share, and socialize around these topics and more.",
                        end = new DateTime(2012, 10, 19),
                        facebookUrl = "",
                        homepageUrl = "http://monkeyspace.org",
                        imageUrl = "",
                        lanyrdUrl = "",
                        location = "Boston, MA",
                        meetupUrl = "",
                        name = "MonkeySpace",
                        sessions = new List<SessionEntity>(),
                        slug = "MonkeySpace-2012",
                        start = new DateTime(2012, 10, 17),
                        tagLine = ".net. Everywhere.",
                        twitterHashTag = "#monkeySpace",
                        twitterName = "@monkeySpaceConf"
                    };

                    foreach (var day in root.days)
                    {
                        foreach (var session in day.sessions)
                        {
                            var sessionEntity = new SessionEntity()
                            {
                                _id = Guid.NewGuid(),
                                description = session.@abstract,
                                start = session.begins,
                                end = session.ends,
                                title = session.title,
                                room = session.location,
                                slug = session.title.ToLower().GenerateSlug(),
                                twitterHashTag = "#ms-" + session.title.ToLower().Trim().Replace(" ", "-").SafeSubstring(0, 10)
                            };
                            sessionEntity.speakers = new List<SpeakerEntity>();
                            foreach (var speaker in session.speakers)
                            {

                                if (!string.IsNullOrWhiteSpace(speaker.twitterHandle) && !speaker.twitterHandle.StartsWith("@"))
                                {
                                    speaker.twitterHandle = "@" + speaker.twitterHandle;
                                }

                                var speakerEntity = new SpeakerEntity()
                                {
                                    _id = Guid.NewGuid(),
                                    firstName = speaker.name.Split(' ')[0],
                                    lastName = speaker.name.Split(' ')[1],
                                    twitterName = speaker.twitterHandle,
                                    description = speaker.bio,
                                    profileImageUrl = speaker.headshotUrl,
                                    slug = speaker.name.ToLower().GenerateSlug()
                                };
                                sessionEntity.speakers.Add(speakerEntity);
                            }

                            conference.sessions.Add(sessionEntity);
                        }
                    }

                    collection.Save(conference);
                }
            }
        
        
        
        
        }
    }


    public class root
    {
        public List<day> days { get; set; }
    }

    public class day
    {
        public DateTime date { get; set; }
        public List<session> sessions { get; set; }
    }
    public class session
    {
        public int id { get; set; }
        public string title { get; set; }
        public string @abstract { get; set; }
        public List<speaker> speakers { get; set; }
        public DateTime begins { get; set; }
        public DateTime ends { get; set; }
        public string location { get; set; }
    }
    public class speaker
    {
        public int id { get; set; }
        public string name { get; set; }
        public string twitterHandle { get; set; }
        public string bio { get; set; }
        public string headshotUrl { get; set; }
    }
}
