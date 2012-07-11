using System;
using System.Collections.Generic;
using System.Net;
using ConferencesIO.RemoteData.Dtos;
using ConferencesIO.RemoteData.Dtos.v1;
using ServiceStack.Text;

namespace ConferencesIO.UI.Api.Tests.Int
{
  public class RestTestBase
  {
    //public static string rootUrl = "http://conferencesioapi.azurewebsites.net";
    public static string rootUrl = "http://localhost/ConferencesIO.UI.Api/v1";

    
    public SpeakerDto andrewGlover = new SpeakerDto()
    {
      firstName = "Andrew",
      lastName = "Glover",
      slug = "andrew-glover",
      description = "Andrew is the CTO of App47, where he gets to play with iOS, Android, Ruby, Rails, Heroku, AWS, MongoDB and everything else that is cool these days. He carries around an iPhone, iPad, and HTC Droid phone and in his free time hacks on Node.js.",
      blogUrl = "http://www.thediscoblog.com",
      twitterName = "@aglover",
      emailAddress = "",
      facebookUrl = "",
      linkedInUrl = "",
      phoneNumber = ""
    };

    public List<SessionsDto> codemashSessions = new List<SessionsDto>();

    public SessionDto phonegap = new SessionDto()
    {
      //conferenceSlug = "CodeMash-2012",
      description = "You’ve been tasked to build an app for your company that does x,y, and z. You’ve also been informed that it needs to work on iOS and Android. You think “no problem!” -- that’s what HTML 5 is for! But then you find out that the app needs native features like GPS and a camera. What are you to do? PhoneGap is an innovative framework that allows you to build mobile apps in HTML 5 that have access to device features reserved for native apps. Simply put: with PhoneGap, you can build HTML 5 apps that can use device features like geolocation, the accelerometer, and even a camera, for example. In this session, you’ll learn how to build a web-based mobile app using HTML 5 and JavaScript that is able to live inside the PhoneGap container and take advantage of native features, such as GPS. You’ll see that with PhoneGap the same web app can then be deployed onto an iOS device and an Android one. One app. Multiple device platforms. Job done.",
      slug = "ubiquitous-app-development-with-phonegap",
      start = DateTime.Parse("1/13/2012 3:45:00 PM"),
      end = DateTime.Parse("1/13/2012 4:45:00 PM"),
      difficulty = "Beginner",
      links = new List<string>(),
      linksUrl = null,
      room = "Indigo Bay",
      sessionType = "Mobile",
      speakers = new List<SpeakersDto>() { new SpeakersDto() { slug = "andrew-glover", firstName = "Andrew", lastName = "Glover", url = "http://localhost/ConferencesIO.UI.Api/v1/conferences/sessions/speakers/andrew-glover" } },
      speakersUrl = "http://localhost/ConferencesIO.UI.Api/v1/conferences/sessions/ubiquitous-app-development-with-phonegap/speakers",
      subjects = new List<string>() { "Mobile" },
      subjectsUrl = null,
      tags = new List<string>() { "Mobile" },
      tagsUrl = null,
      title = "Ubiquitous App development with PhoneGap",
      twitterHashTag = "#cm-ubiquitous",
      url = "http://localhost/ConferencesIO.UI.Api/v1/conferences/sessions/ubiquitous-app-development-with-phonegap"
    };

    public ConferencesDto codemashs = new ConferencesDto()
    {
      name = "CodeMash",
      url = "http://localhost/ConferencesIO.UI.Api/v1/conferences/CodeMash-2012",
      location = "Sandusky, OH, USA",
      start = DateTime.Parse("2012/01/10 5:00:00 AM"),
      end = DateTime.Parse("2012/01/13 5:00:00 AM"),
      slug = "CodeMash-2012",

    };

    public ConferenceDto codemash = new ConferenceDto()
    {
      name = "CodeMash",
      url = "http://localhost/ConferencesIO.UI.Api/v1/conferences/CodeMash-2012",
      location = "Sandusky, OH, USA",
      start = DateTime.Parse("2012/01/10 5:00:00 AM"),
      end = DateTime.Parse("2012/01/13 5:00:00 AM"),
      slug = "CodeMash-2012",
      sessionsUrl = "http://localhost/ConferencesIO.UI.Api/v1/conferences/CodeMash-2012/sessions",
      speakersUrl = "http://localhost/ConferencesIO.UI.Api/v1/conferences/CodeMash-2012/speakers",
    };

    public ScheduleDto robsSchedule = new ScheduleDto()
    {
      conferenceSlug = "CodeMash-2012",
      userSlug = "rob-gibbens",
      url = "http://localhost/ConferencesIO.UI.Api/v1/conferences/CodeMash-2012/schedule/rob-gibbens",
      sessions = new List<string>()
          {
            "http://localhost/ConferencesIO.UI.Api/v1/conferences/CodeMash-2012/sessions/ubiquitous-app-development-with-phonegap",
            "http://localhost/ConferencesIO.UI.Api/v1/conferences/CodeMash-2012/sessions/new---dealing-with-information-overload",
            "http://localhost/ConferencesIO.UI.Api/v1/conferences/CodeMash-2012/sessions/actor-model-programming-in-c",
            "http://localhost/ConferencesIO.UI.Api/v1/conferences/CodeMash-2012/sessions/an-introduction-to-signalr",
            "http://localhost/ConferencesIO.UI.Api/v1/conferences/CodeMash-2012/sessions/asp.net-mvc-vs.-ruby-on-rails",
            "http://localhost/ConferencesIO.UI.Api/v1/conferences/CodeMash-2012/sessions/beautiful-front-end-code-with-backbone.js-and-coffeescript"
  
          }
    };
  }

}