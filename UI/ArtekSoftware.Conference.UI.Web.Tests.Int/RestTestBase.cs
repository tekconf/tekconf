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

    public ConferenceDto GetConference(string conferenceSlug)
    {
      string url = rootUrl + "/api/conferences/" + conferenceSlug;

      var client = new WebClient();
      //client.Headers[HttpRequestHeader.ContentType] = "application/json";
      client.Headers[HttpRequestHeader.Accept] = "application/json";
      var returnString = client.DownloadString(new Uri(url));
      var conference = JsonSerializer.DeserializeFromString<ConferenceDto>(returnString);
      return conference;
    }

    public List<SessionsDto> GetConferenceSessions(string conferenceSlug)
    {
      string url = rootUrl + "/api/conferences/" + conferenceSlug + "/sessions";

      var client = new WebClient();
      //client.Headers[HttpRequestHeader.ContentType] = "application/json";
      client.Headers[HttpRequestHeader.Accept] = "application/json";
      var returnString = client.DownloadString(new Uri(url));
      var sessions = JsonSerializer.DeserializeFromString<List<SessionsDto>>(returnString);
      return sessions;
    }

    public SessionDto GetConferenceSession(string conferenceSlug, string sessionSlug)
    {
      string url = rootUrl + "/api/conferences/" + conferenceSlug + "/sessions/" + sessionSlug;

      var client = new WebClient();
      //client.Headers[HttpRequestHeader.ContentType] = "application/json";
      client.Headers[HttpRequestHeader.Accept] = "application/json";
      var returnString = client.DownloadString(new Uri(url));
      var session = JsonSerializer.DeserializeFromString<SessionDto>(returnString);
      return session;
    }

    public List<string> GetConferenceSessionPrerequisites(string conferenceSlug, string sessionSlug)
    {
      string url = rootUrl + "/api/conferences/" + conferenceSlug + "/sessions/" + sessionSlug + "/prerequisites";

      var client = new WebClient();
      //client.Headers[HttpRequestHeader.ContentType] = "application/json";
      client.Headers[HttpRequestHeader.Accept] = "application/json";
      var returnString = client.DownloadString(new Uri(url));
      var prerequisites = JsonSerializer.DeserializeFromString<List<string>>(returnString);
      return prerequisites;
    }

    public List<string> GetConferenceSessionLinks(string conferenceSlug, string sessionSlug)
    {
      string url = rootUrl + "/api/conferences/" + conferenceSlug + "/sessions/" + sessionSlug + "/links";

      var client = new WebClient();
      //client.Headers[HttpRequestHeader.ContentType] = "application/json";
      client.Headers[HttpRequestHeader.Accept] = "application/json";
      var returnString = client.DownloadString(new Uri(url));
      var links = JsonSerializer.DeserializeFromString<List<string>>(returnString);
      return links;
    }

    public List<string> GetConferenceSessionResources(string conferenceSlug, string sessionSlug)
    {
      string url = rootUrl + "/api/conferences/" + conferenceSlug + "/sessions/" + sessionSlug + "/resources";

      var client = new WebClient();
      //client.Headers[HttpRequestHeader.ContentType] = "application/json";
      client.Headers[HttpRequestHeader.Accept] = "application/json";
      var returnString = client.DownloadString(new Uri(url));
      var resources = JsonSerializer.DeserializeFromString<List<string>>(returnString);
      return resources;
    }

    public List<SpeakersDto> GetConferenceSessionSpeakers(string conferenceSlug, string sessionSlug)
    {
      string url = rootUrl + "/api/conferences/" + conferenceSlug + "/sessions/" + sessionSlug + "/speakers";

      var client = new WebClient();
      //client.Headers[HttpRequestHeader.ContentType] = "application/json";
      client.Headers[HttpRequestHeader.Accept] = "application/json";
      var returnString = client.DownloadString(new Uri(url));
      var speakers = JsonSerializer.DeserializeFromString<List<SpeakersDto>>(returnString);
      return speakers;
    }


    public List<SpeakersDto> GetConferenceSpeakers(string conferenceSlug)
    {
      string url = rootUrl + "/api/conferences/" + conferenceSlug + "/speakers";

      var client = new WebClient();
      //client.Headers[HttpRequestHeader.ContentType] = "application/json";
      client.Headers[HttpRequestHeader.Accept] = "application/json";
      var returnString = client.DownloadString(new Uri(url));
      var speakers = JsonSerializer.DeserializeFromString<List<SpeakersDto>>(returnString);
      return speakers;
    }

    public SpeakerDto GetConferenceSessionSpeaker(string conferenceSlug, string sessionSlug, string speakerSlug)
    {
      string url = rootUrl + "/api/conferences/" + conferenceSlug + "/sessions/" + sessionSlug + "/speakers/" + speakerSlug;

      var client = new WebClient();
      //client.Headers[HttpRequestHeader.ContentType] = "application/json";
      client.Headers[HttpRequestHeader.Accept] = "application/json";
      var returnString = client.DownloadString(new Uri(url));
      var speaker = JsonSerializer.DeserializeFromString<SpeakerDto>(returnString);
      return speaker;
    }

    public SpeakerDto GetConferenceSpeaker(string conferenceSlug, string speakerSlug)
    {
      string url = rootUrl + "/api/conferences/" + conferenceSlug + "/speakers/" + speakerSlug;

      var client = new WebClient();
      //client.Headers[HttpRequestHeader.ContentType] = "application/json";
      client.Headers[HttpRequestHeader.Accept] = "application/json";
      var returnString = client.DownloadString(new Uri(url));
      var speaker = JsonSerializer.DeserializeFromString<SpeakerDto>(returnString);
      return speaker;
    }

    public List<ConferencesDto> GetConferences()
    {
      string url = rootUrl + "/api/conferences";

      var client = new WebClient();
      //client.Headers[HttpRequestHeader.ContentType] = "application/json";
      client.Headers[HttpRequestHeader.Accept] = "application/json";
      var returnString = client.DownloadString(new Uri(url));
      var conferences = JsonSerializer.DeserializeFromString<List<ConferencesDto>>(returnString);
      return conferences;
    }

    public SpeakerDto robGibbens = new SpeakerDto();

    public List<SessionsDto> thatConferenceSessions = new List<SessionsDto>();

    public SessionDto activejdbc = new SessionDto()
      {
        conferenceSlug = "ThatConference-2012",
        description = "activejdbc",
        slug = "activejdbc",
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
        twitterHashTag = "#tc-activejdbc",
        url = "http://localhost:6327/api/conferences/ThatConference-2012/sessions/activejdbc",


      };

    public ConferencesDto thatConference = new ConferencesDto()
      {
        name = "That Conference",
        url = "http://localhost:6327/api/conferences/ThatConference-2012",
        location = "Kalahari Resort, Wisconsin Dells, WI",
        start = DateTime.Parse("2012/08/13"),
        end = DateTime.Parse("2012/08/15"),
        slug = "ThatConference-2012"
      };
  }
}