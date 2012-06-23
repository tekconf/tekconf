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

    [Test(Description = "http://localhost:6327/api/conferences/ThatConference-2012/sessions/activejdbc/prerequisites")]
    public void given_a_GET_request_for_a_prerequisites_it_returns_prerequisites()
    {
      var prerequisites = GetConferenceSessionPrerequisites(thatConference.slug, activejdbc.slug);
      var compareObjects = new CompareObjects();
      var areSame = compareObjects.Compare(prerequisites, null);
      if (!areSame)
      {
        Assert.Fail(compareObjects.DifferencesString);
      }
    }

    [Test(Description = "http://localhost:6327/api/conferences/ThatConference-2012/sessions/activejdbc/links")]
    public void given_a_GET_request_for_links_it_returns_links()
    {
      var links = GetConferenceSessionLinks(thatConference.slug, activejdbc.slug);
      var compareObjects = new CompareObjects();
      var areSame = compareObjects.Compare(links, null);
      if (!areSame)
      {
        Assert.Fail(compareObjects.DifferencesString);
      }
    }

    [Test(Description = "http://localhost:6327/api/conferences/ThatConference-2012/sessions/activejdbc/resources")]
    public void given_a_GET_request_for_resources_it_returns_resources()
    {
      var resources = GetConferenceSessionResources(thatConference.slug, activejdbc.slug);
      var compareObjects = new CompareObjects();
      var areSame = compareObjects.Compare(resources, null);
      if (!areSame)
      {
        Assert.Fail(compareObjects.DifferencesString);
      }
    }
  }
}
