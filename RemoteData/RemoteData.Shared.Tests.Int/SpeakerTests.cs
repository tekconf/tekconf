using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework;
using Should;

namespace RemoteData.Shared.Tests.Int
{
  [TestFixture]
  public class SpeakerTests
  {
    [Test]
    public void GetSpeakers()
    {
      var slug = "Speaker-Slug2";
      var conferenceSlug = "CodeMash-2013";
      RemoteData remoteData = new RemoteData();
      IList<Speaker> speakers = null;
      remoteData.GetSpeakers(conferenceSlug, s =>
                                               {
                                                 speakers = s;
                                               });

      Stopwatch stopwatch = new Stopwatch();
      stopwatch.Start();
      bool gotData = false;
      while (speakers == null && stopwatch.ElapsedMilliseconds < 3000)
      {
        if (speakers != null)
        {
          speakers.Count.ShouldEqual(2);
          speakers.FirstOrDefault().Slug.ShouldEqual(slug);
          gotData = true;
        }
      }

      gotData.ShouldBeTrue();
      speakers.ShouldNotBeNull();
    }

    [Test]
    public void GetSpeaker()
    {
      RemoteData remoteData = new RemoteData();
      Speaker speaker = null;
      string conferenceSlug = "CodeMash-2013";
      string slug = "Speaker-Slug2";
      remoteData.GetSpeaker(conferenceSlug, slug, s =>
                                                    {
                                                      speaker = s;
                                                    });

      Stopwatch stopwatch = new Stopwatch();
      stopwatch.Start();
      bool gotData = false;
      while (speaker == null && stopwatch.ElapsedMilliseconds < 10000)
      {
        if (speaker != null)
        {
          speaker.Slug.ShouldEqual(slug);
          gotData = true;
        }
      }

      gotData.ShouldBeTrue();
      speaker.ShouldNotBeNull();
    }
  }
}