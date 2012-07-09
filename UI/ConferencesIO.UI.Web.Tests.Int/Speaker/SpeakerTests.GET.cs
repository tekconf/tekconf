using KellermanSoftware.CompareNetObjects;
using NUnit.Framework;

namespace ConferencesIO.UI.Api.Tests.Int
{
  [TestFixture]
  public partial class SpeakerTests : RestTestBase
  {
    [Test(Description = "http://localhost/ConferencesIO/conferences/CodeMash-2012/sessions/ubiquitous-app-development-with-phonegap/speakers")]
    public void given_a_GET_request_for_session_speakers_it_returns_speakers()
    {
      var speakers = GetConferenceSessionSpeakers(codemashs.slug, phonegap.slug);
      var compareObjects = new CompareObjects();
      var areSame = compareObjects.Compare(speakers, phonegap.speakers);
      if (!areSame)
      {
        Assert.Fail(compareObjects.DifferencesString);
      }
    }

    [Test(Description = "http://localhost/ConferencesIO/conferences/CodeMash-2012/sessions/ubiquitous-app-development-with-phonegap/speakers/igor-polevoy")]
    public void given_a_GET_request_for_a_single_session_speaker_it_returns_the_speaker()
    {
      var speaker = GetConferenceSessionSpeaker(codemashs.slug, phonegap.slug, andrewGlover.slug);
      var compareObjects = new CompareObjects();
      var areSame = compareObjects.Compare(speaker, andrewGlover);
      if (!areSame)
      {
        Assert.Fail(compareObjects.DifferencesString);
      }
    }

    [Test(Description = "http://localhost/ConferencesIO/conferences/CodeMash-2012/speakers")]
    public void given_a_GET_request_for_conference_speakers_it_returns_all_speakers()
    {
      var speakers = GetConferenceSpeakers(codemashs.slug);
      var compareObjects = new CompareObjects();
      var areSame = compareObjects.Compare(speakers.Count, 100);
      if (!areSame)
      {
        Assert.Fail(compareObjects.DifferencesString);
      }
    }

    [Test(Description = "http://localhost/ConferencesIO/conferences/CodeMash-2012/speakers/igor-polevoy")]
    public void given_a_GET_request_for_a_single_conference_speaker_it_returns_the_speaker()
    {
      var speaker = GetConferenceSpeaker(codemashs.slug, andrewGlover.slug);
      var compareObjects = new CompareObjects();
      var areSame = compareObjects.Compare(speaker, andrewGlover);
      if (!areSame)
      {
        Assert.Fail(compareObjects.DifferencesString);
      }
    }

    

  }
}
