using System;
using System.Net;
using ConferencesIO.RemoteData.Dtos;
using ConferencesIO.UI.Web.Services.Requests;
using NUnit.Framework;
using ServiceStack.Text;

namespace ConferencesIO.UI.Web.Tests.Int
{
  [TestFixture]
  public partial class ScheduleTests : RestTestBase
  {
    public ScheduleDto GetSchedule(ScheduleRequest request)
    {
      string url = rootUrl + "/api/conferences/" + request.conferenceSlug + "/schedule/" + request.userSlug;

      var client = new WebClient { Encoding = System.Text.Encoding.UTF8 };
      client.Headers[HttpRequestHeader.Accept] = "application/json";
      var returnString = client.DownloadString(new Uri(url));
      var schedule = JsonSerializer.DeserializeFromString<ScheduleDto>(returnString);
      return schedule;
    }
  }
}