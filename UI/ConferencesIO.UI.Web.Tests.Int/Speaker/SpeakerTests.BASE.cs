using System;
using System.Collections.Generic;
using System.Net;
using ConferencesIO.RemoteData.Dtos;
using NUnit.Framework;
using ServiceStack.Text;

namespace ConferencesIO.UI.Web.Tests.Int
{
  [TestFixture]
  public partial class SpeakerTests : RestTestBase
  {
    public List<SpeakersDto> GetConferenceSessionSpeakers(string conferenceSlug, string sessionSlug)
    {
      string url = rootUrl + "/api/conferences/" + conferenceSlug + "/sessions/" + sessionSlug + "/speakers";

      var client = new WebClient();

      client.Headers[HttpRequestHeader.Accept] = "application/json";
      var returnString = client.DownloadString(new Uri(url));
      var speakers = JsonSerializer.DeserializeFromString<List<SpeakersDto>>(returnString);
      return speakers;
    }


    public List<SpeakersDto> GetConferenceSpeakers(string conferenceSlug)
    {
      string url = rootUrl + "/api/conferences/" + conferenceSlug + "/speakers";

      var client = new WebClient();

      client.Headers[HttpRequestHeader.Accept] = "application/json";
      var returnString = client.DownloadString(new Uri(url));
      var speakers = JsonSerializer.DeserializeFromString<List<SpeakersDto>>(returnString);
      return speakers;
    }

    public SpeakerDto GetConferenceSessionSpeaker(string conferenceSlug, string sessionSlug, string speakerSlug)
    {
      string url = rootUrl + "/api/conferences/" + conferenceSlug + "/sessions/" + sessionSlug + "/speakers/" + speakerSlug;

      var client = new WebClient();

      client.Headers[HttpRequestHeader.Accept] = "application/json";
      var returnString = client.DownloadString(new Uri(url));
      var speaker = JsonSerializer.DeserializeFromString<SpeakerDto>(returnString);
      return speaker;
    }

    public SpeakerDto GetConferenceSpeaker(string conferenceSlug, string speakerSlug)
    {
      string url = rootUrl + "/api/conferences/" + conferenceSlug + "/speakers/" + speakerSlug;

      var client = new WebClient();

      client.Headers[HttpRequestHeader.Accept] = "application/json";
      var returnString = client.DownloadString(new Uri(url));
      var speaker = JsonSerializer.DeserializeFromString<SpeakerDto>(returnString);
      return speaker;
    }
  }
}