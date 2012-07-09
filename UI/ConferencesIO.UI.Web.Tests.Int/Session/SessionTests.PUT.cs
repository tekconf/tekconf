using NUnit.Framework;

namespace ConferencesIO.UI.Web.Tests.Int
{
   [TestFixture]
  public partial class SessionTests : RestTestBase
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