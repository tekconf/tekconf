using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ConferencesIO.RemoteData;
using ConferencesIO.RemoteData.Dtos;
using ConferencesIO.RemoteData.Dtos.v1;
using ConferencesIO.RemoteData.v1;
using NUnit.Framework;
using Should;

namespace RemoteData.Shared.Tests.Int
{
  [TestFixture]
  public class SpeakerTests
  {
    private const string _baseUrl = "http://localhost/ConferencesIO/v1";
    //private const string _baseUrl = "http://conferencesioapi.azurewebsites.net/v1";
    
    [Test]
    public void GetSpeakers()
    {
      var slug = "Speaker-Slug2";
      var conferenceSlug = "CodeMash-2013";
      RemoteDataRepository remoteData = new RemoteDataRepository(_baseUrl);
      IList<SpeakersDto> speakers = null;
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
          speakers.FirstOrDefault().slug.ShouldEqual(slug);
          gotData = true;
        }
      }

      gotData.ShouldBeTrue();
      speakers.ShouldNotBeNull();
    }

    [Test]
    public void GetSpeaker()
    {
      RemoteDataRepository remoteData = new RemoteDataRepository(_baseUrl);
      SpeakerDto speaker = null;
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
          speaker.slug.ShouldEqual(slug);
          gotData = true;
        }
      }

      gotData.ShouldBeTrue();
      speaker.ShouldNotBeNull();
    }
  }
}