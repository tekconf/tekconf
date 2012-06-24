using System;
using System.Collections.Generic;
using System.Net;
using ArtekSoftware.Conference.RemoteData.Dtos;
using ServiceStack.Text;

namespace ArtekSoftware.Conference.UI.Web.Tests.Int
{
  public class RestTestBase
  {
    //private string rootUrl = "http://conference.azurewebsites.net";
    public string rootUrl = "http://localhost:6327";

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
        url = "http://localhost:6327/api/conferences/CodeMash-2012/sessions/ubiquitous-app-development-with-phonegap",
      };

    public ConferencesDto codemash = new ConferencesDto()
      {
        name = "That Conference",
        url = "http://localhost:6327/api/conferences/CodeMash-2012",
        location = "Kalahari Resort, Wisconsin Dells, WI",
        start = DateTime.Parse("2012/08/13"),
        end = DateTime.Parse("2012/08/15"),
        slug = "CodeMash-2012"
      };

    public ScheduleDto robsSchedule = new ScheduleDto()
      {
        
      };
  }
}