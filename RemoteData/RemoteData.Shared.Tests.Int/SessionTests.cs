using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
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
    public async Task GetSessions()
    {
      const string slug = "Android-Pro-Tips";
      const string conferenceSlug = "CodeMash-2013";
      var remoteData = new RemoteDataRepository(_baseUrl);
      
      var sessions = await remoteData.GetSessionsAsync(conferenceSlug);

      var stopwatch = new Stopwatch();
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
    public async Task GetSession()
    {
      var remoteData = new RemoteDataRepository(_baseUrl);
      
      const string conferenceSlug = "CodeMash-2013";
      const string slug = "Android-Pro-Tips";
      var session = await remoteData.GetSession(conferenceSlug, slug);

      var stopwatch = new Stopwatch();
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