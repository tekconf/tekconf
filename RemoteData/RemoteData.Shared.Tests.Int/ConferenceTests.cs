using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Xunit;
using Should;
using Should.Core;

namespace RemoteData.Shared.Tests.Int
{
  [TestFixture]
  public class ConferenceTests
  {
    [Test]
    public void GetConferences()
    {
      RemoteData remoteData = new RemoteData();
      IList<Conference> conferences = null;
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
          conferences.FirstOrDefault().Slug.ShouldEqual("CodeMash-2013");
          gotData = true;
        }
      }

      gotData.ShouldBeTrue();
      conferences.ShouldNotBeNull();
    }

    [Test]
    public void GetConference()
    {
      RemoteData remoteData = new RemoteData();
      Conference conference = null;
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
          conference.Slug.ShouldEqual(slug);
          gotData = true;
        }
      }

      gotData.ShouldBeTrue();
      conference.ShouldNotBeNull();
    }

  }
}
