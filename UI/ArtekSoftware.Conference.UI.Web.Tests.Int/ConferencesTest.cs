using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using KellermanSoftware.CompareNetObjects;
using NUnit.Framework;
using ServiceStack.Text;
using Should;

namespace ArtekSoftware.Conference.UI.Web.Tests.Int
{
  [TestFixture]
  public class ConferencesTest
  {
    //private string rootUrl = "http://conference.azurewebsites.net";
    private string rootUrl = "http://localhost:6327";

    [Test(Description = "http://localhost:6327/api/conferences")]
    public void given_a_GET_request_for_all_conferences_it_returns_subset_of_conference_info()
    {
      //GetConferences().FirstOrDefault().IsTheSameAs(thatConference).ShouldBeTrue();
      var conference = GetConferences().FirstOrDefault();
      var compareObjects = new CompareObjects();
      var areSame = compareObjects.Compare(conference, thatConference);
      if (!areSame)
      {
        Assert.Fail(compareObjects.DifferencesString);
      }
    }

    [Test(Description = "http://localhost:6327/api/conferences")]
    public void given_a_GET_request_for_all_conferences_it_returns_list_of_conferences_with_urls_to_details()
    {
      GetConferences().Count.ShouldBeInRange(1, int.MaxValue);
    }

    [Test(Description = "http://localhost:6327/api/conferences/ThatConference-2012")]
    public void given_a_GET_request_for_a_single_conference_it_returns_general_info_with_a_link_to_sessions()
    {
      var conference = GetConference(thatConference.slug);
      var compareObjects = new CompareObjects();
      var areSame = compareObjects.Compare(conference, thatConference);
      if (!areSame)
      {
        Assert.Fail(compareObjects.DifferencesString);
      }
    }

    [Test(Description = "http://localhost:6327/api/conferences/ThatConference-2012/sessions")]
    public void given_a_GET_request_for_a_single_conference_sessions_it_returns_sessions_with_speakers()
    {
      var sessions = GetConferenceSessions(thatConference.slug);
      var compareObjects = new CompareObjects();
      var areSame = compareObjects.Compare(sessions, thatConferenceSessions);
      if (!areSame)
      {
        Assert.Fail(compareObjects.DifferencesString);
      }
    }

    [Test(Description = "http://localhost:6327/api/conferences/ThatConference-2012/sessions/activejdbc")]
    public void given_a_GET_request_for_a_single_session_it_returns_a_session_with_child_collections()
    {
      var session = GetConferenceSession(thatConference.slug, activejdbc.slug);
      var compareObjects = new CompareObjects();
      var areSame = compareObjects.Compare(session, activejdbc);
      if (!areSame)
      {
        Assert.Fail(compareObjects.DifferencesString);
      }
    }

    [Test(Description = "http://localhost:6327/api/conferences/ThatConference-2012/sessions/activejdbc/speakers")]
    public void given_a_GET_request_for_session_speakers_it_returns_speakers()
    {
      var speakers = GetConferenceSessionSpeakers(thatConference.slug, activejdbc.slug);
      var compareObjects = new CompareObjects();
      var areSame = compareObjects.Compare(speakers, activejdbc.speakers);
      if (!areSame)
      {
        Assert.Fail(compareObjects.DifferencesString);
      }
    }

    [Test(Description = "http://localhost:6327/api/conferences/ThatConference-2012/sessions/activejdbc/speakers/igor-polevoy")]
    public void given_a_GET_request_for_a_single_session_speaker_it_returns_the_speaker()
    {
      var speaker = GetConferenceSessionSpeaker(thatConference.slug, activejdbc.slug, robGibbens.slug);
      var compareObjects = new CompareObjects();
      var areSame = compareObjects.Compare(speaker, robGibbens);
      if (!areSame)
      {
        Assert.Fail(compareObjects.DifferencesString);
      }
    }

    [Test(Description = "http://localhost:6327/api/conferences/ThatConference-2012/sessions/activejdbc/prerequisites")]
    public void given_a_GET_request_for_a_prerequisites_it_returns_prerequisites()
    {
      Assert.Fail();
    }

    [Test(Description = "http://localhost:6327/api/conferences/ThatConference-2012/sessions/activejdbc/links")]
    public void given_a_GET_request_for_links_it_returns_links()
    {
      Assert.Fail();
    }

    [Test(Description = "http://localhost:6327/api/conferences/ThatConference-2012/sessions/activejdbc/resources")]
    public void given_a_GET_request_for_resources_it_returns_resources()
    {
      Assert.Fail();
    }

    [Test(Description = "http://localhost:6327/api/conferences/ThatConference-2012/speakers")]
    public void given_a_GET_request_for_conference_speakers_it_returns_all_speakers()
    {
      Assert.Fail();
    }

    [Test(Description = "http://localhost:6327/api/conferences/ThatConference-2012/speakers/igor-polevoy")]
    public void given_a_GET_request_for_a_single_conference_speaker_it_returns_the_speaker()
    {
      Assert.Fail();
    }







    ConferenceDto GetConference(string conferenceSlug)
    {
      string url = rootUrl + "/api/conferences/" + conferenceSlug;

      var client = new WebClient();
      //client.Headers[HttpRequestHeader.ContentType] = "application/json";
      client.Headers[HttpRequestHeader.Accept] = "application/json";
      var returnString = client.DownloadString(new Uri(url));
      var conference = JsonSerializer.DeserializeFromString<ConferenceDto>(returnString);
      return conference;
    }

    List<SessionsDto> GetConferenceSessions(string conferenceSlug)
    {
      string url = rootUrl + "/api/conferences/" + conferenceSlug + "/sessions";

      var client = new WebClient();
      //client.Headers[HttpRequestHeader.ContentType] = "application/json";
      client.Headers[HttpRequestHeader.Accept] = "application/json";
      var returnString = client.DownloadString(new Uri(url));
      var sessions = JsonSerializer.DeserializeFromString<List<SessionsDto>>(returnString);
      return sessions;
    }

    SessionDto GetConferenceSession(string conferenceSlug, string sessionSlug)
    {
      string url = rootUrl + "/api/conferences/" + conferenceSlug + "/sessions/" + sessionSlug;

      var client = new WebClient();
      //client.Headers[HttpRequestHeader.ContentType] = "application/json";
      client.Headers[HttpRequestHeader.Accept] = "application/json";
      var returnString = client.DownloadString(new Uri(url));
      var session = JsonSerializer.DeserializeFromString<SessionDto>(returnString);
      return session;
    }

    List<SpeakersDto> GetConferenceSessionSpeakers(string conferenceSlug, string sessionSlug)
    {
      string url = rootUrl + "/api/conferences/" + conferenceSlug + "/sessions/" + sessionSlug + "/speakers";

      var client = new WebClient();
      //client.Headers[HttpRequestHeader.ContentType] = "application/json";
      client.Headers[HttpRequestHeader.Accept] = "application/json";
      var returnString = client.DownloadString(new Uri(url));
      var speakers = JsonSerializer.DeserializeFromString<List<SpeakersDto>>(returnString);
      return speakers;
    }

    SpeakerDto GetConferenceSessionSpeaker(string conferenceSlug, string sessionSlug, string speakerSlug)
    {
      string url = rootUrl + "/api/conferences/" + conferenceSlug + "/sessions/" + sessionSlug + "/speakers/" + speakerSlug;

      var client = new WebClient();
      //client.Headers[HttpRequestHeader.ContentType] = "application/json";
      client.Headers[HttpRequestHeader.Accept] = "application/json";
      var returnString = client.DownloadString(new Uri(url));
      var speaker = JsonSerializer.DeserializeFromString<SpeakerDto>(returnString);
      return speaker;
    }

    List<ConferencesDto> GetConferences()
    {
      string url = rootUrl + "/api/conferences";

      var client = new WebClient();
      //client.Headers[HttpRequestHeader.ContentType] = "application/json";
      client.Headers[HttpRequestHeader.Accept] = "application/json";
      var returnString = client.DownloadString(new Uri(url));
      var conferences = JsonSerializer.DeserializeFromString<List<ConferencesDto>>(returnString);
      return conferences;
    }

    SpeakerDto robGibbens = new SpeakerDto();

    List<SessionsDto> thatConferenceSessions = new List<SessionsDto>();

    SessionDto activejdbc = new SessionDto()
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
        url = "http://localhost:6327/api/conferences/ThatConference-2012/sessions/activejdbc"

      };

    private ConferencesDto thatConference = new ConferencesDto()
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