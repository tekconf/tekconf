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
  public class ConferencesTest : RestTestBase
  {
    [Test(Description = "http://localhost:6327/api/conferences")]
    public void given_a_GET_request_for_all_conferences_it_returns_subset_of_conference_info()
    {
      //GetConferences().FirstOrDefault().IsTheSameAs(codemashs).ShouldBeTrue();
      var conference = GetConferences(new ConferencesRequest()).FirstOrDefault();
      var compareObjects = new CompareObjects();
      var areSame = compareObjects.Compare(conference, codemashs);
      if (!areSame)
      {
        Assert.Fail(compareObjects.DifferencesString);
      }
    }

    [Test(Description = "http://localhost:6327/api/conferences")]
    public void given_a_GET_request_for_all_conferences_it_returns_list_of_conferences_with_urls_to_details()
    {
      GetConferences(new ConferencesRequest()).Count.ShouldBeInRange(1, int.MaxValue);
    }

    [Test(Description = "http://localhost:6327/api/conferences/CodeMash-2012")]
    public void given_a_GET_request_for_a_single_conference_it_returns_general_info_with_a_link_to_sessions()
    {
      var conference = GetConference(new ConferencesRequest() { conferenceSlug = codemash.slug } );
      var compareObjects = new CompareObjects();
      var areSame = compareObjects.Compare(conference, codemash);
      if (!areSame)
      {
        Assert.Fail(compareObjects.DifferencesString);
      }
    }


    public List<ConferencesDto> GetConferences(ConferencesRequest request)
    {
      string url = rootUrl + "/api/conferences";

      var client = new WebClient();
      //client.Headers[HttpRequestHeader.ContentType] = "application/json";
      client.Headers[HttpRequestHeader.Accept] = "application/json";
      var returnString = client.DownloadString(new Uri(url));
      var conferences = JsonSerializer.DeserializeFromString<List<ConferencesDto>>(returnString);
      return conferences;
    }

    public ConferenceDto GetConference(ConferencesRequest request)
    {
      string url = rootUrl + "/api/conferences/" + request.conferenceSlug;

      var client = new WebClient();
      //client.Headers[HttpRequestHeader.ContentType] = "application/json";
      client.Headers[HttpRequestHeader.Accept] = "application/json";
      var returnString = client.DownloadString(new Uri(url));
      var conference = JsonSerializer.DeserializeFromString<ConferenceDto>(returnString);
      return conference;
    }

  }
}