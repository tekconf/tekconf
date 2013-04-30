using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentMongo.Linq;
using TekConf.UI.Api;

namespace UberImporter.Importers.AlohaRubyConf2012
{
    public class AlohaRubyConfImporter
    {
        public void Import()
        {
            var connection = new MongoDbConnection();
            var collection = connection.RemoteDatabase.GetCollection<ConferenceEntity>("conferences");
            if (!collection.AsQueryable().Any(c => c.slug == "AlohaRubyConf-2012"))
            {
                var conference = new ConferenceEntity()
                                     {
                                         //_id = Guid.NewGuid(),
                                         description =
                                             "Join us for this 2 day event which brings the Ruby and Rails community’s top speakers and talent together with excited attendees for an unforgettable experience in beautiful Hawaii.",
                                         end = new DateTime(2012, 10, 09),
                                         facebookUrl = "",
                                         homepageUrl = "http://aloharubyconf.com",
                                         imageUrl = "/img/conferences/AlohaRubyConf.png",
                                         lanyrdUrl = "",
                                         location = "Honolulu, HI",
                                         meetupUrl = "",
                                         name = "Aloha Ruby Conference",
                                         //sessions = new List<SessionEntity>(),
                                         //slug = "AlohaRubyConf-2012",
                                         start = new DateTime(2012, 10, 08),
                                         tagLine = "",
                                         twitterHashTag = "#alohaRuby",
                                         twitterName = "@alohaRuby"
                                     };

                conference.AddSession(AddSession1());
                conference.Save(collection);
                
            }
        }


        private static SessionEntity AddSession1()
        {
            var title = @"Registration & Breakfast";

            var sessionEntity = new SessionEntity()
            {
                _id = Guid.NewGuid(),
                title = title,
                description =
                    @"",
                start = new DateTime(2012, 10, 08, 08, 00, 00),
                end = new DateTime(2012, 10, 08, 09, 00, 00),
                room = "",
                slug = title.ToLower().GenerateSlug(),
                twitterHashTag = "#rc-" + title.ToLower().Trim().Replace(" ", "-").SafeSubstring(0, 10),
                difficulty = "",
                links = null,
                prerequisites = null,
                resources = null,
                sessionType = null,
                subjects = new List<string>() { },
                tags = new List<string>() { },
                speakers = new List<SpeakerEntity>(),
            };

           
            return sessionEntity;
        }

        private static SessionEntity AddSession2()
        {
            var title = @"Welcome & Introduction";

            var sessionEntity = new SessionEntity()
            {
                _id = Guid.NewGuid(),
                title = title,
                description =
                    @"",
                start = new DateTime(2012, 10, 08, 09, 00, 00),
                end = new DateTime(2012, 10, 08, 09, 15, 00),
                room = "",
                slug = title.ToLower().GenerateSlug(),
                twitterHashTag = "#rc-" + title.ToLower().Trim().Replace(" ", "-").SafeSubstring(0, 10),
                difficulty = "",
                links = null,
                prerequisites = null,
                resources = null,
                sessionType = null,
                subjects = new List<string>() { },
                tags = new List<string>() { },
                speakers = new List<SpeakerEntity>(),
            };

            
            return sessionEntity;
        }

        private static SessionEntity AddSession3()
        {
            var title = @"Keynote - Rails 4 and the Future of Web";

            var sessionEntity = new SessionEntity()
            {
                _id = Guid.NewGuid(),
                title = title,
                description =
                    @"What's new in Rails 4? How does Rails 4 fit in to the future of web development? Why are cats so important to the development of Ruby and Rails? All these questions and more will be answered if you attend this talk. Seats are limited, so act now!",
                start = new DateTime(2012, 10, 08, 09, 30, 00),
                end = new DateTime(2012, 10, 08, 10, 15, 00),
                room = "",
                slug = title.ToLower().GenerateSlug(),
                twitterHashTag = "#rc-" + title.ToLower().Trim().Replace(" ", "-").SafeSubstring(0, 10),
                difficulty = "",
                links = null,
                prerequisites = null,
                resources = null,
                sessionType = null,
                subjects = new List<string>() { },
                tags = new List<string>() { },
                speakers = new List<SpeakerEntity>(),
            };

            var twitterName = "";
            if (!string.IsNullOrWhiteSpace(twitterName) && !twitterName.StartsWith("@"))
            {
                twitterName = "@" + twitterName;
            }
            var firstName = "Aaron";
            var lastName = "Patterson";
            var speakerEntity = new SpeakerEntity()
            {
                _id = Guid.NewGuid(),
                firstName = firstName,
                lastName = lastName,
                description =
                    @"When he isn’t ruining people’s lives by writing software like phuby, enterprise, and neversaydie, Aaron can be found writing slightly more useful software like nokogiri. To keep up his Gameboy Lifestyle, Aaron spends his weekdays writing high quality software for ATTi. Be sure to catch him on Karaoke night, where you can watch him sing his favorite smooth rock hits of the 70’s and early 80’s.",
                bitbucketUrl = "",
                blogUrl = "http://tenderlovemaking.com/",
                codeplexUrl = null,
                coderWallUrl = null,
                company = "ATTi",
                emailAddress = null,
                facebookUrl = null,
                githubUrl = "https://github.com/tenderlove",
                googlePlusUrl = null,
                isFeatured = false,
                linkedInUrl = null,
                phoneNumber = null,
                profileImageUrl = null,
                slug = (firstName.ToLower() + " " + lastName.ToLower()).GenerateSlug(),
                stackoverflowUrl = null,
                twitterName = "@tenderlove",
                vimeoUrl = null,
                youtubeUrl = null,
            };

            sessionEntity.speakers.Add(speakerEntity);
            return sessionEntity;
        }

        private static SessionEntity AddSession4()
        {
            var title = @"Git and GitHub Secrets";

            var sessionEntity = new SessionEntity()
            {
                _id = Guid.NewGuid(),
                title = title,
                description =
                    "We tuck a lot of features away on github.com.\r\nSometimes the UI just hasn't been fleshed out. Or we have bigger plans in mind for the feature in the future. Or it just hasn't been finished yet. But we still want to give you the flexibility of using that feature today.\r\n\r\nThe same can be said about Git. If you've ever looked at the manpages, there's feature after feature and option after option in its binaries. Part of the strength of Git and GitHub is having access to those features when you need them, and getting them out of your way when you don't.\r\n\r\nThis talk covers both Git and GitHub: different tricks I've picked up after two years at GitHub, helpful advice on common gripes I've seen in support tickets and tweets, and just general nifty things that make you a faster, more capable technologist.",
                start = new DateTime(2012, 10, 08, 10, 30, 00),
                end = new DateTime(2012, 10, 08, 11, 15, 00),
                room = "",
                slug = title.ToLower().GenerateSlug(),
                twitterHashTag = "#rc-" + title.ToLower().Trim().Replace(" ", "-").SafeSubstring(0, 10),
                difficulty = "",
                links = null,
                prerequisites = null,
                resources = null,
                sessionType = null,
                subjects = new List<string>() { },
                tags = new List<string>() { },
                speakers = new List<SpeakerEntity>(),
            };

            var twitterName = "";
            if (!string.IsNullOrWhiteSpace(twitterName) && !twitterName.StartsWith("@"))
            {
                twitterName = "@" + twitterName;
            }
            var firstName = "Zach";
            var lastName = "Holman";
            var speakerEntity = new SpeakerEntity()
            {
                _id = Guid.NewGuid(),
                firstName = firstName,
                lastName = lastName,
                description =
                    "A Ruby developer with sound fundamentals, firm grasp on the industry, and innovative development approaches are all phrases inapplicable to Zach Holman. He works at GitHub, and hacks on sparkline generators, robot music DJs, and ethically frightening FaceTime + Chatroulette mashups. He blogs, he tweets, he evades his taxes.",
                bitbucketUrl = "",
                blogUrl = "http://zachholman.com/",
                codeplexUrl = null,
                coderWallUrl = null,
                company = "GitHub",
                emailAddress = null,
                facebookUrl = null,
                githubUrl = "https://github.com/holman",
                googlePlusUrl = null,
                isFeatured = false,
                linkedInUrl = null,
                phoneNumber = null,
                profileImageUrl = null,
                slug = (firstName.ToLower() + " " + lastName.ToLower()).GenerateSlug(),
                stackoverflowUrl = null,
                twitterName = "@holman",
                vimeoUrl = null,
                youtubeUrl = null,
            };

            sessionEntity.speakers.Add(speakerEntity);
            return sessionEntity;
        }

        private static SessionEntity AddSession5()
        {
            var title = @"My Server for Aiur: How Starcraft Taught Me To Scale";

            var sessionEntity = new SessionEntity()
            {
                _id = Guid.NewGuid(),
                title = title,
                description =
                    @"All the Starcraft n00bs know exactly how to win. They take all the resources they can, and upgrade all the expensive tech and think to themselves, ""soon i'll be unstoppable"". Unfortunately ""eventually unstoppable"" is the same as dead right now. This type of premature optimization and abstraction can kill a business faster than not being able to scale. In this talk we'll take a look at how to pick the right unit composition (databases vs. NoSQL), balance your macro and micro (scale out vs. up), and choose the right race (programing language). If you've never played Starcraft, and can't tell a ultralisk from a firebat, don't worry there's still a room for you. Sorry, no Zerg allowed.",
                start = new DateTime(2012, 10, 08, 10, 30, 00),
                end = new DateTime(2012, 10, 08, 11, 15, 00),
                room = "",
                slug = title.ToLower().GenerateSlug(),
                twitterHashTag = "#rc-" + title.ToLower().Trim().Replace(" ", "-").SafeSubstring(0, 10),
                difficulty = "",
                links = null,
                prerequisites = null,
                resources = null,
                sessionType = null,
                subjects = new List<string>() { },
                tags = new List<string>() { },
                speakers = new List<SpeakerEntity>(),
            };

            var twitterName = "";
            if (!string.IsNullOrWhiteSpace(twitterName) && !twitterName.StartsWith("@"))
            {
                twitterName = "@" + twitterName;
            }
            var firstName = "Richard";
            var lastName = "Schneeman";
            var speakerEntity = new SpeakerEntity()
            {
                _id = Guid.NewGuid(),
                firstName = firstName,
                lastName = lastName,
                description =
                    @"Richard was a Platinum 1v1 Random Starcraft 2 player, before he decided to resume seeing daylight. He now works for Heroku helping to scale millions of applications. He loves developing in Ruby and collecting vespene gas. When he's not at work he teaches Rails at the University of Texas and enjoys snowboarding.",
                bitbucketUrl = "",
                blogUrl = "http://schneems.com/",
                codeplexUrl = null,
                coderWallUrl = null,
                company = "Heroku",
                emailAddress = null,
                facebookUrl = null,
                githubUrl = "https://github.com/schneems",
                googlePlusUrl = null,
                isFeatured = false,
                linkedInUrl = null,
                phoneNumber = null,
                profileImageUrl = null,
                slug = (firstName.ToLower() + " " + lastName.ToLower()).GenerateSlug(),
                stackoverflowUrl = null,
                twitterName = "@schneems",
                vimeoUrl = null,
                youtubeUrl = null,
            };

            sessionEntity.speakers.Add(speakerEntity);
            return sessionEntity;
        }

        

        


    }
}
