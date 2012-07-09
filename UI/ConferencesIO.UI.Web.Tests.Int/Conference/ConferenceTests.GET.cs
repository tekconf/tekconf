using System.Linq;
using ConferencesIO.UI.Api.Services.Requests;
using KellermanSoftware.CompareNetObjects;
using NUnit.Framework;
using Should;

namespace ConferencesIO.UI.Api.Tests.Int
{
  [TestFixture]
  public partial class ConferenceTests : RestTestBase
  {
    [Test(Description = "http://localhost/ConferencesIO.UI.Api/conferences")]
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

    [Test(Description = "http://localhost/ConferencesIO.UI.Api/conferences")]
    public void given_a_GET_request_for_all_conferences_it_returns_list_of_conferences_with_urls_to_details()
    {
      GetConferences(new ConferencesRequest()).Count.ShouldBeInRange(1, int.MaxValue);
    }

    [Test(Description = "http://localhost/ConferencesIO.UI.Api/conferences/CodeMash-2012")]
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

  }
}