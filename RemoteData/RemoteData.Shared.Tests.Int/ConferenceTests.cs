using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ConferencesIO.RemoteData.Dtos.v1;
using ConferencesIO.RemoteData.v1;
using NUnit.Framework;
using Should;

namespace RemoteData.Shared.Tests.Int
{
  [TestFixture]
  public class ConferenceTests
  {
    private const string _baseUrl = "http://localhost/ConferencesIO/";
    //private const string _baseUrl = "http://conferencesioapi.azurewebsites.net/";
    [Test]
    public void GetConferences()
    {
      RemoteDataRepository remoteData = new RemoteDataRepository(_baseUrl);
      IList<ConferencesDto> conferences = null;
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
      ConferenceDto conference = null;
      string slug = "CodeMash-2013";
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
