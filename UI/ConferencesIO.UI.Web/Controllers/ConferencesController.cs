using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ConferencesIO.RemoteData.Dtos.v1;
using ConferencesIO.RemoteData.v1;

namespace ConferencesIO.UI.Web.Controllers
{
    public class ConferencesController : AsyncController
    {
        //
        // GET: /Conferences/

        public void IndexAsync(string conferenceSlug)
        {
            string baseUrl = ConfigurationManager.AppSettings["BaseUrl"];
            var repository = new RemoteDataRepository(baseUrl);

            AsyncManager.OutstandingOperations.Increment();
            repository.GetConferences(conferences =>
            {
                AsyncManager.Parameters["conferences"] = conferences;
                AsyncManager.OutstandingOperations.Decrement();
            });

        }

        public ActionResult IndexCompleted(List<ConferencesDto> conferences)
        {
            return View(conferences);
        }



        public void DetailAsync(string conferenceSlug)
        {
            string baseUrl = ConfigurationManager.AppSettings["BaseUrl"];
            var repository = new RemoteDataRepository(baseUrl);

            AsyncManager.OutstandingOperations.Increment();
            repository.GetConference(conferenceSlug, conference =>
            {
                AsyncManager.Parameters["conference"] = conference;
                AsyncManager.OutstandingOperations.Decrement();
            });

//            FullConferenceDto conference = null;

//            conference = new FullConferenceDto()
//              {
//                  slug = conferenceSlug,
//                  name = "That Conference",
//                  location = "Wisconsin Dells, WI",
//                  start = new DateTime(2012, 8, 15),
//                  end = new DateTime(2012, 8, 17),
//                  description = "Summer camp for geeks",
//                  facebookUrl = "http://facebook.com/ThatConference",
//                  homepageUrl = "http://thatConference.com",
//                  lanyrdUrl = "http://lanyrd.com",
//                  meetupUrl = "http://meetup.com",
//                  twitterHashTag = "#thatConference",
//                  twitterName = "@thatConference",
//                  imageUrl = "/ConferencesIO/img/conferences/ThatConference.png",
//                  tagline =
//                    "Spend 3 days, with 1000 of your fellow campers in 150 sessions geeking out on everything Mobile, Web and Cloud at a giant waterpark.",
//                  sessions = new List<FullSessionDto>()
//            {

//              new FullSessionDto()
//                {
//                  slug =
//                    "more-or-less-everything-you-wanted-to-know-about-making-games-with-xna-but-were-afraid-to-ask.",
//                  title =
//                    "(More or less) everything you wanted to know about making games with XNA but were afraid to ask.",
//                  start = new DateTime(2012, 8, 15, 9, 0, 0),
//                  end = new DateTime(2012, 8, 15, 10, 0, 0),
//                  room = "Tamarind",
//                  description =
//                    @"This session will take you from Game Dev Zero to Hero in 50 minutes flat. We'll take a whirlwind tour of XNA and cover the basics of creating a game, along with the different programming model that XNA uses. We'll cover device features too, like touch, gestures, GPS, accelerometer, etc.  At some point we'll even put some pretty pictures up on the screen and make them move around too.
//                    Afterwards, the speaker (that's me) will hang out and answer your questions about XNA, Game Development, Life in General and what it's like to own a game store / coffee shop in South Carolina.",
//                  twitterHashTag = "#cm-(more-or-l",
//                  sessionType = "Mobile",
//                  //url= "http://conferencesio.cloudapp.net:81/v1/conferences/ThatConference-2012/sessions/more-or-less-everything-you-wanted-to-know-about-making-games-with-xna-but-were-afraid-to-ask",
//                  //linksUrl= "http://conferencesio.cloudapp.net:81/v1/conferences/ThatConference-2012/sessions/more-or-less-everything-you-wanted-to-know-about-making-games-with-xna-but-were-afraid-to-ask/links",
//                  //tagsUrl= "http://conferencesio.cloudapp.net:81/v1/conferences/ThatConference-2012/sessions/more-or-less-everything-you-wanted-to-know-about-making-games-with-xna-but-were-afraid-to-ask/tags",
//                  //subjectsUrl= "http://conferencesio.cloudapp.net:81/v1/conferences/ThatConference-2012/sessions/more-or-less-everything-you-wanted-to-know-about-making-games-with-xna-but-were-afraid-to-ask/subjects",
//                  //speakersUrl= "http://conferencesio.cloudapp.net:81/v1/conferences/ThatConference-2012/sessions/more-or-less-everything-you-wanted-to-know-about-making-games-with-xna-but-were-afraid-to-ask/speakers",
//                  //prerequisitesUrl= "http://conferencesio.cloudapp.net:81/v1/conferences/ThatConference-2012/sessions/more-or-less-everything-you-wanted-to-know-about-making-games-with-xna-but-were-afraid-to-ask/prerequisites",
//                  links = new List<string>(),
//                  tags = new List<string>(),
//                  subjects = new List<string>(),
//                  prerequisites = new List<string>(),
//                  speakers =
//                    new List<FullSpeakerDto>()
//                      {new FullSpeakerDto() {slug = "chris-williams", firstName = "Chris", lastName = "Williams"}},
//                },
//                new FullSessionDto()
//                  {
//                    slug = "a-developer's-guide-to-web-images",
//                    title = "A Developer's Guide to Web Images",
//                    start = new DateTime(2012, 8, 15, 10, 0, 0),
//                    end = new DateTime(2012, 8, 15, 11, 0, 0),
//                    room = "E",
//                    description = "Most web developers have only a casual understanding of image formats and how to optimize images for web sites.  Learn which image formats are appropriate for different types of images (logos, photos, etc).  Discover various techniques for improving the performance of your site by serving images from a CDN, CSS Sprites, hosting images on multiple domains, and other approaches.  Web developers understand HTML, CSS, JavaScript, jQuery and other frameworks, why not review how to best utilize images as well.  ",
//                    twitterHashTag = "#cm-a-develope",
//                    sessionType = "Web",

//                    speakers = new List<FullSpeakerDto>()
//                      {
//                        new FullSpeakerDto()
//                          {
                            
//                         slug= "robert-boedigheimer",
//firstName= "Robert",
//lastName= "Boedigheimer"

//                          }
//                      }

//                  }
//            }
//              };

//            //repository.GetConference(conferenceSlug, c =>
//            //                          {
//            //                            conference = c;
//            //                          });
            
        }

        public ActionResult DetailCompleted(ConferenceDto conference)
        {
            return View(conference);
        }
    }
}
