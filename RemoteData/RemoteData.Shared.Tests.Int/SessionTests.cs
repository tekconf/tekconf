using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework;
using Should;

namespace RemoteData.Shared.Tests.Int
{
  [TestFixture]
  public class SessionTests
  {
    [Test]
    public void GetSessions()
    {
      var slug = "Android-Pro-Tips";
      var conferenceSlug = "CodeMash-2013";
      RemoteData remoteData = new RemoteData();
      IList<Session> sessions = null;
      remoteData.GetSessions(conferenceSlug, s =>
                                               {
                                                 sessions = s;
                                               });

      Stopwatch stopwatch = new Stopwatch();
      stopwatch.Start();
      bool gotData = false;
      while (sessions == null && stopwatch.ElapsedMilliseconds < 3000)
      {
        if (sessions != null)
        {
          sessions.Count.ShouldEqual(1);
          sessions.FirstOrDefault().Slug.ShouldEqual(slug);
          gotData = true;
        }
      }

      gotData.ShouldBeTrue();
      sessions.ShouldNotBeNull();
    }

    [Test]
    public void GetSession()
    {
      RemoteData remoteData = new RemoteData();
      Session session = null;
      string conferenceSlug = "CodeMash-2013";
      string slug = "Android-Pro-Tips";
      remoteData.GetSession(conferenceSlug, slug, s =>
                                                    {
                                                      session = s;
                                                    });

      Stopwatch stopwatch = new Stopwatch();
      stopwatch.Start();
      bool gotData = false;
      while (session == null && stopwatch.ElapsedMilliseconds < 10000)
      {
        if (session != null)
        {
          session.Slug.ShouldEqual(slug);
          gotData = true;
        }
      }

      gotData.ShouldBeTrue();
      session.ShouldNotBeNull();
    }
  }
}