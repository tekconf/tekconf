using System.Linq;
using KellermanSoftware.CompareNetObjects;
using NUnit.Framework;
using Should;

namespace ArtekSoftware.Conference.UI.Web.Tests.Int
{
  [TestFixture]
  public class ConferencesTest : RestTestBase
  {
    [Test(Description = "http://localhost:6327/api/conferences")]
    public void given_a_GET_request_for_all_conferences_it_returns_subset_of_conference_info()
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

    [Test(Description = "http://localhost:6327/api/conferences")]
    public void given_a_GET_request_for_all_conferences_it_returns_list_of_conferences_with_urls_to_details()
    {
      GetConferences().Count.ShouldBeInRange(1, int.MaxValue);
    }

    [Test(Description = "http://localhost:6327/api/conferences/ThatConference-2012")]
    public void given_a_GET_request_for_a_single_conference_it_returns_general_info_with_a_link_to_sessions()
    {
      var conference = GetConference(thatConference.slug);
      var compareObjects = new CompareObjects();
      var areSame = compareObjects.Compare(conference, thatConference);
      if (!areSame)
      {
        Assert.Fail(compareObjects.DifferencesString);
      }
    }

  }
}