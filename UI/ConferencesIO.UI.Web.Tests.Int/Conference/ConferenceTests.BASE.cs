using System;
using System.Collections.Generic;
using System.Net;
using ConferencesIO.RemoteData.Dtos.v1;
using ConferencesIO.UI.Api.Services.Requests.v1;
using NUnit.Framework;
using ServiceStack.Text;

namespace ConferencesIO.UI.Api.Tests.Int
{
  [TestFixture]
  public partial class ConferenceTests : RestTestBase
  {
    public List<ConferencesDto> GetConferences(ConferencesRequest request)
    {
      string url = rootUrl + "/conferences";

      var client = new WebClient { Encoding = System.Text.Encoding.UTF8 };
      client.Headers[HttpRequestHeader.Accept] = "application/json";
      var returnString = client.DownloadString(new Uri(url));
      var conferences = JsonSerializer.DeserializeFromString<List<ConferencesDto>>(returnString);
      return conferences;
    }

    public ConferenceDto GetConference(ConferencesRequest request)
    {
      string url = rootUrl + "/conferences/" + request.conferenceSlug;

      var client = new WebClient { Encoding = System.Text.Encoding.UTF8 };
      client.Headers[HttpRequestHeader.Accept] = "application/json";
      var returnString = client.DownloadString(new Uri(url));
      var conference = JsonSerializer.DeserializeFromString<ConferenceDto>(returnString);
      return conference;
    }
  }
}