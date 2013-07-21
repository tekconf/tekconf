using NUnit.Framework;

namespace ConferencesIO.UI.Api.Tests.Int
{
  /// <summary>
  /// Create can be implemented using an HTTP PUT, if (and only if) the payload of the request contains the full content of the exactly specified URL.
  /// This command is idempotent, and calling it multiple times with the same payload will not cause changes. 
  /// </summary>
  [TestFixture]
  public partial class ConferenceTests : RestTestBase
  {
    [Test]
    public void given_a_PUT_request_for_a_resource_that_already_exists_it_should_return_an_error()
    {
      Assert.Fail("Not Implemented");
    }

    [Test]
    public void given_a_PUT_request_for_a_resource_that_does_not_already_exist_it_should_create_the_resource()
    {
      Assert.Fail("Not Implemented");
    }
  }
}