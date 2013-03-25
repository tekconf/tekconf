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
  public class ConferenceTests
  {
    private const string _baseUrl = "http://localhost:25825/";
    //private const string _baseUrl = "http://api.tekconf.com/";
    [Test]
    public void GetConferences()
    {
      RemoteDataRepository remoteData = new RemoteDataRepository(_baseUrl);
      IList<FullConferenceDto> conferences = null;
      remoteData.GetConferences(c =>
                                     {
                                       conferences = c;
                                     });

      Stopwatch stopwatch = new Stopwatch();
      stopwatch.Start();
      bool gotData = false;
      while(conferences == null && stopwatch.ElapsedMilliseconds < 3000)
      {
        if (conferences != null)
        {
          conferences.Count.ShouldEqual(1);
          conferences.FirstOrDefault().slug.ShouldEqual("CodeMash-2013");
          gotData = true;
        }
      }

      gotData.ShouldBeTrue();
      conferences.ShouldNotBeNull();
    }

    [Test]
    public void GetConference()
    {
      RemoteDataRepository remoteData = new RemoteDataRepository(_baseUrl);
      FullConferenceDto conference = null;
      string slug = "codeMash-2013";
      remoteData.GetConference(slug, c =>
      {
        conference = c;
      });

      Stopwatch stopwatch = new Stopwatch();
      stopwatch.Start();
      bool gotData = false;
      while (conference == null && stopwatch.ElapsedMilliseconds < 3000)
      {
        if (conference != null)
        {
          conference.slug.ShouldEqual(slug);
          gotData = true;
        }
      }

      gotData.ShouldBeTrue();
      conference.ShouldNotBeNull();
    }

  }
}
