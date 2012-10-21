using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using FluentMongo.Linq;
using ServiceStack.Text;
using TekConf.UI.Api;
using TinyMessenger;

namespace UberImporter.Importers.CodeMash2013
{
    //
    //
    public class CodeMash2013Importer
    {
        public void Import()
        {
            var sessionsJsonUrl = "http://rest.codemash.org:8080/api/sessions.jsonp";
            var sessionsRequest = new WebClient();
            var sessionsJson = sessionsRequest.DownloadString(sessionsJsonUrl);
            var sessionsRoot = sessionsJson.FromJson<List<session>>();


            var speakersJsonUrl = "http://rest.codemash.org:8080/api/speakers.jsonp";
            var speakersRequest = new WebClient();
            var speakersJson = speakersRequest.DownloadString(speakersJsonUrl);
            var speakersRoot = speakersJson.FromJson<List<speaker>>();


            if (sessionsRoot != null)
            {
                MongoDbConnection connection = new MongoDbConnection();
                var collection = connection.RemoteDatabase.GetCollection<ConferenceEntity>("conferences");
                var existingConf = collection.AsQueryable().FirstOrDefault(c => c.slug == "codemash-2013");
                ConferenceEntity conference = null;

                if (existingConf == null)
                {
                    conference = new ConferenceEntity()
                                         {
                                             //_id = Guid.NewGuid(),
                                             description =
                                                 @"CodeMash is a unique event that will educate developers on current practices, methodologies, and technology trends in a variety of platforms and development languages such as Java, .Net, Ruby, Python and PHP.",
                                             end = new DateTime(2013, 1, 11),
                                             facebookUrl = "",
                                             homepageUrl = "http://codestock.org",
                                             imageUrl = "/img/conferences/CodeStock.png",
                                             lanyrdUrl = "",
                                             location = "Knoxville, TN",
                                             meetupUrl = "",
                                             name = "CodeStock",
                                             //sessions = new List<SessionEntity>(),
                                             //slug = "CodeStock-2012",
                                             start = new DateTime(2013, 1, 8),
                                             tagLine = "Gathered together from the cosmic reaches of the universe...",
                                             twitterHashTag = "#codeStock",
                                             twitterName = "@codeStock",

                                         };
                }
                else
                {
                    conference = existingConf;
                }
                conference.Hub = new TinyMessengerHub();

                foreach (var session in sessionsRoot)
                {
                    var sessionEntity = new SessionEntity()
                    {
                        _id = Guid.NewGuid(),
                        description = session.Abstract,
                        start = DateTime.MinValue, //session.StartTime,
                        end = DateTime.MinValue, //session.EndTime,
                        title = session.Title,
                        room = "", //session.Room,
                        slug = session.Title.GenerateSlug(),
                        twitterHashTag = "#ms-" + session.Title.ToLower().Trim().Replace(" ", "-").SafeSubstring(0, 10),
                        subjects = new List<string>() { },
                        sessionType = session.EventType,
                        tags = new List<string>() { },
                        difficulty = session.Difficulty
                    };

                    session session1 = session;
                    var speaker = speakersRoot.FirstOrDefault(s => s.SpeakerURI == session1.SpeakerURI);

                    if (speaker != null)
                    {
                        var speakerEntity = new SpeakerEntity();

                        if (!string.IsNullOrWhiteSpace(speaker.TwitterHandle) && !speaker.TwitterHandle.StartsWith("@"))
                        {
                            speaker.TwitterHandle = "@" + speaker.TwitterHandle;
                        }

                        var nameArray = speaker.Name.Split(' ');
                        speakerEntity._id = Guid.NewGuid();
                        speakerEntity.description = speaker.Biography;
                        if (nameArray.Count() > 1)
                        {
                            speakerEntity.firstName = speaker.Name.Split(' ')[0];
                            speakerEntity.lastName = speaker.Name.Split(' ')[1];
                        }
                        else
                        {
                            speakerEntity.firstName = string.Empty;
                            speakerEntity.lastName = speaker.Name;
                        }
                        speakerEntity.slug = (speakerEntity.firstName.ToLower() + " " + speakerEntity.lastName.ToLower()).Trim().Replace(" ", "-");
                        speakerEntity.profileImageUrl = ""; //speaker.PhotoUrl;
                        speakerEntity.twitterName = speaker.TwitterHandle;
                        speakerEntity.blogUrl = speaker.BlogURL; //speaker.Website;

                        sessionEntity.speakers = new List<SpeakerEntity>() { speakerEntity };
                    }

                    conference.AddSession(sessionEntity);
                }

                var dsds = conference;
                conference.Publish();
                conference.Save(collection);


            }



        }
    }


    public class sessionsRoot
    {
        public List<session> d { get; set; }
    }

    public class d
    {
        public List<session> sessions { get; set; }
    }

    public class session
    {
        public string Title { get; set; }
        public string @Abstract { get; set; }
        public string Difficulty { get; set; }
        public string SpeakerName { get; set; }
        public string Technology { get; set; }
        public string URI { get; set; }
        public string EventType { get; set; }
        public string SessionLookupId { get; set; }
        public string SpeakerURI { get; set; }


    }

    public class speakersRoot
    {
        public List<speaker> d { get; set; }
    }

    public class speaker
    {
        public string SpeakerURI { get; set; }
        public string Name { get; set; }
        public string Biography { get; set; }
        public string TwitterHandle { get; set; }
        public string BlogURL { get; set; }
        public string LookupId { get; set; }
        public string Url { get; set; }

    }

}
