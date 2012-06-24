using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using ArtekSoftware.Conference.RemoteData.Dtos;
using KellermanSoftware.CompareNetObjects;
using NUnit.Framework;
using ServiceStack.Text;
using Should;

namespace ArtekSoftware.Conference.UI.Web.Tests.Int
{
  [TestFixture]
  public class SpeakerTests : RestTestBase
  {
    [Test(Description = "http://localhost:6327/api/conferences/CodeMash-2012/sessions/ubiquitous-app-development-with-phonegap/speakers")]
    public void given_a_GET_request_for_session_speakers_it_returns_speakers()
    {
      var speakers = GetConferenceSessionSpeakers(codemash.slug, phonegap.slug);
      var compareObjects = new CompareObjects();
      var areSame = compareObjects.Compare(speakers, phonegap.speakers);
      if (!areSame)
      {
        Assert.Fail(compareObjects.DifferencesString);
      }
    }

    [Test(Description = "http://localhost:6327/api/conferences/CodeMash-2012/sessions/ubiquitous-app-development-with-phonegap/speakers/igor-polevoy")]
    public void given_a_GET_request_for_a_single_session_speaker_it_returns_the_speaker()
    {
      var speaker = GetConferenceSessionSpeaker(codemash.slug, phonegap.slug, robGibbens.slug);
      var compareObjects = new CompareObjects();
      var areSame = compareObjects.Compare(speaker, robGibbens);
      if (!areSame)
      {
        Assert.Fail(compareObjects.DifferencesString);
      }
    }

    [Test(Description = "http://localhost:6327/api/conferences/CodeMash-2012/speakers")]
    public void given_a_GET_request_for_conference_speakers_it_returns_all_speakers()
    {
      var speakers = GetConferenceSpeakers(codemash.slug);
      var compareObjects = new CompareObjects();
      var areSame = compareObjects.Compare(speakers, phonegap.speakers);
      if (!areSame)
      {
        Assert.Fail(compareObjects.DifferencesString);
      }
    }

    [Test(Description = "http://localhost:6327/api/conferences/CodeMash-2012/speakers/igor-polevoy")]
    public void given_a_GET_request_for_a_single_conference_speaker_it_returns_the_speaker()
    {
      var speaker = GetConferenceSpeaker(codemash.slug, robGibbens.slug);
      var compareObjects = new CompareObjects();
      var areSame = compareObjects.Compare(speaker, robGibbens);
      if (!areSame)
      {
        Assert.Fail(compareObjects.DifferencesString);
      }
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

  }
}
