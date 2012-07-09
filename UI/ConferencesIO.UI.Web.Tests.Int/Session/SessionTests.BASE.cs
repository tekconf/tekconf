using System;
using System.Collections.Generic;
using System.Net;
using ConferencesIO.RemoteData.Dtos;
using NUnit.Framework;
using ServiceStack.Text;

namespace ConferencesIO.UI.Api.Tests.Int
{
  [TestFixture]
  public partial class SessionTests : RestTestBase
  {
    public List<SessionsDto> GetConferenceSessions(string conferenceSlug)
    {
      string url = rootUrl + "/conferences/" + conferenceSlug + "/sessions";

      var client = new WebClient { Encoding = System.Text.Encoding.UTF8 };

      client.Headers[HttpRequestHeader.Accept] = "application/json";
      var returnString = client.DownloadString(new Uri(url));
      var sessions = JsonSerializer.DeserializeFromString<List<SessionsDto>>(returnString);
      return sessions;
    }

    public SessionDto GetConferenceSession(string conferenceSlug, string sessionSlug)
    {
      string url = rootUrl + "/conferences/" + conferenceSlug + "/sessions/" + sessionSlug;

      var client = new WebClient { Encoding = System.Text.Encoding.UTF8 };

      client.Headers[HttpRequestHeader.Accept] = "application/json";
      var returnString = client.DownloadString(new Uri(url));
      var session = JsonSerializer.DeserializeFromString<SessionDto>(returnString);
      return session;
    }

    public List<string> GetConferenceSessionPrerequisites(string conferenceSlug, string sessionSlug)
    {
      string url = rootUrl + "/conferences/" + conferenceSlug + "/sessions/" + sessionSlug + "/prerequisites";

      var client = new WebClient { Encoding = System.Text.Encoding.UTF8 };

      client.Headers[HttpRequestHeader.Accept] = "application/json";
      var returnString = client.DownloadString(new Uri(url));
      var prerequisites = JsonSerializer.DeserializeFromString<List<string>>(returnString);
      return prerequisites;
    }

    public List<string> GetConferenceSessionLinks(string conferenceSlug, string sessionSlug)
    {
      string url = rootUrl + "/conferences/" + conferenceSlug + "/sessions/" + sessionSlug + "/links";

      var client = new WebClient { Encoding = System.Text.Encoding.UTF8 };

      client.Headers[HttpRequestHeader.Accept] = "application/json";
      var returnString = client.DownloadString(new Uri(url));
      var links = JsonSerializer.DeserializeFromString<List<string>>(returnString);
      return links;
    }

    public List<string> GetConferenceSessionResources(string conferenceSlug, string sessionSlug)
    {
      string url = rootUrl + "/conferences/" + conferenceSlug + "/sessions/" + sessionSlug + "/resources";

      var client = new WebClient { Encoding = System.Text.Encoding.UTF8 };

      client.Headers[HttpRequestHeader.Accept] = "application/json";
      var returnString = client.DownloadString(new Uri(url));
      var resources = JsonSerializer.DeserializeFromString<List<string>>(returnString);
      return resources;
    }
  }
}