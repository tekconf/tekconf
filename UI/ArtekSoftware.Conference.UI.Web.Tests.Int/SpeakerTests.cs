using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using ArtekSoftware.Conference.RemoteData.Dtos;
using KellermanSoftware.CompareNetObjects;
using NUnit.Framework;
using ServiceStack.Text;
using Should;

namespace ArtekSoftware.Conference.UI.Web.Tests.Int
{
  [TestFixture]
  public class SpeakerTests : RestTestBase
  {
    [Test(Description = "http://localhost:6327/api/conferences/ThatConference-2012/sessions/activejdbc/speakers")]
    public void given_a_GET_request_for_session_speakers_it_returns_speakers()
    {
      var speakers = GetConferenceSessionSpeakers(thatConference.slug, activejdbc.slug);
      var compareObjects = new CompareObjects();
      var areSame = compareObjects.Compare(speakers, activejdbc.speakers);
      if (!areSame)
      {
        Assert.Fail(compareObjects.DifferencesString);
      }
    }

    [Test(Description = "http://localhost:6327/api/conferences/ThatConference-2012/sessions/activejdbc/speakers/igor-polevoy")]
    public void given_a_GET_request_for_a_single_session_speaker_it_returns_the_speaker()
    {
      var speaker = GetConferenceSessionSpeaker(thatConference.slug, activejdbc.slug, robGibbens.slug);
      var compareObjects = new CompareObjects();
      var areSame = compareObjects.Compare(speaker, robGibbens);
      if (!areSame)
      {
        Assert.Fail(compareObjects.DifferencesString);
      }
    }

    [Test(Description = "http://localhost:6327/api/conferences/ThatConference-2012/speakers")]
    public void given_a_GET_request_for_conference_speakers_it_returns_all_speakers()
    {
      var speakers = GetConferenceSpeakers(thatConference.slug);
      var compareObjects = new CompareObjects();
      var areSame = compareObjects.Compare(speakers, activejdbc.speakers);
      if (!areSame)
      {
        Assert.Fail(compareObjects.DifferencesString);
      }
    }

    [Test(Description = "http://localhost:6327/api/conferences/ThatConference-2012/speakers/igor-polevoy")]
    public void given_a_GET_request_for_a_single_conference_speaker_it_returns_the_speaker()
    {
      var speaker = GetConferenceSpeaker(thatConference.slug, robGibbens.slug);
      var compareObjects = new CompareObjects();
      var areSame = compareObjects.Compare(speaker, robGibbens);
      if (!areSame)
      {
        Assert.Fail(compareObjects.DifferencesString);
      }
    }

  }
}
