using System;
using System.Collections.Generic;
using System.Net;
using ArtekSoftware.Conference.RemoteData.Dtos;
using ServiceStack.Text;

namespace ArtekSoftware.Conference.UI.Web.Tests.Int
{
    public class RestTestBase
    {
        //public static string rootUrl = "http://conference.azurewebsites.net";
        public static string rootUrl = "http://localhost/ArtekSoftware.Conference.UI.Web";

        public SpeakerDto robGibbens = new SpeakerDto();

        public List<SessionsDto> codemashSessions = new List<SessionsDto>();

        public SessionDto phonegap = new SessionDto()
          {
              conferenceSlug = "CodeMash-2012",
              description = "phonegap",
              slug = "ubiquitous-app-development-with-phonegap",
              start = DateTime.Parse("11/08/2012"),
              end = DateTime.Parse("11/08/2012"),
              difficulty = "100",
              links = new List<string>(),
              linksUrl = "",
              room = "Lobby",
              sessionType = "Mobile",
              speakers = new List<SpeakersDto>(),
              speakersUrl = "",
              subjects = new List<string>(),
              subjectsUrl = "",
              tags = new List<string>(),
              tagsUrl = "",
              title = "Cross Platform Apps with Mono",
              twitterHashTag = "#tc-phonegap",
              url = rootUrl + "/api/conferences/CodeMash-2012/sessions/ubiquitous-app-development-with-phonegap",
          };

        public ConferencesDto codemash = new ConferencesDto()
          {
              name = "CodeMash",
              url = rootUrl + 2222222222"/api/conferences/CodeMash-2012",
              location = "Sandusky, OH, USA",
              start = DateTime.Parse("2012/01/10 5:00:00 AM"),
              end = DateTime.Parse("2012/01/13 5:00:00 AM"),
              slug = "CodeMash-2012"
          };

        public ScheduleDto robsSchedule = new ScheduleDto()
          {
              conferenceSlug = "CodeMash-2012",
              userSlug = "rob-gibbens",
              sessions = new List<string>()
                             {
                                rootUrl + "/api/conferences/CodeMash-2012/sessions/ubiquitous-app-development-with-phonegap",
                                rootUrl + "/api/conferences/CodeMash-2012/sessions/new---dealing-with-information-overload",
                                rootUrl + "/api/conferences/CodeMash-2012/sessions/actor-model-programming-in-c",
                                rootUrl + "/api/conferences/CodeMash-2012/sessions/an-introduction-to-signalr",
                                rootUrl + "/api/conferences/CodeMash-2012/sessions/asp.net-mvc-vs.-ruby-on-rails",
                                rootUrl + "/api/conferences/CodeMash-2012/sessions/beautiful-front-end-code-with-backbone.js-and-coffeescript"
                             }
          };
    }
}