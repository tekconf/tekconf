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
  public class SessionTests : RestTestBase
  {
    [Test(Description = "http://localhost:6327/api/conferences/CodeMash-2012/sessions")]
    public void given_a_GET_request_for_a_single_conference_sessions_it_returns_sessions_with_speakers()
    {
      var sessions = GetConferenceSessions(codemash.slug);
      var compareObjects = new CompareObjects();
      var areSame = compareObjects.Compare(sessions, codemashSessions);
      if (!areSame)
      {
        Assert.Fail(compareObjects.DifferencesString);
      }
    }

    [Test(Description = "http://localhost:6327/api/conferences/CodeMash-2012/sessions/ubiquitous-app-development-with-phonegap")]
    public void given_a_GET_request_for_a_single_session_it_returns_a_session_with_child_collections()
    {
      var session = GetConferenceSession(codemash.slug, phonegap.slug);
      var compareObjects = new CompareObjects();
      var areSame = compareObjects.Compare(session, phonegap);
      if (!areSame)
      {
        Assert.Fail(compareObjects.DifferencesString);
      }
    }

    [Test(Description = "http://localhost:6327/api/conferences/CodeMash-2012/sessions/ubiquitous-app-development-with-phonegap/prerequisites")]
    public void given_a_GET_request_for_a_prerequisites_it_returns_prerequisites()
    {
      var prerequisites = GetConferenceSessionPrerequisites(codemash.slug, phonegap.slug);
      var compareObjects = new CompareObjects();
      var areSame = compareObjects.Compare(prerequisites, null);
      if (!areSame)
      {
        Assert.Fail(compareObjects.DifferencesString);
      }
    }

    [Test(Description = "http://localhost:6327/api/conferences/CodeMash-2012/sessions/ubiquitous-app-development-with-phonegap/links")]
    public void given_a_GET_request_for_links_it_returns_links()
    {
      var links = GetConferenceSessionLinks(codemash.slug, phonegap.slug);
      var compareObjects = new CompareObjects();
      var areSame = compareObjects.Compare(links, null);
      if (!areSame)
      {
        Assert.Fail(compareObjects.DifferencesString);
      }
    }

    [Test(Description = "http://localhost:6327/api/conferences/CodeMash-2012/sessions/ubiquitous-app-development-with-phonegap/resources")]
    public void given_a_GET_request_for_resources_it_returns_resources()
    {
      var resources = GetConferenceSessionResources(codemash.slug, phonegap.slug);
      var compareObjects = new CompareObjects();
      var areSame = compareObjects.Compare(resources, null);
      if (!areSame)
      {
        Assert.Fail(compareObjects.DifferencesString);
      }
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
  
  
  }

}
