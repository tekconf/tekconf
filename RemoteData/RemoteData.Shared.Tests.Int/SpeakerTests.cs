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
  public class SpeakerTests
  {
    private const string _baseUrl = "http://localhost:25825/v1";
    //private const string _baseUrl = "http://api.tekconf.com/v1";
    
    [Test]
		[Ignore]
    public async Task GetSpeakers()
    {
      const string slug = "Speaker-Slug2";
      const string conferenceSlug = "CodeMash-2013";
      var remoteData = new RemoteDataRepository(_baseUrl);
     
      var speakers = await remoteData.GetSpeakers(conferenceSlug);

      var stopwatch = new Stopwatch();
      stopwatch.Start();
      var gotData = false;
      while (speakers == null && stopwatch.ElapsedMilliseconds < 3000)
      {
        if (speakers != null)
        {
          speakers.Count.ShouldEqual(2);
          speakers.FirstOrDefault().slug.ShouldEqual(slug);
          gotData = true;
        }
      }

      gotData.ShouldBeTrue();
      speakers.ShouldNotBeNull();
    }

    [Test]
		[Ignore]
    public async Task GetSpeaker()
    {
      var remoteData = new RemoteDataRepository(_baseUrl);

      const string conferenceSlug = "CodeMash-2013";
      const string slug = "Speaker-Slug2";
      var speaker = await remoteData.GetSpeaker(conferenceSlug, slug);

      var stopwatch = new Stopwatch();
      stopwatch.Start();
      var gotData = false;
      while (speaker == null && stopwatch.ElapsedMilliseconds < 10000)
      {
        if (speaker != null)
        {
          speaker.slug.ShouldEqual(slug);
          gotData = true;
        }
      }

      gotData.ShouldBeTrue();
      speaker.ShouldNotBeNull();
    }
  }
}