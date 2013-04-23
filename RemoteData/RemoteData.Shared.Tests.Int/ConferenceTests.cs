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
  public class ConferenceTests
  {
    private const string _baseUrl = "http://localhost:25825/";
    //private const string _baseUrl = "http://api.tekconf.com/";
    [Test]
    public async Task GetConferences()
    {
      var remoteData = new RemoteDataRepository(_baseUrl);
      
      var conferences = await remoteData.GetConferencesAsync("user");

      var stopwatch = new Stopwatch();
      stopwatch.Start();
      var gotData = false;
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
    public async Task GetConference()
    {
      var remoteData = new RemoteDataRepository(_baseUrl);
      
      const string slug = "codeMash-2013";
      var conferences = await remoteData.GetConferencesAsync(slug);

      var stopwatch = new Stopwatch();
      stopwatch.Start();
      bool gotData = false;
      while (conferences == null && stopwatch.ElapsedMilliseconds < 3000)
      {
        if (conferences != null)
        {
          conferences.First().slug.ShouldEqual(slug);
          gotData = true;
        }
      }

      gotData.ShouldBeTrue();
      conferences.ShouldNotBeNull();
    }

  }
}
