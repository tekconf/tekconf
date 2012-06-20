using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using KellermanSoftware.CompareNetObjects;
using NUnit.Framework;
using ServiceStack.Text;
using Should;
using Should.Core;
namespace ArtekSoftware.Conference.UI.Web.Tests.Int
{
  [TestFixture]
  public class conferences
  {
    //private string rootUrl = "http://conference.azurewebsites.net";
    private string rootUrl = "http://localhost:6327";
    private ConferencesDto thatConference = new ConferencesDto() 
    { 
      name = "That Conference", 
      url = "http://localhost:6327/api/conferences/ThatConference-2012", 
      location = "Kalahari Resort, Wisconsin Dells, WI",
      start = DateTime.Parse("2012/08/13"),
      end = DateTime.Parse("2012/08/15")
    };

    [Test]
    public void given_a_GET_request_to_conferences_it_returns_at_least_one_conference()
    {
      GetConferences().Count.ShouldBeInRange(1, int.MaxValue);
      
    }

    [Test]
    public void given_a_GET_request_to_conferences_it_returns_ThatConference()
    {
      //GetConferences().FirstOrDefault().IsTheSameAs(thatConference).ShouldBeTrue();
      var conference = GetConferences().FirstOrDefault();
      var compareObjects = new CompareObjects();
      var areSame = compareObjects.Compare(conference, thatConference);
      if (!areSame)
      {
        Assert.Fail(compareObjects.DifferencesString);
      }
    }

    List<ConferencesDto> GetConferences()
    {
      string url = rootUrl + "/api/conferences";

      var client = new WebClient();
      //client.Headers[HttpRequestHeader.ContentType] = "application/json";
      client.Headers[HttpRequestHeader.Accept] = "application/json";
      var returnString = client.DownloadString(new Uri(url));
      var conferences = JsonSerializer.DeserializeFromString<List<ConferencesDto>>(returnString);
      return conferences;
    }
  }

  public static class ConferencesExtensions
  {
    public static bool IsTheSameAs(this ConferencesDto source, ConferencesDto expected)
    {
      var compareObjects = new CompareObjects();
      return compareObjects.Compare(source, expected);
    }
  }
}
