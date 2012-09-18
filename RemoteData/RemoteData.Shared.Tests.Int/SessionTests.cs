using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TekConf.RemoteData.Dtos.v1;
using TekConf.RemoteData.v1;
using NUnit.Framework;
using Should;

namespace RemoteData.Shared.Tests.Int
{
  [TestFixture]
  public class SessionTests
  {
    private const string _baseUrl = "http://localhost:25825/v1";
    //private const string _baseUrl = "http://api.tekconf.com/v1";
    [Test]
    public void GetSessions()
    {
      var slug = "Android-Pro-Tips";
      var conferenceSlug = "CodeMash-2013";
      RemoteDataRepository remoteData = new RemoteDataRepository(_baseUrl);
      IList<SessionsDto> sessions = null;
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
          sessions.FirstOrDefault().slug.ShouldEqual(slug);
          gotData = true;
        }
      }

      gotData.ShouldBeTrue();
      sessions.ShouldNotBeNull();
    }

    [Test]
    public void GetSession()
    {
      RemoteDataRepository remoteData = new RemoteDataRepository(_baseUrl);
      SessionDto session = null;
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
          session.slug.ShouldEqual(slug);
          gotData = true;
        }
      }

      gotData.ShouldBeTrue();
      session.ShouldNotBeNull();
    }
  }
}