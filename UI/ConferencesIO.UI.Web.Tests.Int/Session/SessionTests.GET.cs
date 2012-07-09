using System.Collections.Generic;
using KellermanSoftware.CompareNetObjects;
using NUnit.Framework;

namespace ConferencesIO.UI.Web.Tests.Int
{
  [TestFixture]
  public partial class SessionTests : RestTestBase
  {
    [Test(Description = "http://localhost/ConferencesIO/api/conferences/CodeMash-2012/sessions")]
    public void given_a_GET_request_for_a_single_conference_sessions_it_returns_sessions_with_speakers()
    {
      var sessions = GetConferenceSessions(codemashs.slug);
      var compareObjects = new CompareObjects();
      var areSame = compareObjects.Compare(sessions.Count, 156);
      if (!areSame)
      {
        Assert.Fail(compareObjects.DifferencesString);
      }
    }

    [Test(Description = "http://localhost/ConferencesIO/api/conferences/CodeMash-2012/sessions/ubiquitous-app-development-with-phonegap")]
    public void given_a_GET_request_for_a_single_session_it_returns_a_session_with_child_collections()
    {
      var session = GetConferenceSession(codemashs.slug, phonegap.slug);
      var compareObjects = new CompareObjects();
      var areSame = compareObjects.Compare(session, phonegap);
      if (!areSame)
      {
        Assert.Fail(compareObjects.DifferencesString);
      }
    }

    [Test(Description = "http://localhost/ConferencesIO/api/conferences/CodeMash-2012/sessions/ubiquitous-app-development-with-phonegap/prerequisites")]
    public void given_a_GET_request_for_a_prerequisites_it_returns_prerequisites()
    {
      var prerequisites = GetConferenceSessionPrerequisites(codemashs.slug, phonegap.slug);
      var compareObjects = new CompareObjects();
      var areSame = compareObjects.Compare(prerequisites, new List<string>());
      if (!areSame)
      {
        Assert.Fail(compareObjects.DifferencesString);
      }
    }

    [Test(Description = "http://localhost/ConferencesIO/api/conferences/CodeMash-2012/sessions/ubiquitous-app-development-with-phonegap/links")]
    public void given_a_GET_request_for_links_it_returns_links()
    {
      var links = GetConferenceSessionLinks(codemashs.slug, phonegap.slug);
      var compareObjects = new CompareObjects();
      var areSame = compareObjects.Compare(links, new List<string>());
      if (!areSame)
      {
        Assert.Fail(compareObjects.DifferencesString);
      }
    }

    [Test(Description = "http://localhost/ConferencesIO/api/conferences/CodeMash-2012/sessions/ubiquitous-app-development-with-phonegap/resources")]
    public void given_a_GET_request_for_resources_it_returns_resources()
    {
      var resources = GetConferenceSessionResources(codemashs.slug, phonegap.slug);
      var compareObjects = new CompareObjects();
      var areSame = compareObjects.Compare(resources, new List<string>());
      if (!areSame)
      {
        Assert.Fail(compareObjects.DifferencesString);
      }
    }

    
  
  
  }

}
