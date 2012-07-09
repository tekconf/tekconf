using ConferencesIO.UI.Web.Services.Requests;
using KellermanSoftware.CompareNetObjects;
using NUnit.Framework;

namespace ConferencesIO.UI.Web.Tests.Int
{


  [TestFixture]
  public partial class ScheduleTests : RestTestBase
  {
    [Test(Description = "http://localhost/ConferencesIO/api/conferences/CodeMash-2012/schedule/rob-gibbens")]
    public void given_a_GET_request_for_a_schedule_it_returns_the_schedule_with_sessions()
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

  }
}
