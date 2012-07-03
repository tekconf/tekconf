using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using ArtekSoftware.Conference.RemoteData.Dtos;
using ArtekSoftware.Conference.UI.Web.Services.Requests;
using KellermanSoftware.CompareNetObjects;
using NUnit.Framework;
using ServiceStack.Text;
using Should;

namespace ArtekSoftware.Conference.UI.Web.Tests.Int
{
  [TestFixture]
  public class ScheduleTests : RestTestBase
  {
    [Test(Description = "http://localhost:6327/api/conferences/CodeMash-2012/schedule/rob-gibbens")]
    public void given_a_GET_request_for_all_conferences_it_returns_subset_of_conference_info()
    {
      //GetConferences().FirstOrDefault().IsTheSameAs(codemashs).ShouldBeTrue();
      var schedule = GetSchedule(new ScheduleRequest() { conferenceSlug = "CodeMash-2012", userSlug = "rob-gibbens" });
      var compareObjects = new CompareObjects();
      var areSame = compareObjects.Compare(schedule, robsSchedule);
      if (!areSame)
      {
        Assert.Fail(compareObjects.DifferencesString);
      }
    }


    public ScheduleDto GetSchedule(ScheduleRequest request)
    {
      string url = rootUrl + "/api/conferences/" + request.conferenceSlug + "/schedule/" + request.userSlug;

      var client = new WebClient();
      //client.Headers[HttpRequestHeader.ContentType] = "application/json";
      client.Headers[HttpRequestHeader.Accept] = "application/json";
      var returnString = client.DownloadString(new Uri(url));
      var schedule = JsonSerializer.DeserializeFromString<ScheduleDto>(returnString);
      return schedule;
    }
  }
}
