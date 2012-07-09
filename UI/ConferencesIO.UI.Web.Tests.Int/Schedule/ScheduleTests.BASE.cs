using System;
using System.Net;
using ConferencesIO.RemoteData.Dtos;
using ConferencesIO.RemoteData.Dtos.v1;
using ConferencesIO.UI.Api.Services.Requests.v1;
using NUnit.Framework;
using ServiceStack.Text;

namespace ConferencesIO.UI.Api.Tests.Int
{
  [TestFixture]
  public partial class ScheduleTests : RestTestBase
  {
    public ScheduleDto GetSchedule(ScheduleRequest request)
    {
      string url = rootUrl + "/conferences/" + request.conferenceSlug + "/schedule/" + request.userSlug;

      var client = new WebClient { Encoding = System.Text.Encoding.UTF8 };
      client.Headers[HttpRequestHeader.Accept] = "application/json";
      var returnString = client.DownloadString(new Uri(url));
      var schedule = JsonSerializer.DeserializeFromString<ScheduleDto>(returnString);
      return schedule;
    }
  }
}