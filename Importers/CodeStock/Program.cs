using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Net;
using FluentMongo.Linq;
using ServiceStack.Text;
using TekConf.UI.Api;

namespace CodeStock
{
    class Program
    {
        static void Main(string[] args)
        {
            var sessionsJsonUrl = "http://codestock.org/api/v2.0.svc/AllSessionsJson";
            var sessionsRequest = new WebClient();
            var sessionsJson = sessionsRequest.DownloadString(sessionsJsonUrl);
            var sessionsRoot = sessionsJson.FromJson<sessionsRoot>();


            var speakersJsonUrl = "http://codestock.org/api/v2.0.svc/AllSpeakersJson";
            var speakersRequest = new WebClient();
            var speakersJson = speakersRequest.DownloadString(speakersJsonUrl);
            var speakersRoot = speakersJson.FromJson<speakersRoot>();


            if (sessionsRoot != null)
            {
                MongoDbConnection connection = new MongoDbConnection();
                var collection = connection.RemoteDatabase.GetCollection<ConferenceEntity>("conferences");
                if (!collection.AsQueryable().Any(c => c.name == "CodeStock"))
                {
                    var conference = new ConferenceEntity()
                    {
                        _id = Guid.NewGuid(),
                        description = @"CodeStock is a two day event for technology and information exchange.  Created by the community, for the community – this is not an industry trade show pushing the latest in marketing as technology, but a gathering of working professionals sharing knowledge and experience.",
                        end = new DateTime(2012, 6, 16),
                        facebookUrl = "",
                        homepageUrl = "http://codestock.org",
                        imageUrl = "/img/conferences/CodeStock.png",
                        lanyrdUrl = "",
                        location = "Knoxville, TN",
                        meetupUrl = "",
                        name = "CodeStock",
                        sessions = new List<SessionEntity>(),
                        slug = "CodeStock-2012",
                        start = new DateTime(2012, 6, 14),
                        tagLine = "Gathered together from the cosmic reaches of the universe...",
                        twitterHashTag = "#codeStock",
                        twitterName = "@codeStock",
                        
                    };

                    foreach (var session in sessionsRoot.d)
                    {
                        var sessionEntity = new SessionEntity()
                        {
                            _id = Guid.NewGuid(),
                            description = session.Abstract,
                            start = session.StartTime,
                            end = session.EndTime,
                            title = session.Title,
                            room = session.Room,
                            slug = session.Title.ToLower().Replace(" ", "-"),
                            twitterHashTag = "#ms-" + session.Title.ToLower().Trim().Replace(" ", "-").SafeSubstring(0, 10),
                            subjects = new List<string>() { session.Area, session.Technology },
                            sessionType = session._type,
                            tags = new List<string>() { session.Track, session.Area, session.Technology },
                            difficulty = session.LevelGeneral + " " + session.LevelSpecific
                        };



                        session session1 = session;
                        var speaker = speakersRoot.d.FirstOrDefault(s => s.SpeakerID == session1.SpeakerID);

                        if (speaker != null)
                        {
                            var speakerEntity = new SpeakerEntity();

                            if (!string.IsNullOrWhiteSpace(speaker.TwitterID) && !speaker.TwitterID.StartsWith("@"))
                            {
                                speaker.TwitterID = "@" + speaker.TwitterID;
                            }

                            var nameArray = speaker.Name.Split(' ');
                            speakerEntity._id = Guid.NewGuid();
                            speakerEntity.description = speaker.Bio;
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
                            speakerEntity.profileImageUrl = speaker.PhotoUrl;
                            speakerEntity.twitterName = speaker.TwitterID;
                            speakerEntity.blogUrl = speaker.Website;

                            sessionEntity.speakers = new List<SpeakerEntity>() { speakerEntity };
                        }

                        conference.sessions.Add(sessionEntity);
                    }

                    var dsds = conference;
                    collection.Save(conference);
                }

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
        public int SessionID { get; set; }
        public string _type { get; set; }
        public string @Abstract { get; set; }
        public List<int> AdditionalSpeakerIDs { get; set; }
        public string Area { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string LevelGeneral { get; set; }
        public string LevelSpecific { get; set; }
        public string Room { get; set; }
        public int SpeakerID { get; set; }
        public string Technology { get; set; }
        public string Title { get; set; }
        public string Track { get; set; }
        public string Url { get; set; }
        public string VoteRank { get; set; }

    }


    public class speakersRoot
    {
        public List<speaker> d { get; set; }
    }

    public class speaker
    {
        public int SpeakerID { get; set; }
        public string _type { get; set; }
        public string Bio { get; set; }
        public string Company { get; set; }
        public string Name { get; set; }
        public string PhotoUrl { get; set; }
        public string TwitterID { get; set; }
        public string Url { get; set; }
        public string Website { get; set; }

    }

    public static class StringExtensions
    {
        public static string SafeSubstring(this string input, int startIndex, int length)
        {
            // Todo: Check that startIndex + length does not cause an arithmetic overflow
            if (input.Length >= (startIndex + length))
            {
                return input.Substring(startIndex, length);
            }
            else
            {
                if (input.Length > startIndex)
                {
                    return input.Substring(startIndex);
                }
                else
                {
                    return string.Empty;
                }
            }
        }
    }

}